<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminViewEnrollment.aspx.cs" Inherits="EduCampus.AdminViewEnrollment" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>View Enrollemnt</title>

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
                            <a class="nav-link" href="AssignLecturer.aspx">Assign Lecturer</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="RegisterStudent.aspx">Register Student</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="AdminViewEnrollment.aspx">Enrollment</a>
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

        <!--Student enrollment list -->
        <div class="container-fluid mt-4 px-4">
            
            <h3 class="text-center mb-2">Student Enrollment List</h3>

            <div class="d-flex align-items-end gap-3 mb-3">

                <div>
                    <!-- Session dropdown -->
                    <label class="form-label fw-semibold">Session</label>

                    <asp:DropDownList ID="ddlSession"
                        runat="server"
                        CssClass="form-select"
                        Width="250px"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div>
                    <!-- Status dropdown-->
                    <label class="form-label fw-semibold">Status</label>

                    <asp:DropDownList ID="ddlStatus"
                        runat="server"
                        CssClass="form-select"
                        Width="250px"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">

                        <asp:ListItem value="">All</asp:ListItem>
                        <asp:ListItem value="Pending">Pending</asp:ListItem>
                        <asp:ListItem value="Approved">Approved</asp:ListItem>
                        <asp:ListItem value="Rejected">Rejected</asp:ListItem>

                    </asp:DropDownList>

                </div>

            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="text-muted d-block mb-2"></asp:Label>

            <!-- Main Grid -->
            <asp:GridView ID="gvEnrollment"
                runat="server"
                CssClass="table table-bordered table-striped"
                AutoGenerateColumns="False"
                DataKeyNames="EnrolmentID"
                EmptyDataText="No enrollment records found"
                OnRowDataBound="gvEnrollment_RowDataBound">

                <Columns>
                    <asp:BoundField DataField="EnrolmentID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Student Name" />
                    <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
                    <asp:BoundField DataField="Session" HeaderText="Session" />
                    <asp:BoundField DataField="Semester" HeaderText="Semester" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                        
                    <asp:TemplateField HeaderText="Approve">

                        <ItemTemplate>

                            <asp:Button ID="btnApprove"
                                runat="server"
                                Text="Approve"
                                CssClass="btn btn-success btn-sm"
                                Width="90px"
                                CommandArgument='<%# Eval("EnrolmentID") %>'
                                OnClick="btnApprove_Click"
                                Visible='<%# Eval("Status").ToString() == "Pending" %>' />

                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Reject">
                        <ItemTemplate>

                            <asp:Button ID="btnReject"
                                runat="server"
                                Text="Reject"
                                CssClass="btn btn-danger btn-sm"
                                Width="90px"
                                CommandArgument='<%# Eval("EnrolmentID") %>'
                                OnClick="btnReject_Click"
                                Visible='<%# Eval("Status").ToString() == "Pending" %>' />

                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Enrolled Courses">

                        <ItemTemplate>
                            <asp:GridView ID="gvCourses"
                                runat="server"
                                AutoGenerateColumns="False"
                                Width="100%"
                                CssClass="table table-bordered course-grid">

                                <Columns>

                                    <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                                    <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                    <asp:BoundField DataField="CreditHours" HeaderText="Credit Hours" />

                                </Columns>

                            </asp:GridView>

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>

    </form>
</body>
</html>
