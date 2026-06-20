<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentEnrollCourse.aspx.cs" Inherits="EduCampus.StudentEnrollCourse" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Student Enroll Course</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="style.css" rel="stylesheet" />
</head>

<body>

<form id="form2" runat="server">

    <!-- Navigation bar -->
    <nav class="navbar navbar-expand-lg bg-white">
        <div class="container-fluid">
            <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2">

            <div class="collapse navbar-collapse">
                <ul class="navbar-nav me-auto">

                    <li class="nav-item">
                        <a class="nav-link" href="StudentDashboard.aspx">Home</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link active" href="StudentEnrollCourse.aspx">Course</a>
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

<!-- CONTENT -->
<div class="container mt-4">

    <div class="card shadow-sm border-0">

        <div class="card-body">

            <h2 class="mb-4 text-center">
                Course Enrollment
            </h2>

            <!-- Session + Semester -->
            <div class="row mb-4">

                <div class="col-md-6">

                    <label class="form-label fw-bold">
                        Select Session
                    </label>

                    <asp:DropDownList ID="ddlSession"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                    </asp:DropDownList>

                </div>

                <div class="col-md-6">

                    <label class="form-label fw-bold">
                        Select Semester
                    </label>

                    <asp:DropDownList ID="ddlSemester"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged">

                        <asp:ListItem>Semester 1</asp:ListItem>
                        <asp:ListItem>Semester 2</asp:ListItem>

                    </asp:DropDownList>

                </div>

            </div>

            <!-- Available Courses -->
            <div class="card mb-4">

                <div class="card-header bg-primary text-white">
                    Available Courses
                </div>

                <div class="card-body">

                    <asp:GridView ID="gvCourses"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataKeyNames="CourseID"
                    CssClass="table table-hover table-bordered">

                        <Columns>

                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect"
                                        runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="CourseCode"
                                HeaderText="Course Code" />

                            <asp:BoundField DataField="CourseName"
                                HeaderText="Course Name" />

                            <asp:BoundField DataField="CreditHours"
                                HeaderText="Credit Hours" />

                        </Columns>

                    </asp:GridView>

                    <div class="text-center mt-3">

                        <asp:Button ID="btnSubmit"
                            runat="server"
                            Text="Submit Enrollment"
                            CssClass="btn btn-primary px-5"
                            OnClick="btnSubmit_Click" />

                    </div>

                    <div class="text-center mt-3">

                        <asp:Label ID="lblMessage"
                            runat="server">
                        </asp:Label>

                    </div>

                </div>

            </div>

            <!-- Submitted Enrollment -->
            <div class="card">

                <div class="card-header bg-success text-white">
                    Submitted Enrollment Details
                </div>

                <div class="card-body">

                    <asp:GridView ID="gvEnrollment"
                        runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-hover table-bordered">

                        <Columns>

                            <asp:BoundField DataField="CourseCode"
                                HeaderText="Code" />

                            <asp:BoundField DataField="CourseName"
                                HeaderText="Course" />

                            <asp:BoundField DataField="Status"
                                HeaderText="Status" />

                            <asp:TemplateField HeaderText="Action">

                                <ItemTemplate>

                                    <asp:Button ID="btnDrop"
                                        runat="server"
                                        Text="Drop"
                                        CssClass="btn btn-danger btn-sm"
                                        CommandArgument='<%# Eval("EnrolmentID") %>'
                                        OnClick="btnDrop_Click" />

                                </ItemTemplate>

                            </asp:TemplateField>

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