<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="RestoApp.Main1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Updatea Panel Botón Mesero -->
    <asp:UpdatePanel ID="UpdatePanel1"
        runat="server">
        <ContentTemplate>
            <div class="d-flex align-items-center gap-3 mb-3">
                <h4 class="text-gray-100">Hola, <%= usuario?.Nombres %> <%= usuario?.Apellidos %> (<%= usuario?.Tipo %>)</h4>
                <!-- Boton Mesero -->
                <%if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)
                    { %>
                <asp:Button CssClass="btn btn-sm" Text="Darse de Alta" runat="server" ID="btnMeseroAlta" OnClick="btnMeseroAlta_Click" />
                <%} %>
                <!-- Fin Boton Mesero -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin Updatea Panel Botón Mesero -->

    <!-- Agregar Mesas habilitadas, mozos habilitados, productos, -->
    <div class="row">

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
                                <%# Convert.IsDBNull(Eval("Cierre")) ? "<i class=\"fa-sharp fa-solid fa-circle text-warning\"></i>" : "<i class=\"fa-sharp fa-solid fa-circle text-success\"></i>" %>
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

        <div id="modalMesas" class="modal">
            <div class="modal-content">
                <div class="d-flex justify-content-between align-items-center">
                    <h5 class="modal-title">Mesero Asignado</h5>
                    <span class="close">&times;</span>
                </div>
                <div id="modal-content" class="modal-body">

                    <!-- Desde JS -->
                </div>
            </div>
        </div>


        <!-- VISTA MESERO -->
        <%
            if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)

            {%>

        <div class="row p-3 justify-content-between ">

            <!-- SECCION MESAS ASIGNDAS -->
            <section class="row bg-white rounded p-2 justify-content-around m-1">
                <h3>Mis Mesas</h3>
                <div class="row justify-content-around justify-items-start p-3">
                    <asp:Label runat="server" ID="lbSinMesasAsignadas"></asp:Label>
                    <!-- MESAS ASIGNADAS-->
                    <asp:Repeater runat="server" ID="repeaterMesasAsigndas">
                        <ItemTemplate>

                            <!-- MESAS -->
                            <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
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
                            </div>
                            <!-- FIN MESAS -->

                        </ItemTemplate>
                    </asp:Repeater>
                    <!-- FIN MESAS ASIGNADAS -->
                </div>
            </section>
            <!-- FIN SECCION MESAS ASIGNDAS -->



            <%--SECCION PEDIDOS EN CURSO--%>
            <div class="row bg-white rounded p-2 justify-content-around m-1 mt-2">

                <div class="row h3">Mis pedidos en curso </div>

                <div class="col d-inline-flex p-2  justify-content-around">
                    <div class="card">
                        <div class="card-body">
                            <p class="card-text">
                                Mesa ## | Pedido ## | Estado 
                                <br />
                                <button class="btn btn-dark">Detalle</button>
                                <button class="btn btn-dark">Cerrar</button>
                            </p>
                        </div>
                    </div>
                </div>

                <div class="col d-inline-flex p-2  justify-content-around">
                    <div class="card">
                        <div class="card-body">
                            <p class="card-text">
                                Mesa ## | Pedido ## | Estado 
                                <br />
                                <button class="btn btn-dark">Detalle</button>
                                <button class="btn btn-dark">Cerrar</button>
                            </p>
                        </div>
                    </div>
                </div>

                <div class="col d-inline-flex p-2  justify-content-around">
                    <div class="card">
                        <div class="card-body">
                            <p class="card-text">
                                Mesa ## | Pedido ## | Estado 
                                <br />
                                <button class="btn btn-dark">Detalle</button>
                                <button class="btn btn-dark">Cerrar</button>
                            </p>
                        </div>
                    </div>
                </div>


            </div>



            <%--SECCION MENU RAPIDO--%>

            <div class="row bg-white rounded p-2 justify-content-around m-1 mt-2 " >
                <div class="col p-2">
                    <h2 class="h3">Platos Disponibles:</h2>
                    <asp:Repeater runat="server" ID="PlatosDelDia">
                        <ItemTemplate>
                            <div class="row">
                                <div class="col h5"><%#Eval("Nombre")%> </div>
                                <div class="col h5"><%#Eval("Valor","{0:C}")%> </div>
                                <div class="col-2">
                                    <asp:Button runat="server" Text="+" CssClass="btn btn-sm btn-dark" ToolTip="Agregar a pedido" />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>


                <div class="col p-2">
                    <h2 class="h3">Bebidas Disponibles:</h2>
                    <asp:Repeater runat="server" ID="BebidasDelDia">
                        <ItemTemplate>
                            <div class="row">
                                <div class="col h5"><%#Eval("Nombre")%> </div>
                                <div class="col h5"><%#Eval("Valor","{0:C}")%> </div>
                                <div class="col-2">
                                    <asp:Button runat="server" Text="+" CssClass="btn btn-sm btn-dark" ToolTip="Agregar a pedido" />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>


    <% } %>
    </div>

<!--************* ESTILOS Y SCRIPTS *************** -->
|<!-- Styles Mesas -->
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

        .mesaTop{
            width: 100%;
            height: 15px;
            position: absolute;
            top: 0;
            left: 0;
        }

        .mesaBottom{
            width: 100%;
            height: 15px;
            background-color: var(--bg-danger);
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
          width: 20%;
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
    </style>
|<!-- Fin Styles Mesas -->

|<!-- Scripts Mesas -->
<script>
    let numeroMesa = <%: MesasActivas %>;
    let sectionMesa = document.getElementById("section-mesa");
    const modal = document.getElementById("modalMesas");
    const closeModalBtn = document.getElementsByClassName("close")[0];
    let contenidoModal = document.getElementById("modal-content");

    //Traemos datos de mesas desde codebehind
    function obtenerDatosMesas() {
        return new Promise((resolve) => {
            if (typeof datosMesasArray !== 'undefined') {
                const datosMesas = JSON.parse(datosMesasArray)
                const numeroMesas = JSON.parse(numeroMesasActivasArray)
                resolve({ datosMesas, numeroMesas });
            } else {
                const intervalo = setInterval(() => {
                    if (typeof datosMesasArray !== 'undefined') {
                        clearInterval(intervalo);
                        const datosMesas = JSON.parse(datosMesasArray)
                        const numeroMesas = JSON.parse(numeroMesasActivasArray)
                        resolve({ datosMesas, numeroMesas });
                    }
                }, 0);
            }
        });
    }

    //Llamamos datos de las mesas
    obtenerDatosMesas()
        .then(({ datosMesas, numeroMesas }) => {
            renderMesa(datosMesas, numeroMesas);
        });

    function renderMesa(datosMesa, numeroMesas) {

        //BORRAR!
        console.log("Datos", datosMesa)
        console.log("Numeros", numeroMesas)

        for (let i = 0; i < numeroMesa; i++) {
            //Buscamos mesa
            let mesa = datosMesa.find(item => item.mesa == numeroMesas[i].Numero)

            //color del Mesero
            let colorMesero;

            if (mesa) {
                colorMesero = convertirAHexadecimal(mesa.mesero)
            } else {
                colorMesero = "#666"
            }

            //Main
            let mainDiv = document.createElement("div");
            mainDiv.classList.add("d-flex", "mesa", "bg-body-secondary");
            let idMesa = mesa ? 'mesa_'+mesa.mesa : 'mesa'
            mainDiv.setAttribute("id", idMesa);
            let meseroNoAsignado = "No Asignado";
            mainDiv.setAttribute("title", `Mesero: ${mesa ? mesa.nombre + ' ' + mesa.apellido : meseroNoAsignado}`);

            //Top -- Background según mesero
            let mesaTop = document.createElement("div");
            mesaTop.classList.add("mesaTop");
            mesaTop.style.background = colorMesero

            //Bottom -- Background según estado
            let mesaBottom = document.createElement("div");
            mesaBottom.classList.add("mesaBottom");

            //Numero
            let mesaNumber = document.createElement("div");
            //TODO poner numero de mesa
            mesaNumber.innerHTML = `<p>${numeroMesas[i].Numero}</p>`;

            //Appends
            mainDiv.appendChild(mesaTop);
            mainDiv.appendChild(mesaNumber);
            mainDiv.appendChild(mesaBottom);
            sectionMesa.appendChild(mainDiv);

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
                    <p class="fw-bold">Estado: <span class="fw-normal">Cerrada</span></p>
                    `;
                }

            })
        }
    }

    //Boton modal para cerrar
    closeModalBtn.addEventListener("click", function () {
        modal.style.display = "none";
    });


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
|<!-- Fin Scripts Mesas -->


</asp:Content>



