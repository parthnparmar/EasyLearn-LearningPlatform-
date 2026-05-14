-- Check current exam attempts for exam ID 7
SELECT Id, StudentId, ExamId, PartACompleted, PartBCompleted, IsCompleted, StartedAt
FROM ExamAttempts
WHERE ExamId = 7 AND IsCompleted = 0;

-- If you need to reset a specific attempt (replace ATTEMPT_ID with actual ID from above query)
-- UPDATE ExamAttempts 
-- SET PartACompleted = 0, PartBCompleted = 0, PartAScore = 0, PartBScore = 0
-- WHERE Id = ATTEMPT_ID;

-- Or delete the incomplete attempt to start fresh (replace ATTEMPT_ID)
-- DELETE FROM ExamAnswers WHERE ExamAttemptId = ATTEMPT_ID;
-- DELETE FROM ExamAttempts WHERE Id = ATTEMPT_ID;
