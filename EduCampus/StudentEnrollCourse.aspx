<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentEnrollCourse.aspx.cs" Inherits="EduCampus.StudentEnrollCourse" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Enroll Course</title>

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