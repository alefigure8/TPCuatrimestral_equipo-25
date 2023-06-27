<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="RestoApp.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Perfil" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<div class="container">
        <div class="col" style="align-items:center; justify-content:center;">
                <h1>Perfil</h1>
               
        <div class="row">
                <asp:Label ID="LblID" runat="server" Text="Nombre"></asp:Label>
                <asp:TextBox ID="TxtId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        <div class="row">
            
                <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
       
        <div class="row">
            
                <asp:Label ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                <asp:TextBox ID="TxtApellido" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
     
        <div class="row">
      
                <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
       <div class="row">
      
                <asp:Label ID="LblPassword" runat="server" Text="Email"></asp:Label>
                <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          </div>
    </div>
    
</asp:Content>

