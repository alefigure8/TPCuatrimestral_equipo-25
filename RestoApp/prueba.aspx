﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="prueba.aspx.cs" Inherits="RestoApp.prueba" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
        <asp:Textbox ID="Txtreloj" runat="server" Width="200px"></asp:Textbox>
           <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000"></asp:Timer>
               


            </ContentTemplate>
            </asp:UpdatePanel>
   
  
            
    <div class="col d-flex"style="overflow-x:auto;" >
          <asp:UpdatePanel  runat="server">
        <ContentTemplate>
         <asp:Gridview ID="GVDCocina" runat="server" OnRowDataBound="GVDCocina_RowDataBound" AutoPostBack="true" OnDataBound="GVDCocina_DataBound" >

         </asp:Gridview>

            </ContentTemplate>
            </asp:UpdatePanel>
    </div>

   
</asp:Content>
