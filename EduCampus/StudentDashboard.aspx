﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentDashboard.aspx.cs" Inherits="EduCampus.StudentDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Student Dashboard</title>

    <style>

        body
        {
            font-family: Arial;
            margin: 0;
            background-color: #f5f5f5;
        }

        .menu
        {
            background-color: darkblue;
            padding: 15px;
        }

        .menu a
        {
            color: white;
            text-decoration: none;
            margin-right: 25px;
            font-weight: bold;
            font-size: 16px;
        }

        .dashboard
        {
            padding: 30px;
        }

        .card
        {
            width: 250px;
            background-color: white;
            padding: 20px;
            border-radius: 10px;
            display: inline-block;
            margin-right: 20px;
            margin-bottom: 20px;
            box-shadow: 0px 0px 10px lightgray;
            text-align: center;
        }

        .card h3
        {
            color: darkblue;
        }

        .title
        {
            color: darkblue;
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <!-- MENU -->

    <div class="menu">

        <a href="StudentDashboard.aspx">
            Dashboard
        </a>

        <a href="StudentEnrollCourse.aspx">
            View Enrolled Courses
        </a>

        <a href="StudentProfile.aspx">
            Profile
        </a>

        <asp:Button ID="btnLogout"
            runat="server"
            Text="Logout"
            OnClick="btnLogout_Click" />

    </div>

    <!-- DASHBOARD -->

    <div class="dashboard">

        <h1 class="title">
            Student Dashboard
        </h1>

        <h3>

            Welcome :

            <asp:Label ID="lblStudentName"
                runat="server">
            </asp:Label>

        </h3>

        <!-- CARD 1 -->

        <div class="card">

            <h3>Total Enrolled Courses</h3>

            <asp:Label ID="lblTotalCourses"
                runat="server"
                Font-Size="25px"
                Font-Bold="True">
            </asp:Label>

        </div>

        <!-- CARD 2 -->

        <div class="card">

            <h3>Approved Courses</h3>

            <asp:Label ID="lblApproved"
                runat="server"
                Font-Size="25px"
                Font-Bold="True">
            </asp:Label>

        </div>

        <!-- CARD 3 -->

        <div class="card">

            <h3>Pending Courses</h3>

            <asp:Label ID="lblPending"
                runat="server"
                Font-Size="25px"
                Font-Bold="True">
            </asp:Label>

        </div>

    </div>

</form>

</body>

</html>