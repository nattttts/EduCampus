<%@ Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="Markspage.aspx.cs" 
    Inherits="lecturer.Markspage" %>

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
            align-items: center;
        }

        .button-row {
            display: flex;
            justify-content: space-between;
            margin-top: 20px;
        }

        .btn {
            width: 130px;
            height: 40px;
        }

        .markBox {
            width: 70px;
        }

        .message {
            display: block;
            margin: 10px 0;
            font-weight: bold;
            color: #b00020;
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
        Width="100%">

        <Columns>

            <asp:TemplateField HeaderText="Mark ID">
                <ItemTemplate>
                    <asp:HiddenField ID="hfMarkID" runat="server" Value='<%# Eval("MarkID") %>' />
                    <asp:Label ID="lblMarkID" runat="server" Text='<%# Eval("MarkID") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Detail ID" Visible="false">
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
            OnClick="btnSave_Click" />

    </div>

</div>

</form>

</body>
</html>
