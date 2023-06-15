<%@ Page Language="C#" MasterPageFile="~/Masters/Default.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="RestoApp.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="container">

    <div class="row" style="justify-content:center; margin-top:20px;">
        <div class="col-6">
   <div class="input-group mb-3" style="box-shadow:0 2px 4px rgba(0, 0, 0, 0.3)">
  <asp:Textbox type="text" class="form-control" placeholder="Ingresa tu busqueda" id="TxtBusqueda" runat="server"></asp:Textbox>
  <asp:LinkButton class="btn btn-outline-primary" type="button" id="BtnBusqueda" runat="server"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
    </div>
            </div>
        </div>
      <div class="row" style="margin-top:20px; justify-content:center;">
          <div class="col-6">
      <asp:GridView ID="GDVEmpleados" runat="server" style="border-radius:5px; height:150%;">
            <HeaderStyle HorizontalAlign="Center" BackColor="#0066ff" cssClass="celda" />
             <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" cssClass="celda"/>
            
      </asp:GridView>
              </div>
          <div class="col-3">
              <div class="row">
                  <asp:LinkButton ID="BtnAgregar" runat="server" class="btn btn-primary" style="margin-left:20px; margin-bottom:10px;">Agregar</asp:LinkButton>
              </div>
                   <div class="row">
                  <asp:LinkButton ID="BtnModificar" runat="server" class="btn btn-primary" style="margin-left:20px; margin-bottom:10px;">Modificar</asp:LinkButton>
              </div>
               <div class="row">
                  <asp:LinkButton ID="BtnEliminar" runat="server" class="btn btn-primary" style="margin-left:20px; margin-bottom:10px;" >Eliminar</asp:LinkButton>
              </div>
               <div class="row">
                  <asp:LinkButton ID="BtnVer" runat="server" class="btn btn-primary" style="margin-left:20px; margin-bottom:10px;" >Ver</asp:LinkButton>
              </div>

          </div>
          </div>
      
      </div>
</asp:Content>

