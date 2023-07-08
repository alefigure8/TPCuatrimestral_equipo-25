<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Cocina.aspx.cs" Inherits="RestoApp.Cocina" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
    
    
              
    <div class="row">

    <div class="col-9 d-flex"style="overflow-x:auto;" >
          <asp:UpdatePanel  runat="server">
        <ContentTemplate>
         <asp:Gridview ID="GVDCocina" runat="server" OnRowDataBound="GVDCocina_RowDataBound" AutoPostBack="true" CssClass="gridview-style">
                      <HeaderStyle HorizontalAlign="right" VerticalAlign="Middle" CssClass="small" />
                  
             </asp:Gridview>
            </ContentTemplate>
            </asp:UpdatePanel>
    </div>
            <div class ="col-3 d-flex">
     <div style="background-color:gray; border-radius:5px; height:600px; width:100%;padding:10px; display:flex; justify-content:center;">
       <asp:UpdatePanel runat="server">
            <ContentTemplate>
        <asp:Textbox ID="Txtreloj" runat="server" Width="150px" Font-Size="XX-Large" CssClass="form-control" TextMode="Time"></asp:Textbox>
         <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000"></asp:Timer>
                 <asp:GridView ID="GVDProductosenprep" runat="server">

                           <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="small" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="small" />
    
     </asp:GridView>
                 <asp:GridView ID="GVDEstadopedidos" runat="server" OnRowDataBound="GVDEstadopedidos_RowDataBound">

                           <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="small" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="small" />
           
             <Columns>
                              <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="InformarDemora" runat="server" class="btn btn-dark"  CommandName="InformarDemora" CommandArgument='<%# Container.DataItemIndex %>'><i class="fa-solid fa-clock-rotate-left fa-flip-horizontal"></i></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                 <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="ListoparaEntregar" runat="server" class="btn btn-dark"  CommandName="ListoparaEntregar" CommandArgument='<%# Container.DataItemIndex %>' ><i class="fa-solid fa-check"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
     </asp:GridView>


            </ContentTemplate>
            </asp:UpdatePanel>
   
         </div>
    </div>
        </div>


 


</asp:Content>
