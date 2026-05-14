using EasyLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyLearn.Services;

public interface IPYQService
{
    Task ConvertExpiredExamsToPYQAsync();
    Task<List<PYQ>> GetPYQsForCourseAsync(int courseId);
    Task<PYQ?> GetPYQWithQuestionsAsync(int pyqId);
}

public class PYQService : IPYQService
{
    private readonly ApplicationDbContext _context;

    public PYQService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ConvertExpiredExamsToPYQAsync()
    {
        var now = DateTime.UtcNow;
        var bufferTime = now.AddMinutes(-45); // 45 minutes buffer after exam end
        
        // Get exams that ended more than 45 minutes ago and not yet converted to PYQ
        var expiredExams = await _context.Exams
            .Include(e => e.Course)
            .Include(e => e.ExamQuestions)
            .ThenInclude(eq => eq.Options)
            .Where(e => e.ScheduledEndTime < bufferTime && 
                       e.IsApproved && 
                       !_context.PYQs.Any(p => p.ExamId == e.Id))
            .ToListAsync();

        foreach (var exam in expiredExams)
        {
            var pyq = new PYQ
            {
                ExamId = exam.Id,
                CourseId = exam.CourseId,
                Title = $"{exam.Title} - {exam.ScheduledStartTime:yyyy}",
                ExamDate = exam.ScheduledStartTime,
                AddedToPYQAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.PYQs.Add(pyq);
            await _context.SaveChangesAsync();

            // Copy questions
            foreach (var question in exam.ExamQuestions)
            {
                var pyqQuestion = new PYQQuestion
                {
                    PYQId = pyq.Id,
                    Text = question.Text,
                    ImageUrl = question.ImageUrl,
                    Type = question.Type,
                    Part = question.Part,
                    Points = question.Points,
                    OrderIndex = question.OrderIndex
                };

                _context.PYQQuestions.Add(pyqQuestion);
                await _context.SaveChangesAsync();

                // Copy options for MCQs
                foreach (var option in question.Options)
                {
                    var pyqOption = new PYQQuestionOption
                    {
                        PYQQuestionId = pyqQuestion.Id,
                        Text = option.Text,
                        IsCorrect = option.IsCorrect,
                        OrderIndex = option.OrderIndex
                    };
                    _context.PYQQuestionOptions.Add(pyqOption);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<PYQ>> GetPYQsForCourseAsync(int courseId)
    {
        return await _context.PYQs
            .Include(p => p.Course)
            .Include(p => p.Exam)
            .Where(p => p.CourseId == courseId && p.IsActive)
            .OrderByDescending(p => p.ExamDate)
            .ToListAsync();
    }

    public async Task<PYQ?> GetPYQWithQuestionsAsync(int pyqId)
    {
        return await _context.PYQs
            .Include(p => p.Course)
            .Include(p => p.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(p => p.Id == pyqId && p.IsActive);
    }
}
