<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navside.ascx.cs" Inherits="RestoApp.Layouts.Navside" %>

<nav class="col-sm col-lg-2 min-vh-100">
    <div
        class="d-flex flex-column p-3 text-white bg-dark h-100">
        <a
            href="/"
            class="d-flex justify-content-center align-items-center mb-3 mb-md-0 me-md-auto text-white text-decoration-none w-100">
            <img src="/Content/Image/logo.jpg" style="width: 150px; height: 150px;" class="rounded-circle mb-3" />
        </a>
        <hr />
        <ul class="list-unstyled ps-0">
            <li class="align-items-center rounded collapsed text-light btn">

                <% if (usuario?.Tipo != Opciones.ColumnasDB.TipoUsuario.Cocinero)
                { %>
                <a href="Main.aspx" class="nav-link active" aria-current="page">Inicio
                </a>
                <%}
                    else
                    {  %>
                <a href="Cocina.aspx" class="nav-link active" aria-current="page">Cocina
                </a>
                <%} %>
            </li>
            <!-- Si el usuario es Gerente o Admin pueden ver las mesas-->
            <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente || usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Admin)
                { %>
            <li class="mb-1">
                <a class="btn btn-toggle align-items-center rounded collapsed text-light" data-bs-toggle="collapse" data-bs-target="#mesa-collapse" aria-bs-expanded="true">Mesas
                </a>
                <div class="collapse " id="mesa-collapse">
                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                        <li><a href="Mesas.aspx" class="nav-link ps-3">Asignar Mesas</a></li>
                        <li><a href="MesaHabilitar.aspx" class="nav-link ps-3">Habilitar Mesas</a></li>
                        <li><a href="#" class="nav-link ps-3">Configuración</a></li>
                    </ul>
                </div>
            </li>
            <%}%>
            <li class="mb-1">
                <%if (usuario?.Tipo != Opciones.ColumnasDB.TipoUsuario.Cocinero) {  %>
                <a class="btn btn-toggle align-items-center rounded collapsed text-light" data-bs-toggle="collapse" data-bs-target="#productos-collapse" aria-bs-expanded="true">
                    <%--Si es gerente o admin la solapa se llama productos, si no, se llama menu--%>
                    <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente || usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Admin)
                    { %>  
                    Productos
                    <%} else
                { %>
                    Menu
                    <% } %>
                </a>
                <div class="collapse" id="productos-collapse">
                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                        <%--Si es gerente o admin puede ver la opcion administrar producto--%>
                        <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente || usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Admin)
                              { %>
                        <li><a href="Productos.aspx" class="nav-link ps-3">Administrar Productos</a></li>
                        <%} %>
                        <li><a href="ProductosDelDia.aspx" class="nav-link ps-3">
                            <%--Si es gerente o admin la solapa se llama productos, si no, se llama menu--%>
                            <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente || usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Admin)
                              { %> 
                                Productos del día
                               <%} else
                            { %>
                                Menu completo
                             <% } %>
                        </a></li>
                    </ul>
                </div>
            </li>
            <%}%>
            <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente || usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Admin)
                { %>
            <li class="mb-1">
                <a class="btn btn-toggle align-items-center rounded collapsed text-light" data-bs-toggle="collapse" data-bs-target="#usuarios-collapse" aria-bs-expanded="true">Usuarios
                </a>
                <div class="collapse" id="usuarios-collapse">
                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                        <li><a href="usuarios.aspx" class="nav-link ps-3">Administrar usuarios</a></li>
                    </ul>
                </div>
            </li>
            <%}%>
            <li class="mb-1">
                <a class="btn btn-toggle align-items-center rounded collapsed text-light" data-bs-toggle="collapse" data-bs-target="#ticket-collapse" aria-bs-expanded="true">Tickets
                </a>
                <div class="collapse " id="ticket-collapse">
                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                        <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente || usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Admin)
                    { %>
                        <li><a href="Tickets.aspx" class="nav-link ps-3">Tickets Abiertos</a></li>
                        <li><a href="Tickets.aspx?dia=<%: DateTime.Now.ToString("yyyy-MM-dd") %>" class="nav-link ps-3">Tickets Del Dia</a></li>
                        <li><a href="#" class="nav-link ps-3">Estadística</a></li>
                        <%}%>
                        <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)
                    { %>
                        <li><a href="Tickets.aspx" class="nav-link ps-3">Tickets Abiertos</a></li>
                        <li><a href="#" class="nav-link ps-3">Tickets Del Dia</a></li>
                        <%}%>
                    </ul>
                </div>
            </li>
                        <li class="mb-1">
                <a class="btn btn-toggle align-items-center rounded collapsed text-light" data-bs-toggle="collapse" data-bs-target="#peidos-collapse" aria-bs-expanded="true">Pedidos
                </a>
                <div class="collapse " id="peidos-collapse">
                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                        <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente || usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Admin)
                    { %>
                        <li><a href="Pedidos.aspx" class="nav-link ps-3">Pedidos del Día</a></li>
                        <li><a href="#" class="nav-link ps-3">Estadística</a></li>
                        <%}%>
                        <% if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)
                    { %>
                        <li><a href="Pedidos.aspx?abiertos=true" class="nav-link ps-3">Pedidos Abiertos</a></li>
                        <li><a href="Pedidos.aspx?dia=<%: DateTime.Now.ToString("yyyy-MM-dd") %>" class="nav-link ps-3">Pedidos Del Dia</a></li>
                        <%}%>
                    </ul>
                </div>
            </li>
        </ul>
        <hr />
        <div class="dropdown">
            <a
                href="#"
                class="d-flex align-items-center text-white text-decoration-none dropdown-toggle"
                id="dropdownUser1"
                data-bs-toggle="dropdown"
                aria-expanded="false">
                <img
                    src="https://surgassociates.com/wp-content/uploads/610-6104451_image-placeholder-png-user-profile-placeholder-image-png-286x300.jpg"
                    alt=""
                    class="rounded-circle me-2"
                    width="32"
                    height="32" />
                <strong class="d-flex align-items-center gap-2"><%= usuario?.Mail %>
                    <asp:UpdatePanel ID="UpdatePanel1"
                        runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hiddenBotonID" runat="server" />
                            <!-- Estado del mesero -->
                            <%if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero && meseroPorDia == null)
                                { %>
                            <i class="fa-sharp fa-solid fa-circle text-warning"></i>
                            <%}
                                else if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)
                                { %>
                            <i class="fa-sharp fa-solid fa-circle text-success"></i>
                            <%} %>
                            <!-- Fin Estado del mesero -->
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="hiddenBotonID" EventName="ValueChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </strong>
            </a>
            <ul
                class="dropdown-menu dropdown-menu-dark text-small shadow"
                aria-labelledby="dropdownUser1">
                <li>
                    <asp:LinkButton ID="Btn_Perfil" runat="server" class="dropdown-item" Text="Perfil" OnClick="Btn_Perfil_Click" />
                </li>
                <li>
                    <hr class="dropdown-divider" />
                </li>
                <li>
                    <asp:LinkButton runat="server" OnClick="salir_click" class="dropdown-item" Text="Salir" />

                </li>
            </ul>
        </div>
    </div>
</nav>
