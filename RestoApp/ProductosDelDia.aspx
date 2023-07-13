<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Default.Master" AutoEventWireup="true" CodeBehind="ProductosDelDia.aspx.cs" Inherits="RestoApp.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <%if (!Helper.AutentificacionUsuario.esUser((Dominio.Usuario)Session[Opciones.Configuracion.Session.Usuario]))
        { %>

    <div class="row">
        <div class="col-3 bg-dark"></div>


        <div class="col-6 p-4">
            <div class="display-4 bg-secondary text-white row rounded-top p-2" style="font-weight: bolder;">Menú disponible</div>

            <div class="row bg-white rounded-bottom p-4 justify-content-around">
                <asp:Repeater runat="server" ID="MenuRepetidor">
                    <ItemTemplate>
                        <div class="row border-bottom">
                            <div style="font-size: 1.5rem; font-family: Verdana;"><%#Eval("Nombre")%>  </div>
                            <div class="row">
                                <div class="col-8" style="font-style: italic; color: #808080"><%#Eval("Descripcion")%></div>
                                <div class="col" style="font-weight: bold; text-align: right;"><%#Eval("Valor","{0:C}")%></div>
                            </div>
                        </div>
                    </ItemTemplate>

                </asp:Repeater>
            </div>

        </div>


        <div class="col-3 bg-dark"></div>

    </div>
    <%}
        else if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente)
        {
    %>

    <%--    Contenedor  --%>
      <div class="row col-12 bg-dark display-6 rounded-top text-white">
     <div class="col-6">Productos disponibles</div>
        <div class="col-6">Productos en menú actual</div>
          </div>

    <div class="row col-12 bg-white rounded-bottom">
        <%--Productos Disponibles--%>
        <div class="col">
            <div class="row p-1 h5" style="font-weight: bold;">
                <span class="col-6 align-middle">Producto </span>
                <span class="col-2 align-middle" style="text-align: center;">Stock </span>
                <span class="col-2 align-middle">Categoria </span>
                <span class="col-2 d-flex justify-content-around"></span>
            </div>


            <asp:Repeater runat="server" ID="ProductoRepetidor" OnItemDataBound="ProductoRepetidor_ItemDataBound">
                <ItemTemplate>
                    <div class="row p-1 border-bottom">
                        <span class="col-6 align-middle"><%#Eval("Nombre")%></span>
                        <span class="col-2 align-middle" style="text-align: center;"><%#Eval("Stock")%> </span>
                        <asp:Label ID="lblCategoria" runat="server" CssClass="col-2 align-middle" Font-Size="15px" Text='<%#Eval("Categoria")%> '></asp:Label>
                        <div class="col-2 d-flex justify-content-around">
                            <asp:Button runat="server" ID="BtnAgregarAPDD" OnClick="BtnAgregarAPDD_Click" ToolTip="Agregar a Menú" CommandArgument='<%#Eval("Id")%>' Text=">" CssClass=" btn btn-dark rounded-circle" />
                        </div>

                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <%--Productos Del Dia--%>
        <div class="col">
            <div class="row p-1 h5" style="font-weight: bold;">
                <span class="col-5 align-middle"></span>
                <span class="col-2 align-middle">Estado </span>
                <span class="col columna-btn" style="text-align: center;">Stock </span>
            </div>
            <asp:Repeater runat="server" ID="ProductoDelDiaRepetidor">
                <ItemTemplate>
                    <div class="row p-1 border-bottom">
                        <span class="col-5"><%#Eval("Nombre")%></span>
                        <span class="col-2">
                            <asp:Button runat="server" ID="BtnDesactivar" OnClick="BtnDesactivar_Click" Text='<%#Eval("Activo").Equals(true) ? "Cerrar" : "Reabrir" %>' CommandArgument='<%#Eval("Id") %>' CssClass="btn btn-dark" />
                        </span>
                        <span class="col" style="text-align: center;">
                            <asp:Button runat="server" ID="BtnAgregarStock" Text="+" ToolTip="Agregar Stock" OnClick="BtnAgregarStock_Click" CssClass="btn btn-dark rounded-circle" CommandArgument='<%#Eval("Id") %>' />
                            <asp:TextBox runat="server" TextMode="Number" min="0" ID="tbAgregarStock" CssClass="col-3"></asp:TextBox>
                            <asp:Button runat="server" ID="BtnQuitarStock" Text="-" ToolTip="Restar Stock" OnClick="BtnQuitarStock_Click" CssClass="btn btn-dark rounded-circle" CommandArgument='<%#Eval("Id") %>' />
                        </span>
                    </div>

                </ItemTemplate>

            </asp:Repeater>
        </div>
    </div>
    <%}
        else if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Mesero)
        {
    %>

    <div class="col-11 p-2 display-4 text-white bg-dark rounded-top" style="font-weight: bolder;">Menú disponible</div>
    <div class="col-11 p-3 bg-white rounded-bottom ">


        <asp:Repeater runat="server" ID="MenuMeseroRep">
            <ItemTemplate>

                <div class="row border-bottom p-2">

                    <div class="col">

                        <div class="row text-uppercase align-middle" style="font-size: 20px; padding-left: 1rem; font-weight: bolder"><%#Eval("Nombre")%> </div>

                        <asp:Panel runat="server" ID="PanelDetalles" CssClass="row" Visible="false">
                            <div class="row">
                                <div class="col" style="font-style: italic; padding-left: 1rem;"><%#Eval("Descripcion")%></div>
                                <div class="col-2" style="font-weight: bold;"><%#Eval("Valor","{0:C}")%> </div>
                            </div>

                            <div class="row" style="padding-left: 2rem;">
                                <%#Eval("AptoVegano").Equals(true) ? "· Es apto vegano" : string.Empty %>
                                <%#Eval("AptoCeliaco").Equals(true) ? "· Es apto celiaco" : string.Empty %>
                                <%#Eval("Alcohol").Equals(true) ? "· Contiene Alcohol" : string.Empty %>
                                <%#Eval("TiempoCoccion")!=null ? "· Cocción:" : string.Empty  %> <%#Eval("TiempoCoccion")%>
                            </div>


                        </asp:Panel>

                    </div>


                    <div class="col-2">
                        <%--  <asp:Button runat="server" CssClass="row btn btn-dark btn-sm small m-1" Text="Agregar  a pedido" />--%>
                        <asp:Button runat="server" CssClass="row btn btn-dark" Text="Ver detalle" ID="BtnVerDetalle" OnClick="BtnVerDetalle_Click" />
                    </div>
                </div>


            </ItemTemplate>

        </asp:Repeater>
    </div>





    <%}
    %>


</asp:Content>
