<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignLecturer.aspx.cs" Inherits="EduCampus.AssignLecturer" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <title>Assign Lecturer</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="style.css" rel="stylesheet" />
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
                    <a class="nav-link active" href="AssignLecturer.aspx">Assign Lecturer</a>
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
                    <a class="nav-link" href="AcademicResults.aspx">Results</a>
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

<div class="container mt-5">

    <!-- Form -->
    <div class="row justify-content-center">
        <div class="col-md-7">
            <div class="card shadow-lg p-4">

                <h4 class="text-center mb-4">Assign Course to Lecturer</h4>

                <!-- Lecturer -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Lecturer</label>
                    <asp:DropDownList ID="ddlLecturer" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <!-- Session -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Session</label>

                    <asp:DropDownList ID="ddlSession"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">

                        <asp:ListItem Selected="True">Jan2026</asp:ListItem>
                        <asp:ListItem>Apr2026</asp:ListItem>
                        <asp:ListItem>Aug2026</asp:ListItem>

                    </asp:DropDownList>
                </div>

                <!-- Subjects -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Select Subjects</label>

                    <div class="border p-3 rounded bg-light cbl-container" style="max-height:250px; overflow-y:auto;">
                        <asp:CheckBoxList ID="cblCourses" runat="server"
                            CssClass="cblCourses"
                            RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </div>
                </div>

                <!-- Button -->
                <div class="d-grid">
                    <asp:Button ID="btnAssign" runat="server"
                        Text="Assign Selected Subjects"
                        CssClass="btn btn-primary btn-lg"
                        OnClick="btnAssign_Click" />
                </div>

                <!-- Message -->
                <div class="text-center mt-3">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </div>

            </div>
        </div>
    </div>

    <!-- Table -->
    <div class="row mt-5">
        <div class="col-12">
            <div class="card shadow-lg p-4">

                <h4 class="mb-3">Assigned Subjects</h4>

                <!-- Search -->
                <div class="row mb-3">
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchAssignment" runat="server"
                            CssClass="form-control"
                            Placeholder="Enter lecturer name/course name/course code (e.g. Tan Mei Ling, DCS2101, Data Structures)" />
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="btnSearch" runat="server" Text="Search"
                            CssClass="btn btn-primary w-100"
                            OnClick="btnSearch_Click" />
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="btnReset" runat="server" Text="Reset"
                            CssClass="btn btn-secondary w-100"
                            OnClick="btnReset_Click" />
                    </div>
                </div>

                <asp:GridView ID="gvAssign" runat="server"
                    CssClass="table table-striped table-hover"
                    AutoGenerateColumns="false"
                    DataKeyNames="OfferingID"
                    OnRowCommand="gvAssign_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="FullName" HeaderText="Lecturer" />

                        <asp:BoundField DataField="Session" HeaderText="Session" />

                        <asp:BoundField DataField="CourseCode" HeaderText="Code" />

                        <asp:BoundField DataField="CourseName" HeaderText="Course" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CssClass="btn btn-danger btn-sm"
                                    CommandName="DeleteRow"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    OnClientClick="return confirm('Delete this assignment?');">
                                    Delete
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>
        </div>
    </div>

</div>

</form>
</body>
</html>
