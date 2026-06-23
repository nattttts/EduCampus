<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance.aspx.cs" Inherits="EduCampus.Attendance" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Attendance</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
    <link rel="stylesheet" href="student.css" />
</head>

<body>

<form id="form2" runat="server">
    <!-- Navigation bar -->
    <nav class="navbar navbar-expand-lg bg-white">
        <div class="container-fluid">
            <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2">

            <div class="collapse navbar-collapse">
                <!-- Menu -->
                <ul class="navbar-nav me-auto">
                <li class="nav-item">
                    <a class="nav-link" href="StudentDashboard.aspx">Dashboard</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link" href="StudentEnrollCourse.aspx">Course</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link active" href="Attendance.aspx">Attendance</a>
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

<!-- CONTENT -->
<div class="container mt-4">

    <div class="card shadow-sm border-0">

        <div class="card-body">

            <h2 class="text-center mb-4">
                Attendance Records
            </h2>

            <!-- Course Filter -->
            <div class="row justify-content-center mb-4">

                <div class="col-md-6">

                    <label class="form-label fw-bold">
                        Select Course
                    </label>

                    <asp:DropDownList ID="ddlCourse"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                    </asp:DropDownList>

                </div>

            </div>

            <!-- Attendance Table -->
            <div class="card">

                <div class="card-header bg-primary text-white">
                    Attendance Details
                </div>

                <div class="card-body">

                    <asp:GridView ID="gvAttendance"
                        runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table table-hover table-bordered">

                        <Columns>

                            <asp:BoundField DataField="CourseName" HeaderText="Course" />

                            <asp:BoundField DataField="AttendanceDate"
                                HeaderText="Date" />

                            <asp:TemplateField HeaderText="Status">

                                <ItemTemplate>

                                    <span class='<%# Eval("Status").ToString() == "Present" ? "present" : "absent" %>'>
                                        <%# Eval("Status") %>
                                    </span>

                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:BoundField DataField="Remarks"
                                HeaderText="Remarks" />

                        </Columns>

                    </asp:GridView>

                </div>

            </div>

        </div>

    </div>

</div>

</form>

</body>
</html>