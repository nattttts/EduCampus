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

        width:95%;

        max-width:1200px;

        margin:auto;

        padding:35px;

        border-radius:10px;

        box-shadow:
        0 5px 20px rgba(0,0,0,0.15);

        overflow:hidden;

    }


    .student-info{

        display:flex;

        justify-content:space-between;

        margin-bottom:30px;

        font-size:16px;

    }


    /* Result Table */

    .result-table{

        width:100%;

        table-layout:auto;

        border-radius:10px;

        overflow:hidden;

        font-size:13px;

    }


    /* Header */

    .result-table th{

        background:#f1f5f9;

        color:#334155;

        font-weight:600;

        text-align:center;

        padding:10px 6px;

        letter-spacing:0.2px;

        font-size:12px;

    }


    /* Body */

    .result-table td{

        text-align:center;

        padding:8px 6px;

        color:#475569;

        font-size:13px;

        word-wrap:break-word;

        white-space:normal;

    }


    /* Course Code */

    .result-table th:nth-child(1),
    .result-table td:nth-child(1){

        width:90px;

        font-weight:600;

    }


    /* Course Name */

    .result-table th:nth-child(2),
    .result-table td:nth-child(2){

        width:220px;

        max-width:220px;

        text-align:left;

        white-space:normal;

        word-wrap:break-word;

    }


    /* Credit Hours */

    .result-table th:nth-child(3),
    .result-table td:nth-child(3){

        width:70px;

    }


    /* Marks */

    .result-table th:nth-child(4),
    .result-table td:nth-child(4),
    .result-table th:nth-child(5),
    .result-table td:nth-child(5),
    .result-table th:nth-child(6),
    .result-table td:nth-child(6),
    .result-table th:nth-child(7),
    .result-table td:nth-child(7),
    .result-table th:nth-child(8),
    .result-table td:nth-child(8){

        width:85px;

    }


    /* Grade */

    .result-table th:nth-child(9),
    .result-table td:nth-child(9){

        width:70px;

        font-weight:bold;

    }


    /* Grade Point */

    .result-table th:nth-child(10),
    .result-table td:nth-child(10){

        width:80px;

    }


    /* GPA CGPA */

    .result-summary{

        margin-top:30px;

        display:flex;

        justify-content:center;

        gap:50px;

        font-size:18px;

        font-weight:bold;

        flex-wrap:wrap;

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

            <div class="text-center mb-4">

                <asp:DropDownList ID="ddlSemester"
                    runat="server"
                    CssClass="form-select w-25 mx-auto"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged">
                </asp:DropDownList>

            </div>

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

            <asp:GridView ID="gvResults"
                runat="server"
                CssClass="table table-bordered table-hover result-table"
                AutoGenerateColumns="true">
            </asp:GridView>


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

</form>

</body>
</html>