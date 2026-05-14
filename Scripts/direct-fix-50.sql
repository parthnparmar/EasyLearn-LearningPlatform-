-- DIRECT FIX: Set all Part A scores to 50 for students with all correct answers
-- This bypasses the custom points calculation and directly fixes the scores

-- First, let's see the current state
SELECT 
    ea.Id as AttemptId,
    ea.PartAScore as CurrentScore,
    COUNT(ans.Id) as TotalAnswers,
    SUM(CASE WHEN ans.IsCorrect = 1 THEN 1 ELSE 0 END) as CorrectAnswers
FROM ExamAttempts ea
INNER JOIN ExamAnswers ans ON ea.Id = ans.ExamAttemptId
INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id
WHERE eq.Part = 1 AND ea.PartACompleted = 1
GROUP BY ea.Id, ea.PartAScore;

-- IMMEDIATE FIX: Set Part A score to 50 for students who got all 13 questions correct
UPDATE ExamAttempts 
SET PartAScore = 50
WHERE Id IN (
    SELECT ea.Id
    FROM ExamAttempts ea
    INNER JOIN ExamAnswers ans ON ea.Id = ans.ExamAttemptId
    INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id
    WHERE eq.Part = 1 AND ea.PartACompleted = 1 AND ans.IsCorrect = 1
    GROUP BY ea.Id
    HAVING COUNT(ans.Id) = 13  -- All 13 questions correct
);

-- Update total scores
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

-- Verify the fix
SELECT 
    'AFTER FIX' as Status,
    ea.PartAScore,
    COUNT(*) as StudentCount
FROM ExamAttempts ea
WHERE ea.PartACompleted = 1
GROUP BY ea.PartAScore
ORDER BY ea.PartAScore;