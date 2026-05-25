USE EduCampusDB;

INSERT INTO Users (FullName, Email, Password, Role) 
VALUES 
('admin', 'admin@gmail.com', 'admin123', 'Admin'),
('Tan Mei Ling', 'meilingtan@gmail.com', '123456', 'Lecturer'),
('Siti Aminah', 'sitiaminah@gmail.com', '123456', 'Lecturer'),
('Amanda Tan', 'amandatan@gmail.com', '123456', 'Student'),
('Daniel Lim Wei Ming', 'limweimingdaniel@gmail.com', '123456', 'Student'),
('Raju Subramaniam', 'rajusubramaniam@gmail.com', '123456', 'Student');

INSERT INTO Programmes (ProgrammeCode, ProgrammeName)
VALUES
('DCS', 'Diploma in Computer Science'),
('DITN', 'Diploma in Information Technology');

SELECT * FROM CourseOfferings;

INSERT INTO Lecturers (UserID) VALUES (2), (3);

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

SELECT * FROM Enrolments;

INSERT INTO Enrolments (StudentID, OfferingID)
VALUES
('P260001', 1), ('P260001', 2), ('P260002', 1), ('P260002', 2), ('P260003', 3);