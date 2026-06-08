<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterLecturer.aspx.cs" Inherits="EduCampus.RegisterLecturer" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Register Lecturer</title>

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
                         <a class="nav-link" href="AdminDashboard.aspx">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="ManageProgramme.aspx">Programme</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="ManageCourse.aspx">Courses</a>
                    </li>
                     <li class="nav-item">
                        <a class="nav-link active" href="RegisterLecturer.aspx">Register Lecturer</a>
                    </li>
                     <li class="nav-item">
                        <a class="nav-link" href="AssignLecturer.aspx">Assign Lecturer</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="RegisterStudent.aspx">Register Student</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Enrolment</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="AdminAnnouncements.aspx">Announcements</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Calendar</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Attendance</a>
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

    <!-- Form -->
    <div class="container mt-5 d-flex justify-content-center">
        <div class="card shadow p-4" style="width: 400px;">
            
            <h3 class="text-center mb-4">👩‍🏫 Register Lecturer</h3>

            <div class="mb-3">
                <label class="form-label">Full Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter name" />
            </div>

            <div class="mb-3">
                <label class="form-label">Lecturer Email</label>
                <asp:TextBox ID="txtLecturerEmail" runat="server" CssClass="form-control" placeholder="Enter email e.g. lecturer@example.com" autocomplete="off" />
            </div>

            <div class="mb-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtLecturerPass" runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Enter password"
                    autocomplete="new-password" />
            </div>

            <div class="mb-3">
                <label class="form-label">Department</label>
                <asp:TextBox ID="txtDept" runat="server" CssClass="form-control" placeholder="Enter department e.g. SOC" />
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

            <h4 class="mb-3">📋 Registered Lecturers</h4>

            <!-- Search -->
            <div class="row mb-3">
                <div class="col-md-4">
                    <asp:TextBox ID="txtSearchDept" runat="server"
                        CssClass="form-control"
                        Placeholder="Enter Department (e.g., IT)" />
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
            <asp:GridView ID="gvLecturers" runat="server"
                CssClass="table table-bordered table-striped"
                AutoGenerateColumns="false"
                DataKeyNames="LecturerID"
                EmptyDataText="No lecturers found"
                OnRowEditing="gvLecturer_RowEditing"
                OnRowUpdating="gvLecturer_RowUpdating"
                OnRowCancelingEdit="gvLecturer_RowCancelingEdit"
                OnRowCommand="gvLecturer_RowCommand">

                <Columns>
                    <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Department" HeaderText="Department" />

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
                                OnClientClick="return confirm('Are you sure you want to delete this lecturer?');" />
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
