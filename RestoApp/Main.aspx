<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="RestoApp.Main1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h4 class="text-gray-100">Hola, <%= usuario?.Nombres %> <%= usuario?.Apellidos %> (<%= usuario?.Tipo %>)</h4>

    <!-- Agregar Mesas habilitadas, mozos habilitados, productos, -->
    <div class="row">

        <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente)
            {%>
        <div class="col-5">
            <div class="bg-gray-100 rounded border-1 p-5">
                <div class="d-flex">
                    <p>Mesas Activas:<span class="fw-semibold"> <%= MesasActivas %></span></p>
                    <a class="link-dark ms-4" href="MesaHabilitar.aspx">Activar Mesas</a>
                </div>
                <div class="d-flex">
                    <p>Mesas Asignadas:<span class="fw-semibold"> <%= MesasAsignadas %></span></p>
                    <a class="link-dark ms-4" href="MesaHabilitar.aspx">Asignar Mesas</a>
                </div>
            </div>
        </div>
        <div class="col-5">
            <div class="bg-gray-100 p-5 rounded border-1">
                 <div class="d-flex">
                    <p>Meseros Presentes:<span class="fw-semibold"> <%= MesasAsignadas %></span></p>
                    <a class="link-dark ms-4" href="MesaHabilitar.aspx">Asignar Mesas</a>
                </div>
            </div>
        </div>
        <div class="col-5 mt-3">
            <div class="bg-gray-100 p-5 rounded border-1">
                Pedidos Abiertos
            </div>
        </div>
        <div class="col-5 mt-3">
            <div class="bg-gray-100 p-5 rounded border-1">
                Pedidos Cerredos
            </div>
        </div>
        <% }%>
   </div>
    

</asp:Content>
