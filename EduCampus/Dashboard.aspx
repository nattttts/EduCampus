<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs"
    Inherits="EduCampus.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization"
    Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Lecturer Dashboard</title>
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
                            <a class="nav-link" href="Dashboard">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="CourseMaterial">Courses</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Attendance.aspx">Attendance</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="Markspage.aspx">Marks</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="LecturerAnnouncements.aspx">Announcements</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="LecturerProfile.aspx">Profile</a>
                        </li>
                    </ul>

                    <!-- Logout button -->
                    <asp:Button ID="Button1" runat="server"
                        Text="Logout"
                        CssClass="btn btn-danger"
                        OnClick="btnLogout_Click" />

                </div>

            </div>
        </nav>

    <h2>Lecturer Dashboard</h2>

    <asp:Button ID="btnLogout" runat="server"
        Text="Logout"
        OnClick="btnLogout_Click" />

    <hr />

    <h3>Assigned Courses</h3>

    <asp:GridView ID="gvAssignedCourses" runat="server"
        AutoGenerateColumns="False"
        BorderWidth="1">
        <Columns>
            <asp:BoundField DataField="OfferingID" HeaderText="Offering ID" />
            <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
            <asp:BoundField DataField="Session" HeaderText="Session" />
        </Columns>
    </asp:GridView>

    <hr />

    <h3>Select Course</h3>

    <asp:DropDownList ID="ddlCourse" runat="server"
        AutoPostBack="true"
        OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
    </asp:DropDownList>

    <br /><br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>

    <hr />

    <h3>Poor Attendance Students</h3>

    <asp:GridView ID="gvPoorAttendance" runat="server"
        AutoGenerateColumns="False"
        BorderWidth="1">
        <Columns>
            <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
            <asp:BoundField DataField="FullName" HeaderText="Student Name" />
            <asp:BoundField DataField="TotalClass" HeaderText="Total Classes" />
            <asp:BoundField DataField="AbsentCount" HeaderText="Absent" />
            <asp:BoundField DataField="AttendancePercent" HeaderText="Attendance %" />
        </Columns>
    </asp:GridView>

    <h3>Student Grade Distribution</h3>

    <asp:Chart ID="chartGrades" runat="server" Width="600px" Height="350px">
    <Series>
        <asp:Series Name="Grades" ChartType="Column"></asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
    </ChartAreas>
</asp:Chart>

</form>
</body>
</html>