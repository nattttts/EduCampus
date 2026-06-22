USE EduCampusDB;

INSERT INTO Users (FullName, Email, PasswordHash, Role) 
VALUES 
('admin', 'admin@gmail.com', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Admin'),
('Tan Mei Ling', 'meilingtan@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Lecturer'),
('Siti Aminah', 'sitiaminah@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Lecturer'),
('Amanda Tan', 'amandatan@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Student'),
('Daniel Lim Wei Ming', 'limweimingdaniel@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Student'),
('Raju Subramaniam', 'rajusubramaniam@gmail.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Student');

INSERT INTO Programmes (ProgrammeCode, ProgrammeName)
VALUES
('DCS', 'Diploma in Computer Science'),
('DITN', 'Diploma in Information Technology');

SELECT * FROM Users;

SELECT * FROM Programmes;

INSERT INTO Lecturers (Department, UserID) VALUES ('SOC', 2), ('SOC', 3);

INSERT INTO Students (StudentID, UserID, ProgrammeID) 
VALUES ('P260001', 4, 1), ('P260002', 5, 1), ('P260003', 6, 2);

INSERT INTO Courses (CourseCode, CourseName, CreditHours, ProgrammeID)
VALUES
('DCS1104', 'Introduction to Statistics and Analytics', 4, 1),
('STA1101', 'Quantitative Methods', 4, 2),
('DCS2101', 'Data Structures', 4, 1),
('DCS1102', 'Rapid Application Development', 4, 1),
('DCS1105', 'High-Level Programming', 4, 1);

INSERT INTO CourseOfferings (Session, CourseID, LecturerID)
VALUES
('Apr2026', 1, 1),
('Apr2026', 3, 1),
('Apr2026', 2, 2),
('Aug2026', 4, 1),
('Aug2026', 5, 2);

SELECT * FROM EnrollmentMaster;

INSERT INTO EnrollmentMaster (Session, Semester, StudentID)
VALUES
('Apr2026', 'Semester 1', 'P260001'), 
('Apr2026', 'Semester 1', 'P260002'), 
('Apr2026', 'Semester 2', 'P260003'),
('Aug2026', 'Semester 2', 'P260001'), 
('Aug2026', 'Semester 2', 'P260002');

INSERT INTO EnrollmentDetails (EnrolmentID, OfferingID)
VALUES
(1, 1), (1, 2), (2, 1), (3, 3),(4, 4), (4, 5), (5, 4), (5, 5);

INSERT INTO Attendance (AttendanceDate, Status, Remarks, DetailID)
VALUES
('2026-06-10','Present', NULL, 1),
('2026-06-18','Present', NULL, 1),
('2026-06-19','Present', NULL, 1),
('2026-06-20','Absent', 'Medical Leave', 1),

('2026-06-15','Present', NULL, 2),
('2026-06-19','Present', NULL, 2),

('2026-06-10','Present', NULL, 3),
('2026-06-18','Absent', NULL, 3),
('2026-06-19','Absent', 'No Show', 3),
('2026-06-20','Absent', NULL, 3),

('2026-06-11','Present', NULL, 4),
('2026-06-16','Absent', NULL, 4);

INSERT INTO CourseMarks 
(AssignmentMark, QuizMark, MidTestMark, FinalExamMark, FinalMark, FinalGrade, GradePoint, DetailID)
VALUES
(30, 10, 20, 36, 96, 'A+', 4.00, 1),
(25, 8, 18, 32, 83, 'A', 4.00, 2),
(25, 7, 19, 21, 72, 'B+', 3.33, 3),
(25, 7, 17, 21, 70, 'B+', 3.33, 4),
(27, 8, 20, 32, 87, 'A', 4.00, 5),
(21, 8, 16, 30, 75, 'A-', 3.67, 6),
(25, 7, 18, 20, 70, 'B+', 3.33, 7),
(11, 5, 10, 12, 38, 'F', 0.00, 8);

INSERT INTO Announcements
(Title, Message, OfferingID)
VALUES
-- General announcements (Admin)
('Semester Enrolment Open',
 'Course enrolment for 2026 is now open. Please complete your enrolment before the deadline.',
 NULL),

('Examination Timetable Released',
 'The final examination timetable has been published. Students are advised to review their schedules carefully.',
 NULL),

('Fee Payment Reminder',
 'Students with outstanding fees are required to settle payments before the examination period.',
 NULL),

-- Course-specific announcements
('Week 5 Lecture Notes Uploaded',
 'The Week 5 lecture notes have been uploaded to the course materials section.',
 1),

('Quiz 1 Schedule',
 'Quiz 1 will be conducted during next week''s class session. Please review Chapters 1 to 3.',
 1),

('Assignment Submission Reminder',
 'Assignment 1 is due this Friday at 11:59 PM. Late submissions will be penalized.',
 2),

('Lab Session Change',
 'This week''s lab session has been moved to ICT Lab B due to maintenance work.',
 2),

('Mid-Term Test Announcement',
 'The Mid-Term Test will be held during Week 8. Further details will be provided in class.',
 3);

INSERT INTO Notifications (Title, Message, UserID)
VALUES
('Attendance Warning',
 'Your attendance for Data Structure has fallen below 80%.',
 4),

('Semester Results Published',
 'Your results for Semester 1 have been released.',
 4);

INSERT INTO AcademicCalendar (Session, StartDate, EndDate, Event)
VALUES
('Apr2026', '2026-03-30', '2026-04-3', 'Class begin'),

('Apr2026', '2026-04-08', '2026-04-08', 
'Last day for enrollment fee without late payment charge for continuing students'),

('Apr2026', '2026-04-15', '2026-04-15', 
'The CANVAS will be blocked if no payment is made'),

('Apr2026', '2026-05-01', '2026-05-01', 'Public Holiday: Labour Day'),

('Apr2026', '2026-05-18', '2026-05-24', 'Mid Semester Break');

INSERT INTO Notes
(FileName, FilePath, OfferingID)
VALUES

-- DATA STRUCTURES NOTES
('Week1_OOD_Design.pdf',
 '~/Uploads/DataStructures/Apr2026/Week1_OOD_Design.pdf',
2),

('Week2_OOD_Design_Part2.pdf',
 '~/Uploads/DataStructures/Apr2026/Week2_OOD_Design_Part2.pdf',
2),

-- INTRO TO STATS AND ANALYTICS NOTES
('DCS1104_Chapter1.pdf',
 '~/Uploads/IntroductionToStatisticsAndAnalytics/Apr2026/DCS1104_Chapter1.pdf',
1),

('DCS1104_Chapter2.pdf',
 '~/Uploads/IntroductionToStatisticsAndAnalytics/Apr2026/DCS1104_Chapter2.pdf',
1);

SELECT * FROM Notes;