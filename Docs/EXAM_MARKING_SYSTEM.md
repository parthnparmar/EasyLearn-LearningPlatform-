# Fixed Exam Marking System

## Overview
The exam marking system has been updated with a fixed marking scheme to ensure consistency and fairness.

## Marking Structure

### Part A: Multiple Choice Questions (MCQs)
- **Total Questions**: 25 MCQs
- **Marks per Question**: 2 marks
- **Total Marks**: 50 marks (25 × 2)
- **Grading**: Automatic (system-checked)
- **Marking Scheme**:
  - Correct Answer: +2 marks
  - Wrong Answer: 0 marks (no negative marking)
  - Unattempted: 0 marks

### Part B: Theory Questions
- **Total Marks**: 30 marks
- **Grading**: Manual (instructor-graded)
- **Question Types**: Written/descriptive answers
- **Instructor Responsibility**: Grade each theory answer individually

### Internal Assessment
- **Total Marks**: 20 marks
- **Grading**: Manual (instructor-assigned)
- **Purpose**: Continuous assessment, attendance, assignments, etc.

### Total Marks
- **Part A (MCQ)**: 50 marks
- **Part B (Theory)**: 30 marks
- **Internal**: 20 marks
- **Grand Total**: 100 marks

### Passing Criteria
- **Minimum Percentage**: 70%
- **Minimum Marks**: 70 out of 100

## Student Answer Isolation

### Security Features
1. **Per-Student Storage**: Each student's answers are stored with their unique `ExamAttemptId`
2. **No Cross-Visibility**: Students cannot see other students' answers
3. **Database Isolation**: Answers are filtered by `ExamAttemptId` ensuring complete separation
4. **Automatic Scoring**: Part A (MCQ) is automatically scored - no manual intervention possible
5. **Manual Grading**: Part B and Internal marks are graded by instructor only

### How It Works
```
Student A submits exam → ExamAttemptId = 1 → Answers stored with AttemptId 1
Student B submits exam → ExamAttemptId = 2 → Answers stored with AttemptId 2

When retrieving answers:
- Student A sees only answers where ExamAttemptId = 1
- Student B sees only answers where ExamAttemptId = 2
```

## Instructor Grading Process

### Step 1: Part A (Automatic)
- System automatically grades all MCQs
- Each correct answer = 2 marks
- Total Part A score calculated instantly
- **Instructor cannot modify Part A scores** (automatic grading ensures fairness)

### Step 2: Part B (Manual Grading)
1. Instructor navigates to "Internal Assessments"
2. Selects student's exam attempt
3. Reviews each theory answer
4. Assigns marks for each question (max 30 marks total)
5. System calculates Part B total

### Step 3: Internal Marks (Manual Assignment)
1. Instructor assigns internal marks (max 20 marks)
2. Based on attendance, assignments, class participation, etc.

### Step 4: Final Calculation
```
Total Score = Part A (auto) + Part B (manual) + Internal (manual)
Percentage = (Total Score / 100) × 100
Pass/Fail = Percentage >= 70%
```

## Key Changes Made

### 1. Fixed Marking Scheme
- **Before**: Variable points per question
- **After**: Fixed 2 marks per MCQ (25 questions = 50 marks)

### 2. Answer Isolation
- **Before**: Potential for answer visibility issues
- **After**: Complete isolation using `ExamAttemptId` as unique identifier

### 3. Automatic MCQ Scoring
- **Before**: Manual intervention possible
- **After**: Fully automatic, tamper-proof MCQ scoring

### 4. Clear Grading Workflow
- **Before**: Unclear grading process
- **After**: Step-by-step grading with clear responsibilities

## Database Schema

### ExamAttempt Table
```
- Id (Primary Key)
- ExamId
- StudentId
- PartAScore (0-50, automatic)
- PartBScore (0-30, manual)
- InternalScore (0-20, manual)
- TotalScore (0-100, calculated)
- Percentage (calculated)
- IsPassed (calculated)
```

### ExamAnswer Table
```
- Id (Primary Key)
- ExamAttemptId (Foreign Key - ensures isolation)
- ExamQuestionId
- SelectedOptionId (for MCQs)
- AnswerText (for theory)
- IsCorrect (for MCQs)
- Points (marks awarded)
```

## Benefits

1. **Fairness**: Fixed marking scheme ensures all students are evaluated equally
2. **Security**: Complete answer isolation prevents cheating
3. **Transparency**: Clear marking breakdown (Part A + Part B + Internal)
4. **Efficiency**: Automatic MCQ grading saves instructor time
5. **Accuracy**: No manual errors in MCQ scoring
6. **Flexibility**: Instructor can grade theory questions based on quality

## Student View

Students will see:
- Part A Score: XX/50 (automatic, immediate after submission)
- Part B Score: XX/30 (after instructor grading)
- Internal Score: XX/20 (after instructor assignment)
- Total Score: XX/100
- Percentage: XX%
- Result: PASS/FAIL

## Instructor View

Instructors will see:
- List of pending assessments (Part B + Internal)
- Student-by-student grading interface
- Part A scores (read-only, automatic)
- Part B answer review and grading
- Internal marks assignment
- Final result publication

---

**Last Updated**: 2024
**System Version**: EasyLearn v1.0
**Marking Scheme**: Fixed (25 MCQs × 2 marks + 30 theory + 20 internal = 100 total)
