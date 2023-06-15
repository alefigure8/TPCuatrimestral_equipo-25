<%@ Page Language="C#" MasterPageFile="~/Masters/Default.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="RestoApp.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    	public int Id { get; set; }
		public string Nombres { get; set; }
		public string Apellidos { get; set; }
		public string Mail { get; set; }
		public string Password { get; set; }
		public TipoUsuarios Tipo { get; set; }
	
    <div class="container">
        <div class="form-group">
  <div class="mb-3">
      <div class="row">
          <div class="col-6">
    <label for="Nombre" class="form-label">Nombres:</label>
    <input type="Text" class="form-control" id="Nombre" >
              </div>
          <div class="col-6">
     <label for="Nombre" class="form-label">Apellidos:</label>
    <input type="Text" class="form-control" id="Apellido">
              </div>
          </div>
  </div>
  <div class="mb-3">
    <label for="exampleInputPassword1" class="form-label">Password</label>
    <input type="password" class="form-control" id="exampleInputPassword1">
  </div>
  <div class="mb-3 form-check">
    <input type="checkbox" class="form-check-input" id="exampleCheck1">
    <label class="form-check-label" for="exampleCheck1">Check me out</label>
  </div>
  <button type="submit" class="btn btn-primary">Submit</button>

    </div>
        </div>
</asp:Content>

