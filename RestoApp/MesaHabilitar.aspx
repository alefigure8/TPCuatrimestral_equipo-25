<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="MesaHabilitar.aspx.cs" Inherits="RestoApp.MesaHabilitar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1>Habilitar Mesas</h1>
        <p id="titulo_gerente_Mesas">Activar las mesas que desea asignar</p>
        <div class="d-flex row">
            <div class="col-8 d-flex flex-column">
                <div class="row" id="mesas"></div>
            </div>
            <div class="col-4 p-3 border border-muted rounded bg-opacity-10 bg-secondary h-50">
                <div class="ms-2 d-flex justify-content-center gap-4">
                    <!-- GERENTE: BTN GUARDAR MESA-->
                    <div class="d-flex justify-content-between my-2">
                        <input type="button" class="btn btn-dark" value="Habilitar Mesas" id="btnHabilitarMesas">
                    </div>
                    <div class="d-flex justify-content-end my-2">
                        <input type="button" class="btn btn-dark" value="Guardar Mesas" id="btnGuardarMesas" disabled>
                    </div>
                    <!--FIN GERENTE: BTN GUARDAR MESA-->
                </div>
            </div>
        </div>
        <!-- FIN MESEROS-->
    </div>

</asp:Content>
