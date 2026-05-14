# PYQ System Migration

Run this command to create the database migration for PYQ system:

```bash
dotnet ef migrations add AddPYQSystem
dotnet ef database update
```

This will add the following tables:
- PYQs
- PYQQuestions
- PYQQuestionOptions

The system will automatically convert expired exams to PYQ papers every hour.
