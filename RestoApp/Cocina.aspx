<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Cocina.aspx.cs" Inherits="RestoApp.Cocina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <div class="row">

        <div class="col-9 d-flex" style="overflow-x: auto;">
            <asp:UpdatePanel runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:GridView ID="GVDCocina" runat="server" OnRowDataBound="GVDCocina_RowDataBound" AutoPostBack="true" CssClass="gridview-style">
                        <HeaderStyle HorizontalAlign="right" VerticalAlign="Middle" CssClass="small" />

                    </asp:GridView>
                </ContentTemplate>
                          <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
    </Triggers>
            </asp:UpdatePanel>
        </div>





        <div class="col-3 d-flex">
            <div style="background-color: gray; border-radius: 5px; height: 600px; width: 100%; display: flex; justify-content:flex-start; flex-direction: column;  padding: 10px;">
                  
                <div style="display: flex; flex-direction: column; justify-content: flex-start; align-items:center;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="Txtreloj" runat="server" Width="160px" Font-Size="XX-Large" CssClass="form-control" TextMode="Time"></asp:TextBox>
                            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000"></asp:Timer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
              
                     </div>
                <div style="display: flex; flex-direction: row; justify-content: center; align-items:center; margin-top:10px;">
                  
                    
                    <div style="margin-right: 5px;">

                        
                             <asp:Label Text="Pedidos Nuevos:" runat="server" Font-Bold="true" />
                    </div>
  
                    <div style="margin-right: 5px;"> 

                        <asp:TextBox runat="server" Width="25px" CssClass="form-control"></asp:TextBox>

                    </div>                            
                            

                        
      
                        <asp:LinkButton class="btn btn-dark"  runat="server" ><i class="fa-solid fa-check"></i>  </asp:LinkButton>
      
                    </div>




                            <div style="margin-top: 10px">
                                <asp:Label ID="LblProdenPrep" runat="server" Text="Productos en Preparacion" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </div>

                            <div style="margin-top: 10px; min-width: 220px; height: 180px; overflow-y: auto; background-color: white; border-radius: 5px; display: flex; align-items: flex-start;">

                                <%if ((bool)Session["sinproductos"])  { %> <div style="display:flex; justify-content:center; align-items:center;"><asp:Label runat="server" Text="Nada por aqui.."> </asp:Label> </div>  <%} %>
                       
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GVDProductosenprep" runat="server" OnRowDataBound="GVDProductosenprep_RowDataBound" BorderColor="Transparent">
                                <HeaderStyle HorizontalAlign="Center" />

                            </asp:GridView>
                         </div>
                                  </ContentTemplate>
                    </asp:UpdatePanel>

                    <div style="margin-top: 10px">
                        <asp:Label ID="LblEstadopedidos" runat="server" Text="Estado Pedidos" Font-Bold="true" Font-Size="Large"></asp:Label>
                    </div>
                    <div style="margin-top: 10px; min-width: 220px; height: 220px; overflow-y: auto; background-color: white; border-radius: 5px; display: flex; align-items: flex-start;">
                                        <asp:UpdatePanel runat="server">
                        <ContentTemplate>
           

                                <asp:GridView ID="GVDEstadopedidos" runat="server" OnRowCommand="GVDEstadopedidos_RowCommand" OnRowDataBound="GVDEstadopedidos_RowDataBound" BorderColor="Transparent" autoPostBack="true">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="InformarDemora" Height="80%" runat="server" class="btn btn-dark" CommandName="InformarDemora" CommandArgument='<%# Container.DataItemIndex %>' Visible="false" Enabled="false"><i class="fa-solid fa-clock-rotate-left fa-flip-horizontal"></i></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>

                                            <ItemTemplate>
                                                <asp:LinkButton ID="ListoparaEntregar" runat="server" class="btn btn-dark" CommandName="ListoparaEntregar" CommandArgument='<%# Container.DataItemIndex %>'><i class="fa-solid fa-check"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                               </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
 </div>

                </div>
   
    </div>
        </div>


 


</asp:Content>
