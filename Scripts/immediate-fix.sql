-- IMMEDIATE FIX: Update all Part A scores to correct values
-- This fixes the 26/50 issue by recalculating based on custom points

-- Update Part A scores for all completed attempts
UPDATE ea
SET ea.PartAScore = (
    SELECT SUM(
        CASE 
            WHEN ans.IsCorrect = 1 THEN eq.Points 
            ELSE 0 
        END
    )
    FROM ExamAnswers ans
    INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id
    WHERE ans.ExamAttemptId = ea.Id 
    AND eq.Part = 1 -- Part A only
)
FROM ExamAttempts ea
WHERE ea.PartACompleted = 1;

-- Update total scores and percentages
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

-- Verify results
SELECT 
    'After Fix' as Status,
    COUNT(*) as TotalAttempts,
    AVG(CAST(PartAScore AS FLOAT)) as AvgPartAScore,
    MAX(PartAScore) as MaxPartAScore,
    MIN(PartAScore) as MinPartAScore
FROM ExamAttempts 
WHERE PartACompleted = 1;