
CREATE DATABASE EduCampusDB;

USE EduCampusDB;

-- USERS TABLE
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL -- Admin / Lecturer / Student
);

-- LECTURERS TABLE
CREATE TABLE Lecturers (
    LecturerID INT IDENTITY(1,1) PRIMARY KEY,
    Department VARCHAR(10) NOT NULL,
    UserID INT NOT NULL,

    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- PROGRAMMES TABLE
CREATE TABLE Programmes (
    ProgrammeID INT IDENTITY(1,1) PRIMARY KEY,
    ProgrammeCode VARCHAR(10) NOT NULL,
    ProgrammeName VARCHAR(255) NOT NULL
);

-- STUDENTS TABLE
CREATE TABLE Students (
    StudentID VARCHAR(10) PRIMARY KEY,
    UserID INT NOT NULL,
    ProgrammeID INT NOT NULL,

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProgrammeID) REFERENCES Programmes(ProgrammeID)
);

-- NOTIFICATIONS TABLE
CREATE TABLE Notifications (
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    CreatedDateTime DATETIME DEFAULT GETDATE(),
    UserID INT NOT NULL,

    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- COURSES TABLE
CREATE TABLE Courses (
    CourseID INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode VARCHAR(10) NOT NULL,
    CourseName VARCHAR(100) NOT NULL,
    CreditHours INT NOT NULL,
    ProgrammeID INT NOT NULL,

    FOREIGN KEY (ProgrammeID) REFERENCES Programmes(ProgrammeID)
);

-- ACADEMIC CALENDAR TABLE
CREATE TABLE AcademicCalendar (
    CalendarID INT IDENTITY(1,1) PRIMARY KEY,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Event NVARCHAR(MAX) NOT NULL
);

-- COURSE OFFERINGS TABLE
CREATE TABLE CourseOfferings (
    OfferingID INT IDENTITY(1,1) PRIMARY KEY,
    Session VARCHAR(10) NOT NULL,
    CourseID INT NOT NULL,
    LecturerID INT NOT NULL,

    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
    FOREIGN KEY (LecturerID) REFERENCES Lecturers(LecturerID)
);

-- ANNOUNCEMENTS TABLE
CREATE TABLE Announcements (
    AnnouncementID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    PostedDateTime DATETIME DEFAULT GETDATE(),
    OfferingID INT,

    FOREIGN KEY (OfferingID) REFERENCES CourseOfferings(OfferingID)
);

-- ENROLMENTS TABLE
CREATE TABLE Enrolments (
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    DateEnrolled DATE DEFAULT GETDATE(),
    Status VARCHAR(10) DEFAULT 'Pending',
    StudentID VARCHAR(10) NOT NULL,
    OfferingID INT NOT NULL,

    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (OfferingID) REFERENCES CourseOfferings(OfferingID)
);

-- ATTENDANCE TABLE
CREATE TABLE Attendance (
    AttendanceID INT IDENTITY(1,1) PRIMARY KEY,
    AttendanceDate DATE DEFAULT GETDATE(),
    Status VARCHAR(10) NOT NULL,
    Remarks NVARCHAR(255),
    StudentID VARCHAR(10) NOT NULL,
    OfferingID INT NOT NULL,

    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (OfferingID) REFERENCES CourseOfferings(OfferingID)
);

-- RESULTS TABLE
CREATE TABLE Results (
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    Mark DECIMAL(5,1),
    Grade NVARCHAR(5),
    EnrolmentID INT NOT NULL,
    LecturerID INT NOT NULL,
    StudentID VARCHAR(10) NOT NULL,

    FOREIGN KEY (EnrolmentID) REFERENCES Enrolments(EnrolmentID),
    FOREIGN KEY (LecturerID) REFERENCES Lecturers(LecturerID),
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
);