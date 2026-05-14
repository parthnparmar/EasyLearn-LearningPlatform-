# Add Image Support to Exam Questions

Run these commands to add image support:

```bash
dotnet ef migrations add AddImageToExamQuestions
dotnet ef database update
```

This adds:
- ImageUrl field to ExamQuestion table
- ImageUrl field to PYQQuestion table
- Upload endpoint for question images
- Image display in exam interface and PYQ viewer
