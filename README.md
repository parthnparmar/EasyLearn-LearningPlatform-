# 🎓 EasyLearn — Online Learning Platform

> A full-featured Learning Management System built with **ASP.NET Core 9.0**

**🌐 Live:** https://easylearnworld.runasp.net

---

## 📌 What is EasyLearn?

EasyLearn is a complete online education platform where:
- **Students** enroll in courses, watch lessons, take exams, and earn certificates
- **Instructors** create courses, manage exams, and grade students
- **Admins** control the entire platform — users, courses, exams, and announcements

---

## ⚡ Tech Stack

| | Technology |
|---|---|
| **Framework** | ASP.NET Core 9.0 (MVC) |
| **Language** | C# |
| **Database** | SQL Server + Entity Framework Core 9 |
| **Auth** | ASP.NET Core Identity + Google / Facebook / GitHub OAuth |
| **Email** | MailKit (Gmail SMTP) |
| **PDF** | iTextSharp LGPLv2 |
| **QR Code** | ZXing.Net |
| **Frontend** | Bootstrap 5, jQuery |
| **Hosting** | runasp.net (IIS, win-x86 self-contained) |
| **DB Host** | databaseasp.net (SQL Server) |

---

## 🗂️ Project Structure

```
EasyLearn/
├── Controllers/        → 12 MVC controllers (Account, Admin, Student, Instructor...)
├── Models/             → 25+ entity models + ViewModels
├── Services/           → 14 business logic services
├── Views/              → Razor views for all roles
├── Data/               → DbInitializer (seed data)
├── Migrations/         → 24 EF Core migrations
├── wwwroot/            → CSS, JS, uploads, certificates
├── Program.cs          → App startup & DI
└── appsettings.json    → Config (DB, Email, OAuth)
```

---

## 👥 Roles & What They Can Do

### 🧑‍🎓 Student
| Feature | Details |
|---|---|
| Browse & Enroll | Free enrollment in any approved course |
| Watch Lessons | YouTube-embedded video lessons with progress tracking |
| Take Quizzes | Auto-scored MCQ/True-False/Short Answer quizzes |
| Take Exams | Part A (MCQ, auto-scored) + Part B (descriptive, instructor-graded) |
| Certificates | Download PDF certificate after paying ₹800 |
| Re-Exam | Pay ₹200 to retake a failed exam |
| PYQ | View Previous Year Questions for enrolled courses |
| Brain Games | Memory, Math, Logic, Pattern, Number Recall games |
| Puzzle Games | Sudoku, Word Puzzle, Logic Puzzle |
| Achievements | Earn badges and see activity feed |
| Export Progress | Download learning history as CSV |
| Doubt Chat | Ask questions to instructors |

### 👨‍🏫 Instructor
| Feature | Details |
|---|---|
| Courses | Create, edit, manage courses with lessons |
| Lessons | Add video URL, PDF materials, lesson script |
| Quizzes | Create quizzes with custom point scoring |
| Exams | Create Part A (MCQ) + Part B (descriptive) exams |
| Schedule Exams | Assign exam dates per student |
| Grade Part B | Manually grade descriptive answers |
| Internal Marks | Assign internal assessment marks |
| Analytics | View student performance and enrollment stats |
| Doubt Chat | Reply to student queries |

### 🛡️ Admin
| Feature | Details |
|---|---|
| Approve Courses | Approve or reject instructor-submitted courses |
| Approve Exams | Approve or reject exams before they go live |
| Manage Users | Activate/deactivate student and instructor accounts |
| Categories | Add/edit course categories |
| Announcements | Post platform-wide announcements |
| Dashboard | View total users, courses, enrollments, stats |

---

## 💰 Payment Flow

```
Enroll in Course → FREE
        ↓
Complete Course (100% lessons done)
        ↓
Pay ₹800 (Certificate + Exam Access)
  ├── ₹500 → Certificate fee
  └── ₹300 → Exam access fee
        ↓
Take Exam
  ├── PASS → Download Exam Certificate (PDF)
  └── FAIL → Pay ₹200 Re-Exam fee → Retake
```

---

## 📝 Exam Flow

```
Student enrolled + Course 100% complete + ₹800 paid
        ↓
Instructor creates exam & assigns date to student
        ↓
Student: Pre-Exam Verification (name + enrollment no. + math CAPTCHA)
        ↓
Part A — MCQ (time-limited, auto-scored: +2 correct, 0 wrong)
        ↓
Part B — Descriptive (instructor grades manually)
        ↓
Instructor assigns internal marks → Result published
        ↓
PASS → Download Exam Certificate PDF
FAIL → Request Re-Exam (₹200)
```

---

## 🗄️ Database Overview

**Host:** `db47791.public.databaseasp.net` | **DB:** `db47791`

**51 tables total.** Key ones:

| Group | Tables |
|---|---|
| Users | AspNetUsers, AspNetRoles, UserProfiles, LoginEntries, RegistrationEntries |
| Courses | Courses, Lessons, Categories, Enrollments, LessonProgresses, Reviews |
| Quizzes | Quizzes, Questions, Answers, QuizAttempts, StudentAnswers |
| Exams | Exams, ExamQuestions, ExamQuestionOptions, ExamAttempts, ExamAnswers, ExamSchedules, ExamVerifications, ExamCertificates |
| Payments | CertificatePayments, CertificatePaymentReceipts, ReExamPayments, PaymentReceipts, PaymentTransactions |
| Certificates | Certificates, ExamCertificates |
| PYQ | PYQs, PYQQuestions, PYQQuestionOptions |
| Games | Games, GameScores, PuzzleGames, PuzzleAttempts, PuzzleMoves, PuzzleLeaderboards |
| Other | Achievements, StudentActivityFeeds, DoubtChats, DoubtMessages, Announcements, MissedExamRequests |

---

## 🔑 Default Login Credentials

| Role | Email | Password |
|---|---|---|
| Admin | admineasylearn@gmail.com | parth123 |
| Instructor | instructor@easylearn.com | Instructor123! |
| Student | student@easylearn.com | Student123! |

---

## 🚀 Local Setup

```bash
# 1. Restore packages
dotnet restore

# 2. Set local DB in appsettings.json
# "Server=(localdb)\\mssqllocaldb;Database=EasyLearnDb;Trusted_Connection=true"

# 3. Apply migrations
dotnet ef database update --context ApplicationDbContext

# 4. Run
dotnet run
```

App runs at `https://localhost:5001`

---

## 📦 Deploy to Hosting (runasp.net)

```cmd
rmdir /s /q publish_output2
dotnet publish -c Release -r win-x86 --self-contained true -o ./publish_output2
"C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe" ^
  -verb:sync ^
  -source:contentPath=".\publish_output2" ^
  -dest:contentPath="site63336",computerName="https://site63336.siteasp.net:8172/msdeploy.axd?site=site63336",userName="site63336",password="<password>",authtype="Basic" ^
  -allowUntrusted -enableRule:DoNotDeleteRule -enableRule:AppOffline
```

> Published as **self-contained win-x86** to match the 32-bit IIS app pool on the host.

---

## 📦 NuGet Packages

| Package | Version | Use |
|---|---|---|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.0 | Identity + EF Core |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.0 | SQL Server ORM |
| Microsoft.EntityFrameworkCore.Tools | 9.0.0 | Migrations CLI |
| Microsoft.AspNetCore.Authentication.Google | 9.0.0 | Google OAuth |
| Microsoft.AspNetCore.Authentication.Facebook | 9.0.0 | Facebook OAuth |
| AspNet.Security.OAuth.GitHub | 9.0.0 | GitHub OAuth |
| MailKit | 4.15.1 | Email via Gmail SMTP |
| iTextSharp.LGPLv2.Core | 1.7.1 | PDF certificates |
| OpenAI | 2.1.0 | AI assistant |
| ZXing.Net | 0.16.9 | QR code on certificates |
| System.Drawing.Common | 9.0.0 | Image processing |

---

## 🌐 Key Routes

| Route | Who | Description |
|---|---|---|
| `/` | Public | Landing page |
| `/account/login` | Public | Login |
| `/account/register` | Public | Register |
| `/verification/verify-certificate` | Public | Verify certificate by number |
| `/student/dashboard` | Student | Student home |
| `/student/browse-courses` | Student | Browse all courses |
| `/student/my-courses` | Student | My enrolled courses |
| `/student/enrolldirect?courseid={id}` | Student | Enroll in course |
| `/student/my-exams` | Student | My scheduled exams |
| `/student/certificate-payment/{courseId}` | Student | Pay ₹800 |
| `/student/pyq` | Student | Previous Year Questions |
| `/instructor/dashboard` | Instructor | Instructor home |
| `/instructor/create-course` | Instructor | Create new course |
| `/instructor/manage-exams` | Instructor | Manage exams |
| `/instructor/grade-part-b/{examId}` | Instructor | Grade Part B |
| `/admin/index` | Admin | Admin dashboard |
| `/admin/approve-courses` | Admin | Approve courses |
| `/admin/manage-users` | Admin | Manage users |
| `/braingames` | Student | Brain games hub |
| `/puzzle` | Student | Puzzle games hub |

---

