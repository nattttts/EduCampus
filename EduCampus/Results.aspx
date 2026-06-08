<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="EduCampus.Results" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Results</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            font-family: Arial;
            margin: 0;
            background-color: #f5f5f5;
        }

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

        .container {
            margin-top: 40px;
        }
    </style>

</head>

<body>

<form id="form1" runat="server">

<!-- NAVBAR -->
<div class="topbar">

    <div class="menu">
        <a href="StudentDashboard.aspx">Dashboard</a>
        <a href="StudentEnrollCourse.aspx">Courses</a>
        <a href="Results.aspx">Results</a>
        <a href="Attendance.aspx">Attendance</a>
        <a href="StudentProfile.aspx">Profile</a>
    </div>

</div>

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