<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RestoApp.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="container">
        <div class="d-flex flex-column justify-content-center align-items-center vh-100">
            <h1 class="my-4">RestoApp</h1>
            <div class="col-sm-4 bg-body-secondary p-3 border rounded-3">
                <h2 class="fw-bold fs-4 text-center my-3">LOGIN</h2>
                <div class="mb-3 row">
                    <label for="staticEmail" class="col-sm-2 col-form-label">Email</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txb_Usuario" type="text" class="form-control" value="email@example.com" />
                    </div>
                </div>
                <div class="mb-3 row">
                    <label for="inputPassword" class="col-sm-2 col-form-label">Password</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txb_Password" type="password" class="form-control" />
                    </div>
                </div>
                <div class="d-flex justify-content-center">
                    <asp:Button Text="Acceder" runat="server" OnClick="EnviarDatos_Click" CssClass="btn btn-dark btn-block" />
                </div>
                <asp:Label CssClass="text-danger" runat="server" ID="lbl_error" />
            </div>
        </div>
    </div>
</asp:Content>
