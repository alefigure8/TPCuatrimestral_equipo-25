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
            <asp:Repeater runat="server" ID="ProductoDDRepetidor">
                <ItemTemplate>
                    <div class="row border-bottom ">
                        <h5 class="card-title hover-shadow "><%#Eval("Nombre")%> </h5>
                        <br />
                        <p class="blockquote-footer">
                            Descripción: <%#Eval("Descripcion")%>
                            <br />
                            Valor: $<%#Eval("Valor")%>
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
    <div class="row bg-white m-2">

        <%--Productos Disponibles--%>
        <div class="col m-3">
             <h2 class="row rounded">Productos disponibles</h2>
            <asp:Repeater runat="server" ID="ProductoRepetidor">
                
                <ItemTemplate> 
                    <div class="row border border-1">
                        <span class="col-3 align-middle"><%#Eval("Nombre")%></span>
                        <span class="col-3 align-middle"> Stock: <%#Eval("Stock")%> </span> 
                    </div>
                </ItemTemplate>
                
                
                <%--<ItemTemplate> 
                    <div class="row border align-middle">
                        <div class="col-3 align-middle"><p class="hover-shadow text-uppercase align-middle"><%#Eval("Nombre")%></p></div>
                        <div class="col-3"><p> Stock: <%#Eval("Stock")%></p> </div> 
                    </div>
                </ItemTemplate>--%>

            </asp:Repeater>
        </div>

        <%--Productos Del Dia--%>
        <div class="col">
             <h2 class="bg-secondary text-white row">Menú disponible</h2>
            <asp:Repeater runat="server" ID="Repeater2">
                <ItemTemplate>
                    <div class="row border-bottom ">
                        <h5 class="card-title hover-shadow "><%#Eval("Nombre")%> </h5>
                        <br />
                        <p class="blockquote-footer">
                            Descripción: <%#Eval("Descripcion")%>
                            <br />
                            Valor: $<%#Eval("Valor")%>
                        </p>
                    </div>
                </ItemTemplate>

            </asp:Repeater>
        </div>




    </div>





    <%}%>
</asp:Content>
