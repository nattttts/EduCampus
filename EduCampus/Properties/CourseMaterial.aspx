<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="CourseMaterials.aspx.cs"
    Inherits="lecturer.CourseMaterials" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>View Course</title>

    <style>
        body {
            background-color: #F8DDDD;
            font-family: Arial;
        }

        .page-title {
            color: #999;
            margin-left: 10px;
        }

        .container {
            width: 1000px;
            margin: 10px auto;
            background-color: #A4D8FF;
            padding: 35px;
        }

        .box {
            background: white;
            padding: 25px;
            margin: 25px auto;
            width: 85%;
            text-align: center;
        }

        .filter-row {
            margin-bottom: 20px;
        }

        .grid {
            width: 100%;
            margin-top: 15px;
        }

        .btn {
            padding: 8px 18px;
            margin: 5px;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

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