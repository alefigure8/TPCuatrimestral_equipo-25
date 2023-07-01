<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="RestoApp.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      

    <div class="container" >
    <div class="row d-flex" style="justify-content:flex-start; margin-top:40px; margin-left:40px";>
            
        <div class="col-6 text-white bg-dark" style="width:400px; border-radius:5px; padding:10px;padding-left:20px;padding-right:20px;">
                            
            <div class="row d-flex" style="justify-content:center;">
                    
                <h1>Mi Perfil</h1>          
                                   
             <div style="background-color:darkgrey; border-radius:5px; width:170px; height:170px; padding:10px;">
                <asp:Image runat="server" ImageUrl="https://www.nicepng.com/png/full/128-1280406_view-user-icon-png-user-circle-icon-png.png" Height="150px" Width="150px"/>                               

           </div>         
           
               </div>
        <div class="row">
                <asp:Label ID="LblID" runat="server" Text="Id"></asp:Label>
                <asp:TextBox ID="TxtId" runat="server" CssClass="form-control"></asp:TextBox>
        </div> 
        <div class="row">
            
                <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
       
        <div class="row">
            
                <asp:Label ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                <asp:TextBox ID="TxtApellido" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
     
        <div class="row">
      
                <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
          
            <asp:UpdatePanel ID="UpdatePanelPassword" runat="server">
               <ContentTemplate>
            <div class="row" >
             
           <asp:Label ID="LblPassword" runat="server" Text="Password"></asp:Label>
          
           <div class="col-9" style="padding:0px; margin-right:0px;">
             
                     <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control" ></asp:TextBox>
         
                      </div>
         
            <div class="col-3" >
                <div class="row d-flex" style="justify-content:space-around; padding:0px">
                <div class="col-6">
                         
                   <asp:LinkButton ID="btnVerPassword" runat="server" BorderColor="#797c81"  style="margin-left: -5px;" BorderWidth="1px" OnClick="btnVerPassword_Click" class="btn btn-outline-dark"><i class="fa-solid fa-eye" style="color:  #797c81;"></i></asp:LinkButton>
                      
                     </div>
                <div class="col-6">

                   <asp:LinkButton ID="BtnModificarPass" runat="server" BorderColor="#797c81"  style="margin-left: -5px;" OnClick="BtnModificarPass_Click" class="btn btn-dark">
                       <% if ((bool)Session["Modificarpass"] == false) { %><i class="fa-solid fa-pen" style="color: #797c81;"></i><% } else { %> <i class="fa-solid fa-check" style="color: #797c81;"></i> <%} %> </asp:LinkButton>
                        </div>
                   </div>

            </div>
            </div>
                            </ContentTemplate>
               </asp:UpdatePanel>
            <div class="row">
                <asp:Label ID="LblAvisopass" runat="server" Visible="false"></asp:Label>

            </div>
          </div>
    </div>
    </div>

</asp:Content>
