<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Mesas.aspx.cs" Inherits="RestoApp.Mesas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- PANEL -->
        <div class="d-flex flex-column container">
                            <h1>Mesas</h1>
                <p id="titulo_gerente_Mesas">Asignar Mesas a Meseros</p>
            <div class="col">
                <!--MESAS-->
                <div class="row" id="mesas">
                </div>
                <!--FIN MESAS-->
            </div>
            <!-- GERENTE: BTN GUARDAR MESA-->
            <div class="d-flex justify-content-end my-2">
                <input type="button" class="btn btn-dark invisible" value="Guardar Mesas" id="btnGuardarMesas">
            </div>
            <!--FIN GERENTE: BTN GUARDAR MESA-->
            <div class="p-3">
                <!-- DROPDOWN-->
                <% if (usuario?.Tipo.Descripcion == Opciones.ColumnasDB.TipoUsuario.Gerente)
                    { %>
                <div class="dropdown">
                    <button
                        class="btn btn-secondary dropdown-toggle"
                        type="button"
                        id="btnDDMesa"
                        data-bs-toggle="dropdown"
                        aria-expanded="false">
                        Agregar Mesas
                    </button>
                    <ul
                        class="dropdown-menu"
                        aria-labelledby="dropdownMenuButton1"
                        id="dropdown-mesa">
                    </ul>
                </div>

                <%} %>
                <!--FIN DROPDOWN-->
            </div>
        </div>
    <!-- FIN PANEL-->
</asp:Content>
