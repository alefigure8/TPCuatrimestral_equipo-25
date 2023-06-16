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
            <div class="d-flex justify-content-between">
                <div class="row col-8" id="mesas">
                </div>
                <!-- MESEROS-->
                <div class="col p-3 border border-muted rounded bg-opacity-10 bg-secondary h-50">
                    <div class="row ms-2">
                        <p class="fs-6 fw-semibold text-muted opacity-50">Meseros No Asignados</p>
                        <asp:Repeater runat="server" ID="repeaterMeseros">
                            <ItemTemplate>
                                <div class="row border-bottom d-flex align-items-center">
                                    <div class="bg-danger rounded" style="width: 20px; height: 20px;"></div>
                                    <div class="col-5 fw-bold fs-6 d-flex justify-content-between align-items-center flex-grow-1">
                                        <span class="text-start"><%# Eval("Nombres") %> <%# Eval("Apellidos") %></span>
                                    </div>
                                    <div class="col fw-semibold">0</div>
                                    <div class="col fw-semibold">0</div>
                                    <div class="col btn text-end">
                                        <i class="fa-solid fa-eye" data-bs-toggle="collapse" href="#<%#Eval("Id") %>" role="button" aria-expanded="false" aria-controls="<%#Eval("Id") %>"></i>
                                    </div>
                                </div>

                                <!-- Collide -->
                                <div class="collapse" id="<%#Eval("Id") %>">
                                    <div class="p-2">
                                        <btn class="btn btn-dark" id="asignarMesa">Asignar Mesas</btn>
                                        <btn class="btn btn-primary">Guardar</btn>
                                    </div>
                                </div>
                                <!-- Fin Collide -->
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <!-- FIN MESEROS-->
            </div>
            <!--FIN MESAS-->
        </div>
    </div>
    <!-- FIN PANEL-->
</asp:Content>
