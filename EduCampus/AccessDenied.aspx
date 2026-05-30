<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="EduCampus.AccessDenied" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Access Denied</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>

<body class="bg-light">

<form id="form1" runat="server">

<div class="container vh-100 d-flex justify-content-center align-items-center">

    <div class="text-center">

        <h2 class="text-danger mb-3">Access Denied</h2>

        <p class="mb-4">
            You do not have permission to access this page.
        </p>

        <asp:Button 
            ID="btnBack" 
            runat="server"
            Text="Go Back"
            CssClass="btn btn-dark"
            OnClick="btnBack_Click" />

    </div>

</div>

</form>

</body>
</html>