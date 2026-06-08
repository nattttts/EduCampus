<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentEnrollCourse.aspx.cs" Inherits="EduCampus.StudentEnrollCourse" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Course Management</title>

    <!-- CSS -->
    <style>
        body { font-family: Arial; margin: 0; background-color: #f5f5f5; }

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

        .container {
            padding: 30px;
        }

        h2 {
            color: darkblue;
        }

        .grid {
            width: 100%;
            margin-top: 15px;
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
    </div>>

</div>

<!-- PAGE CONTENT -->
<div class="container">

    <h2>Available Courses</h2>

    <asp:GridView ID="gvCourses" runat="server"
        AutoGenerateColumns="False"
        CssClass="grid">

        <Columns>
            <asp:BoundField DataField="OfferingID" HeaderText="ID" />
            <asp:BoundField DataField="CourseCode" HeaderText="Code" />
            <asp:BoundField DataField="CourseName" HeaderText="Name" />
            <asp:BoundField DataField="CreditHours" HeaderText="Credit" />

            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnEnroll" runat="server"
                        Text="Enroll"
                        CommandArgument='<%# Eval("OfferingID") %>'
                        OnClick="btnEnroll_Click" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

    <hr />

    <h2>My Enrolled Courses</h2>

    <asp:GridView ID="gvEnrollment" runat="server"
        AutoGenerateColumns="False"
        CssClass="grid">

        <Columns>
            <asp:BoundField DataField="CourseCode" HeaderText="Code" />
            <asp:BoundField DataField="CourseName" HeaderText="Course" />
            <asp:BoundField DataField="Status" HeaderText="Status" />

            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnDrop" runat="server"
                        Text="Drop"
                        CommandArgument='<%# Eval("EnrolmentID") %>'
                        OnClick="btnDrop_Click" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>

    <br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>

</div>

</form>

</body>
</html>