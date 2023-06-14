<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Default.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="RestoApp.Menu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<div class="container">
    <div class="row">
        <h2 class="h2">Menú disponible</h2>
    </div>
    <div class="row">
        <h2 class="h2"> Platos </h2>
    </div>

   <asp:Repeater runat="server" ID="ProductoRepetidor">
       <ItemTemplate>
           <div class="row">
                   <div class="card-body">
                       <h5 class="card-title"> <%#Eval("Nombre")%> </h5>
                       <br />
                       <p class="blockquote-footer">Descrípcion: <%#Eval("Descripcion")%> <br />Valor: $<%#Eval("Valor")%></p>
                   </div>
           </div>
       </ItemTemplate>
   </asp:Repeater>

</div>

</asp:Content>
