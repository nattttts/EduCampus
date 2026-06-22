<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentDashboard.aspx.cs" Inherits="EduCampus.StudentDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Student Dashboard</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />

    <style>
        /* CENTER DASHBOARD CONTENT */
        .dashboard {
            min-height: 85vh;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            text-align: center;
            padding: 20px;
        }

        /* CARD LAYOUT */
        .card-container {
            display: flex;
            justify-content: center;
            gap: 20px;
            flex-wrap: wrap;
            margin-top: 20px;
        }

        .card {
            width: 250px;
            padding: 20px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <!-- NAVBAR -->
        <nav class="navbar navbar-expand-lg bg-white shadow-sm">
            <div class="container-fluid">

                <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2" />

                <div class="collapse navbar-collapse">
                    <ul class="navbar-nav me-auto">

                        <li class="nav-item">
                            <a class="nav-link active" href="StudentDashboard.aspx">Home</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="StudentEnrollCourse.aspx">Course</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="Attendance.aspx">Attendance</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="Results.aspx">Results</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="StudentNotification.aspx">Notifications</a>
                        </li>


                        <li class="nav-item">
                            <a class="nav-link" href="StudentNotes.aspx">Notes</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="StudentCalendar.aspx">Academic Calendar</a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="StudentProfile.aspx">Profile</a>
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

        <!-- DASHBOARD CONTENT -->
        <div class="dashboard">

            <h1 class="mb-3">Student Dashboard</h1>

            <h4 class="mb-4">
                Welcome,
                <asp:Label ID="lblStudentName" runat="server"></asp:Label>
            </h4>

            <div class="card-container">

                <div class="card shadow">
                    <h5>Total Enrolled Courses</h5>
                    <asp:Label ID="lblTotalCourses" runat="server" Font-Size="25px" Font-Bold="True"></asp:Label>
                </div>

                <div class="card shadow">
                    <h5>Approved Courses</h5>
                    <asp:Label ID="lblApproved" runat="server" Font-Size="25px" Font-Bold="True"></asp:Label>
                </div>

                <div class="card shadow">
                    <h5>Pending Courses</h5>
                    <asp:Label ID="lblPending" runat="server" Font-Size="25px" Font-Bold="True"></asp:Label>
                </div>

            </div>

        </div>

    </form>
</body>

</html>