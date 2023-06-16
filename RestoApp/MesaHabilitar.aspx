<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="MesaHabilitar.aspx.cs" Inherits="RestoApp.MesaHabilitar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="d-flex flex-column container">
        <h1 class="text-gray-100">Habilitar Mesas</h1>
        <p id="titulo_gerente_Mesas" class="text-gray-100">Activar las mesas que desea asignar</p>
        <div class="d-flex gap-5 align-content-center">
            <div class="d-flex justify-content-between ">
                <div class="row col bg-gray-100  border border-muted rounded" id="mesas"></div>
            </div>
            <div class="col-4 p-3 border border-muted rounded bg-gray-100 h-50">
                <div class="d-flex justify-content-center gap-5 ms-2">
                    <!-- GERENTE: BTN GUARDAR MESA-->
                    <div class="d-flex justify-content-between my-2">
                        <input type="button" class="btn btn-dark" value="Habilitar Mesas" id="btnHabilitarMesas">
                    </div>
                    <div class="d-flex justify-content-end my-2">
                        <input type="button" class="btn btn-primary" value="Guardar Mesas" id="btnGuardarMesas" disabled>
                    </div>
                    <!--FIN GERENTE: BTN GUARDAR MESA-->
                </div>
            </div>
        </div>
        <!-- FIN MESEROS-->
    </div>

</asp:Content>
