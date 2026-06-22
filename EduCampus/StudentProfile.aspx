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

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

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


<!-- ================= PROFILE ================= -->

<div class="container mt-5">

    <div class="row justify-content-center">

        <div class="col-md-6">

            <div class="card shadow">

                <div class="card-header bg-dark text-white text-center">

                    <h3>
                        Student Profile
                    </h3>

                </div>

                <div class="card-body">

                    <!-- Student ID -->

                    <div class="mb-3">

                        <label class="fw-bold">
                            Student ID
                        </label>

                        <asp:TextBox ID="txtStudentID"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>

                    </div>

                    <!-- Name -->

                    <div class="mb-3">

                        <label class="fw-bold">
                            Name
                        </label>

                        <asp:TextBox ID="txtName"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>

                    </div>

                    <!-- Email -->

                    <div class="mb-3">

                        <label class="fw-bold">
                            Email
                        </label>

                        <asp:TextBox ID="txtEmail"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>

                    </div>

                    <!-- Programme -->

                    <div class="mb-3">

                        <label class="fw-bold">
                            Programme
                        </label>

                        <asp:TextBox ID="txtProgramme"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>

                    </div>

                    <hr />

                    <!-- Change Password Button -->

                    <div class="text-center">

                        <button type="button"
                            class="btn btn-primary"
                            data-bs-toggle="modal"
                            data-bs-target="#changePasswordModal">

                            Change Password

                        </button>

                    </div>

                </div>

            </div>

        </div>

    </div>

</div>

<!-- ================= CHANGE PASSWORD MODAL ================= -->

<div class="modal fade" id="changePasswordModal" tabindex="-1" runat="server">

    <div class="modal-dialog">

        <div class="modal-content">

            <div class="modal-header bg-dark text-white">

                <h5 class="modal-title">
                    Change Password
                </h5>

                <button type="button"
                    class="btn-close"
                    data-bs-dismiss="modal">
                </button>

            </div>

            <div class="modal-body">

                <!-- Current Password -->

                <div class="mb-3">

                    <label class="fw-bold">
                        Current Password
                    </label>

                    <div class="input-group">

                        <asp:TextBox ID="txtOldPassword"
                            runat="server"
                            TextMode="Password"
                            CssClass="form-control"
                            onkeyup="checkPassword()">
                        </asp:TextBox>

                        <button type="button"
                            class="btn btn-outline-secondary"
                            onclick="togglePassword('txtOldPassword',this)">

                            👁

                        </button>

                    </div>

                </div>

                <!-- New Password -->

                <div class="mb-3">

                    <label class="fw-bold">
                        New Password
                    </label>

                    <div class="input-group">

                        <asp:TextBox ID="txtNewPassword"
                            runat="server"
                            TextMode="Password"
                            CssClass="form-control"
                            onkeyup="checkPassword()">
                        </asp:TextBox>

                        <button type="button"
                            class="btn btn-outline-secondary"
                            onclick="togglePassword('txtNewPassword',this)">

                            👁

                        </button>

                    </div>

                    <small class="text-muted">
                        Password must be at least 8 characters.
                    </small>
                    <small id="passwordWarning"
                        class="text-danger">
                    </small>

                </div>

                <!-- Confirm Password -->

                <div class="mb-3">

                    <label class="fw-bold">
                        Confirm New Password
                    </label>

                    <div class="input-group">

                    <asp:TextBox ID="txtConfirmPassword"
                        runat="server"
                        TextMode="Password"
                        CssClass="form-control">
                    </asp:TextBox>

                        <button type="button"
                            class="btn btn-outline-secondary"
                            onclick="togglePassword('txtConfirmPassword',this)">

                            👁

                        </button>

                    </div>

                </div>

                <!-- Message -->

                <asp:Label ID="lblMessage"
                    runat="server"
                    CssClass="d-block text-center fw-bold">
                </asp:Label>

            </div>

            <div class="modal-footer">

                <button type="button"
                    class="btn btn-secondary"
                    data-bs-dismiss="modal">

                    Cancel

                </button>

                <asp:Button ID="btnChangePassword"
                    runat="server"
                    Text="Change Password"
                    CssClass="btn btn-primary"
                    OnClick="btnChangePassword_Click" />

            </div>

        </div>

    </div>

</div>

<!-- Bootstrap JS -->

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

<script>

    function openChangePasswordModal() {
        var modal = new bootstrap.Modal(
            document.getElementById("changePasswordModal")
        );

        modal.show();
    }

    function closeChangePasswordModal() {
        var modalElement =
            document.getElementById("changePasswordModal");

        var modal =
            bootstrap.Modal.getInstance(modalElement);

        if (modal) {
            modal.hide();
        }
    }

    function togglePassword(id, button) {
        var password =
            document.getElementById(id);


        if (password.type === "password") {
            password.type = "text";
            button.innerHTML = "🙈";
        }
        else {
            password.type = "password";
            button.innerHTML = "👁";
        }
    }

</script>

</form>

</body>

</html>