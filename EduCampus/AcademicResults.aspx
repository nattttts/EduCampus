<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcademicResults.aspx.cs" Inherits="EduCampus.AcademicResults" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Student Academic Results</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation bar -->
        <nav class="navbar navbar-expand-lg bg-white">
            <div class="container-fluid">
                <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2">

                <div class="collapse navbar-collapse">
                    <!-- Menu -->
                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                             <a class="nav-link" href="AdminDashboard.aspx">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ManageProgramme.aspx">Programme</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ManageCourse.aspx">Courses</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="RegisterLecturer.aspx">Register Lecturer</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="AssignLecturer.aspx">Assign Lecturer</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="RegisterStudent.aspx">Register Student</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AdminViewEnrollment.aspx">Enrollment</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AdminAnnouncements.aspx">Announcements</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ManageCalendar.aspx">Calendar</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AdminAttendance.aspx">Attendance</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="AcademicResults.aspx">Results</a>
                        </li>
                    </ul>

                    <!-- Logout button -->
                    <asp:Button ID="btnLogout" runat="server"
                        Text="Logout"
                        CssClass="btn btn-danger"
                        OnClick="btnLogout_Click" />

                </div>

            </div>
        </nav>

        <!-- Manage programme form -->
        <div class="container mt-5">

            <h3 class="text-center">Academic Performance</h3>

            <div class="text-center mt-5">

                <asp:Button ID="btnCourseResults"
                    runat="server"
                    Text="Course Results"
                    CssClass="btn btn-primary btn-lg m-2"
                    OnClick="btnCourseResults_Click"/>

                <asp:Button ID="btnStudentTranscript"
                    runat="server"
                    Text="Student Transcript"
                    CssClass="btn btn-success btn-lg m-2"
                    OnClick="btnStudentTranscript_Click"/>

            </div>

        </div>
    </form>
</body>
</html>
