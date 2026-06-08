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

INSERT INTO CourseMarks (AssignmentMark, TestMark, FinalMark, FinalGrade, DetailID)
VALUES
(85, 90, 88, 'A', 1),
(75, 80, 78, 'B+', 2);

INSERT INTO Results (GPA, CGPA, EnrolmentID)
VALUES
(3.75, 3.75, 1),
(3.10, 3.10, 2);

SELECT * FROM Attendance;