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

        <style>
        body {
    background-color: #A4D8FF;
    font-family: Arial, sans-serif;
    margin: 0;
}

.container {
    width: 95%;
    max-width: 1200px;
    margin: 30px auto;
    text-align: center;
}

.section {
    margin-top: 40px;
    text-align: center;
}

h2, h3 {
    text-align: center;
    margin-bottom: 20px;
}

.grid {
    margin: 0 auto;
    width: auto;
}

asp\:GridView,
table {
    margin: 0 auto;
}

select {
    padding: 8px;
    min-width: 450px;
}

.chart-container {
    display: flex;
    justify-content: center;
    margin-top: 20px;
}

.message {
    text-align: center;
    font-weight: bold;
    margin-top: 15px;
}
    </style>

    <div class="container">

    <h2>Lecturer Dashboard</h2>

    <div class="section">
        <h3>Assigned Courses</h3>
        <asp:GridView ID="gvAssignedCourses"
            runat="server"
            CssClass="grid">
        </asp:GridView>
    </div>

    <div class="section">
        <h3>Select Course</h3>

        <asp:DropDownList
            ID="ddlCourse"
            runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
        </asp:DropDownList>

        <br /><br />

        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message">
        </asp:Label>
    </div>

    <div class="section">
        <h3>Poor Attendance Students</h3>

        <asp:GridView
            ID="gvPoorAttendance"
            runat="server"
            CssClass="grid">
        </asp:GridView>
    </div>

    <div class="section">
        <h3>Student Grade Distribution</h3>

        <div class="chart-container">
    <asp:Chart ID="chartGrades" runat="server" Width="700px" Height="400px">
        <Series>
            <asp:Series Name="Grades" ChartType="Column"></asp:Series>
        </Series>

        <ChartAreas>
            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
</div>
        </div>
    </div>

</div>

</form>
</body>
</html>