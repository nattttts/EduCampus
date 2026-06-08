<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentProfile.aspx.cs" Inherits="EduCampus.StudentProfile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Profile</title>

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="style.css" rel="stylesheet" />

     <style>
        body {
            font-family: Arial;
            margin: 0;
            background-color: #f5f5f5;
        }

        /* NAVBAR */
        .topbar {
            background-color: darkblue;
            padding: 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .menu {
            display: flex;
            gap: 25px;
            justify-content: center;
            flex: 1;
        }

        .menu a {
            color: white;
            text-decoration: none;
            font-weight: bold;
        }

        .logout {
            color: white;
        }

        /* PAGE */
        .container {
            margin-top: 40px;
        }
    </style>

</head>

<body>

<form id="form1" runat="server">

<!-- NAVIGATION -->
<div class="topbar">

    <div class="menu">
        <a href="StudentDashboard.aspx">Dashboard</a>
        <a href="StudentEnrollCourse.aspx">Courses</a>
        <a href="Results.aspx">Results</a>
        <a href="Attendance.aspx">Attendance</a>
        <a href="StudentProfile.aspx">Profile</a>
    </div>

</div>

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