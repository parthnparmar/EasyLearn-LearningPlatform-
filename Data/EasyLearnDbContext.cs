using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EasyLearn.Models;

namespace EasyLearn.Data;

public partial class EasyLearnDbContext : DbContext
{
    public EasyLearnDbContext()
    {
    }

    public EasyLearnDbContext(DbContextOptions<EasyLearnDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }
    public virtual DbSet<Announcement> Announcements { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<CertificatePayment> CertificatePayments { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<ExamVerification> ExamVerifications { get; set; }
    public virtual DbSet<Game> Games { get; set; }
    public virtual DbSet<Lesson> Lessons { get; set; }
    public virtual DbSet<LoginEntry> LoginEntries { get; set; }
    public virtual DbSet<PaymentReceipt> PaymentReceipts { get; set; }
    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public virtual DbSet<PuzzleGame> PuzzleGames { get; set; }
    public virtual DbSet<PYQ> Pyqs { get; set; }
    public virtual DbSet<Quiz> Quizzes { get; set; }
    public virtual DbSet<ReExamPayment> ReExamPayments { get; set; }
    public virtual DbSet<RegistrationEntry> RegistrationEntries { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EasyLearnDb;Trusted_Connection=true;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
