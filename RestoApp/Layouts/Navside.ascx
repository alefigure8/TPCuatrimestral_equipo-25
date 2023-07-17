<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navside.ascx.cs" Inherits="RestoApp.Layouts.Navside" %>

<nav class="col-sm col-lg-2 nav-heigth d-flex justify-content-between bg-dark" id="nav">
    <div
        class="d-flex flex-column p-lg-3 p-0 text-white nav-heigth-div bg-dark w-100" id="navDiv">
        <a
            href="Main.aspx"
            class="d-lg-flex justify-content-center align-items-center mb-3 mb-lg-0 ms-5 ms-lg-0 me-lg-auto text-white text-decoration-none ">
            <img src="/Content/Image/logo.jpg" class="rounded-circle mb-lg-3 m-0 min-w-25 h-auto img-width" />
        </a>
        <hr class="d-none d-lg-block" />
        <ul class="list-unstyled ps-0 d-flex flex-column d-none d-lg-block bg-dark" id="ul_opciones">
            <li class="mb-1">

                <% if (usuario?.Tipo != Opciones.ColumnasDB.TipoUsuario.Cocinero)
                { %>
                <a href="Main.aspx" class="nav-link ms-3" aria-current="page">Inicio
                </a>
                <%}
                    else
                    {  %>
                <a href="Cocina.aspx" class="nav-link ms-3" aria-current="page">Cocina
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
                        <li><a href="Tickets.aspx?dia=<%: DateTime.Now.ToString("yyyy-MM-dd") %>" class="nav-link ps-3">Tickets Del Dia</a></li>
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
                        <li><a href="Pedidos.aspx" class="nav-link ps-3">Pedidos del día</a></li>
                        <%}%>
                    </ul>
                </div>
            </li>
        </ul>
        <hr class="d-none d-lg-block"/>
        <div class="dropdown d-none d-lg-block w-100">
            <a
                href="#"
                class="d-flex align-items-center text-white text-decoration-none dropdown-toggle"
                id="dropdownUser1"
                data-bs-toggle="dropdown"
                aria-expanded="false">
                <img
                    src="https://surgassociates.com/wp-content/uploads/610-6104451_image-placeholder-png-user-profile-placeholder-image-png-286x300.jpg"
                    alt=""
                    class="rounded-circle me-2 d-none d-lg-block"
                    width="25"
                    height="25" />
                <strong class="d-xl-flex align-items-center gap-2 d-none"><%= usuario?.Mail %>
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
    <!-- RESPONSIVE -->
    <div class="d-flex justify-content-center align-items-center d-lg-none" id="hamburger">
        <i class="fa-solid fa-bars text-gray-100 me-5 fs-3 cursor-pointer"></i>
    </div>
    <!-- FIN RESPONSIVE -->
</nav>

<style>
    .img-width{
        width: 150px;
    }

    @media (max-width: 991.98px) {
        .img-width {
            width: 60px;
        }
    }

    @media (min-width: 991.98px) { 
        .img-width{
        width: 150px;
        }
    }

    .nav-heigth{
        min-height: 60px;
        transition: all 0.3s ease;
    }

    @media (max-width: 991.98px) {
    .nav-heigth{
        max-height: 60px;
        }
    }

    @media (min-width: 991.98px) { 
        .nav-heigth{
            min-height: 100vh;
        }
    }

    .nav-heigth-div{
    min-height: 60px;
    transition: all 0.3s ease;
    }

    @media (max-width: 991.98px) {
    .nav-heigth-div{
        max-height: 40px;
        }
    }

    @media (min-width: 991.98px) { 
        .nav-heigth-div{
            min-height: 100vh;
        }
    }

    .wvh-50{
        height: 40vh; /* Ajusta automáticamente la altura */
        z-index:3;
        transition: all 0.3s ease;
    }

    .autoHeigth{
        min-height: 40vh; /* Ajusta automáticamente la altura */
        transition: all 0.3s ease;
         min-width: 50vw;
    }

    .ul_opciones_hamburger{
        min-width: 100%;
        min-height: auto;
       
    }

</style>

<script>
    const hamburger = document.getElementById("hamburger");
    const ul_opciones = document.getElementById("ul_opciones");
    const nav = document.getElementById("nav");
    const navDiv = document.getElementById("navDiv");
    let isOpen = false;

    hamburger.addEventListener('click', () => {
        if (!isOpen) {
            ul_opciones.classList.remove("d-none")
            ul_opciones.classList.remove("ul_opciones_hamburger")
            nav.classList.add("wvh-50")
            navDiv.classList.add("autoHeigth")
        } else {
            ul_opciones.classList.add("d-none")
            ul_opciones.classList.add("ul_opciones_hamburger")
            nav.classList.remove("wvh-50")
            navDiv.classList.remove("autoHeigth")
        }

        isOpen = !isOpen
       
    })

    window.addEventListener('resize', function () {
        var anchoPantalla = window.innerWidth;
        if (anchoPantalla > 990) {
            ul_opciones.classList.add("d-none")
            ul_opciones.classList.add("ul_opciones_hamburger")
            nav.classList.remove("wvh-50")
            navDiv.classList.remove("autoHeigth")
            isOpen = !isOpen
        }
    });

</script>
