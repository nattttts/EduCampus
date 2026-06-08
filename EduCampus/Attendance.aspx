<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance.aspx.cs" Inherits="EduCampus.Attendance" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Attendance</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

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

        /* PAGE */
        .container {
            margin-top: 40px;
        }

        .present {
            background-color: #28a745;
            color: white;
            padding: 5px 10px;
            border-radius: 6px;
            display: inline-block;
        }

        .absent {
            background-color: #dc3545;
            color: white;
            padding: 5px 10px;
            border-radius: 6px;
            display: inline-block;
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