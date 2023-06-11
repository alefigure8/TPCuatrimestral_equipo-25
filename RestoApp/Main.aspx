<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="RestoApp.Main1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-2">
            <div
                class="d-flex flex-column p-3 text-white bg-dark min-vh-100"
                style="width: 280px">
                <a
                    href="/"
                    class="d-flex align-items-center mb-3 mb-md-0 me-md-auto text-white text-decoration-none">
                    <svg class="bi me-2" width="40" height="32">
                                            </svg>
                    <span class="fs-4">RestoApp</span>
                </a>
                <hr />
                <ul class="nav nav-pills flex-column mb-auto">
                    <li class="nav-item">
                        <a href="#" class="nav-link active" aria-current="page">
                            <svg class="bi me-2" width="16" height="16">
                                                            </svg>
                            Inicio
                        </a>
                    </li>
                    <li>
                        <a href="#" class="nav-link text-white">
                            <svg class="bi me-2" width="16" height="16">
                                                            </svg>
                            Mesas
                        </a>
                    </li>
                    <li>
                        <a href="#" class="nav-link text-white">
                            <svg class="bi me-2" width="16" height="16">
                                                            </svg>
                            Ordenes
                        </a>
                    </li>
                    <li>
                        <a href="#" class="nav-link text-white">
                            <svg class="bi me-2" width="16" height="16">
                                                            </svg>
                            Menú
                        </a>
                    </li>
                    <li>
                        <a href="#" class="nav-link text-white">
                            <svg class="bi me-2" width="16" height="16">
                                se>
                            </svg>
                            Reservas
                        </a>
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
                            src="https://github.com/mdo.png"
                            alt=""
                            class="rounded-circle me-2"
                            width="32"
                            height="32" />
                        <strong>Usuario</strong>
                    </a>
                    <ul
                        class="dropdown-menu dropdown-menu-dark text-small shadow"
                        aria-labelledby="dropdownUser1">
                        <li><a class="dropdown-item" href="#">Perfil</a></li>
                        <li>
                            <hr class="dropdown-divider" />
                        </li>
                        <li>
                            <asp:LinkButton runat="server" OnClick="salir_click" class="dropdown-item" Text="Salir" /></li>
                    </ul>
                </div>
            </div>
        </div>
        <!-- Panel -->
        <div class="col ">
            <div class="d-flex flex-column container">
                <div class="p-3">
                    <!-- Dropdown-->
                    <div class="dropdown">
                        <button
                            class="btn btn-secondary dropdown-toggle"
                            type="button"
                            id="dropdownMenuButton1"
                            data-bs-toggle="dropdown"
                            aria-expanded="false">
                            Cantidad de Mesas
                        </button>
                        <ul
                            class="dropdown-menu"
                            aria-labelledby="dropdownMenuButton1"
                            id="dropdown-mesa">
                        </ul>
                    </div>
                    <!--Fin Dropdown-->
                </div>
                <div class="col">
                    <!--Mesas-->
                    <div class="row" id="mesas">
                    </div>
                    <!--Fin Mesas-->
                </div>
                <div class="d-flex justify-content-end my-2">
                    <input type="number" class="me-2 rounded-3 cursor-pointer" id="txb_guardar_mesa" disabled>
                    <input type="button" class="btn btn-dark" value="Guardar Mesas">
                </div>
            </div>
        </div>
    </div>
    <!-- Fin Panel -->
</asp:Content>
