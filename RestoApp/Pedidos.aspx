<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="RestoApp.Pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <header class="col-10 p-3">
            <asp:Label runat="server" ID="lbTituloTicket" class="fs-2 text-gray-100">Pedidos</asp:Label>
        </header>
    </div>

    <%if (esListadoPorServicio)
        {  %>
      <div class="row">
        <div class="col-10 bg-gray-100 rounded p-3 m-3">
            <div class="d-flex gap-4">
                <asp:DropDownList runat="server" ID="ddlMesaPedidos" CssClass="w-25 form-control" OnSelectedIndexChanged="ddlMesaPedidos_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                <asp:DropDownList runat="server" ID="ddlServicioPedidos" CssClass="w-25 form-control"></asp:DropDownList>
                <asp:Button runat="server" Text="Buscar" CssClass="btn btn-secondary" OnClick="BtnBuscar_Click"></asp:Button>
            </div>
        </div>
    </div>
    <%} %>
    <div class="row">
        <!-- Repeater de Pedidos-->
            <div class="col-10 bg-gray-100 p-5 m-3 rounded border-1">
                <!-- Pedidos -->
                 <asp:DataGrid ID="datagridPedidosGerente" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                    <Columns>
                        <asp:BoundColumn DataField="PedidoComida" HeaderText="Pedido de Comida" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:BoundColumn DataField="Actualización" HeaderText="Actualizacion" DataFormatString="{0:HH:mm}" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:BoundColumn DataField="Estado" HeaderText="Estado" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />

                        <asp:TemplateColumn HeaderText="Estado">
                            <ItemTemplate>
                                <%# (string)Eval("Estado") != "Entregado"  ? "<i class=\"fa-sharp fa-solid fa-circle text-warning\"></i>" : "<i class=\"fa-sharp fa-solid fa-circle text-success\"></i>" %>
                            </ItemTemplate>
                            <ItemStyle CssClass="bg-light p-2 rounded" />
                            <HeaderStyle CssClass="bg-light p-2 rounded" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <!-- Fin Pedidos -->
            </div>
        <!-- Fin Repeater de Pedidos-->
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
