<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="EduCampus.Results" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Results</title>

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
                        <a class="nav-link" href="StudentDashboard.aspx">Home</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link" href="StudentEnrollCourse.aspx">Course</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link" href="Attendance.aspx">Attendance</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link active" href="Results.aspx">Results</a>
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

    <div class="container mt-4">

    <div class="card shadow-sm border-0">

        <div class="card-body">

            <h2 class="text-center mb-4">
                Academic Results
            </h2>

            <div class="text-center mb-4">

                <asp:Button ID="btnSem1"
                    runat="server"
                    Text="Semester 1"
                    CssClass="btn btn-primary mx-2"
                    OnClick="btnSem_Click"
                    CommandArgument="Semester 1" />

                <asp:Button ID="btnSem2"
                    runat="server"
                    Text="Semester 2"
                    CssClass="btn btn-outline-primary mx-2"
                    OnClick="btnSem_Click"
                    CommandArgument="Semester 2" />

            </div>

            <asp:GridView ID="gvResults"
                runat="server"
                CssClass="table table-bordered table-hover"
                AutoGenerateColumns="true">
            </asp:GridView>

        </div>

    </div>

</div>

</form>

</body>
</html>