<%@ Page Language="C#" MasterPageFile="~/Masters/Default.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="RestoApp.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="container">
    <div class="row" style="justify-content:center; margin-top:20px; ">
        <div class="col-6">
   <div class="input-group mb-3" style="box-shadow:0 2px 4px rgba(0, 0, 0, 0.3)">
  <asp:Textbox type="text" class="form-control" placeholder="Ingresa tu busqueda" id="TxtBusqueda" runat="server"></asp:Textbox>
  <asp:LinkButton class="btn btn-outline-primary" type="button" id="BtnBusqueda" runat="server"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
    </div>
            </div>
        </div>
      <asp:GridView ID="GDVEmpleados" runat="server">
            <HeaderStyle HorizontalAlign="Center" BackColor="#a5d5e0" cssClass="celda" />
             <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" cssClass="celda"/>
               <Columns>
                   <asp:BoundField HeaderText="Precio/Un." DataFormatString="{0:C}" DataField="Articulo.Precio" />
                   </Columns>
      </asp:GridView>




      </div>
</asp:Content>

