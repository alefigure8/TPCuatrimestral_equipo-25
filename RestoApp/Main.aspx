<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="RestoApp.Main1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Panel -->
    <main class="col ">
        <div class="d-flex flex-column container">
            <div class="p-3">
                <asp:Label runat="server" ID="lblTest">Estos es una prueba</asp:Label>
                <!-- Dropdown-->
                <div class="dropdown">
                    <button
                        class="btn btn-secondary dropdown-toggle"
                        type="button"
                        id="btnDDMesa"
                        data-bs-toggle="dropdown"
                        aria-expanded="false"
                        >
                        Agregar Mesas
                    </button>
                    <ul
                        class="dropdown-menu"
                        aria-labelledby="dropdownMenuButton1"
                        id="dropdown-mesa">
                    </ul>
                </div>
                <!--Fin Dropdown-->
            </div>
            <div class="col">
                <!--Mesas-->
                <div class="row" id="mesas">
                </div>
                <!--Fin Mesas-->
            </div>
            <div class="d-flex justify-content-end my-2">
                <input type="button" class="btn btn-dark invisible" value="Guardar Mesas" id="btnGuardarMesas">
            </div>
        </div>
    </main>
    <!-- Fin Panel -->
</asp:Content>
