<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Marks.aspx.cs"
    Inherits="lecturer.Marks" %>

<!DOCTYPE html>

<html>
<head runat="server">

    <title>Marks Management</title>

    <style>

        body {
            background-color: #A4D8FF;
            font-family: Arial;
        }

        .container {
            width: 1000px;
            margin: 20px auto;
            background: white;
            padding: 20px;
            border-radius: 10px;
        }

        .filter-row {
            display: flex;
            gap: 15px;
            margin-bottom: 20px;
        }

        .button-row {
            display: flex;
            justify-content: space-between;
            margin-top: 20px;
        }

        .btn {
            width: 100px;
            height: 40px;
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

    <h2>Marks Management</h2>

    <div class="filter-row">

        <asp:DropDownList
            ID="ddlSemester"
            runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged">
        </asp:DropDownList>

        <asp:DropDownList
            ID="ddlCourse"
            runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
        </asp:DropDownList>

        <asp:DropDownList
            ID="ddlClass"
            runat="server">
        </asp:DropDownList>

        <asp:Button
            ID="btnFilter"
            runat="server"
            Text="Filter"
            OnClick="btnFilter_Click" />

    </div>

    <asp:GridView
        ID="gvMarks"
        runat="server"
        Width="100%"
        AutoGenerateColumns="False">

        <Columns>

            <asp:BoundField
                DataField="ResultID"
                HeaderText="Result ID" />

            <asp:BoundField
                DataField="StudentID"
                HeaderText="Student ID" />

            <asp:BoundField
                DataField="StudentName"
                HeaderText="Student Name" />

            <asp:TemplateField HeaderText="Mark">

                <ItemTemplate>

                    <asp:TextBox
                        ID="txtMark"
                        runat="server"
                        Text='<%# Eval("Mark") %>'
                        Enabled="false"
                        Width="80">
                    </asp:TextBox>

                </ItemTemplate>

            </asp:TemplateField>

            <asp:BoundField
                DataField="Grade"
                HeaderText="Grade" />

        </Columns>

    </asp:GridView>

    <div class="button-row">

        <asp:Button
            ID="btnEdit"
            runat="server"
            Text="Edit"
            CssClass="btn"
            OnClick="btnEdit_Click" />

        <asp:Button
            ID="btnSave"
            runat="server"
            Text="Save"
            CssClass="btn"
            OnClick="btnSave_Click" />

    </div>

</div>

</form>

</body>
</html>