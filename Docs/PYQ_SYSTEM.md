# PYQ (Previous Year Questions) System

## Overview
Automatic system that converts expired exam papers into PYQ for student practice.

## Features

### 1. Automatic Conversion
- **Trigger**: Exam date expires (ScheduledEndTime < current time)
- **Frequency**: Checks every 1 hour
- **Process**: Copies exam questions and options to PYQ tables

### 2. Student Access
- **Route**: `/student/pyq`
- **Access**: Only enrolled students can view PYQ for their courses
- **View**: Complete question paper with correct answers highlighted

### 3. Question Display
- **Part A (MCQs)**: Shows all options with correct answers marked in green
- **Part B (Theory)**: Shows theory questions for practice
- **Marks**: Displays marks for each question

## How It Works

### Background Service
```
PYQBackgroundService runs every 1 hour:
1. Finds exams where ScheduledEndTime < now
2. Checks if exam already converted to PYQ
3. Creates PYQ record
4. Copies all questions and options
5. Marks as active for student viewing
```

### Database Tables

#### PYQs
- Id
- ExamId (reference to original exam)
- CourseId
- Title (e.g., "Midterm Exam - 2024")
- ExamDate
- AddedToPYQAt
- IsActive

#### PYQQuestions
- Id
- PYQId
- Text
- Type (MCQ/Theory)
- Part (PartA/PartB)
- Points
- OrderIndex

#### PYQQuestionOptions
- Id
- PYQQuestionId
- Text
- IsCorrect
- OrderIndex

## Student Panel Routes

### View PYQ List
**URL**: `/student/pyq`
**Shows**: All PYQ papers from enrolled courses

### View PYQ Paper
**URL**: `/student/pyq/{pyqId}`
**Shows**: Complete question paper with answers

## Benefits

1. **Automatic**: No manual work required
2. **Practice**: Students can practice with real exam papers
3. **Learning**: Correct answers shown for self-assessment
4. **Historical**: Maintains exam history for reference
5. **Secure**: Only enrolled students can access

## Setup Instructions

1. Run migration:
```bash
dotnet ef migrations add AddPYQSystem
dotnet ef database update
```

2. Service automatically starts with application

3. Access from student panel: Dashboard → PYQ

## Technical Details

- **Background Service**: Runs continuously, checks every hour
- **Isolation**: Each student sees only PYQ from enrolled courses
- **Performance**: Efficient queries with proper indexing
- **Storage**: Separate tables to avoid affecting live exams

---

**Auto-Conversion**: Exams automatically become PYQ after expiry ✅
**Student Access**: View all PYQ papers from enrolled courses ✅
**Answer Display**: MCQ correct answers highlighted in green ✅
