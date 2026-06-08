<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentDashboard.aspx.cs" Inherits="EduCampus.StudentDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Student Dashboard</title>

    <style>
        body {
            font-family: Arial;
            margin: 0;
            background-color: #f5f5f5;
        }

        /* TOP BAR */
        .topbar {
            background-color: darkblue;
            padding: 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        /* CENTER MENU */
        .menu {
            display: flex;
            gap: 25px;
            justify-content: center;
            flex: 1;
        }

        .menu a {
            color: white;
            text-decoration: none;
            font-weight: bold;
            font-size: 16px;
        }

        /* LOGOUT RIGHT */
        .logout {
            color: white;
        }

        /* DASHBOARD CENTER */
        .dashboard {
            padding: 40px;
            text-align: center;
        }

        .title {
            color: darkblue;
        }

        .card-container {
            display: flex;
            justify-content: center;
            flex-wrap: wrap;
            margin-top: 30px;
            gap: 20px;
        }

        .card {
            width: 250px;
            background-color: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px lightgray;
            text-align: center;
        }

        .card h3 {
            color: darkblue;
        }
    </style>

</head>

<body>

<form id="form1" runat="server">

    <!-- TOP BAR -->
    <div class="topbar">

        <!-- MENU CENTER -->
        <div class="menu">

            <a href="StudentDashboard.aspx">Dashboard</a>

            <a href="StudentEnrollCourse.aspx">Courses</a>

            <a href="Results.aspx">Results</a>

            <a href="Attendance.aspx">Attendance</a>

            <a href="StudentProfile.aspx">Profile</a>

        </div>

        <!-- LOGOUT RIGHT -->
        <div class="logout">
            <asp:Button ID="btnLogout"
                runat="server"
                Text="Logout"
                OnClick="btnLogout_Click" />
        </div>

    </div>

    <!-- DASHBOARD CONTENT -->
    <div class="dashboard">

        <h1 class="title">Student Dashboard</h1>

        <h3>
            Welcome,
            <asp:Label ID="lblStudentName" runat="server"></asp:Label>
        </h3>

        <div class="card-container">

            <div class="card">
                <h3>Total Enrolled Courses</h3>
                <asp:Label ID="lblTotalCourses" runat="server"
                    Font-Size="25px"
                    Font-Bold="True"></asp:Label>
            </div>

            <div class="card">
                <h3>Approved Courses</h3>
                <asp:Label ID="lblApproved" runat="server"
                    Font-Size="25px"
                    Font-Bold="True"></asp:Label>
            </div>

            <div class="card">
                <h3>Pending Courses</h3>
                <asp:Label ID="lblPending" runat="server"
                    Font-Size="25px"
                    Font-Bold="True"></asp:Label>
            </div>

        </div>

    </div>

</form>

</body>
</html>