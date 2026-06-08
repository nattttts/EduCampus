<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Results.aspx.cs" Inherits="EduCampus.Results" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Results</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body>
<form id="form1" runat="server">

<div class="container mt-5">

    <!-- SEMESTER BUTTONS -->
    <div class="d-flex justify-content-start mb-3">

    <asp:Button ID="btnBackDashboard"
        runat="server"
        Text="← Back to Dashboard"
        CssClass="btn btn-secondary"
        OnClick="btnBackDashboard_Click" />
    
    </div>
    <div class="text-center mb-4">
        <asp:Button ID="btnSem1" runat="server" Text="SEM 1"
            CssClass="btn btn-primary mx-2"
            OnClick="btnSem_Click"
            CommandArgument="Semester 1" />

        <asp:Button ID="btnSem2" runat="server" Text="SEM 2"
            CssClass="btn btn-secondary mx-2"
            OnClick="btnSem_Click"
            CommandArgument="Semester 2" />
    </div>

    <!-- RESULTS TABLE (SCROLLABLE) -->
    <div style="max-height:500px; overflow-y:auto;">
        <asp:GridView ID="gvResults" runat="server"
            CssClass="table table-bordered table-striped"
            AutoGenerateColumns="true">
        </asp:GridView>
    </div>

</div>

</form>
</body>
</html>