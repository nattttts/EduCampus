<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentProfile.aspx.cs" Inherits="EduCampus.StudentProfile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Profile</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
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
                             <a class="nav-link" href="StudentDashboard.aspx">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="StudentEnrollCourse.aspx">Course</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Attendance.aspx">Attendance</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="Results.aspx">Results</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link active" href="StudentProfile.aspx">Profile</a>
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

    <div class="row justify-content-center">

        <div class="col-md-6">

            <div class="card shadow">

                <div class="card-header bg-dark text-white text-center">
                    <h3>Student Profile</h3>
                </div>

                <div class="card-body">

                    <div class="mb-3">
                        <label class="fw-bold">Student ID</label>
                        <asp:TextBox ID="txtStudentID"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="fw-bold">Name</label>
                        <asp:TextBox ID="txtName"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="fw-bold">Email</label>
                        <asp:TextBox ID="txtEmail"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="fw-bold">Programme</label>
                        <asp:TextBox ID="txtProgramme"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                </div>

            </div>

        </div>

    </div>

</div>

</form>

</body>
</html>