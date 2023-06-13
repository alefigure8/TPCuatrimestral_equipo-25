<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MenuAdministrador.aspx.cs" Inherits="RestoApp.MenuUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:Button ID="MenuUsuarios" runat="server" Text="Adminitrar Usuarios" />
    <asp:Button ID="MenuMesas" runat="server" Text="Administrar Mesas" />
    <asp:Button ID="MenuProductos" runat="server" Text="Administrar Productos" />
    <asp:Button ID="MenuCalendario" runat="server" Text="Ver Calendario" />
    <asp:Button ID="MenuReportes" runat="server" Text="Ver Reportes" />
    
<%--    menu usuarios: ABML meseros
    menu mesas: ABML mesas
    menu productos: ABML productos
    menu calendario:  ABML Calendario 
    - meseros y mesas activos, mesero x mesa, menu disponible 
    - ver/setear/modificar meseros, mesas, pedidos, menues del día
    - ver meseros/mesas/pedidos pasados
    - cargar meseros/mesas/menues para un rango de fechas (futuro)
    menu reportes: ver reportes por fecha
--%>

    

</asp:Content>
