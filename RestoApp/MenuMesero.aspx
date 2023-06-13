<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MenuMesero.aspx.cs" Inherits="RestoApp.MenuMesero" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Button ID="VerMesas" runat="server" Text="Ver mis mesas" />
   <%-- el mesero ve un listado / mapa de sus mesas, con el detalle de cuales se encuentran en curso
    / opcion cerrar mesa / valida si tiene  algun pedido en curso--%>

    <asp:Button ID="AbrirMesa" runat="server" Text="Abrir mesa" />
   <%-- abre una mesa disponible, validar si tengo mesas disponibles--%>

    <asp:Button ID="Pedidos" runat="server" Text="Pedidos en curso" />
   <%-- lista los pedidos en curso, ver/modificar/cerrar pedidos--%>

    <asp:Button ID="Ver menu disponible" runat="server" Text="Ver menu disponible" />
   <%-- lista el menu disponible, deberia poder agregar a los pedidos abiertos?--%>

</asp:Content>
