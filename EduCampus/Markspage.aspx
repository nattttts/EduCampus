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
            width: 1200px;
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

        .markBox {
            width: 70px;
        }

    </style>
</head>

<body>

<form id="form1" runat="server">

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
            ID="btnFilter"
            runat="server"
            Text="Filter"
            OnClick="btnFilter_Click" />

    </div>

    <asp:GridView
        ID="gvMarks"
        runat="server"
        AutoGenerateColumns="False"
        Width="100%">

        <Columns>

            <asp:BoundField DataField="MarkID" HeaderText="Mark ID" />
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