# EasyLearn - Comprehensive Online Learning Platform

A feature-rich online learning platform built with ASP.NET Core 9.0, featuring advanced payment systems, certificate generation, AI assistance, brain games, and comprehensive role-based access control.

## 🚀 Core Features

### 🎯 Multi-Role System
- **Admin**: Platform management, user oversight, course approval, exam management
- **Instructor**: Course creation, content management, student analytics, exam scheduling
- **Student**: Course enrollment, learning progress, certificate acquisition, brain games

### 💳 Advanced Payment System
- **Multiple Payment Methods**: UPI, Credit/Debit Cards, QR Codes, Digital Wallets (Google Pay, PhonePe, Paytm)
- **Certificate Payment System**: Separate payment for exam access and certificate generation
- **Payment Receipts**: Automated PDF receipt generation with detailed transaction information
- **Payment Verification**: Secure transaction verification and status tracking
- **Refund Management**: Automated refund processing capabilities

### 📜 Certificate Management
- **Digital Certificates**: Professional PDF certificate generation with security features
- **Certificate Validation**: QR code-based certificate verification system
- **Certificate Payments**: Dedicated payment system for certificate access (₹500 certificate fee + ₹300 exam fee)
- **Validity Tracking**: Certificate expiration and renewal management (1-year validity)
- **Verification Portal**: Public certificate verification interface

### 🎓 Learning Management
- **Course Creation**: Rich course creation with multimedia support
- **Lesson Management**: Video lessons with YouTube integration
- **Progress Tracking**: Detailed learning progress with visual indicators
- **Quiz System**: Comprehensive assessment with multiple question types
- **Material Downloads**: Course material management and downloads
- **MCQ Marking Scheme**: +2 points for correct answers, 0 points for wrong/unattempted (no negative marking)
- **Flexible Exam Patterns**: Theory Only, MCQ Only, or Mixed exams

### 🧠 Brain Games & Puzzles
- **Math Games**: Number sequence, arithmetic challenges
- **Logic Games**: Pattern matching, logical reasoning
- **Memory Games**: Memory enhancement exercises
- **Word Games**: Vocabulary and word puzzles
- **Sudoku**: Classic Sudoku puzzles with difficulty levels
- **Achievement System**: Points and badges for game completion

### 📊 Advanced Exam System
- **Scheduled Exams**: Time-bound exam scheduling
- **Exam Verification**: Pre-exam identity verification
- **Mixed Question Types**: MCQ and theory questions
- **Auto-grading**: Automatic MCQ scoring
- **Manual Grading**: Instructor grading for theory questions
- **Re-exam System**: Paid re-examination facility

## 🛠 Technology Stack

### Backend
- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity with external providers
- **PDF Generation**: iTextSharp for certificates and receipts
- **QR Codes**: ZXing.Net for QR code generation
- **AI Integration**: OpenAI API integration

### Frontend
- **UI Framework**: Bootstrap 5 with custom CSS
- **Icons**: Font Awesome
- **JavaScript**: jQuery with custom modules
- **Responsive Design**: Mobile-first approach

### External Integrations
- **Social Login**: Google, Facebook, GitHub authentication
- **Payment Processing**: Multiple payment gateway support
- **Video Hosting**: YouTube integration for video lessons
- **AI Assistant**: OpenAI integration for learning assistance

## 📋 Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code
- Git for version control

## 🔧 Installation & Setup

### 1. Clone Repository
```bash
git clone <repository-url>
cd EasyLearn
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Configure Database
Update `appsettings.json` with your database connection:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EasyLearnDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4. Configure External Services (Optional)
Copy `appsettings.Example.json` to `appsettings.json` and configure:
```json
{
  "Authentication": {
    "Google": {
      "ClientId": "your-google-client-id",
      "ClientSecret": "your-google-client-secret"
    },
    "Facebook": {
      "AppId": "your-facebook-app-id",
      "AppSecret": "your-facebook-app-secret"
    },
    "GitHub": {
      "ClientId": "your-github-client-id",
      "ClientSecret": "your-github-client-secret"
    }
  },
  "OpenAI": {
    "ApiKey": "your-openai-api-key"
  },
  "BaseUrl": "https://localhost:5001"
}
```

### 5. Run Database Migrations
```bash
dotnet ef database update
```

### 6. Start Application
```bash
dotnet run
```

### 7. Access Application
Navigate to `https://localhost:5001`

## 👥 Default User Accounts

### Admin Account
- **Email**: admin@easylearn.com
- **Password**: Admin123!
- **Capabilities**: Full platform administration

### Instructor Account
- **Email**: instructor@easylearn.com
- **Password**: Instructor123!
- **Capabilities**: Course creation and management

### Student Account
- **Email**: student@easylearn.com
- **Password**: Student123!
- **Capabilities**: Course enrollment and learning

## 📊 Database Architecture

### Core Entities
- **ApplicationUser**: Extended Identity user with role management
- **Course**: Course information with instructor and category relationships
- **Lesson**: Individual lessons with video and material links
- **Quiz/Question/Answer**: Comprehensive assessment system
- **Enrollment**: Student-course relationships with progress tracking
- **Certificate**: Digital certificate management with validation
- **PaymentTransaction**: Complete payment transaction tracking
- **CertificatePayment**: Specialized certificate payment system
- **PaymentReceipt**: Automated receipt generation and storage

### Exam System Entities
- **Exam**: Exam configuration and scheduling
- **ExamVerification**: Pre-exam verification records
- **Question**: Exam questions with multiple types
- **StudentAnswer**: Student responses and scoring
- **ExamAttempt**: Complete exam attempt tracking

### Gaming System Entities
- **Game**: Brain game definitions
- **PuzzleGame**: Puzzle-specific game data
- **Achievement**: User achievement tracking
- **GameScore**: Game performance records

### Progress Tracking
- **LessonProgress**: Individual lesson completion tracking
- **QuizAttempt**: Quiz attempt records with scoring
- **Review**: Course rating and review system

## 🔐 Security Features

### Authentication & Authorization
- **Role-based Access Control**: Granular permissions per user type
- **External Authentication**: Google, Facebook, GitHub integration
- **Password Policies**: Configurable password requirements
- **Account Management**: Admin-controlled account activation/deactivation

### Payment Security
- **Transaction Verification**: Multi-layer payment verification
- **Secure Receipt Generation**: Tamper-proof PDF receipts
- **Payment Method Validation**: Comprehensive payment method support
- **Refund Protection**: Secure refund processing

### Certificate Security
- **Digital Signatures**: Cryptographic certificate validation
- **QR Code Verification**: Secure certificate verification system
- **Expiration Management**: Automatic certificate validity tracking
- **Anti-fraud Measures**: Multiple verification layers

## 🌐 API Endpoints

### Authentication Routes
```
GET/POST /Account/Login - User authentication
GET/POST /Account/Register - User registration
GET/POST /Account/ForgotPassword - Password reset
POST /Account/Logout - User logout
```

### Admin Routes (Admin Role Required)
```
GET /Admin - Dashboard with analytics
GET /Admin/ManageUsers - User management interface
GET /Admin/ApproveCourses - Course approval system
GET /Admin/ApproveExams - Exam approval system
GET /Admin/ManageCategories - Category management
GET /Admin/ManageAnnouncements - Platform announcements
```

### Instructor Routes (Instructor Role Required)
```
GET /Instructor - Instructor dashboard
GET /Instructor/MyCourses - Course management
GET/POST /Instructor/CreateCourse - Course creation
GET /Instructor/ManageLessons/{courseId} - Lesson management
GET /Instructor/ManageExams - Exam management
GET /Instructor/CreateExam - Exam creation
GET /Instructor/AssignExamDates - Exam scheduling
GET /Instructor/ExamResults/{examId} - Exam results
GET /Instructor/GradePartB/{attemptId} - Manual grading
```

### Student Routes (Student Role Required)
```
GET /Student - Student dashboard
GET /Student/BrowseCourses - Course catalog
GET /Student/CourseDetails/{id} - Course information
POST /Student/EnrollCourse - Course enrollment
GET /Student/MyCourses - Enrolled courses
GET /Student/WatchLesson/{lessonId} - Video learning
GET /Student/TakeExam/{examId} - Exam interface
GET /Student/ExamResults - Exam results
GET /Student/CertificatePayment/{courseId} - Certificate payment
GET /Student/DownloadCertificate/{courseId} - Certificate download
GET /Student/ApprovedExams - Available exams
```

### Brain Games Routes
```
GET /BrainGames - Games dashboard
GET /BrainGames/Math - Math games
GET /BrainGames/Logic - Logic games
GET /BrainGames/Memory - Memory games
GET /BrainGames/Word - Word games
GET /BrainGames/Sudoku - Sudoku puzzles
POST /BrainGames/SubmitScore - Score submission
```

### Payment Routes
```
POST /Student/ProcessPayment - Course payment processing
GET /Student/PaymentConfirmation - Payment confirmation
POST /Student/ProcessCertificatePayment - Certificate payment
GET /Student/CertificatePaymentConfirmation - Certificate payment confirmation
POST /Student/ReExamPayment - Re-exam payment
```

### Verification Routes
```
GET /Verification/VerifyCertificate - Public certificate verification
```

## 💰 Payment System Details

### Supported Payment Methods
- **UPI**: Direct UPI payments with QR code support
- **Credit/Debit Cards**: Secure card processing
- **Digital Wallets**: Google Pay, PhonePe, Paytm integration
- **QR Code Payments**: Dynamic QR code generation

### Payment Processing Flow
1. **Course Payment**: Standard course enrollment payment
2. **Certificate Payment**: Separate payment for certificate access
3. **Re-exam Payment**: Additional payment for re-examination
4. **Payment Verification**: Multi-step verification process
5. **Receipt Generation**: Automatic PDF receipt creation
6. **Transaction Tracking**: Complete payment audit trail

### Certificate Payment System
- **Certificate Fee**: ₹500 for certificate generation
- **Exam Fee**: ₹300 for exam access
- **Total Amount**: ₹800 for complete certificate package
- **Re-exam Fee**: ₹200 for additional attempts

## 📜 Certificate System

### Certificate Features
- **Professional Design**: High-quality PDF certificates with security features
- **Digital Verification**: QR code-based verification system
- **Validity Period**: 1-year certificate validity
- **Security Elements**: Watermarks, digital signatures, verification URLs
- **Instructor Signatures**: Digital instructor and platform signatures

### Certificate Generation Process
1. **Course Completion**: 100% course completion required
2. **Exam Passing**: Minimum passing score achievement
3. **Payment Processing**: Certificate payment completion
4. **Certificate Generation**: Automated PDF generation
5. **Verification Setup**: QR code and verification URL creation
6. **Download Access**: Secure certificate download

## 🧠 Brain Games System

### Available Games
- **Number Sequence**: Pattern recognition with numbers
- **Pattern Matching**: Visual pattern identification
- **Word Puzzles**: Vocabulary and word formation
- **Sudoku**: Classic 9x9 Sudoku puzzles
- **Memory Games**: Memory enhancement exercises
- **Math Challenges**: Arithmetic and calculation games

### Achievement System
- **Points System**: Earn points for game completion
- **Badges**: Achievement badges for milestones
- **Leaderboards**: Competition with other users
- **Progress Tracking**: Game performance analytics

## 📱 User Interface Features

### Modern Design System
- **Responsive Layout**: Mobile-first responsive design
- **Dark/Light Themes**: Multiple theme options
- **Accessibility**: WCAG compliant interface
- **Interactive Elements**: Rich user interactions

### Dashboard Features
- **Admin Dashboard**: Platform analytics and management tools
- **Instructor Dashboard**: Course performance and earnings tracking
- **Student Dashboard**: Learning progress and achievement tracking

### Course Interface
- **Course Catalog**: Advanced search and filtering
- **Course Details**: Rich course information display
- **Video Player**: Integrated video learning interface
- **Progress Tracking**: Visual progress indicators

## 🔧 Services Architecture

### Business Services
- **CertificateService**: Certificate generation and management
- **ProgressService**: Learning progress tracking
- **PaymentService**: Payment processing and verification
- **CertificatePaymentService**: Certificate-specific payments
- **QRCodeService**: QR code generation and management
- **ProfileService**: User profile management
- **ReceiptService**: Receipt generation and management
- **ExamService**: Exam management and scoring
- **AchievementService**: Achievement tracking
- **PuzzleService**: Brain games management
- **CaptchaService**: Security verification

## 📊 Project Structure

```
EasyLearn/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── InstructorController.cs
│   ├── StudentController.cs
│   ├── BrainGamesController.cs
│   ├── PuzzleController.cs
│   └── VerificationController.cs
├── Models/              # Data models and entities
│   ├── ApplicationUser.cs
│   ├── Course.cs
│   ├── Exam.cs
│   ├── Game.cs
│   ├── Achievement.cs
│   └── ViewModels.cs
├── Services/            # Business logic services
│   ├── CertificateService.cs
│   ├── PaymentService.cs
│   ├── ExamService.cs
│   ├── AchievementService.cs
│   └── PuzzleService.cs
├── Views/               # Razor views and UI
│   ├── Admin/
│   ├── Instructor/
│   ├── Student/
│   ├── BrainGames/
│   └── Shared/
├── wwwroot/            # Static files
│   ├── css/
│   ├── js/
│   ├── images/
│   ├── certificates/
│   └── uploads/
├── Data/               # Database initialization
├── Migrations/         # Entity Framework migrations
└── Properties/         # Application properties
```

## 🚀 Deployment

### Production Configuration
1. **Database Setup**: Configure production SQL Server
2. **Authentication**: Set up external authentication providers
3. **SSL Configuration**: Enable HTTPS and security headers
4. **File Storage**: Configure file upload storage
5. **Email Service**: Set up email notifications
6. **Payment Gateway**: Configure production payment settings

### Environment Variables
```json
{
  "ConnectionStrings:DefaultConnection": "Production database connection",
  "Authentication:Google:ClientId": "Google OAuth client ID",
  "Authentication:Google:ClientSecret": "Google OAuth client secret",
  "Authentication:Facebook:AppId": "Facebook app ID",
  "Authentication:Facebook:AppSecret": "Facebook app secret",
  "Authentication:GitHub:ClientId": "GitHub OAuth client ID",
  "Authentication:GitHub:ClientSecret": "GitHub OAuth client secret",
  "OpenAI:ApiKey": "OpenAI API key",
  "BaseUrl": "Production base URL"
}
```

### Docker Support
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["EasyLearn.csproj", "."]
RUN dotnet restore "EasyLearn.csproj"
COPY . .
RUN dotnet build "EasyLearn.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EasyLearn.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EasyLearn.dll"]
```

## 🔄 Future Enhancements

### Planned Features
- **Advanced Analytics**: Detailed learning analytics and reporting
- **Mobile Application**: Native mobile app development
- **Live Sessions**: Video conferencing integration
- **Discussion Forums**: Course-specific discussion boards
- **Advanced Assessments**: Timed quizzes, question banks, randomization
- **Bulk Operations**: Bulk user and course management
- **API Documentation**: Comprehensive API documentation
- **Caching System**: Redis caching for performance
- **Email Notifications**: Comprehensive email system
- **Advanced Payment**: Subscription models and installment payments

### Technical Improvements
- **Performance Optimization**: Database and query optimization
- **Security Enhancements**: Advanced security measures
- **Scalability**: Microservices architecture
- **Monitoring**: Application performance monitoring
- **Testing**: Comprehensive test coverage

## 📊 Key Statistics

### Platform Capabilities
- **Multi-role Support**: 3 distinct user roles
- **Payment Methods**: 7+ payment method integrations
- **Certificate Security**: Multiple security layers
- **Database Entities**: 25+ core entities
- **API Endpoints**: 60+ REST endpoints
- **Brain Games**: 6+ different game types
- **File Types**: Support for multiple file formats
- **Responsive Breakpoints**: 5+ responsive breakpoints

## 🤝 Contributing

### Development Guidelines
1. Fork the repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

### Code Standards
- Follow C# coding conventions
- Maintain test coverage
- Document new features
- Follow security best practices

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support & Documentation

### Getting Help
- Create GitHub issues for bugs
- Check documentation wiki
- Contact development team
- Review code examples

### Additional Resources
- [Brain Games Summary](BRAIN_GAMES_SUMMARY.md)
- [Exam Pattern Guide](EXAM_PATTERN_GUIDE.md)
- [Scheduled Exam System](SCHEDULED_EXAM_SYSTEM.md)
- [System Diagrams](SYSTEM_DIAGRAMS.md)
- [Fixes Applied](FIXES_APPLIED.md)

---

**EasyLearn** - Transforming online education with comprehensive learning management, secure payments, professional certification, and engaging brain games. 🎓🧠✨

Built with ❤️ using ASP.NET Core 9.0, Entity Framework Core, and modern web technologies.