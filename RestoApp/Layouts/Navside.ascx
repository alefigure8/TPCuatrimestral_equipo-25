﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navside.ascx.cs" Inherits="RestoApp.Layouts.Navside" %>

<nav class="col-2 flex-shrink-0 ">
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
        <ul class="list-unstyled ps-0">
            <li class="align-items-center rounded collapsed text-light btn">
                <a href="#" class="nav-link active" aria-current="page">Inicio
                </a>
            </li>
            <li class="mb-1">
                <a class="btn btn-toggle align-items-center rounded collapsed text-light" data-bs-toggle="collapse" data-bs-target="#otro-collapse" aria-bs-expanded="true">Otro
                </a>
                <div class="collapse " id="otro-collapse">
                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                        <li><a href="#" class="nav-link ps-3">Opcion 1</a></li>
                        <li><a href="#" class="nav-link ps-3">Opcion 2</a></li>
                        <li><a href="#" class="nav-link ps-3">opcion 3</a></li>
                        <li><a href="#" class="nav-link ps-3">opcion 3</a></li>
                    </ul>
                </div>
            </li>
            <li class="mb-1">
                <a class="btn btn-toggle align-items-center rounded collapsed text-light" data-bs-toggle="collapse" data-bs-target="#productos-collapse" aria-bs-expanded="true">Productos
                </a>
                <div class="collapse" id="productos-collapse">
                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                        <li><a href="#" class="nav-link ps-3">Opcion 1</a></li>
                        <li><a href="#" class="nav-link ps-3">Opcion 2</a></li>
                        <li><a href="#" class="nav-link ps-3">opcion 3</a></li>
                        <li><a href="#" class="nav-link ps-3">opcion 3</a></li>
                    </ul>
                </div>
            </li>
            <li class="mb-1">
                <a class="btn btn-toggle align-items-center rounded collapsed text-light" data-bs-toggle="collapse" data-bs-target="#mesa-collapse" aria-bs-expanded="true">Mesa
                </a>
                <div class="collapse" id="mesa-collapse">
                    <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                        <li><a href="#" class="nav-link ps-3">Opcion 1</a></li>
                        <li><a href="#" class="nav-link ps-3">Opcion 2</a></li>
                        <li><a href="#" class="nav-link ps-3">opcion 3</a></li>
                        <li><a href="#" class="nav-link ps-3">opcion 3</a></li>
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
                <strong><%= usuario?.Mail %></strong>
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
</nav>
