<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs"
    Inherits="EduCampus.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization"
    Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Dashboard</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
</head>

    <body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg bg-white">
            <div class="container-fluid">
                <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2" />

                <div class="collapse navbar-collapse">
                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="Dashboard.aspx">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="CourseMaterial.aspx">Courses</a>
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
                            <a class="nav-link" href="#">Profile</a>
                        </li>
                    </ul>

                    <asp:Button ID="btnLogout" runat="server"
                        Text="Logout"
                        CssClass="btn btn-danger"
                        OnClick="btnLogout_Click" />

                </div>

            </div>
        </nav>

    <style>
        body {
            background-color: #A4D8FF;
            font-family: Arial;
        }

        .container {
            width: 1100px;
            margin: 30px auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
        }

        .filter-row {
            display: flex;
            gap: 15px;
            margin-bottom: 25px;
        }

        .section {
            margin-top: 30px;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
        }

        .grid th {
            background-color: #007bff;
            color: white;
            padding: 8px;
        }

        .grid td {
            padding: 8px;
            text-align: center;
        }

        h2, h3 {
            text-align: center;
        }

        .message {
            color: red;
            text-align: center;
            font-weight: bold;
        }
    </style>

<div class="container">

    <h2>Lecturer Dashboard</h2>

    <asp:Label
        ID="lblMessage"
        runat="server"
        CssClass="message">
    </asp:Label>

    <div class="section">
        <h3>View Assigned Courses</h3>

        <asp:GridView
            ID="gvAssignedCourses"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid">

            <Columns>
                <asp:BoundField DataField="OfferingID" HeaderText="Offering ID" />
                <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                <asp:BoundField DataField="SessionName" HeaderText="Session" />
            </Columns>

        </asp:GridView>
    </div>

    <div class="section">
        <h3>Select Course</h3>

        <div class="filter-row">
            <asp:DropDownList
                ID="ddlCourse"
                runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>

    <div class="section">

        <h3>Poor Attendance Students</h3>

        <asp:GridView
            ID="gvPoorAttendance"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid">

            <Columns>

                <asp:BoundField
                    DataField="StudentID"
                    HeaderText="Student ID" />

                <asp:BoundField
                    DataField="StudentName"
                    HeaderText="Student Name" />

                <asp:BoundField
                    DataField="TotalClasses"
                    HeaderText="Total Classes" />

                <asp:BoundField
                    DataField="PresentCount"
                    HeaderText="Present" />

                <asp:BoundField
                    DataField="AttendancePercentage"
                    HeaderText="Attendance %"
                    DataFormatString="{0:N2}%" />

            </Columns>

        </asp:GridView>

    </div>

    <div class="section">

        <h3>Grade Distribution</h3>

        <asp:Chart
            ID="chartGrades"
            runat="server"
            Width="900px"
            Height="400px">

            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <AxisX Title="Grade"></AxisX>
                    <AxisY Title="Number of Students"></AxisY>
                </asp:ChartArea>
            </ChartAreas>

            <Series>
                <asp:Series
                    Name="Grades"
                    ChartType="Column"
                    XValueMember="FinalGrade"
                    YValueMembers="StudentCount">
                </asp:Series>
            </Series>

            <Titles>
                <asp:Title Text="Grade Distribution"></asp:Title>
            </Titles>

        </asp:Chart>

    </div>

</div>

</form>
</body>
</html>