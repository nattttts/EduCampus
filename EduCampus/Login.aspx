<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="EduCampus.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="style.css" rel="stylesheet" />
    <style>
        .login-btn {
            background-color: #35393C;
            color: white;
            border: none;
        }

        .login-btn:hover {
            background-color: black;
            color: white;
        }
    </style>
</head>

<body>

<form id="form1" runat="server">

<div class="container">
    <div class="row justify-content-center align-items-center vh-100">

        <div class="col-md-4">
            <div class="card shadow p-4">

                <div class="text-center mb-3">
                    <img src="logo.jpeg" alt="Logo" style="width:120px; height:auto;" />
                </div>

                <h3 class="text-center mb-4">Login</h3>

                <div class="mb-3">
                    <label class="mb-2">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter email e.g. student@example.com"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="mb-2">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter password"></asp:TextBox>
                </div>

                <asp:Button ID="btnLogin" runat="server"
                    Text="Login"
                    CssClass="btn login-btn w-100 mt-3"
                    OnClick="btnLogin_Click" />

                <br />

                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            </div>
        </div>

    </div>
</div>

</form>
</body>
</html>