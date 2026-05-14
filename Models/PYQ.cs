using System.ComponentModel.DataAnnotations;

namespace EasyLearn.Models;

public class PYQ
{
    public int Id { get; set; }
    
    [Required]
    public int ExamId { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public DateTime ExamDate { get; set; }
    public DateTime AddedToPYQAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Exam Exam { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public ICollection<PYQQuestion> Questions { get; set; } = new List<PYQQuestion>();
}

public class PYQQuestion
{
    public int Id { get; set; }
    
    public int PYQId { get; set; }
    
    [Required]
    [StringLength(1000)]
    public string Text { get; set; } = string.Empty;
    
    public string? ImageUrl { get; set; } // Image for question
    
    public ExamQuestionType Type { get; set; }
    public ExamPart Part { get; set; }
    public int Points { get; set; }
    public int OrderIndex { get; set; }
    
    // Navigation properties
    public PYQ PYQ { get; set; } = null!;
    public ICollection<PYQQuestionOption> Options { get; set; } = new List<PYQQuestionOption>();
}

public class PYQQuestionOption
{
    public int Id { get; set; }
    
    public int PYQQuestionId { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Text { get; set; } = string.Empty;
    
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
    
    // Navigation property
    public PYQQuestion PYQQuestion { get; set; } = null!;
}
