<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="RestoApp.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ContentProductos" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main class="col bg-white ">

        <div class="container">

           
                
            
            <div class="row justify-content-start"">
                <div class="col">
                    <asp:Label ID="lblFiltrarPorCategoria" runat="server" Text="Filtrar por"></asp:Label>
                </div>
                <div class="col">
                    <asp:DropDownList ID="DDLCategorias" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                                <div class="col">
                    <asp:CheckBox ID="CheckBox2" runat="server" Text="Activo" CssClass="form-check-input"/>
                </div>
            </div>

            <div class="row justify-items-start d-flex align-items-start" >
               <div class="col-2 bg-light p-2"><p class="h6">Nombre</p></div>
               <div class="col-2 bg-light p-2"><p class="h6">Valor</p></div>
               <div class="col-2 bg-light p-2"><p class="h6">Activo</p></div>
               <div class="col-2 bg-light p-2"><p class="h6">Stock</p></div>
            </div>

            <div class="row">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
             <asp:UpdatePanel runat="server">
                 <ContentTemplate>
                             <div class="row justify-items-start">
               <asp:Repeater runat="server" ID="ProductoRepetidor">
       <ItemTemplate>
           <div class="row justify-content-start d-flex align-items-start">
               
               <div class="col-2 bg-light p-2 border-top"><p class="small"><%#Eval("Nombre")%></p></div>

               <div class="col-2 bg-light p-2 border-top"><p class="small">
                   <%# String.Format(new System.Globalization.CultureInfo("es-AR"), "{0:C}", Eval("Valor")) %></p></div>

               <div class="col-2 bg-light p-2 border-top"><p class="small">
                   <%# ((bool)Eval("Activo") == true ? "SI" : "NO")%>
                </p></div>

               <div class="col-2 bg-light p-2 border-top"><p class="small"><%#Eval("Stock")%></p></div>

               <div class="col-2 bg-light p-2 border-top">
               <asp:Button Text="Ver Detalle" runat="server" Cssclass="btn btn-primary btm-sm"  
                   CommandArgument='<%#Eval("Id")%>' CommandName="Id"/>
            </div>

               <div class="col-2 bg-light p-2 border-top">
               <asp:Button Text="Activar" runat="server" Cssclass="btn btn-primary btm-sm"  
                   CommandArgument='<%#Eval("Id")%>' CommandName="Id"/>
            </div>
               </div>
       </ItemTemplate>
   </asp:Repeater>
        </div>
                 </ContentTemplate>
        </asp:UpdatePanel>
                </div>

            <div class="row">
                <div class="col-2 me-2 card btn align-items-center justify-content-center">
                    <i class="fa fa-cutlery" style="font-size:5rem;" aria-hidden="true"></i>
                     <asp:LinkButton ID="LinkButton3" runat="server"
                     CssClass="h5">Nuevo Plato
                    </asp:LinkButton>
                </div>
                <div class="col-2 me-2 card btn align-items-center justify-content-center">
                    <i class="fa fa-wine-glass" style="font-size:5rem;" aria-hidden="true"></i>
                     <asp:LinkButton ID="LinkButton1" runat="server"
                     CssClass="h5">Nueva Bebida
                    </asp:LinkButton>
                </div>
                            <div class="col-2 me-2 card btn align-items-center justify-content-center">
                    <i class="fa fa-file" aria-hidden="true" style="font-size:5rem;"></i>
                     <asp:LinkButton ID="LinkButton2" runat="server"
                     CssClass="h5">Categorias
                    </asp:LinkButton>
                </div>
           </div>
       
            </div>
      



          
        </main>



</asp:Content>
