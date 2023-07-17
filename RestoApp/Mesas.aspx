<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Mesas.aspx.cs" Inherits="RestoApp.Mesas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- PANEL -->
    <div class="row">

        <h1 class="text-gray-100">Asignar Mesas</h1>
        <p id="titulo_gerente_Mesas" class="text-gray-100">Asignar Mesas a Meseros</p>

        <div class="row p-3">
            <!--MESAS-->
                <div class="col-12 col-xl-8 d-flex flex-wrap bg-gray-100 border border-muted rounded mb-3 mb-xl-0" id="mesas">
                </div>
                <!-- MESEROS-->
                  <div class="col-12 col-xl-4 d-flex flex-column p-0 ps-xl-3 m-lg-0 mt-3 ">
                        <!-- MESEROS ASIGNADOS-->
                    <div class="border border-muted rounded bg-gray-100 h-auto mb-3">
                        <div class="ms-5 w-75 p-3  m-0 mb-xl-3">
                            <p class="fs-6 fw-semibold text-gray-600">Meseros Asignados</p>
                            <asp:Repeater runat="server" ID="repeaterMeserosAsignados">
                                <ItemTemplate>
                                    <div class="row border-bottom d-flex align-items-center">
                                        <div class="rounded" style="width: 20px; height: 20px;" id="colorMesero" id-mesero="<%# Eval("IdMesero") %>"></div>
                                        <div class="col-5 fw-bold fs-6 d-flex justify-content-between align-items-center flex-grow-1">
                                            <span class="text-start"><%# Eval("Nombres") %> <%# Eval("Apellidos") %></span>
                                        </div>
                                        <div class="col fw-semibold"><%# Eval("MesasAsignadas") %></div>
                                        <div class="col fw-semibold">0</div>
                                        <div class="col btn text-end">
                                            <i class="fa-solid fa-eye" data-bs-toggle="collapse" href="#<%#Eval("Id") %>" role="button" aria-expanded="false" aria-controls="<%#Eval("Id") %>"></i>
                                        </div>
                                    </div>

                                    <!-- Collide -->
                                    <div class="collapse" id="<%#Eval("Id") %>">
                                        <div class="p-2">
                                            <button class="btn btn-dark" id="asignarMesa" onclick="return false;" id-mesero =" <%#Eval("IdMesero") %>" id-meseropordia="<%#Eval("Id") %>">Asignar Mesas</button>
                                            <button class="btn btn-primary" id="guardarMesa" onclick="return false;" id-mesero =" <%#Eval("IdMesero") %>" id-meseropordia="<%#Eval("Id") %>" disabled>Guardar</button>
                                            <button class="btn btn-warning" id="cancelarMesa" onclick="return false;" disabled>Cancelar</button>
                                        </div>
                                    </div>
                                    <!-- Fin Collide -->
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <!-- FIN MESEROS ASIGNADOS-->
                    <!-- MESEROS NO ASIGNADOS-->
                    <div class="border border-muted rounded bg-gray-100 h-auto mb-3 mb-xl-0">
                        <div class="ms-5 w-75 p-3">
                            <p class="fs-6 fw-semibold text-gray-600">Meseros No Asignados</p>
                            <asp:Repeater runat="server" ID="repeaterMeserosNoAsignados">
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
                                            <button class="btn btn-dark" id="asignarMesa" onclick="return false;" id-mesero =" <%#Eval("IdMesero") %>" id-meseropordia="<%#Eval("Id") %>">Asignar Mesas</button>
                                            <button class="btn btn-primary" id="guardarMesa" onclick="return false;" id-mesero =" <%#Eval("IdMesero") %>" id-meseropordia="<%#Eval("Id") %>" disabled>Guardar</button>
                                            <button class="btn btn-primary" id="cancelarMesa" onclick="return false;" disabled>Cancelar</button>
                                        </div>
                                    </div>
                                    <!-- Fin Collide -->
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <!-- FIN MESEROS NO ASIGNADOS-->
                  </div>
                </div>
                <!--FIN MESAS-->
    </div>
    <!-- FIN PANEL-->

    <!-- Modal -->
     <div id="modalMesas" class="modal">
        <div class="modal-content">
            <div class="d-flex justify-content-between align-items-center border-1 border-bottom">
                <h5 class="modal-title" id="modal-titulo">Mesero Asignado</h5>
                <span class="close">&times;</span>
            </div>
            <div id="modal-content" class="modal-body">

                <!-- Desde JS -->
            </div>
        </div>
    </div>

     <style>
        :root {
            --bg-danger: #ffc107;
            --bg-mesero: #17a2b8;
        }

        #modalMesas {
            display: none; /* Ocultar el modal por defecto */
            position: fixed;
            z-index: 1;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0, 0, 0, 0.4); /* Fondo semitransparente */
        }

        .modal-content {
            background-color: #fefefe;
            margin: 15% auto;
            padding: 20px;
            border: 1px solid #888;
            width: 25%;
        }

        .close {
            display: flex;
            justify-content: end;
            color: #aaaaaa;
            font-size: 28px;
            font-weight: bold;
            cursor: pointer;
        }

        .close:hover,
        .close:focus {
            color: #000;
            text-decoration: none;
            cursor: pointer;
        }

        .cursor-pointer {
            cursor: pointer;
        }

        .modalDP .modal-content {
            width: 100%;
        }
    </style>

    <script id="ScriptMesa">
        const modal = document.getElementById("modalMesas");
        const closeModalBtn = document.getElementsByClassName("close")[0];
        const modalTitulo = document.getElementById("modal-titulo");
        let contenidoModal = document.getElementById("modal-content");

        //Modal Alerta
        function alertaModal(msg, mode) {
            console.log(msg, mode)
            modalTitulo.textContent = "Mensaje";
            modal.style.display = "block";
            contenidoModal.innerHTML = "";

            if (mode == "error") {
                contenidoModal.innerHTML = `<p class="text-danger">${msg}</p>`
            } else {
                contenidoModal.innerHTML = `<p class="text-success">${msg}</p>`
            }

        }

        //Boton modal para cerrar
        closeModalBtn.addEventListener("click", function () {
            modal.style.display = "none";
        });
    </script>
</asp:Content>
