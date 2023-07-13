<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="RestoApp.Tickets" enableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <header class="col-10 bg-white rounded p-3 m-3">
            <asp:Label runat="server" ID="lbTituloTicket" class="fs-2">Tickets</asp:Label>
        </header>
    </div>

    <div class="row d-flex">
        <!-- Repeater de Tickets-->
        <asp:Repeater runat="server" ID="repeaterTickets">
            <ItemTemplate>

                <!-- Ticket -->
                <section class="col-3 bg-white rounded d-flex flex-column m-3 p-3 border">
                    <div class="d-flex flex-column w-100">
                        <p class="fw-semibold fs-5 text-center">#<%# Eval("Id") %></p>
                        <div class="d-flex justify-content-between w-100 border-1 border-bottom">
                            <p><i class="fa-sharp fa-solid fa-calendar-days me-2"></i><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></p>
                            <p><i class="fa-regular fa-clock me-2"></i><%# Eval("Cierre") %></p>
                        </div>
                    </div>
                    <div class="w-100 d-flex justify-content-between mt-3">
                        <p><span class="fw-semibold">Mesa:</span> <%# Eval("Mesa") %></p>
                        <p><span class="fw-semibold">Mesero:</span> <%# Eval("IdMesero") %></p>
                    </div>

                    <table class="table table-striped border">
                        <thead class="table-secondary">
                            <tr class="border">
                                <th scope="col">#</th>
                                <th scope="col">Pedido</th>
                                <th scope="col">Cantidad</th>
                                <th scope="col">Precio</th>
                            </tr>
                        </thead>
                        <tbody>
                    <%# RenderDetalles(Eval("Detalle")) %>
                     </tbody>
                    </table>

                    <div class="border-top w-100 pt-3">
                        <p><span class="fw-semibold">Total: $</span> <%# CalcularPrecio() %> </p>
                        <asp:Button ID="btnTicket" runat="server" OnClick="btnCobrar_Click" class="btn btn-dark w-100" Text="Cobrar" CommandArgument='<%# Eval("Id") %>'/>
                    </div>
                </section>

                <!-- Fin Ticket -->

            </ItemTemplate>
        </asp:Repeater>
        <!-- Fin Repeater de Tickets-->

    </div>

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

    <script id="ScriptTicket">
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
