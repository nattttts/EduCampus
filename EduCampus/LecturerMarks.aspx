<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="LecturerMarks.aspx.cs" 
    Inherits="EduCampus.Markspage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Marks Management</title>

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
                            <a class="nav-link" href="CourseMaterial.aspx">Course Materials</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AttendanceManagement.aspx">Attendance</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link active" href="LecturerMarks.aspx">Marks</a>
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

            </div>
        </nav>

<div class="container">

    <h2>Marks Management</h2>

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

        <asp:Button
            ID="btnLoadStudents"
            runat="server"
            Text="Load Students"
            OnClick="btnLoadStudents_Click" />

    </div>

    <asp:Label
        ID="lblMessage"
        runat="server"
        CssClass="message" />

   <asp:GridView
        ID="gvMarks"
        runat="server"
        AutoGenerateColumns="False"
        EnableViewState="true"
        Width="100%">

        <Columns>

            <asp:TemplateField HeaderText="Mark ID">
                <ItemTemplate>
                    <asp:HiddenField ID="hfMarkID" runat="server" Value='<%# Eval("MarkID") %>' />
                    <asp:Label ID="lblMarkID" runat="server" Text='<%# Eval("MarkID") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Detail ID">
                <ItemStyle CssClass="d-none" />
                    <HeaderStyle CssClass="d-none" />
                <ItemTemplate>
                    <asp:HiddenField ID="hfDetailID" runat="server" Value='<%# Eval("DetailID") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
            <asp:BoundField DataField="StudentName" HeaderText="Student Name" />

            <asp:TemplateField HeaderText="Assignment">
                <ItemTemplate>
                    <asp:TextBox ID="txtAssignment"
                        runat="server"
                        CssClass="markBox"
                        Text='<%# Eval("AssignmentMark") %>'
                        Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Quiz">
                <ItemTemplate>
                    <asp:TextBox ID="txtQuiz"
                        runat="server"
                        CssClass="markBox"
                        Text='<%# Eval("QuizMark") %>'
                        Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Mid Test">
                <ItemTemplate>
                    <asp:TextBox ID="txtMidTest"
                        runat="server"
                        CssClass="markBox"
                        Text='<%# Eval("MidTestMark") %>'
                        Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Final Exam">
                <ItemTemplate>
                    <asp:TextBox ID="txtFinalExam"
                        runat="server"
                        CssClass="markBox"
                        Text='<%# Eval("FinalExamMark") %>'
                        Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="FinalMark" HeaderText="Final Mark" />
            <asp:BoundField DataField="FinalGrade" HeaderText="Grade" />
            <asp:BoundField DataField="GradePoint" HeaderText="Grade Point" />

        </Columns>

    </asp:GridView>

    <div class="button-row">

        <asp:Button
            ID="btnEdit"
            runat="server"
            Text="Give Marks"
            CssClass="btn"
            Enabled="false"
            OnClick="btnEdit_Click" />

        <asp:Button
            ID="btnSave"
            runat="server"
            Text="Save Marks"
            CssClass="btn"
            Enabled="false"
            CausesValidation="false"
            OnClick="btnSave_Click" />

    </div>

</div>

</form>
</body>
</html>