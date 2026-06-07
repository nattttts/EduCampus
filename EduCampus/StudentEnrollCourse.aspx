<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentEnrollCourse.aspx.cs" Inherits="EduCampus.StudentEnrollCourse" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Course Management</title>

    <style>
        body { font-family: Arial; margin: 20px; }

        .menu {
            background-color: darkblue;
            padding: 15px;
            margin-bottom: 20px;
        }

        .menu a {
            color: white;
            margin-right: 20px;
            text-decoration: none;
        }

        h2 { color: darkblue; margin-top: 20px; }

        .grid { margin-top: 15px; }
    </style>

        </head>

        <body>

        <form id="form1" runat="server">

        <!-- MENU -->
        <div class="menu">
            <a href="StudentDashboard.aspx">Dashboard</a>
            <a href="StudentEnrollCourse.aspx">Courses</a>
        </div>

        <h2>Available Courses</h2>

        <asp:GridView ID="gvCourses" runat="server"
            AutoGenerateColumns="False"
            CssClass="grid"
            Width="100%">

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
            CssClass="grid"
            Width="100%">

            <Columns>

                <asp:BoundField DataField="CourseCode" HeaderText="Code" />
                <asp:BoundField DataField="CourseName" HeaderText="Course" />
                <asp:BoundField DataField="Status" HeaderText="Status" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnDrop" runat="server"
                            Text="Drop"
                            CommandArgument='<%# Eval("EnrolmentID") %>'
                            OnClick="btnDrop_Click"
                            CssClass="btn btn-danger" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

        <br />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>

        </form>

</body>
</html>