using EasyLearn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyLearn.Services;

public interface IDoubtChatService
{
    Task<DoubtChat> CreateDoubtAsync(string studentId, int courseId, string subject, string message, MessageCategory category = MessageCategory.Doubt);
    Task<DoubtMessage> SendMessageAsync(int chatId, string senderId, string message);
    Task<List<DoubtChat>> GetStudentDoubtsAsync(string studentId);
    Task<List<DoubtChat>> GetInstructorDoubtsAsync(string instructorId);
    Task<DoubtChat?> GetDoubtByIdAsync(int chatId);
    Task MarkAsResolvedAsync(int chatId);
    Task AssignInstructorAsync(int chatId, string instructorId);
    Task MarkMessagesAsReadAsync(int chatId, string userId);
}

public class DoubtChatService : IDoubtChatService
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DoubtChatService(ApplicationDbContext context, IEmailService emailService, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<DoubtChat> CreateDoubtAsync(string studentId, int courseId, string subject, string message, MessageCategory category = MessageCategory.Doubt)
    {
        var course = await _context.Courses.Include(c => c.Instructor).FirstOrDefaultAsync(c => c.Id == courseId);
        var doubt = new DoubtChat
        {
            StudentId = studentId,
            CourseId = courseId,
            InstructorId = course?.InstructorId,
            Subject = subject,
            Category = category,
            Status = "Open"
        };

        _context.DoubtChats.Add(doubt);
        await _context.SaveChangesAsync();

        await SendMessageAsync(doubt.Id, studentId, message);

        // Notify instructor by email
        if (course?.Instructor?.Email != null)
        {
            var student = await _userManager.FindByIdAsync(studentId);
            await _emailService.SendEmailAsync(
                course.Instructor.Email,
                $"[EasyLearn] New {category} Message: {subject}",
                $"<p>Dear {course.Instructor.FirstName},</p>" +
                $"<p>Student <b>{student?.FirstName} {student?.LastName}</b> sent a new <b>{category}</b> message in course <b>{course.Title}</b>.</p>" +
                $"<p><b>Subject:</b> {subject}</p>" +
                $"<p><b>Message:</b> {message}</p>" +
                $"<p>Please login to EasyLearn to respond.</p>");
        }

        return doubt;
    }

    public async Task<DoubtMessage> SendMessageAsync(int chatId, string senderId, string message)
    {
        var msg = new DoubtMessage
        {
            DoubtChatId = chatId,
            SenderId = senderId,
            Message = message
        };

        _context.DoubtMessages.Add(msg);
        await _context.SaveChangesAsync();

        // Send email notification to the other party
        var chat = await _context.DoubtChats
            .Include(d => d.Student)
            .Include(d => d.Instructor)
            .Include(d => d.Course)
            .FirstOrDefaultAsync(d => d.Id == chatId);

        if (chat != null)
        {
            var sender = await _userManager.FindByIdAsync(senderId);
            bool senderIsStudent = senderId == chat.StudentId;

            if (senderIsStudent && chat.Instructor?.Email != null)
            {
                await _emailService.SendEmailAsync(
                    chat.Instructor.Email,
                    $"[EasyLearn] New Reply - {chat.Subject}",
                    $"<p>Dear {chat.Instructor.FirstName},</p>" +
                    $"<p>Student <b>{sender?.FirstName} {sender?.LastName}</b> replied in <b>{chat.Category}</b> thread: <b>{chat.Subject}</b></p>" +
                    $"<p><b>Message:</b> {message}</p>" +
                    $"<p>Login to EasyLearn to respond.</p>");
            }
            else if (!senderIsStudent && chat.Student?.Email != null)
            {
                await _emailService.SendEmailAsync(
                    chat.Student.Email,
                    $"[EasyLearn] Instructor Replied - {chat.Subject}",
                    $"<p>Dear {chat.Student.FirstName},</p>" +
                    $"<p>Your instructor replied to your <b>{chat.Category}</b> message: <b>{chat.Subject}</b></p>" +
                    $"<p><b>Message:</b> {message}</p>" +
                    $"<p>Login to EasyLearn to view the full conversation.</p>");
            }
        }

        return msg;
    }

    public async Task<List<DoubtChat>> GetStudentDoubtsAsync(string studentId)
    {
        return await _context.DoubtChats
            .Include(d => d.Course)
            .Include(d => d.Instructor)
            .Include(d => d.Messages)
            .Where(d => d.StudentId == studentId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<DoubtChat>> GetInstructorDoubtsAsync(string instructorId)
    {
        return await _context.DoubtChats
            .Include(d => d.Course)
            .Include(d => d.Student)
            .Include(d => d.Messages)
            .Where(d => d.InstructorId == instructorId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<DoubtChat?> GetDoubtByIdAsync(int chatId)
    {
        return await _context.DoubtChats
            .Include(d => d.Course)
            .Include(d => d.Student)
            .Include(d => d.Instructor)
            .Include(d => d.Messages)
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(d => d.Id == chatId);
    }

    public async Task MarkAsResolvedAsync(int chatId)
    {
        var doubt = await _context.DoubtChats.FindAsync(chatId);
        if (doubt != null)
        {
            doubt.Status = "Resolved";
            doubt.ResolvedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AssignInstructorAsync(int chatId, string instructorId)
    {
        var doubt = await _context.DoubtChats.FindAsync(chatId);
        if (doubt != null)
        {
            doubt.InstructorId = instructorId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkMessagesAsReadAsync(int chatId, string userId)
    {
        var messages = await _context.DoubtMessages
            .Where(m => m.DoubtChatId == chatId && m.SenderId != userId && !m.IsRead)
            .ToListAsync();

        foreach (var msg in messages)
        {
            msg.IsRead = true;
        }

        await _context.SaveChangesAsync();
    }
}
