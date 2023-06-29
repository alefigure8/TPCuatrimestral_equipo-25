<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Default.Master" AutoEventWireup="true" CodeBehind="ProductosDelDia.aspx.cs" Inherits="RestoApp.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%if (!Helper.AutentificacionUsuario.esUser((Dominio.Usuario)Session[Opciones.Configuracion.Session.Usuario]))
        { %>

    <div class="row">
        <div class="col-4 bg-dark"></div>

        <div class="col-4 bg-white">
            <h2 class="display-4 bg-secondary text-white row">Menú disponible</h2>
            <asp:Repeater runat="server" ID="MenuRepetidor">
                <ItemTemplate>
                    <div class="row border-bottom ">
                        <h5 class="card-title hover-shadow "><%#Eval("Nombre")%> </h5>
                        <br />
                        <p class="blockquote-footer">
                            Descripción: <%#Eval("Descripcion")%>
                            <br />
                              <%#Eval("Valor","{0:C}")%>
                        </p>
                    </div>
                </ItemTemplate>

            </asp:Repeater>
        </div>


        <div class="col-4 bg-dark"></div>

    </div>
    <%}
        else if (usuario?.Tipo == Opciones.ColumnasDB.TipoUsuario.Gerente)
        {
    %>

    <%--    Contenedor  --%>
    <div class="row bg-white rounded m-2 p-4">

        <%--Productos Disponibles--%>
        <div class="col">
            <h2 class="row rounded">Productos disponibles</h2>
            <asp:Repeater runat="server" ID="ProductoRepetidor">
                <ItemTemplate>
                    <div class="row p-1">
                        <span class="col-4 align-middle"><%#Eval("Nombre")%></span>
                        <span class="col-3 align-middle">Stock: <%#Eval("Stock")%> </span>
                        <span class="col-3 align-middle">Categoria <%#Eval("Categoria")%> </span>
                        <asp:Button runat="server" ID="BtnAgregarAPDD" OnClick="BtnAgregarAPDD_Click" CommandArgument='<%#Eval("Id")%>' Text=">" CssClass="col btn btn-sm btn-dark columna-grilla-btn" />

                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <%--Productos Del Dia--%>
        <div class="col">
            <h2 class="row rounded">Menú actual</h2>
            <asp:Repeater runat="server" ID="ProductoDelDiaRepetidor">
                <ItemTemplate>
                    <div class="row p-1">
                        <span class="col-4"><%#Eval("Nombre")%></span>
                        <span class="col">
                            
                            <asp:Button runat="server" ID="BtnDesactivar" OnClick="BtnDesactivar_Click" Text='<%#Eval("Activo").Equals(true) ? "Cerrar" : "Reabrir" %>' CommandArgument='<%#Eval("Id") %>' CssClass="col-3 btn btn-sm btn-dark m-1" />
                            <asp:TextBox runat="server" TextMode="Number" Visible="true" ID="tbAgregarStock" CssClass="col-2"></asp:TextBox>
                           <asp:Button runat="server" ID="BtnAgregarStock" Text="Agregar Stock" OnClick="BtnAgregarStock_Click" CssClass="col btn btn-sm btn-dark m-1" CommandArgument='<%#Eval("Id") %>' />

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
          
    <div class="row m-2 rounded">
        <div class="col-2 bg-dark"></div>

        <div class="col bg-white">
            <h2 class="display-4 bg-secondary text-white row">Menú disponible</h2>
            <asp:Repeater runat="server" ID="MenuMeseroRep">
                <ItemTemplate>

                   <div class="row border-bottom p-2">

                       <div class="col">

                           <div class="row h4 text-uppercase">
                               <%#Eval("Nombre")%> 
                           </div>

                           <asp:Panel runat="server" ID="PanelDetalles" CssClass="row small" Visible="false">
                               <div class="row">
                                   <div class="col">
                                    <%#Eval("Descripcion")%> 
                                </div>
                                   <div class="col-2">
                                <%#Eval("Valor","{0:C}")%>
                                       </div>
                               </div>
                                
                                 <%#Eval("AptoVegano").Equals(true) ? "· Es apto vegano" : string.Empty %>
                                 <%#Eval("AptoCeliaco").Equals(true) ? "· Es apto celiaco" : string.Empty %>
                                 <%#Eval("Alcohol").Equals(true) ? "· Contiene Alcohol" : string.Empty %>
                                 <%#Eval("TiempoCoccion")!=null ? "· Cocción:" : string.Empty  %> <%#Eval("TiempoCoccion")%>  
                           </asp:Panel>

                       </div>


                       <div class="col-3">
                        <asp:Button runat="server" CssClass="row btn btn-dark btn-sm small m-1" Text="Agregar  a pedido"/>
                       <asp:Button runat="server" CssClass="row btn btn-dark btn-sm small m-1" Text="Ver detalle" ID="BtnVerDetalle" OnClick="BtnVerDetalle_Click"/>
                       </div>
                   </div> 
                   

                </ItemTemplate>

            </asp:Repeater>
        </div>


        <div class="col-2 bg-dark"></div>

    </div>



      <%}
    %>


</asp:Content>
