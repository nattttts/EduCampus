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
                          <a class="nav-link active" href="StudentDashboard.aspx">Home</a>
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
                         <a class="nav-link" href="StudentProfile.aspx">Profile</a>
                     </li>
                 </ul>

             </div>

         </div>
     </nav>

    <!-- SEM BUTTONS -->
    <div class="text-center mb-4">

        <asp:Button ID="btnSem1" runat="server"
            Text="SEM 1"
            CssClass="btn btn-primary mx-2"
            OnClick="btnSem_Click"
            CommandArgument="Semester 1" />

        <asp:Button ID="btnSem2" runat="server"
            Text="SEM 2"
            CssClass="btn btn-secondary mx-2"
            OnClick="btnSem_Click"
            CommandArgument="Semester 2" />

    </div>

    <!-- RESULTS -->
    <asp:GridView ID="gvResults" runat="server"
        CssClass="table table-bordered"
        AutoGenerateColumns="true">
    </asp:GridView>

</div>

</form>

</body>
</html>