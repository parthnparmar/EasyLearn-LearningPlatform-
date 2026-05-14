-- Recalculate All Exam Scores with Custom Points
-- This script fixes all existing exam attempts to use correct custom points

-- Step 1: Update ExamAnswer points to match question points for correct answers
UPDATE ea
SET ea.Points = eq.Points
FROM ExamAnswers ea
INNER JOIN ExamQuestions eq ON ea.ExamQuestionId = eq.Id
WHERE ea.IsCorrect = 1 AND eq.Part = 1; -- Part A (MCQ) correct answers

-- Step 2: Set wrong answers to 0 points
UPDATE ea
SET ea.Points = 0
FROM ExamAnswers ea
INNER JOIN ExamQuestions eq ON ea.ExamQuestionId = eq.Id
WHERE ea.IsCorrect = 0 AND eq.Part = 1; -- Part A (MCQ) wrong answers

-- Step 3: Recalculate Part A scores for all attempts
UPDATE attempt
SET attempt.PartAScore = partATotal.TotalScore
FROM ExamAttempts attempt
INNER JOIN (
    SELECT 
        ea.ExamAttemptId,
        SUM(ea.Points) as TotalScore
    FROM ExamAnswers ea
    INNER JOIN ExamQuestions eq ON ea.ExamQuestionId = eq.Id
    WHERE eq.Part = 1 -- Part A only
    GROUP BY ea.ExamAttemptId
) partATotal ON attempt.Id = partATotal.ExamAttemptId;

-- Step 4: Recalculate total scores and percentages
UPDATE attempt
SET 
    attempt.TotalScore = attempt.PartAScore + attempt.PartBScore + attempt.InternalScore,
    attempt.Percentage = CAST((attempt.PartAScore + attempt.PartBScore + attempt.InternalScore) AS FLOAT) / attempt_exam.TotalMarks * 100,
    attempt.IsPassed = CASE 
        WHEN CAST((attempt.PartAScore + attempt.PartBScore + attempt.InternalScore) AS FLOAT) / attempt_exam.TotalMarks * 100 >= attempt_exam.PassingPercentage 
        THEN 1 
        ELSE 0 
    END
FROM ExamAttempts attempt
INNER JOIN Exams attempt_exam ON attempt.ExamId = attempt_exam.Id
WHERE attempt.PartACompleted = 1;

-- Verification Query: Check results
SELECT 
    c.Title as CourseName,
    e.Title as ExamName,
    u.FirstName + ' ' + u.LastName as StudentName,
    ea.PartAScore,
    ea.PartBScore,
    ea.InternalScore,
    ea.TotalScore,
    ea.Percentage,
    ea.IsPassed,
    -- Show question breakdown
    (SELECT COUNT(*) FROM ExamAnswers ans 
     INNER JOIN ExamQuestions q ON ans.ExamQuestionId = q.Id 
     WHERE ans.ExamAttemptId = ea.Id AND q.Part = 1 AND ans.IsCorrect = 1) as CorrectMCQs,
    (SELECT SUM(q.Points) FROM ExamAnswers ans 
     INNER JOIN ExamQuestions q ON ans.ExamQuestionId = q.Id 
     WHERE ans.ExamAttemptId = ea.Id AND q.Part = 1 AND ans.IsCorrect = 1) as EarnedMCQPoints
FROM ExamAttempts ea
INNER JOIN Exams e ON ea.ExamId = e.Id
INNER JOIN Courses c ON e.CourseId = c.Id
INNER JOIN AspNetUsers u ON ea.StudentId = u.Id
WHERE ea.PartACompleted = 1
ORDER BY c.Title, e.Title, u.FirstName;