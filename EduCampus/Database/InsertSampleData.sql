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
('DCS2101', 'Data Structures', 4, 1);

INSERT INTO CourseOfferings (Session, CourseID, LecturerID)
VALUES
('Apr2026', 1, 1),
('Apr2026', 3, 1),
('Apr2026', 2, 2);

SELECT * FROM EnrollmentMaster;

INSERT INTO EnrollmentMaster (Session, Semester, StudentID)
VALUES
('Apr2026', 'Semester 1', 'P260001'), ('Apr2026', 'Semester 1', 'P260002'), ('Apr2026', 'Semester 2', 'P260003');

INSERT INTO EnrollmentDetails (EnrolmentID, OfferingID)
VALUES
(1, 1), (1, 2), (2, 1), (3, 3);

INSERT INTO Attendance (Status, Remarks, DetailID)
VALUES
('Present', NULL, 1),
('Present', NULL, 1),
('Absent', 'Medical Leave', 1),

('Present', NULL, 2),
('Present', NULL, 2),

('Present', NULL, 3),
('Absent', 'No Show', 3);

INSERT INTO CourseMarks (AssignmentMark, MidTestMark, FinalMark, FinalGrade, DetailID)
VALUES
(85, 90, 88, 'A', 1),
(75, 80, 78, 'B+', 2);

INSERT INTO Results (GPA, CGPA, EnrolmentID)
VALUES
(3.75, 3.75, 1),
(3.10, 3.10, 2);

SELECT * FROM Notifications;

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

ALTER TABLE Attendance
ADD CONSTRAINT FK_Attendance_Detail
FOREIGN KEY (DetailID)
REFERENCES EnrollmentDetails(DetailID)
ON DELETE CASCADE;