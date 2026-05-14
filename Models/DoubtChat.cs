namespace EasyLearn.Models;

public enum MessageCategory
{
    Doubt,
    Exam,
    Result,
    ExamDate,
    Assignment,
    General
}

public class DoubtChat
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string? InstructorId { get; set; }
    public int CourseId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public MessageCategory Category { get; set; } = MessageCategory.Doubt;
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
    
    public ApplicationUser Student { get; set; } = null!;
    public ApplicationUser? Instructor { get; set; }
    public Course Course { get; set; } = null!;
    public ICollection<DoubtMessage> Messages { get; set; } = new List<DoubtMessage>();
}

public class DoubtMessage
{
    public int Id { get; set; }
    public int DoubtChatId { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    
    public DoubtChat DoubtChat { get; set; } = null!;
    public ApplicationUser Sender { get; set; } = null!;
}
