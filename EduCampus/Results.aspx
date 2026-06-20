<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="EduCampus.Results" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Results</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />

    <style>

        .result-paper{

            background:white;

            width:90%;

            max-width:1100px;

            margin:auto;

            padding:40px;

            border-radius:10px;

            box-shadow:
            0 5px 20px rgba(0,0,0,0.15);

        }

        .student-info{

            display:flex;

            justify-content:space-between;

            margin-bottom:30px;

            font-size:16px;

        }

        .table-responsive{

            overflow-x:auto;

        }

        .table{

            width:100%;

            white-space:nowrap;

        }

        .result-summary{

            margin-top:30px;

            display:flex;

            justify-content:flex-end;

            gap:40px;

            font-size:20px;

            font-weight:bold;

        }

        </style>
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

    <div class="result-paper">

        <h2 class="text-center mb-4">
            Academic Results
        </h2>

        <!-- Semester Buttons -->
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

        <!-- Student Information -->
        <div class="student-info">

            <div>
                <asp:Label ID="lblStudentName" runat="server"/>
                <br />

                <asp:Label ID="lblStudentID" runat="server"/>
                <br />

                <asp:Label ID="lblProgramme" runat="server"/>
            </div>

            <div>

                <asp:Label ID="lblSession" runat="server"/>
                <br />

                <asp:Label ID="lblSemester" runat="server"/>
                <br />

                <asp:Label ID="lblDate" runat="server"/>

            </div>

        </div>

        <asp:Panel ID="pnlResult" runat="server">

            <div class="table-responsive">

            <asp:GridView ID="gvResults"
                runat="server"
                CssClass="table table-bordered text-center"
                AutoGenerateColumns="true">
            </asp:GridView>

            </div>

            <!-- GPA CGPA -->
            <div class="result-summary">

                <asp:Label ID="lblGPA"
                    runat="server">
                </asp:Label>


                <asp:Label ID="lblCGPA"
                    runat="server">
                </asp:Label>

        </div>

        </asp:Panel>

        <!-- No Result Message -->

        <asp:Panel ID="pnlNoResult"
            runat="server"
            Visible="false">

            <h4 class="text-center text-muted mt-5">

                No results available for this semester.

            </h4>

        </asp:Panel>

    </div>

</div>

</div>

</form>

</body>
</html>