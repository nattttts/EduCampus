<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="EduCampus.AdminDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Admin Dashboard</title>

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
                             <a class="nav-link active" href="AdminDashboard.aspx">Home</a>
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
                            <a class="nav-link" href="#">Register Student</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Enrolment</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Announcements</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Calendar</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Attendance</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Report</a>
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
    </form>
</body>
</html>