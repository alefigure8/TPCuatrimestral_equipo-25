﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="RestoApp.Main1" %>

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
            <div class="bg-gray-100 rounded border-1 p-5">
                <div class="d-flex">
                    <p>Mesas Activas:<span class="fw-semibold"> <%= MesasActivas %></span></p>
                    <a class="link-dark ms-4" href="MesaHabilitar.aspx">Activar Mesas</a>
                </div>
                <div class="d-flex">
                    <p>Mesas Asignadas:<span class="fw-semibold"> <%= MesasAsignadas %></span></p>
                    <a class="link-dark ms-4" href="Mesas.aspx">Asignar Mesas</a>
                </div>
            </div>
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




        <!-- VISTA MESERO -->
        <%
        if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)

        {%>

        <div class="row p-3 justify-content-between ">

            <!-- SECCION MESAS ASIGNDAS -->
            <Section class="row bg-white rounded p-2 justify-content-around m-1">
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
            </Section>
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

            <div class="row bg-white rounded p-2 justify-content-around m-1 mt-2">
                <h2 class="h3">Menú Disponible:</h2>
                <asp:Repeater runat="server" ID="MenuDelDia">
                    <ItemTemplate>
                        <div class="row">
                            <h5 class="card-title"><%#Eval("Nombre")%> </h5>
                            <br />
                            <p class="small text-gray-600">
                                Tiempo Cocción: <%#Eval("TiempoCoccion")%>  -    
                                Valor: $<%#Eval("Valor")%>
                            </p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>


        </div>


    </div>






    <% } %>
    </div>


</asp:Content>
