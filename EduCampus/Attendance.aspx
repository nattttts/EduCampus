<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Attendance.aspx.cs"
    Inherits="EduCampus.Attendance" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Attendance</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
</head>

<body>
    <form id="form1" runat="server">
        <!-- Navigation bar -->
        <nav class="navbar navbar-expand-lg bg-white">
            <div class="container-fluid">
                <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2" />

                <div class="collapse navbar-collapse">
                    <!-- Menu -->
                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="#">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="CourseMaterial.aspx">Courses</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Attendance.aspx">Attendance</a>
                        </li>
                         <li class="nav-item">
                                              <a class="nav-link" href="Markspage.aspx">Marks</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="LecturerAnnouncements.aspx">Announcements</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Profile</a>
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


<div class="container">

    <h2>Attendance Management</h2>

    <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

    <div class="filter-row">

        <asp:DropDownList
            ID="ddlSession"
            runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
        </asp:DropDownList>

        <asp:DropDownList
            ID="ddlCourse"
            runat="server">
        </asp:DropDownList>

        <asp:TextBox
            ID="txtAttendanceDate"
            runat="server"
            TextMode="Date">
        </asp:TextBox>

        <asp:Button
            ID="btnLoadStudents"
            runat="server"
            Text="Load Students"
            OnClick="btnLoadStudents_Click" />

    </div>

    <asp:GridView
        ID="gvAttendance"
        runat="server"
        AutoGenerateColumns="False"
        Width="100%"
        BorderWidth="1">

        <Columns>
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:HiddenField ID="hfAttendanceID" runat="server" Value='<%# Eval("AttendanceID") %>' />
                    <asp:HiddenField ID="hfDetailID" runat="server" Value='<%# Eval("DetailID") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField
                DataField="StudentID"
                HeaderText="Student ID" />

            <asp:BoundField
                DataField="StudentName"
                HeaderText="Student Name" />

            <asp:BoundField
                DataField="AttendanceDate"
                HeaderText="Date"
                DataFormatString="{0:dd/MM/yyyy}" />

            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:DropDownList
                        ID="ddlStatus"
                        runat="server"
                        Enabled="false">
                        <asp:ListItem Value="Present">Present</asp:ListItem>
                        <asp:ListItem Value="Absent">Absent</asp:ListItem>
                        <asp:ListItem Value="Late">Late</asp:ListItem>
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Remarks">
                <ItemTemplate>
                    <asp:TextBox
                        ID="txtRemarks"
                        runat="server"
                        Enabled="false"
                        Text='<%# Eval("Remarks") %>'>
                    </asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <div class="button-row">
        <asp:Button
            ID="btnEdit"
            runat="server"
            Text="Take Attendance"
            CssClass="btn"
            Enabled="false"
            OnClick="btnEdit_Click" />

        <asp:Button
            ID="btnSave"
            runat="server"
            Text="Save"
            CssClass="btn"
            Enabled="false"
            OnClick="btnSave_Click" />
    </div>

</div>

</form>
</body>
</html>