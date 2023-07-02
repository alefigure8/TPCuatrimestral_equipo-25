<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="RestoApp.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ContentProductos" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <h4 class="text-gray-100 small">Hola, <%= usuario?.Nombres %> <%= usuario?.Apellidos %> (<%= usuario?.Tipo %>)</h4>

    <div class="row bg-white rounded m-2 p-4 small">


        <!-- Modal Nuevo Producto-->
        <div class="modal fade" id="modalProductos" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="p-4 d-flex justify-content-between">
                        <h5 class="modal-title" id="exampleModalLabel">Agregar nuevo producto</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="p-2">
                        <div class="d-flex justify-content-center ">
                            <div class="col form-control">

                                <div class="row p-1">
                                    <div class="col-2">
                                        <label class="form-label">Nombre: </label>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox CssClass="form-control" runat="server" Text="" ID="NuevoProductoNombre" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row p-1">
                                    <div class="col-3">
                                        <label class="form-label">Descripcion: </label>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="NuevoProductoDescripcion" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="row p-1">
                                    <div class="col-2">
                                        <label class="form-label">Valor: </label>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox CssClass="form-control" type="number" ID="NuevoProductoValor" runat="server" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row p-1">
                                    <asp:DropDownList ID="modalDDLCategorias" runat="server" CssClass="col btn btn-dark btn-sm m-1" DataMember="modalDDLCategorias"></asp:DropDownList>
                                    <asp:DropDownList ID="modalDDLEstado" runat="server" CssClass="col btn btn-dark btn-sm m-1" DataMember="modalDDLEstado"></asp:DropDownList>
                                </div>



                                <asp:CheckBoxList ID="modalCheckBoxAtributos" runat="server"></asp:CheckBoxList>

                                <div class="row p-1">
                                    <div class="col-3">
                                        <label class="form-label">Tiempo Cocción: </label>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox runat="server" ID="NuevoProductoTiempoCoccion" CssClass="form-control" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row p-1">
                                    <div class="col-2">
                                        <label class="form-label">Stock: </label>
                                    </div>
                                    <div class="col">
                                        <asp:TextBox ID="NuevoProductoStock" runat="server" CssClass="form-control" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                    </div>
                                </div>



                            </div>

                        </div>

                    </div>
                    <div class="d-flex justify-content-center">
                        <asp:Button Text="Cancelar" runat="server" CssClass="btn btn-dark btn-lg m-1" data-bs-toggle="modal" data-bs-target="#modalProductos" />
                        <asp:Button ID="GuardarNuevoProducto" Text="Guardar" runat="server" OnClick="GuardarNuevoProducto_Click" CssClass="btn btn-dark btn-lg m-1" data-bs-toggle="modal" data-bs-target="#modalProductos" OnClientClick="return ValidarCampos();" ClientIDMode="Static" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal -->


        <!-- Modal Modificar Producto-->

        <asp:UpdatePanel runat="server" ID="UPModificarProducto" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal fade" id="modalModificarProductos" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="p-4 d-flex justify-content-between">
                                <h5 class="modal-title">Modificar Producto</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="p-2">
                                <div class="d-flex justify-content-center ">
                                    <div class="col form-control">

                                        <div class="row p-1">
                                            <div class="col-2">
                                                <asp:Label runat="server" class="form-label" ID="MPlblNombre">Nombre: </asp:Label>
                                            </div>
                                            <div class="col">
                                                <asp:TextBox CssClass="form-control" runat="server" ID="MPnombre1" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row p-1">
                                            <div class="col-3">
                                                <label class="form-label">Descripcion: </label>
                                            </div>
                                            <div class="col">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="MPDescripcion" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                            </div>
                                        </div>


                                        <div class="row p-1">
                                            <div class="col-2">
                                                <label class="form-label">Valor: </label>
                                            </div>
                                            <div class="col">
                                                <asp:TextBox CssClass="form-control" type="number" ID="MPvalor" runat="server" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row p-1">
                                            <asp:DropDownList ID="MPDDLCategoria" runat="server" CssClass="col btn btn-dark btn-sm m-1" DataMember="modalDDLCategorias"></asp:DropDownList>
                                            <asp:DropDownList ID="MPDDLEstado" runat="server" CssClass="col btn btn-dark btn-sm m-1" DataMember="modalDDLEstado"></asp:DropDownList>
                                        </div>



                                        <asp:CheckBoxList ID="MPCheckBoxAtributos" runat="server"></asp:CheckBoxList>

                                        <div class="row p-1">
                                            <div class="col-3">
                                                <label class="form-label">Tiempo Cocción: </label>
                                            </div>
                                            <div class="col">
                                                <asp:TextBox runat="server" ID="MPtiempococcion" CssClass="form-control" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row p-1">
                                            <div class="col-2">
                                                <label class="form-label">Stock: </label>
                                            </div>
                                            <div class="col">
                                                <asp:TextBox ID="MPStock" runat="server" CssClass="form-control" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
                                            </div>
                                        </div>



                                    </div>

                                </div>

                            </div>
                            <div class="d-flex justify-content-center">
                                <asp:Button Text="Cancelar" runat="server" CssClass="btn btn-dark btn-lg m-1" data-bs-toggle="modal" data-bs-target="#exampleModal" />
                                <asp:Button ID="MPBtnModificarProducto" Text="Guardar" runat="server" OnClick="MPBtnModificarProducto_Click" UseSubmitBehavior="false" CssClass="btn btn-dark btn-lg m-1" data-bs-toggle="modal" data-bs-target="#exampleModal" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>

        </asp:UpdatePanel>
        <!-- Fin Modal Modificar Producto -->

        <%--Nav filtros--%>
        <div class="row p-2">
            <div class="col-2">
                <p class="h2">Filtrar lista</p>
            </div>
            <asp:TextBox CssClass="col m-1 form-control" placeholder="Ingrese nombre o descripción" ID="TxtBuscar" runat="server"></asp:TextBox>
            <asp:LinkButton runat="server" ID="BtnBuscar" CssClass="col-2 btn btn-dark m-1" OnClick="BtnBuscar_Click"> <i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
        </div>

        <br />
        <br />

        <div class="row">

            <div class="col-2 ">

                <div class="row">
                    <asp:DropDownList ID="DDLEstado" runat="server" CssClass="row-2 btn btn-dark btn-sm"></asp:DropDownList>
                </div>

                <div class="row mt-2">
                    <asp:DropDownList ID="DDLCategorias" runat="server" CssClass="row-2 btn btn-dark btn-sm"></asp:DropDownList>
                </div>

            </div>



            <div class="col-2">
                <div class="row-2 ">
                    <asp:Panel ID="PanelValor" runat="server">
                        <label class="small">VALOR</label>
                        <asp:TextBox ID="tbPrecioMenor" TextMode="Number" min="0" runat="server" Text="Min" CssClass="col-md-3 pr-4"></asp:TextBox>
                        <label>- </label>
                        <asp:TextBox ID="tbPrecioMayor" TextMode="Number" min="0" runat="server" Text="Max" CssClass="col-md-3 pr-4"></asp:TextBox>
                    </asp:Panel>
                </div>
                <div class="row-2 mt-2">
                    <asp:DropDownList ID="DDLPrecios" runat="server" CssClass="btn btn-dark btn-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="col-2">
                <div class="row-2 ">
                    <asp:Panel ID="PanelStock" runat="server">
                        <label class="small">STOCK</label>
                        <asp:TextBox ID="tbStockMenor" TextMode="Number" min="0" runat="server" Text="Min" CssClass="col-md-3 pr-1"></asp:TextBox>
                        <label>- </label>
                        <asp:TextBox ID="tbStockMayor" runat="server" TextMode="Number" min="0" Text="Max" CssClass="col-md-3 pr-1"></asp:TextBox>
                    </asp:Panel>
                </div>
                <div class="row-2 mt-2">
                    <asp:DropDownList ID="DDLStock" runat="server" CssClass="btn btn-dark btn-sm"></asp:DropDownList>
                </div>
            </div>

            <div class="col-2">

                <asp:CheckBoxList ID="CheckBoxAtributos" runat="server"></asp:CheckBoxList>

            </div>

            <div class="col-2 p">
                <div class="row">
                    <asp:Button runat="server" ID="BtnAplicarFiltros" CssClass="btn btn-dark" Text="Aplicar filtros" OnClick="BtnAplicarFiltros_Click" />
                </div>
                <div class="row pt-1">
                    <asp:Button ID="btnLimpiarFiltro" runat="server" CssClass="btn btn-dark" Text="Limpiar filtros" OnClick="btnLimpiarFiltro_Click" />
                </div>
            </div>


        </div>

    </div>

    <%--Fin nav filtros--%>


    <%--    Grid View Lista productos--%>
    <div class="row bg-white rounded m-2 p-4">

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:GridView ID="GVProductos" runat="server" AutoGenerateColumns="false"
                    OnRowDataBound="GVProductos_RowDataBound"
                    CssClass="table small">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="small" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="small" />
                    <Columns>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" HeaderStyle-CssClass="columna-grilla" />
                        <asp:BoundField HeaderText="Categoria" DataField="Categoria" HeaderStyle-CssClass="columna-grilla" />
                        <asp:BoundField HeaderText="Valor" DataFormatString="{0:C}" DataField="Valor" HeaderStyle-CssClass="columna-grilla" />
                        <asp:BoundField HeaderText="Vegano" DataField="AptoVegano" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField HeaderText="Celiaco"  DataField="AptoCeliaco" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Alcohol" DataField="Alcohol" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField HeaderText="Stock" DataField="Stock" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Estado"  DataField="Activo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField HeaderText="Tiempo Cocción" DataField="TiempoCoccion" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>

                    

                        <asp:TemplateField HeaderText="Stock" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate >
                                <asp:Button runat="server" OnClick="BtnAgregarStock_Click" ID="BtnAgregarStock" Text="+" ToolTip="Sumar Stock" CssClass="btn btn-dark" CommandArgument='<%#Eval("Id") %>'/>
                                <asp:TextBox runat="server" TextMode="Number" min="0" ID="tbAgregarStock" CssClass="col-4"></asp:TextBox>
                                <asp:Button ID="BtnQuitarStock" runat="server" Text="-" ToolTip="Restar Producto" OnClick="BtnQuitarStock_Click" CommandArgument='<%#Eval("Id")%>' CssClass="btn btn-dark" />
                             </ItemTemplate>
                        </asp:TemplateField>

                            <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnModificarProducto" runat="server" Text="🖍" ToolTip="Modificar Producto" OnClick="btnModificarProducto_Click" CommandArgument='<%#Eval("Id")%>' data-bs-toggle="modal" data-bs-target="#modalModificarProductos" CssClass="btn btn-dark" />
                                <asp:Button runat="server" Text="🗑" OnClick="EliminarProducto" ToolTip="Eliminar Producto Permanentemente" CommandArgument='<%#Eval("Id")%>' CssClass="btn btn-dark" />
                            </ItemTemplate>
                        </asp:TemplateField>



                    </Columns>

                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>


    </div>


    <%--BOTONERA--%>
    <div class="row bg-white rounded p-2  m-2 rounded justify-content-center">

        <div class="col-2 me-2 btn btn-dark" data-bs-toggle="modal" data-bs-target="#modalProductos" title="Agregar Nuevo Producto">
            <i class="row fa fa-cutlery align-items-center justify-content-center" style="font-size: 3rem;" aria-hidden="true"></i>
            <p class="h5">Nuevo producto</p>
        </div>


        <div class="col-2 me-2 btn btn-dark" title="Menú Categorías">
            <i class="row fa fa-file align-items-center justify-content-center" aria-hidden="true" style="font-size: 3rem;"></i>

            <asp:LinkButton ID="LBtnCategorías" runat="server"
                CssClass="h5">
                        <label>Categorías</label>
            </asp:LinkButton>
        </div>

        <div class="col-2 me-2 btn btn-dark" title="Modificar Selección">
            <i class="row fa fa-file align-items-center justify-content-center" aria-hidden="true" style="font-size: 3rem;"></i>
            <asp:LinkButton ID="LinkButton1" runat="server"
                CssClass="h5">
                        <label>Lote</label>
            </asp:LinkButton>
        </div>

        <div class="col-2 me-2 btn btn-dark" title="Eliminar Selección">
            <i class="row fa fa-trash  align-items-center justify-content-center" aria-hidden="true" style="font-size: 3rem;"></i>
            <asp:LinkButton ID="LinkButton2" runat="server"
                CssClass="h5">
                        <label>Lote</label>
            </asp:LinkButton>
        </div>


    </div>





</asp:Content>
