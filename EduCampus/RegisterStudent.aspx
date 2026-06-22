<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterStudent.aspx.cs" Inherits="EduCampus.RegisterStudent" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Register Student</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="style.css" rel="stylesheet"/>
</head>

<body>
<form id="form1" runat="server" autocomplete="off">
    <!-- Navigation bar -->
    <nav class="navbar navbar-expand-lg bg-white">
        <div class="container-fluid">
            <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2">

            <div class="collapse navbar-collapse">
                <!-- Menu -->
                <ul class="navbar-nav me-auto">
                    <li class="nav-item">
                         <a class="nav-link" href="AdminDashboard.aspx">Dashboard</a>
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
                        <a class="nav-link active" href="RegisterStudent.aspx">Register Student</a>
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

    <!-- Form -->
    <div class="container mt-5 d-flex justify-content-center">
        <div class="card shadow p-4" style="width: 400px;">
        
            <h3 class="text-center mb-4">Register Student</h3>

            <div class="mb-3">
                <label class="form-label">Student ID</label>
                <asp:TextBox ID="txtStudentID" runat="server" CssClass="form-control" placeholder="Enter student ID (e.g. P260001)" />
            </div>

            <div class="mb-3">
                <label class="form-label">Full Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter name" />
            </div>

            <div class="mb-3">
                <label class="form-label">Student Email</label>
                <asp:TextBox ID="txtStudentEmail" runat="server" CssClass="form-control" placeholder="Enter email e.g. student@example.com" autocomplete="off" />
            </div>

            <div class="mb-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtStudentPass" runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Enter password"
                    autocomplete="new-password" />
            </div>

            <div class="mb-3">
                <label class="form-label">Programme</label>
                <asp:DropDownList ID="ddlProgramme"
                    runat="server"
                    CssClass="form-select">
                </asp:DropDownList>
            </div>

            <div class="d-grid">
                <asp:Button ID="btnRegister" runat="server" Text="Register"
                    CssClass="btn btn-primary"
                    OnClick="btnRegister_Click" />
            </div>

            <div class="mt-3 text-center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>

        </div>
    </div>

    <!-- Seach + Grid -->
    <div class="container mt-4">
        <div class="card shadow p-4">

            <h4 class="mb-3">Registered Students</h4>

            <!-- Search -->
            <div class="row mb-3">
                <div class="col-md-4">
                    <asp:TextBox ID="txtSearchProgramme" runat="server"
                        CssClass="form-control"
                        Placeholder="Enter Programme Name (e.g., Diploma in Computer Science)" />
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

            <!-- Gridview -->
            <asp:GridView ID="gvStudents" runat="server"
                CssClass="table table-bordered table-striped"
                AutoGenerateColumns="false"
                DataKeyNames="StudentID"
                EmptyDataText="No students found"
                OnRowEditing="gvStudent_RowEditing"
                OnRowUpdating="gvStudent_RowUpdating"
                OnRowCancelingEdit="gvStudent_RowCancelingEdit"
                OnRowCommand="gvStudent_RowCommand"
                OnRowDataBound="gvStudent_RowDataBound">

                <Columns>
                    <asp:BoundField DataField="StudentID" HeaderText="Student ID" ReadOnly="true" />
                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:TemplateField HeaderText="Programme">

                        <ItemTemplate>
                            <%# Eval("ProgrammeName") %>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditProgramme"
                                runat="server"
                                CssClass="form-select">
                            </asp:DropDownList>
                        </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions">

                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server"
                                CommandName="Edit"
                                Text="Edit"
                                CssClass="btn btn-primary btn-sm me-2" />
                            <asp:LinkButton ID="btnDelete" runat="server"
                                CommandName="DeleteRow"
                                Text="Delete"
                                CssClass="btn btn-danger btn-sm"
                                OnClientClick="return confirm('Are you sure you want to delete this student?');" />
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server"
                                CommandName="Update"
                                Text="Update"
                                CssClass="btn btn-success btn-sm me-2" />

                            <asp:LinkButton ID="btnCancel" runat="server"
                                CommandName="Cancel"
                                Text="Cancel"
                                CssClass="btn btn-secondary btn-sm" />
                        </EditItemTemplate>

                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

        </div>
    </div>

</form>
</body>

</html>