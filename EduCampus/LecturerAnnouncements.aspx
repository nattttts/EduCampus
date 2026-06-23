<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LecturerAnnouncements.aspx.cs" Inherits="EduCampus.LecturerAnnouncements" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Lecturer Announcements</title>

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
                            <a class="nav-link" href="LecturerDashboard.aspx">Dashboard</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="CourseMaterial.aspx">Course Materials</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AttendanceManagement.aspx">Attendance</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="LecturerMarks.aspx">Marks</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="LecturerAnnouncements.aspx">Announcements</a>
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

            </div>
        </nav>

        <!-- Form -->
        <div class="container mt-5 d-flex justify-content-center">
            <div class="card shadow p-4" style="width: 700px;">

                <h3 class="text-center mb-4">📢 Create Announcement</h3>

                <div class="mb-3">
                    <label class="form-label">Course Offering</label>
                    <asp:DropDownList ID="ddlOffering" runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label class="form-label">Title</label>
                    <asp:TextBox ID="txtTitle" runat="server"
                        CssClass="form-control"
                        placeholder="Enter announcement title" />
                </div>

                <div class="mb-3">
                    <label class="form-label">Message</label>
                    <asp:TextBox ID="txtMessage" runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="5"
                        placeholder="Enter announcement message" />
                </div>

                <div class="d-grid">
                    <asp:Button ID="btnPostAnnouncement" runat="server"
                        Text="Post Announcement"
                        CssClass="btn btn-primary"
                        OnClick="btnPostAnnouncement_Click" />
                </div>

                <div class="mt-3 text-center">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>

            </div>
        </div>

        <!-- Search + Grid -->
        <div class="container mt-4">
            <div class="card shadow p-4">

                <h4 class="mb-3">📋 Announcements</h4>

                <!-- Search -->
                <div class="row mb-3">

                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchTitle" runat="server"
                            CssClass="form-control"
                            Placeholder="Enter announcement title" />
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="btnSearch" runat="server"
                            Text="Search"
                            CssClass="btn btn-primary w-100"
                            OnClick="btnSearch_Click" />
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="btnReset" runat="server"
                            Text="Reset"
                            CssClass="btn btn-secondary w-100"
                            OnClick="btnReset_Click" />
                    </div>

                </div>

                        <asp:GridView ID="gvAnnouncements" runat="server"
                            CssClass="table table-bordered table-striped"
                            AutoGenerateColumns="False"
                            DataKeyNames="AnnouncementID"
                            EmptyDataText="No announcements found"
                            OnRowEditing="gvAnnouncements_RowEditing"
                            OnRowUpdating="gvAnnouncements_RowUpdating"
                            OnRowCancelingEdit="gvAnnouncements_RowCancelingEdit"
                            OnRowCommand="gvAnnouncements_RowCommand">

                            <Columns>

                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField
                                    DataField="CourseOffering"
                                    HeaderText="Course Offering"
                                    ReadOnly="True" />

                                <asp:TemplateField HeaderText="Title">

                                    <ItemTemplate>
                                        <%# Eval("Title") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTitle"
                                            runat="server"
                                            CssClass="form-control"
                                            Text='<%# Bind("Title") %>' />
                                    </EditItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Message">

                                    <ItemTemplate>
                                        <%# Eval("Message") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditMessage"
                                            runat="server"
                                            CssClass="form-control"
                                            TextMode="MultiLine"
                                            Rows="5"
                                            Text='<%# Bind("Message") %>' />
                                    </EditItemTemplate>

                                </asp:TemplateField>

                                <asp:BoundField
                                    DataField="PostedDateTime"
                                    HeaderText="Posted Date"
                                    DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                    ReadOnly="True" />

                                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="200px">

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnEdit"
                                            runat="server"
                                            CommandName="Edit"
                                            Text="Edit"
                                            CssClass="btn btn-primary btn-sm me-2" />

                                        <asp:LinkButton ID="btnDelete"
                                            runat="server"
                                            CommandName="DeleteRow"
                                            Text="Delete"
                                            CssClass="btn btn-danger btn-sm"
                                            OnClientClick="return confirm('Are you sure you want to delete this announcement?');" />

                                    </ItemTemplate>

                                    <EditItemTemplate>

                                        <asp:LinkButton ID="btnUpdate"
                                            runat="server"
                                            CommandName="Update"
                                            Text="Update"
                                            CssClass="btn btn-success btn-sm me-2" />

                                        <asp:LinkButton ID="btnCancel"
                                            runat="server"
                                            CommandName="Cancel"
                                            Text="Cancel"
                                            CssClass="btn btn-secondary btn-sm" />

                                    </EditItemTemplate>

                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>

                    </div>
            </div> 
    </form>
</body>
</html>