<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Cocina.aspx.cs" Inherits="RestoApp.Cocina" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row" style="height: 95vh">
            <!-- GRIDVIEW COCINA -->
        <div class="col-9 d-flex" style="overflow-x: auto;">
            <asp:UpdatePanel runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:GridView ID="GVDCocina" runat="server" OnRowDataBound="GVDCocina_RowDataBound" AutoPostBack="true" CssClass="gridview-style" ClientIDMode="AutoID">
                        <HeaderStyle HorizontalAlign="right" VerticalAlign="Middle" CssClass="small" BackColor="#343a40" ForeColor="Ivory" Font-Bold="false" Font-Size="Smaller" />
                            
                    </asp:GridView>
                </ContentTemplate>
              
            </asp:UpdatePanel>
        </div>

        <div class="col-3 d-flex">


            <div style="background-color: #343a40; border-radius: 5px; height: 95vh; width: 100%; display: flex; justify-content: flex-start; flex-direction: column; padding: 10px;">
                    <!-- TIMER -->
                <div style="display: flex; flex-direction: column; justify-content: flex-start; align-items: center;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="Txtreloj" runat="server" Width="160px" Font-Size="XX-Large" CssClass="form-control" TextMode="Time"></asp:TextBox>
                            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000"></asp:Timer>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>

       
                <div style="display: flex; flex-direction: row; justify-content: flex-start;align-items : center; margin-top: 10px; width:260px;padding-right:0px;">
                        <!-- FILA INGRESOS -->

                        <div style="margin-right: 5px;">
                        <asp:Label Text="Ingresos:" runat="server" Font-Size="Large" ForeColor="WhiteSmoke" />
                    </div>
                    
                    <div style="margin-right: 5px;">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="TxtPedidosIngresados" AutoPostBack="true" runat="server" Width="40px" BackColor="LightYellow" CssClass="form-control dark" Enabled="false" Font-Size="Large" Font-Bold="true"></asp:TextBox>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                                     
             
                    <div style="margin-right: 40px;">
                        <asp:LinkButton ID="BtnConfirmarIngresos" class="btn btn-dark" runat="server" OnClick="BtnConfirmarIngresos_Click"><i class="fa-solid fa-check"></i>  </asp:LinkButton>

                    </div>
                     <div style="margin-right: 5px;">
                        <asp:LinkButton ID="BtnDetalle" runat="server" class="btn btn-dark dropdown-toggle" data-bs-theme="dark" OnClick="BtnDetalle_Click"><i class="fa-solid fa-list"></i></asp:LinkButton>
                         </div>
                                        </div>
                        
      <!-- GVD PEDIDOS NUEVOS -->
                <asp:UpdatePanel runat ="server">
                            <ContentTemplate>
                                <div style="margin-top: 10px; width: 260px; overflow-y: auto; min-height: min-content; max-height:150px; background-color: white; border-radius: 5px; display: flex; align-items: flex-start; justify-content:center;">
                         <asp:GridView ID="DGVPedidosnuevos" runat="server" BorderColor="Transparent" OnRowDataBound="DGVPedidosnuevos_RowDataBound" AutoPostBack="false"></asp:GridView>
                                                      <HeaderStyle HorizontalAlign="Center" />
                                               </div>
                        </ContentTemplate>
                        </asp:UpdatePanel>
           
            
                        <!-- FILA PLATOS MARCHANDO -->
                 <div style="display:flex; flex-direction:row; margin-top: 10px; align-items:center; justify-content:space-between; width:260px;padding-right:0px; ">
                    
                     <asp:Label ID="LblProdenPrep" runat="server" Text="Platos Marchando:"  Font-Size="Larger" ForeColor="WhiteSmoke"></asp:Label>                  


                                   
                        <asp:LinkButton ID="BtnDisplaygvdPenP" runat="server" class="btn btn-dark dropdown-toggle" data-bs-theme="dark" OnClick="BtnDisplaygvdPenP_Click"><i class="fa-solid fa-list"></i></asp:LinkButton>
                                      
                    
                </div>
                             
                    <!-- GVD PLATOS MARCHANDO -->
                        <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div style="margin-top: 10px; width: 260px;  max-height:170px; overflow-y: auto; background-color: white; border-radius: 5px; display: flex; align-items: flex-start; justify-content:center;">
                            <asp:GridView ID="GVDProductosenprep" runat="server" OnRowDataBound="GVDProductosenprep_RowDataBound" BorderColor="Transparent">
                                <HeaderStyle HorizontalAlign="Center" />

                            </asp:GridView>
                              </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                      <!-- LABEL ESTADO PEDIDOS -->
                    <div style="margin-top: 10px">
                        <asp:Label ID="LblEstadopedidos" runat="server" Text="Estado Pedidos:" Font-Size="Larger" ForeColor="WhiteSmoke"></asp:Label>
                    </div>
                          
                    <!-- GVD ESTADOPEDIDOS -->
                <div style="margin-top: 10px; width: 260px; height:100%; overflow-y: auto; background-color: white; border-radius: 5px; display: flex; align-items: stretch; justify-content:stretch; ">    
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                   
                                <asp:GridView ID="GVDEstadopedidos" runat="server" OnRowCommand="GVDEstadopedidos_RowCommand" OnRowDataBound="GVDEstadopedidos_RowDataBound" BorderColor="Transparent" autoPostBack="true" Width="260px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                                    <asp:TemplateField >
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


    <script id="scriptcocina">


        window.addEventListener('load', function () {
            var myList = []; // Tu lista

            if (myList.length === 0) {
                // La lista está vacía, ejecutar el código aquí
                console.log('La lista está vacía. Ejecutando código...');
                // ... Tu código aquí
            }
        });


        function closeCard(button) {
            var cardBody = button.parentNode;
            var card = cardBody.parentNode;
            card.style.display = "none";
        }


        function mostrarModalDetallePedido(datosPedidos) {
            var modal = $('#modalDetallePedido');
            modal.empty(); // Limpiar el contenido del modal si ya había información anterior

            // Recorrer los datos de los pedidos
            $.each(datosPedidos, function (index, pedido) {
                // Crear elementos HTML para mostrar los detalles del pedido
                var pedidoElement = $('<div></div>').addClass('pedido');
                pedidoElement.append($('<h3></h3>').text('Pedido ID: ' + pedido.Id));
                pedidoElement.append($('<p></p>').text('Hora de Listo: ' + pedido.HoraListo));

                var productosElement = $('<ul></ul>').addClass('productos-solicitados');
                $.each(pedido.ProductosSolicitados, function (index, producto) {
                    // Crear elementos HTML para mostrar los detalles de cada producto solicitado
                    var productoElement = $('<li></li>').text(producto.Nombre + ' - Cantidad: ' + producto.Cantidad);
                    productosElement.append(productoElement);
                });

                pedidoElement.append(productosElement);
                modal.append(pedidoElement);
            });

            // Mostrar el modal
            modal.show();
        }



    </script>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

</asp:Content>
