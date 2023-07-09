<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="RestoApp.Tickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <header class="col-10 bg-white rounded p-3 m-3">
            <p class="fs-2">Tickets</p>
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
                        <p><span class="fw-semibold">Total: $</span> <%= precio %></p>
                        <button class="btn btn-dark w-100">Cobrar</button>
                    </div>
                </section>

                <!-- Fin Ticket -->

            </ItemTemplate>
        </asp:Repeater>
        <!-- Fin Repeater de Tickets-->

    </div>
</asp:Content>
