-- Direct SQL fix for Digital Marketing Essentials MCQ scores

-- Update Part A scores for Digital Marketing Essentials exam attempts
UPDATE ea
SET PartAScore = (
    SELECT COALESCE(SUM(
        CASE 
            WHEN ans.SelectedOptionId IS NOT NULL AND opt.IsCorrect = 1 THEN 2
            WHEN ans.SelectedOptionId IS NOT NULL AND opt.IsCorrect = 0 THEN -1
            ELSE 0
        END
    ), 0)
    FROM ExamAnswers ans
    INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id
    LEFT JOIN ExamQuestionOptions opt ON ans.SelectedOptionId = opt.Id
    WHERE ans.ExamAttemptId = ea.Id AND eq.Part = 1
)
FROM ExamAttempts ea
INNER JOIN Exams e ON ea.ExamId = e.Id
INNER JOIN Courses c ON e.CourseId = c.Id
WHERE ea.PartACompleted = 1 
AND c.Title LIKE '%Digital Marketing%'
AND ea.PartAScore = 0;

-- Ensure no negative scores
UPDATE ea
SET PartAScore = 0
FROM ExamAttempts ea
INNER JOIN Exams e ON ea.ExamId = e.Id
INNER JOIN Courses c ON e.CourseId = c.Id
WHERE c.Title LIKE '%Digital Marketing%' AND ea.PartAScore < 0;

-- Show updated results
SELECT 
    u.FirstName + ' ' + u.LastName as StudentName,
    c.Title as CourseTitle,
    e.Title as ExamTitle,
    ea.PartAScore,
    ea.PartACompleted,
    (SELECT COUNT(*) FROM ExamAnswers ans 
     INNER JOIN ExamQuestions eq ON ans.ExamQuestionId = eq.Id 
     WHERE ans.ExamAttemptId = ea.Id AND eq.Part = 1) as MCQCount
FROM ExamAttempts ea
INNER JOIN Exams e ON ea.ExamId = e.Id
INNER JOIN Courses c ON e.CourseId = c.Id
INNER JOIN AspNetUsers u ON ea.StudentId = u.Id
WHERE c.Title LIKE '%Digital Marketing%' AND ea.PartACompleted = 1;