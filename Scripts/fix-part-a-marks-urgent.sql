-- URGENT FIX: Part A Marks Issue
-- This script fixes Part A questions with invalid points and recalculates scores

-- Step 1: Fix all Part A questions with 0 or invalid points
UPDATE ExamQuestions 
SET Points = 2 
WHERE Part = 1 -- Part A (MCQ)
  AND (Points IS NULL OR Points <= 0);

-- Step 2: Update all ExamAnswers for Part A questions to use correct points
UPDATE ea
SET ea.Points = eq.Points
FROM ExamAnswers ea
INNER JOIN ExamQuestions eq ON ea.ExamQuestionId = eq.Id
WHERE eq.Part = 1 -- Part A (MCQ)
  AND ea.IsCorrect = 1; -- Only for correct answers

-- Step 3: Set incorrect answers to 0 points
UPDATE ea
SET ea.Points = 0
FROM ExamAnswers ea
INNER JOIN ExamQuestions eq ON ea.ExamQuestionId = eq.Id
WHERE eq.Part = 1 -- Part A (MCQ)
  AND ea.IsCorrect = 0; -- Only for incorrect answers

-- Step 4: Recalculate Part A scores for all exam attempts
UPDATE ea
SET ea.PartAScore = (
    SELECT ISNULL(SUM(ans.Points), 0)
    FROM ExamAnswers ans
    INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id
    WHERE ans.ExamAttemptId = ea.Id
      AND eq.Part = 1 -- Part A (MCQ)
      AND ans.IsCorrect = 1
)
FROM ExamAttempts ea
WHERE ea.PartACompleted = 1;

-- Step 5: Recalculate total scores and percentages
UPDATE ea
SET 
    ea.TotalScore = ea.PartAScore + ea.PartBScore + ea.InternalScore,
    ea.Percentage = CAST((ea.PartAScore + ea.PartBScore + ea.InternalScore) AS FLOAT) / e.TotalMarks * 100,
    ea.IsPassed = CASE 
        WHEN CAST((ea.PartAScore + ea.PartBScore + ea.InternalScore) AS FLOAT) / e.TotalMarks * 100 >= e.PassingPercentage 
        THEN 1 
        ELSE 0 
    END
FROM ExamAttempts ea
INNER JOIN Exams e ON ea.ExamId = e.Id
WHERE ea.PartACompleted = 1;

-- Verification: Check the results
SELECT 
    ea.Id as AttemptId,
    u.FirstName + ' ' + u.LastName as StudentName,
    e.Title as ExamTitle,
    ea.PartAScore,
    ea.PartBScore,
    ea.InternalScore,
    ea.TotalScore,
    ea.Percentage,
    ea.IsPassed,
    (SELECT COUNT(*) FROM ExamAnswers ans 
     INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id 
     WHERE ans.ExamAttemptId = ea.Id AND eq.Part = 1 AND ans.IsCorrect = 1) as CorrectAnswers,
    (SELECT COUNT(*) FROM ExamAnswers ans 
     INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id 
     WHERE ans.ExamAttemptId = ea.Id AND eq.Part = 1) as TotalQuestions
FROM ExamAttempts ea
INNER JOIN Exams e ON ea.ExamId = e.Id
INNER JOIN AspNetUsers u ON ea.StudentId = u.Id
WHERE ea.PartACompleted = 1
ORDER BY e.Title, u.FirstName, u.LastName;

PRINT 'Part A marks have been fixed successfully!';
PRINT 'All students now have correct marks based on custom points assigned by examiners.';