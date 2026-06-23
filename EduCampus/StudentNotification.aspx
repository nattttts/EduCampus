<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentNotification.aspx.cs" Inherits="EduCampus.StudentNotification" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title> 📢 Notification</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
    <link rel="stylesheet" href="student.css" />
</head>

<body>

<form id="form1" runat="server">

    <!-- NAVBAR -->
    <nav class="navbar navbar-expand-lg bg-white shadow-sm">
        <div class="container-fluid">
            <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2">

            <div class="collapse navbar-collapse">

                <ul class="navbar-nav me-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="StudentDashboard.aspx">Dashboard</a>
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
                        <a class="nav-link active" href="StudentNotification.aspx">Notifications</a>
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

                <asp:Button ID="btnLogout" runat="server"
                    Text="Logout"
                    CssClass="btn btn-danger"
                    OnClick="btnLogout_Click" />

            </div>

        </div>
    </nav>

    <!-- CONTENT -->
    <div class="container container-box">

        <h2 class="page-title">📢 Notifications</h2>

        <asp:GridView ID="gvNotifications"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped shadow-sm"
            GridLines="None">

            <Columns>

                <asp:BoundField DataField="Title" HeaderText="Title" />

                <asp:BoundField DataField="Message" HeaderText="Message" />

                <asp:BoundField DataField="CreatedDateTime" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />

            </Columns>

        </asp:GridView>

        <!-- 📢 ANNOUNCEMENTS SECTION -->
        <br />

        <h5 class="mt-4">📢 Announcements</h5>

        <asp:GridView ID="gvAnnouncements"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-warning table-striped shadow-sm"
            GridLines="None">

            <Columns>
                <asp:BoundField DataField="Title" HeaderText="Title" />
                <asp:BoundField DataField="Message" HeaderText="Message" />
                <asp:BoundField DataField="PostedDateTime" HeaderText="Date"
                    DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            </Columns>

        </asp:GridView>

    </div>

</form>

</body>

</html>