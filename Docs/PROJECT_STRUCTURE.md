# EasyLearn Project Structure

## Root Directory
```
EasyLearn/
├── Controllers/          # MVC Controllers
├── Data/                # Database context and initialization
├── Docs/                # Documentation files
├── Migrations/          # EF Core migrations
├── Models/              # Data models and ViewModels
├── Properties/          # Launch settings
├── Scripts/             # SQL scripts and utilities
├── Services/            # Business logic services
├── Views/               # Razor views
├── wwwroot/             # Static files
├── .gitignore           # Git ignore rules
├── appsettings.json     # Application configuration
├── EasyLearn.csproj     # Project file
└── Program.cs           # Application entry point
```

## Controllers
- **AccountController.cs** - Authentication and user management
- **AdminController.cs** - Admin dashboard and management
- **BrainGamesController.cs** - Brain games functionality
- **GamesController.cs** - Game management
- **HelpController.cs** - Help and support pages
- **HomeController.cs** - Home page and public routes
- **InstructorController.cs** - Instructor features
- **ProfileController.cs** - User profile management
- **PuzzleController.cs** - Puzzle games
- **StudentController.cs** - Student learning interface
- **VerificationController.cs** - Certificate verification

## Models
Core entities for the application including User, Course, Exam, Certificate, Payment, and Game models.

## Services
- **AchievementService.cs** - Achievement tracking
- **BusinessServices.cs** - Core business logic
- **CaptchaService.cs** - Security verification
- **CertificatePaymentService.cs** - Certificate payments
- **ExamService.cs** - Exam management
- **PaymentService.cs** - Payment processing
- **ProfileService.cs** - Profile management
- **PuzzleService.cs** - Puzzle game logic
- **PYQBackgroundService.cs** - Background tasks for PYQ
- **PYQService.cs** - Previous Year Questions
- **QRCodeService.cs** - QR code generation
- **ReceiptService.cs** - Receipt generation

## Views Structure
```
Views/
├── Account/             # Login, Register, Password reset
├── Admin/               # Admin management pages
├── BrainGames/          # Brain games interfaces
├── Games/               # Game pages
├── Help/                # Help documentation
├── Home/                # Public pages
├── Instructor/          # Instructor dashboard and tools
├── Profile/             # User profile pages
├── Puzzle/              # Puzzle game interfaces
├── Shared/              # Shared layouts and partials
├── Student/             # Student learning interface
└── Verification/        # Certificate verification
```

## wwwroot Structure
```
wwwroot/
├── certificates/        # Generated certificates (PDF)
├── css/                 # Stylesheets
├── images/              # Images and avatars
├── js/                  # JavaScript files
├── lib/                 # Third-party libraries
├── uploads/             # User uploaded files
│   ├── materials/       # Course materials
│   └── questions/       # Question images
├── favicon.ico
└── manifest.json
```

## Documentation (Docs/)
- **README.md** - Main project documentation
- **EXAM_MARKING_SYSTEM.md** - Exam marking guidelines
- **IMAGE_MIGRATION.md** - Image migration guide
- **PYQ_MIGRATION.md** - PYQ migration guide
- **PYQ_SYSTEM.md** - PYQ system documentation
- **build_output.txt** - Build logs

## Scripts (Scripts/)
SQL scripts for database maintenance and fixes:
- create_exam_tables.sql
- fix_mcq_scores.sql
- recalculate-scores.sql
- And other utility scripts

## Configuration Files
- **appsettings.json** - Application settings (not in Git)
- **appsettings.Example.json** - Example configuration template
- **.gitignore** - Git ignore rules
- **EasyLearn.csproj** - Project dependencies and settings

## Build Artifacts (Ignored by Git)
- bin/ - Compiled binaries
- obj/ - Build objects
- .vs/ - Visual Studio cache

## Notes
- User uploads and generated certificates are excluded from Git
- Sensitive configuration files are not tracked
- Keep .gitkeep files to preserve directory structure
