﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentNotes.aspx.cs" Inherits="EduCampus.StudentNotes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Course Notes</title>

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
                    <a class="nav-link" href="Results.aspx">Results</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link" href="StudentNotification.aspx">Notifications</a>
                </li>


                <li class="nav-item">
                    <a class="nav-link active" href="StudentNotes.aspx">Notes</a>
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
    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger fw-bold"></asp:Label>

    <h2>

        Course Notes :

        <asp:Label ID="lblCourseName"
            runat="server">
        </asp:Label>

    </h2>

    <!-- GRIDVIEW -->

    <asp:GridView ID="gvNotes"
        runat="server"
        AutoGenerateColumns="False"
        Width="1000px">

        <Columns>

            <asp:BoundField DataField="WeekNo"
                HeaderText="Week" />

            <asp:BoundField DataField="FileName"
                HeaderText="File Name" />

            <asp:BoundField DataField="UploadDate"
                HeaderText="Upload Date" />

            <asp:HyperLinkField
                HeaderText="Download"
                Text="Download"
                DataNavigateUrlFields="FilePath" />

        </Columns>

    </asp:GridView>

</form>

</body>

</html>