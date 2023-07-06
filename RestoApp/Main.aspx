<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="RestoApp.Main1" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Updatea Panel Botón Mesero -->
    <asp:UpdatePanel ID="UpdatePanel1"
        runat="server">
        <ContentTemplate>
            <div class="d-flex align-items-center row">
                <div class="d-flex align-items-center gap-3 mb-3 col-5">
                    <h4 class="text-gray-100">Hola, <%= usuario?.Nombres %> <%= usuario?.Apellidos %> (<%= usuario?.Tipo %>)</h4>
                    <!-- Boton Mesero -->
                    <%if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)
                        { %>
                    <asp:Button CssClass="btn btn-sm" Text="Darse de Alta" runat="server" ID="btnMeseroAlta" OnClick="btnMeseroAlta_Click" />
                    <%} %>
                </div>
                <!-- Fin Boton Mesero -->
                <p class="fw-semibold text-gray-100 text-end col-5"><%: DateTime.Now %></p>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin Updatea Panel Botón Mesero -->

    <!-- Agregar Mesas habilitadas, mozos habilitados, productos, -->
    <div class="row content-fluid">

        <!-- VISTA GERENTE -->
        <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente)
            {%>
        <div class="col-5">
            <!-- Estado de las mesas -->
            <div class="bg-gray-100 rounded border-1 p-5">
                <div class="d-flex">
                    <p>Mesas Activas:<span class="fw-semibold"> <%= MesasActivas %></span></p>
                    <a class="link-dark ms-4" href="MesaHabilitar.aspx">Activar Mesas</a>
                </div>
                <div class="d-flex">
                    <p>Mesas Asignadas:<span class="fw-semibold"> <%= MesasAsignadas %></span></p>
                    <a class="link-dark ms-4" href="Mesas.aspx">Asignar Mesas</a>
                </div>
                <div class="d-flex overflow-hidden row" id="section-mesa">
                    <!-- Renderizar mesas -->
                </div>
            </div>
            <!-- Fin  Estado de las mesas -->
        </div>
        <div class="col-5">
            <div class="bg-gray-100 p-5 rounded border-1">
                <div class="d-flex flex-column">
                    <div class="d-flex">
                        <p>Meseros Presentes:<span class="fw-semibold"> <%= MeserosPresentes %></span></p>
                    </div>
                    <!-- Tabla Meseros Presentes -->
                    <div class="row ms-2">
                        <asp:Repeater runat="server" ID="repeaterMeserosPresentes">
                            <ItemTemplate>
                                <div class="row border-bottom d-flex align-items-center">
                                    <i class="col fa-sharp fa-solid fa-circle text-success"></i>
                                    <div class="col-5 fw-bold fs-6 d-flex justify-content-between align-items-center flex-grow-1">
                                        <span class="text-start"><%# Eval("Nombres") %> <%# Eval("Apellidos") %></span>
                                    </div>
                                    <div class="col fw-semibold"><%# Eval("MesasAsignadas") %></div>
                                    <div class="col fw-semibold">0</div>
                                    <a class="col" href="Mesas.aspx"><i class="fa-solid fa-bullseye text-success"></i></a>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <!-- Fin Tabla Meseros Presentes -->

                    <!-- Tabla Meseros Ausentes -->
                    <div class="row ms-2">
                        <asp:Repeater runat="server" ID="repeaterMeserosAusentes">
                            <ItemTemplate>
                                <div class="row border-bottom d-flex align-items-center">
                                    <i class="col fa-sharp fa-solid fa-circle text-danger"></i>
                                    <div class="col-5 fw-bold fs-6 d-flex justify-content-between align-items-center flex-grow-1">
                                        <span class="text-start"><%# Eval("Nombres") %> <%# Eval("Apellidos") %></span>
                                    </div>
                                    <div class="col fw-semibold">0</div>
                                    <div class="col fw-semibold">0</div>
                                    <div class="col"><i class="fa-solid fa-bullseye text-muted"></i></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <!-- Fin Tabla Meseros Presentes -->
                </div>
            </div>
        </div>
        <div class="col-5 mt-3">
            <div class="bg-gray-100 p-5 rounded border-1">
                <h4>Mesas Estado</h4>
                <asp:DataGrid ID="datagrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                    <Columns>
                        <asp:BoundColumn DataField="Numero" HeaderText="Numero" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:BoundColumn DataField="Mesero" HeaderText="Mesero" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:BoundColumn DataField="Apertura" HeaderText="Apertura" DataFormatString="{0:HH:mm}" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:BoundColumn DataField="Cierre" HeaderText="Cierre" DataFormatString="{0:HH:mm}" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:TemplateColumn HeaderText="Estado">
                            <ItemTemplate>
                                <%# Convert.IsDBNull(Eval("Cierre")) ? "<i class=\"fa-sharp fa-solid fa-circle text-success\"></i>" : "<i class=\"fa-sharp fa-solid fa-circle text-warning\"></i>" %>
                            </ItemTemplate>
                            <ItemStyle CssClass="bg-light p-2 rounded" />
                            <HeaderStyle CssClass="bg-light p-2 rounded" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>
        <div class="col-5 mt-3">
            <div class="bg-gray-100 p-5 rounded border-1">
                <h4>Pedidos Estado</h4>
                <asp:DataGrid ID="datagridPedidos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                    <Columns>
                        <asp:BoundColumn DataField="Mesa" HeaderText="Mesa" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:BoundColumn DataField="PedidoComida" HeaderText="Pedido de Comida" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:BoundColumn DataField="Apertura" HeaderText="Apertura" DataFormatString="{0:HH:mm}" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:BoundColumn DataField="Cierre" HeaderText="Cierre" DataFormatString="{0:HH:mm}" ItemStyle-CssClass="bg-light p-2 rounded" HeaderStyle-CssClass="bg-light p-2 rounded" />
                        <asp:TemplateColumn HeaderText="Estado">
                            <ItemTemplate>
                                <%# Convert.IsDBNull(Eval("Cierre")) ? "<i class=\"fa-sharp fa-solid fa-circle text-warning\"></i>" : "<i class=\"fa-sharp fa-solid fa-circle text-success\"></i>" %>
                            </ItemTemplate>
                            <ItemStyle CssClass="bg-light p-2 rounded" />
                            <HeaderStyle CssClass="bg-light p-2 rounded" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>

            </div>
        </div>
        <% } %>

        <!-- VISTA MESERO -->
        <%
            if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)

            {%>

        <div class="row content-fluid">

            <!-- SECCION MESAS ASIGNDAS -->
            <section class="col-10 bg-white rounded p-3 justify-content-around m-1">
                <div class="h3">Mis Mesas </div>
                <div class="row justify-content-around justify-items-start p-3" id="section-mesa-mesero">

                    <!-- Mensaje de Mesas asignadas -->
                    <asp:Label runat="server" ID="lbSinMesasAsignadas"></asp:Label>

                    <!-- MESAS ASIGNADAS-->
                    <%--<asp:Repeater runat="server" ID="repeaterMesasAsigndas">
                        <ItemTemplate>--%>

                    <!-- MESAS -->
                    <%--<div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                                <div class="bg-warning w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                                    <div class="background-color w-100 h-100 rounded-circle d-flex justify-content-center align-items-center">
                                            <i class="fa-solid fa-utensils fs-4"></i>
                                    </div>
                                </div>
                                <div class=" w-100 text-light d-flex justify-content-center">
                                    <div class="w-50 bg-black rounded-4 text-center">
                                        <small class="fw-bold"><%# Eval("Mesa") %></small>
                                    </div>
                                </div>
                            </div>--%>
                    <!-- FIN MESAS -->

                    <%-- </ItemTemplate>
                    </asp:Repeater>--%>

                    <!-- FIN MESAS ASIGNADAS -->
                </div>
            </section>
            <!-- FIN SECCION MESAS ASIGNDAS -->



            <%--SECCION PEDIDOS EN CURSO--%>
            <div class="col-10 bg-white rounded p-3 justify-content-around m-1 mt-2 content-fluid">

                <div class="h3">Mis pedidos en curso </div>

                <div class="col d-inline-flex p-2 justify-content-around">
                    <div class="card">
                        <div class="card-body">
                            <p class="card-text">
                                Mesa ## | Pedido ## | Estado 
                            </p>
                            <br />
                            <button class="btn btn-dark">Detalle</button>
                            <button class="btn btn-dark">Cerrar</button>
                        </div>
                    </div>
                </div>

                <div class="col d-inline-flex p-2  justify-content-around">
                    <div class="card">
                        <div class="card-body">
                            <p class="card-text">
                                Mesa ## | Pedido ## | Estado 
                            </p>
                            <br />
                            <button class="btn btn-dark">Detalle</button>
                            <button class="btn btn-dark">Cerrar</button>
                        </div>
                    </div>
                </div>

                <div class="col d-inline-flex p-2  justify-content-around">
                    <div class="card">
                        <div class="card-body">
                            <p class="card-text">
                                Mesa ## | Pedido ## | Estado 
                            </p>
                            <br />
                            <button class="btn btn-dark">Detalle</button>
                            <button class="btn btn-dark">Cerrar</button>
                        </div>
                    </div>
                </div>


            </div>



            <%--SECCION MENU RAPIDO--%>

            <div class="col-10 row bg-white rounded p-2 justify-content-around m-1 mt-2 ">
                <%--  Label muestra sobre qué mesa estoy haciendo pedido--%>

                <div class="row bg-dark ">

                     <div class="col-6 bg-dark text-white p-2 ">
                         <asp:UpdatePanel runat="server">
                             <ContentTemplate>
                                   <asp:Label ID="lbNumeroMesa" runat="server" CssClass="h4">SIN MESA SELECCIONADA</asp:Label>

                             </ContentTemplate>
                         </asp:UpdatePanel>
                  
                         </div>

                <div class="col-6 d-flex justify-content-end p-2">


                    <asp:UpdatePanel runat="server" ID="UPGuardarPedido">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnGuardarPedido" Text="ENVIAR PEDIDO" Visible="false" CssClass="btn  btn-secondary" OnClick="btnGuardarPedido_Click" />
                            <asp:Button runat="server" ID="btnTerminarPedido" Text="Cancelar" Visible="false" CssClass="btn btn-secondary" OnClick="btnTerminarPedido_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                </div>
                



                <div class="col-6 p-2">
                    <label class="row h3 p-1">Platos Disponibles:</label>
                    <asp:Repeater runat="server" ID="PlatosDelDia">
                        <ItemTemplate>
                            <div class="row">
                                <asp:Label runat="server" CssClass="col-4">
                                    <%#Eval("Nombre")%>
                                </asp:Label>
                                <div class="col">
                                    <asp:UpdatePanel runat="server" ID="PanelAgregarAPedido" CssClass="row">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" CssClass="col-1 form-control small" Style="max-width: 100px; display: inline; box-shadow: 0 2px 4px rgba(0, 0, 0, 0);" ID="tbCantidad" ToolTip="Ingrese Cantidad" Type="Number" min="1" Text="1" Visible="false"></asp:TextBox>
                                            <asp:Button runat="server" CssClass="col btn btn-dark btn-sm small m-1" Text="✚" ID="AgregarAPedido" OnClick="AgregarAPedido_Click" CommandArgument='<%#Eval("Id")%>' ToolTip="Agregar a pedido" />
                                            <asp:Button runat="server" ID="BtnCancelarAgregarA" CssClass="col btn btn-dark btn-sm small m-1" Text="✖" Visible="false" OnClick="BtnCancelarAgregarA_Click" ToolTip="Cancelar" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>


                <%--Bebidas--%>
                <div class="col-6 p-2">
                    <label class="row h3  p-1">Bebidas Disponibles:</label>
                    <asp:Repeater runat="server" ID="BebidasDelDia">
                        <ItemTemplate>
                            <div class="row">
                                <div class="col-4">
                                    <%#Eval("Nombre")%>
                                </div>
                                <div class="col">
                                    <asp:UpdatePanel runat="server" ID="PanelAgregarBebida" CssClass="row">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" CssClass="col-1 form-control small" Style="max-width: 100px; display: inline; box-shadow: 0 2px 4px rgba(0, 0, 0, 0);" Type="Number" min="1" Text="1"></asp:TextBox>
                                            <asp:Button runat="server" CssClass="col btn btn-dark btn-sm small m-1" Text="+" />
                                            <asp:Button runat="server" CssClass="col btn btn-dark btn-sm small m-1" Text="✖" Visible="false" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>






            </div>
        </div>





    </div>
    <% } %>

    <!-- MODAL -->
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

    <!--************* ESTILOS Y SCRIPTS *************** -->
    <!-- Styles Mesas -->
    <style>
        :root {
            --bg-danger: #ffc107;
            --bg-mesero: #17a2b8;
        }

        .mesa {
            width: 75px;
            height: 75px;
            border-radius: 10%;
            margin: 10px;
            padding: 10px;
            display: flex;
            justify-content: center;
            align-items: center;
            position: relative;
            overflow: hidden;
            cursor: pointer;
        }

            .mesa p {
                font-size: 15px;
                font-weight: bold;
                margin: 0;
            }

        .mesaTop {
            width: 100%;
            height: 15px;
            position: absolute;
            top: 0;
            left: 0;
        }

        .mesaBottom {
            width: 100%;
            height: 15px;
            position: absolute;
            bottom: 0;
            left: 0;
        }

        .modal {
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

        .botonPedido {
            cursor: pointer;
            border-radius: 10px;
            display: grid;
            grid-template-rows: 70% 30%;
            justify-content: center;
            align-items: center;
            border: none;
            outline: none;
        }

        .btnAbrirMeasa {
            background-color: #FFC107;
            border-color: #FFC107;
        }

            .btnAbrirMeasa:hover {
                background-color: #ffc107c4;
                border-color: #ffc107c4;
            }

        .btnAbrirPedido {
            background-color: #0d6efd;
            border-color: #0d6efd;
        }

            .btnAbrirPedido:hover {
                background-color: #0d6dfdc4;
                border-color: #0d6dfdc4;
            }

        .btnPedidos {
            background-color: #198754;
            border-color: #198754;
        }

            .btnPedidos:hover {
                background-color: #198754c2;
                border-color: #198754c2;
            }

        .btnTicket {
            background-color: #dc3545;
            border-color: #dc3545;
        }

            .btnTicket:hover {
                background-color: #dc3546c4;
                border-color: #dc3546c4;
            }
    </style>
    <!-- Fin Styles Mesas -->

    <!-- Scripts Mesas -->
    <script id="scriptMain">
        let tipoUsuario = "<%: tipoUsuario%>";
        let numeroMesa = <%: MesasActivas %>;
        let sectionMesa = document.getElementById("section-mesa");
        let sectionMesaMesero = document.getElementById("section-mesa-mesero")
        const modal = document.getElementById("modalMesas");
        const closeModalBtn = document.getElementsByClassName("close")[0];
        const modalTitulo = document.getElementById("modal-titulo");
        let contenidoModal = document.getElementById("modal-content");

        //Traemos datos de mesas desde codebehind del Gerente
        function obtenerDatosMesasGerente() {

            return new Promise((resolve) => {
                if (typeof datosMesasArray !== 'undefined') {
                    const datosMesas = JSON.parse(datosMesasArray)
                    const numeroMesas = JSON.parse(numeroMesasActivasArray)
                    const numeroServicios = JSON.parse(seviciosJSON)

                    resolve({ datosMesas, numeroMesas, numeroServicios });
                } else {
                    const intervalo = setInterval(() => {
                        if (typeof datosMesasArray !== 'undefined') {
                            clearInterval(intervalo);
                            const datosMesas = JSON.parse(datosMesasArray)
                            const numeroMesas = JSON.parse(numeroMesasActivasArray)
                            const numeroServicios = JSON.parse(seviciosJSON)
                            console.log(numeroServicios)
                            resolve({ datosMesas, numeroMesas, numeroServicios });
                        }
                    }, 0);
                }
            });
        }

        //Traemos datos de mesas desde codebehind del Mesero
        function obtenerDatosMesasMesero() {

            return new Promise((resolve) => {
                if (typeof numeroMesasArray !== 'undefined') {
                    const numeroMesas = JSON.parse(numeroMesasArray)
                    const numeroServicios = JSON.parse(seviciosJSON)
                    resolve({ numeroMesas, numeroServicios });
                } else {
                    const intervalo = setInterval(() => {
                        if (typeof numeroMesasArray !== 'undefined') {
                            clearInterval(intervalo);
                            const numeroMesas = JSON.parse(numeroMesasArray)
                            const numeroServicios = JSON.parse(seviciosJSON)

                            resolve({ numeroMesas, numeroServicios });
                        }
                    }, 0);
                }
            });
        }

        //Llamamos datos de las mesas del Gerente
        if (tipoUsuario == "Gerente") {
            obtenerDatosMesasGerente()
                .then(({ datosMesas, numeroMesas, numeroServicios }) => {
                    renderMesaGerente(datosMesas, numeroMesas, numeroServicios);
                });
        }

        //Llamamos datos de las mesas del Mesero
        if (tipoUsuario == "Mesero") {
            obtenerDatosMesasMesero()
                .then(({ numeroMesas, numeroServicios }) => {
                    renderMesaMesero(numeroMesas, numeroServicios);
                });
        }

        //Funcion Gerente
        function renderMesaGerente(datosMesa, numeroMesas, numeroServicios) {

            for (let i = 0; i < numeroMesa; i++) {

                //Buscamos mesa Asignada
                let mesa = datosMesa.find(item => item.mesa == numeroMesas[i].Numero)

                //color del Mesero
                let colorMesero;

                if (mesa) {
                    colorMesero = convertirAHexadecimal(mesa.mesero)
                } else {
                    colorMesero = "#666"
                }

                let colorApertura

                //Buscamos mesas con servicios abierto
                let servicio = numeroServicios.find(item => item.mesa == numeroMesas[i].Numero)
                if (servicio) {
                    colorApertura = "bg-success";
                } else {
                    colorApertura = "bg-warning"
                }

                //Main
                let mainDiv = document.createElement("div");
                mainDiv.classList.add("d-flex", "mesa", "bg-body-secondary");
                let idMesa = mesa ? 'mesa_' + mesa.mesa : 'mesa'
                mainDiv.setAttribute("id", idMesa);
                let meseroNoAsignado = "No Asignado";
                mainDiv.setAttribute("title", `Mesero: ${mesa ? mesa.nombre + ' ' + mesa.apellido : meseroNoAsignado}`);

                //Top -- Background según mesero
                let mesaTop = document.createElement("div");
                mesaTop.classList.add("mesaTop");
                mesaTop.style.background = colorMesero

                //Bottom -- Background según estado
                let mesaBottom = document.createElement("div");
                mesaBottom.classList.add("mesaBottom", colorApertura);

                //Numero
                let mesaNumber = document.createElement("div");
                //TODO poner numero de mesa
                mesaNumber.innerHTML = `<p>${numeroMesas[i].Numero}</p>`;

                //Appends
                mainDiv.appendChild(mesaTop);
                mainDiv.appendChild(mesaNumber);
                mainDiv.appendChild(mesaBottom);
                sectionMesa.appendChild(mainDiv);

                //Titulo
                modalTitulo.textContent = "Mesero Asignado";

                let estado;
                //Texto
                if (servicio) {
                    estado = "Abierta"
                } else {
                    estado = "Cerrada"
                }

                //Evento de la mesa
                let mesaEvento = document.getElementById(idMesa);
                mesaEvento.addEventListener('click', () => {

                    //Buscar ids con numeros
                    let contieneNumero = /\d/.test(idMesa)

                    if (contieneNumero) {
                        modal.style.display = "block";
                        contenidoModal.innerHTML = "";
                        contenidoModal.innerHTML += `
                    <p class="fw-bold">Nombre: <span class="fw-normal">${mesa.nombre} ${mesa.apellido}</span></p>
                    <p class="fw-bold">Mesa: <span class="fw-normal">${mesa.mesa}</span></p>
                    <p class="fw-bold">Estado: <span class="fw-normal">${estado}</span></p>
                    `;
                    }

                })
            }
        }

        //Función Mesero
        function renderMesaMesero(numeroMesas, numeroServicios) {

            for (i = 0; i < numeroMesas.length; i++) {


                const bgMesa = numeroServicios.some(item => item.mesa == numeroMesas[i].mesa) ? "bg-success" : "bg-warning"

                //Mesa
                var mainDiv = document.createElement("div");
                mainDiv.className = "col-6 col-sm-3 d-flex justify-content-center flex-column m-4";
                mainDiv.style.height = "150px";
                mainDiv.style.width = "150px";

                var div1 = document.createElement("div");
                div1.className = `${bgMesa} w-100 h-100 border rounded-circle border-dark-subtle p-1 btn`;
                div1.id = "mesa_" + numeroMesas[i].mesa;

                var div2 = document.createElement("div");
                div2.className = "background-color w-100 h-100 rounded-circle d-flex justify-content-center align-items-center";

                var icon = document.createElement("i");
                icon.className = "fa-solid fa-utensils fs-4";

                div2.appendChild(icon);
                div1.appendChild(div2);
                mainDiv.appendChild(div1);

                var div3 = document.createElement("div");
                div3.className = "w-100 text-light d-flex justify-content-center";

                var div4 = document.createElement("div");
                div4.className = "w-50 bg-black rounded-4 text-center";

                var small = document.createElement("small");
                small.className = "fw-bold";
                small.innerText = numeroMesas[i].mesa;

                div4.appendChild(small);
                div3.appendChild(div4);
                mainDiv.appendChild(div3);
                sectionMesaMesero.appendChild(mainDiv);

                //Evento y Modal
                let numeroDeMesa = numeroMesas[i].mesa;
                let mesaEvento = document.getElementById(`mesa_${numeroMesas[i].mesa}`);

                //Disabled boton
                let isDisabled = numeroServicios.some(item => item.mesa == numeroMesas[i].mesa)
                let textoMesaAbrirPedid = isDisabled ? "Cerrar Servicio" : "Abrir Servicio"

                mesaEvento.addEventListener('click', () => {
                    modalTitulo.textContent = `Mesa Asignada ${numeroDeMesa}`
                    modal.style.display = "block";
                    contenidoModal.innerHTML = "";
                    contenidoModal.innerHTML += `
                <div class="row d-flex flex-column justify-content-center gap-2 p-3 ms-3">
                    <div class="col d-flex gap-4">
                        <button class="btnAbrirMeasa botonPedido" style="width: 150px; height: 150px;" id="btnAbrir_${i + 1}">
                                <div class="d-flex align-items-center justify-content-center">
                                    <i class="fa-solid fa-plus fs-1 text-white"></i>
                                </div>
                                <div class="text-white">
                                    <p class="fw-semibold">${textoMesaAbrirPedid}</p>
                                </div>
                        </button>
                        <button class="btn btnAbrirPedido botonPedido" style="width: 150px; height: 150px;" id="btnPedido_${i + 1}">
                            <div class="d-flex align-items-center justify-content-center">
                                <i class="fa-solid fa-utensils fs-1 text-white"></i>
                            </div>
                            <div class="text-white">
                                <p class="fw-semibold">Abrir Pedido</p>
                            </div>
                        </button>
                    </div>
                    <div class="col d-flex gap-4 mt-3">
                        <button class="btn btnPedidos botonPedido" style="width: 150px; height: 150px;" id="btnLista_${i + 1}">
                            <div class="d-flex align-items-center justify-content-center">
                                <i class="fa-solid fa-list fs-1 text-white"></i>
                            </div>
                            <div class="text-white">
                                <p class="fw-semibold">Pedidos</p>
                            </div>
                        </button>
                        <button class="btn btnTicket botonPedido" style="width: 150px; height: 150px;" id="btnTicket_${i + 1}">
                            <div class="d-flex align-items-center justify-content-center">
                                <i class="fa-solid fa-dollar fs-1 text-white"></i>
                            </div>
                            <div class="text-white">
                                <p class="fw-semibold">Ticket</p>
                            </div>
                        </button>
                    </div>
                </div>
                `;

                    //Creamos los eventos de los botones
                    eventoBotones(i, numeroDeMesa, isDisabled)
                })
            }
        }

        //Evento botones Mesero
        function eventoBotones(i, mesa, isDisabled) {

            let btnServicio = document.getElementById(`btnAbrir_${i + 1}`);
            btnServicio.disabled = isDisabled;

            let btnPedido = document.getElementById(`btnPedido_${i + 1}`);
            let btnLista = document.getElementById(`btnLista_${i + 1}`);
            let btnTicket = document.getElementById(`btnTicket_${i + 1}`);

            btnServicio.addEventListener('click', (e) => {
                console.log("Abrir Servicio")
                //Mandamos datos a CodeBehind
                let data = [{ mesa: mesa }];

                mandarDatos('Main', 'AbrirServicio', data, e)
            })

            btnPedido.addEventListener('click', (e) => {
                //Mandamos datos a CodeBehind
                console.log("Abrir Pedido")
                let data = [{ mesa: mesa }];
                mandarDatos('Main', 'AbrirPedido', data, e)
            })

            btnLista.addEventListener('click', (e) => {
                e.preventDefault();
                console.log("Ver listado")
                //Mandamos datos a CodeBehind
                //mandarDatos('Main', 'AbrirServicio')
                //MOSTRAR LISTADO DE PEDIDO. ¿lINK CON QUERY?
            })

            btnTicket.addEventListener('click', (e) => {
                e.preventDefault();
                console.log("Ticket")
                //Mandamos datos a CodeBehind
                //mandarDatos('Main', 'AbrirServicio')
                //MOSTRAR TICKET. ¿LINK CON QUERY?
            })

        }

        //Boton modal para cerrar
        closeModalBtn.addEventListener("click", function () {
            modal.style.display = "none";
        });

        //Fetch para mandar datos a codebehind
        async function mandarDatos(pag, metodo, data, event) {

            event.preventDefault();

            fetch(`${pag}.aspx/${metodo}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ data: data })
            })
                .then(response => response.json())
                .then(result => {
                    console.log(result)
                    location.reload();
                })
                .catch(error => {
                    console.log(error)
                });
        }

        //Función para pasar numero de mesero a color
        function convertirAHexadecimal(numero) {
            let r = (numero * 4321) % 256; // rojo
            let g = (numero * 1234) % 256; // verde
            let b = (numero * 9876) % 256; // azul
            let opacity = 0.9; // Valor de opacidad deseado (por ejemplo, 0.5 para 50%)

            let colorHexadecimal = '#' + toDosDigitosHex(r) + toDosDigitosHex(g) + toDosDigitosHex(b);

            return colorHexadecimal + toOpacityHex(opacity);
        }

        //Armamos digitos hexadecimal
        function toDosDigitosHex(numero) {
            let hex = numero.toString(16);
            return hex.length === 1 ? '0' + hex : hex;
        }

        //armamos opacidad
        function toOpacityHex(opacity) {
            let opacityDecimal = Math.floor(opacity * 255);
            return toDosDigitosHex(opacityDecimal);
        }
    </script>
    <!-- Fin Scripts Mesas -->

</asp:Content>



