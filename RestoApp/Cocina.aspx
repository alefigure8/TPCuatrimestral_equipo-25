<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Cocina.aspx.cs" Inherits="RestoApp.Cocina" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
  
            
    <div class="col d-flex"style="overflow-x:auto;" >
          <asp:UpdatePanel  runat="server">
        <ContentTemplate>
         <asp:Gridview ID="GVDCocina" runat="server" OnRowDataBound="GVDCocina_RowDataBound" AutoPostBack="true" OnDataBound="GVDCocina_DataBound" >

         </asp:Gridview>

            </ContentTemplate>
            </asp:UpdatePanel>
    </div>
</asp:Content>
