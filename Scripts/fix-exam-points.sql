-- Fix Exam Questions with Invalid Points
-- This script updates all Part A (MCQ) questions that have 0 or invalid points to 2 points

UPDATE ExamQuestions 
SET Points = 2 
WHERE Part = 1 -- Part A (MCQ)
  AND (Points IS NULL OR Points <= 0);

-- Verify the update
SELECT 
    eq.Id,
    eq.Text,
    eq.Part,
    eq.Points,
    e.Title as ExamTitle,
    c.Title as CourseTitle
FROM ExamQuestions eq
INNER JOIN Exams e ON eq.ExamId = e.Id
INNER JOIN Courses c ON e.CourseId = c.Id
WHERE eq.Part = 1 -- Part A (MCQ)
ORDER BY c.Title, e.Title, eq.Id;

-- Check total possible points for each exam
SELECT 
    e.Id as ExamId,
    e.Title as ExamTitle,
    c.Title as CourseTitle,
    COUNT(eq.Id) as TotalQuestions,
    SUM(eq.Points) as TotalPossiblePoints,
    AVG(eq.Points) as AveragePointsPerQuestion
FROM Exams e
INNER JOIN Courses c ON e.CourseId = c.Id
INNER JOIN ExamQuestions eq ON e.Id = eq.ExamId
WHERE eq.Part = 1 -- Part A (MCQ)
GROUP BY e.Id, e.Title, c.Title
ORDER BY c.Title, e.Title;