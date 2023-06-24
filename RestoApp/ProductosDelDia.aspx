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
        <asp:Repeater runat="server" ID="ProductoRepetidor">
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
else { %>


   


    <%} %>

    


</asp:Content>
