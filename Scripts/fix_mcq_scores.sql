-- Fix MCQ Scores for Existing Exam Attempts
-- This script recalculates Part A scores based on correct/incorrect answers

UPDATE ea
SET 
    ea.PartAScore = (
        SELECT ISNULL(SUM(
            CASE 
                WHEN ans.SelectedOptionId IS NOT NULL AND opt.IsCorrect = 1 THEN 2  -- Correct answer: +2
                WHEN ans.SelectedOptionId IS NOT NULL AND opt.IsCorrect = 0 THEN -1 -- Wrong answer: -1
                ELSE 0  -- Unattempted: 0
            END
        ), 0)
        FROM ExamAnswers ans
        INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id
        LEFT JOIN ExamQuestionOptions opt ON ans.SelectedOptionId = opt.Id
        WHERE ans.ExamAttemptId = ea.Id 
        AND eq.Part = 1  -- Part A (MCQ)
    )
FROM ExamAttempts ea
WHERE ea.PartACompleted = 1
AND ea.PartAScore = 0;

-- Also update individual answer points and correctness
UPDATE ans
SET 
    ans.Points = CASE 
        WHEN ans.SelectedOptionId IS NOT NULL AND opt.IsCorrect = 1 THEN 2
        WHEN ans.SelectedOptionId IS NOT NULL AND opt.IsCorrect = 0 THEN -1
        ELSE 0
    END,
    ans.IsCorrect = CASE 
        WHEN ans.SelectedOptionId IS NOT NULL AND opt.IsCorrect = 1 THEN 1
        ELSE 0
    END
FROM ExamAnswers ans
INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id
LEFT JOIN ExamQuestionOptions opt ON ans.SelectedOptionId = opt.Id
WHERE eq.Part = 1  -- Part A (MCQ)
AND ans.Points = 0;

-- Ensure no negative total scores
UPDATE ExamAttempts 
SET PartAScore = 0 
WHERE PartAScore < 0;

-- Show results
SELECT 
    ea.Id as AttemptId,
    u.FirstName + ' ' + u.LastName as StudentName,
    e.Title as ExamTitle,
    ea.PartAScore,
    ea.PartACompleted,
    (SELECT COUNT(*) FROM ExamAnswers ans 
     INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id 
     WHERE ans.ExamAttemptId = ea.Id AND eq.Part = 1) as MCQAnswerCount
FROM ExamAttempts ea
INNER JOIN Exams e ON ea.ExamId = e.Id
INNER JOIN AspNetUsers u ON ea.StudentId = u.Id
WHERE ea.PartACompleted = 1
ORDER BY ea.Id;