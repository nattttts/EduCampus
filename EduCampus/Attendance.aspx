<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance.aspx.cs" Inherits="EduCampus.Attendance" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Attendance</title>

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

    <!-- FILTER -->
    <div class="row mb-3">
        <div class="col-md-4">

            <asp:DropDownList ID="ddlCourse"
                runat="server"
                CssClass="form-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
            </asp:DropDownList>

        </div>
    </div>

    <!-- GRID -->
    <asp:GridView ID="gvAttendance" runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-bordered">

        <Columns>

            <asp:BoundField DataField="CourseName" HeaderText="Course" />
            <asp:BoundField DataField="AttendanceDate" HeaderText="Date" />

            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <span class='<%# Eval("Status").ToString() == "Present" ? "present" : "absent" %>'>
                        <%# Eval("Status") %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />

        </Columns>

    </asp:GridView>

</div>

</form>

</body>
</html>