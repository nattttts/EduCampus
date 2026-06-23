<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="CourseMaterial.aspx.cs"
    Inherits="EduCampus.CourseMaterial" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>View Course</title>

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
                            <a class="nav-link" href="LecturerDashboard.aspx">Dashboard</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="CourseMaterial.aspx">Course Materials</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AttendanceManagement.aspx">Attendance</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="LecturerMarks.aspx">Marks</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="LecturerAnnouncements.aspx">Announcements</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="LecturerProfile.aspx">Profile</a>
                        </li>
                    </ul>

                    <!-- Logout button -->
                    <asp:Button ID="btnLogout" runat="server"
                        Text="Logout"
                        CssClass="btn btn-danger"
                        OnClick="btnLogout_Click" />


            </div>
        </nav>

    <div class="page-title">View Course</div>

    <div class="container">

        <div class="box">
            <h3>Course materials (Upload / delete)</h3>

            <div class="filter-row">
                <asp:DropDownList
                    ID="ddlCourse"
                    runat="server"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                </asp:DropDownList>
            </div>

            <asp:FileUpload ID="fileUploadNotes" runat="server" />

            <asp:Button
                ID="btnUpload"
                runat="server"
                Text="Upload"
                CssClass="btn"
                OnClick="btnUpload_Click" />

            <br />

            <asp:Label
                ID="lblMessage"
                runat="server"
                ForeColor="Red">
            </asp:Label>

            <asp:GridView
                ID="gvNotes"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="grid"
                DataKeyNames="NoteID,FilePath"
                OnRowCommand="gvNotes_RowCommand">

                <Columns>

                    <asp:BoundField
                        DataField="FileName"
                        HeaderText="File Name" />

                    <asp:BoundField
                        DataField="UploadDate"
                        HeaderText="Upload Date"
                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />

                    <asp:TemplateField HeaderText="Download">
                        <ItemTemplate>
                            <asp:HyperLink
                                ID="lnkDownload"
                                runat="server"
                                Text="Download"
                                NavigateUrl='<%# Eval("FilePath") %>'
                                Target="_blank">
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:Button
                                ID="btnDelete"
                                runat="server"
                                Text="Delete"
                                CommandName="DeleteNote"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                OnClientClick="return confirm('Delete this file?');" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>
        </div>

        <div class="box">
            <h3>Student List</h3>

            <asp:GridView
                ID="gvStudents"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="grid">

                <Columns>

                    <asp:BoundField
                        DataField="StudentID"
                        HeaderText="Student ID" />

                    <asp:BoundField
                        DataField="FullName"
                        HeaderText="Student Name" />

                    <asp:BoundField
                        DataField="Email"
                        HeaderText="Email" />

                </Columns>

            </asp:GridView>
        </div>

    </div>

</form>
</body>
</html>
