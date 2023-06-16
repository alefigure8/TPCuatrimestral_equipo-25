<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="RestoApp.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ContentProductos" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main class="col bg-white ">

        <div class="container">
            <div class="row justify-content-start"">
                FILTRAR POR CATEGORIA
                <div class="col">
                    <asp:DropDownList ID="DDLCategorias" runat="server" CssClass="">
                    </asp:DropDownList>
                </div>
                RANGO PRECIOS
                <div class="col">
                    <asp:TextBox ID="TextBox1" runat="server" CssClass=" col-md-2"></asp:TextBox>
                    
                    
                    a
                     <asp:TextBox ID="TextBox2" runat="server" CssClass=" col-md-2"></asp:TextBox>
                </div>


                <div class="col">
                    <asp:Button runat="server" Text="Limpiar filtros"/>
                </div>
            </div>

            <div class="row">
                                <div class="col-4">
                                    VER SOLO
                       <asp:CheckBox ID="CheckBox3" runat="server" Text="Activos" CssClass="form-check-input"/>
                    <asp:CheckBox ID="CheckBox2" runat="server" Text="Vegano" CssClass="form-check-input"/>
                 <asp:CheckBox ID="CheckBox1" runat="server" Text="Celiaco" CssClass="form-check-input"/>
                                    </div>
                                    <div class="col-4">
                                     ORDENAR
                    <asp:CheckBox ID="CheckBox5" runat="server" Text="↑valor" CssClass="form-check-input"/>
                                    <asp:CheckBox ID="CheckBox7" runat="server" Text="↓valor" CssClass="form-check-input"/>
          <asp:CheckBox ID="CheckBox4" runat="server" Text="↑stock" CssClass="form-check-input"/>
                                    <asp:CheckBox ID="CheckBox6" runat="server" Text="stock↓" CssClass="form-check-input"/>
           </div>
                                        <div class="col">
                     <asp:TextBox ID="TextBox3" runat="server" CssClass=" col" Text="buscar"></asp:TextBox>
                </div>
                               
            </div>

            <div class="row" >
               <div class="col columna-grilla bg-dark text-white"><p class="h6">Nombre</p></div>
               <div class="col columna-grilla bg-dark text-white"><p class="h6">Categoria</p></div>
               <div class="col columna-grilla bg-dark text-white"><p class="h6">Valor</p></div>
               <div class="col columna-grilla-sm bg-dark text-white"><p class="h6">Activo</p></div>
                <div class="col columna-grilla-sm bg-dark text-white"><p class="h6">AP. Vegano</p></div>
                 <div class="col columna-grilla-sm bg-dark text-white"><p class="h6">Apto Celiaco</p></div>
                <div class="col columna-grilla-sm bg-dark text-white"><p class="h6">Cont. Alcohol</p></div>
                 <div class="col columna-grilla-sm bg-dark text-white"><p class="h6">Stock </p></div>
                <div class="col columna-grilla-sm bg-dark text-white"><p class="h6"></p></div>
                 <div class="col columna-grilla-sm bg-dark text-white"><p class="h6"></p></div>
            </div>

            <div class="row">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
             <asp:UpdatePanel runat="server">
                 <ContentTemplate>
                             <div class="row justify-items-start">
               <asp:Repeater runat="server" ID="ProductoRepetidor">
       <ItemTemplate>
           <div class="row justify-content-start d-flex align-items-start">
               
               <div class="col small columna-grilla"><p class="small"><%#Eval("Nombre")%></p></div>

             
     <div class="col border-top small columna-grilla"><p class="small">
                   <%#Eval("Categoria")%>
                </p></div>
   


               <div class="col border-top small columna-grilla"><p class="small">
                   <%# String.Format(new System.Globalization.CultureInfo("es-AR"), "{0:C}", Eval("Valor")) %></p></div>
                <div class="col border-top small columna-grilla-sm"><p class="small">
                   <%# ((bool)Eval("Activo") == true ? "SI" : "NO")%>
                </p></div>


               <div class="col border-top small columna-grilla-sm"><p class="small">
                   <%# ((bool)Eval("AptoVegano") == true ? "SI" : "NO")%>
                </p></div>

                <div class="col border-top small columna-grilla-sm"><p class="small">
                   <%# ((bool)Eval("AptoCeliaco") == true ? "SI" : "NO")%>
                </p></div>

                <div class="col border-top small columna-grilla-sm"><p class="small">
                   <%# ((bool)Eval("Alcohol") == true ? "SI" : "NO")%>
                </p></div>


               <div class="col border-top small columna-grilla-sm"><p class="small"><%#Eval("Stock")%></p></div>

               <div class="col border-top small columna-grilla">
               <asp:Button Text="Ver Ficha" runat="server" Cssclass=""  
                   CommandArgument='<%#Eval("Id")%>' CommandName="Id"/>
            </div>
               </div>
       </ItemTemplate>
   </asp:Repeater>
        </div>
                 </ContentTemplate>
        </asp:UpdatePanel>
                </div>

            <div class="row align-items-center justify-content-center fixed-bottom">
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
