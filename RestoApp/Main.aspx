<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="RestoApp.Main1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h4 class="text-gray-100">Hola, <%= usuario?.Nombres %> <%= usuario?.Apellidos %> (<%= usuario?.Tipo %>)</h4>

    <!-- Agregar Mesas habilitadas, mozos habilitados, productos, -->
    <div class="row">

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
                <div class="d-flex">
                    <p>Meseros Presentes:<span class="fw-semibold"> <%= MesasAsignadas %></span></p>
                    <a class="link-dark ms-4" href="MesaHabilitar.aspx">Asignar Mesas</a>
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
        <% }

            if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)

            {%>

        <div class="row p-3 justify-content-between ">

            <div class="row bg-white rounded p-2 justify-content-around">

                <div class="row h3">Mis mesas </div>


                <div class="row justify-content-around justify-items-start p-3">

                    
                    <div class="col-2">
                 <div class=" status-free rounded p-3">
                    Mesa #02
                     <p>(Disponible)</p>
                    </div>
                    <button class="btn btn-dark">Abrir </button>
                        </div>
                    
                     <div class="col-2">
                 <div class=" status-busy rounded p-3">
                    Mesa #05
                     <p>(Ocupada)</p>
                    </div>
                    <button class="btn btn-dark">Cerrar </button>
                        </div>

                 <div class="col-2">
                 <div class=" status-busy rounded p-3">
                    Mesa #07
                     <p>(Ocupada)</p>
                    </div>
                     <a class="btn btn-dark">Cerrar</a>
                 </div>


                 <div class="col-2">
                 <div class=" status-free rounded p-3">
                    Mesa #12
                     <p>(Disponible)</p>
                    </div>
                    <button class="btn btn-dark">Abrir </button>
                        </div>
                </div>
              
            </div>
            
            <div><br /></div>

            <div class="row bg-white rounded p-3 justify-content-around">

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

            <br />

            <div><br /></div>
            <div class="row bg-white rounded p-3">
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
