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
                        <a class="nav-link" href="#">Courses</a>
                    </li>
                     <li class="nav-item">
                        <a class="nav-link active" href="RegisterLecturer.aspx">Register Lecturer</a>
                    </li>
                     <li class="nav-item">
                        <a class="nav-link" href="#">Assign Lecturer</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Register Student</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Enrolment</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Announcements</a>
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

    <!-- FORM -->
    <div class="container mt-5 d-flex justify-content-center">
        <div class="card shadow p-4" style="width: 400px;">
            
            <h3 class="text-center mb-4">👩‍🏫 Register Lecturer</h3>

            <div class="mb-3">
                <label class="form-label">Full Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label class="form-label">Lecturer Email</label>
                <asp:TextBox ID="txtLecturerEmail" runat="server" CssClass="form-control" autocomplete="off" />
            </div>

            <div class="mb-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtLecturerPass" runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    autocomplete="new-password" />
            </div>

            <div class="mb-3">
                <label class="form-label">Department</label>
                <asp:TextBox ID="txtDept" runat="server" CssClass="form-control" />
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

    <!-- SEARCH + GRID -->
    <div class="container mt-4">
        <div class="card shadow p-4">

            <h4 class="mb-3">📋 Registered Lecturers</h4>

            <!-- SEARCH -->
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

            <!-- GRIDVIEW -->
            <asp:GridView ID="gvLecturers" runat="server"
                CssClass="table table-bordered table-striped"
                AutoGenerateColumns="false"
                EmptyDataText="No lecturers found">

                <Columns>
                    <asp:BoundField DataField="LecturerID" HeaderText="ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Department" HeaderText="Department" />
                </Columns>

            </asp:GridView>

        </div>
    </div>

</form>
</body>
</html>
