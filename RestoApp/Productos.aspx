﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="RestoApp.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ContentProductos" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <h4 class="text-gray-100 small" style="text-align: right; margin-right: 1rem;">Hola, <%= usuario?.Nombres %> <%= usuario?.Apellidos %> (<%= usuario?.Tipo %>)</h4>

    <div class="row col-12 justify-content-between">


            <div class="col-10">

        <%--Nav filtros--%>
        <div class="row p-2 bg-dark text-white rounded-top align-middle">
            <div class="col-3 display-6"> Filtrar lista </div>
            <asp:TextBox CssClass="col m-1 form-control" placeholder="Ingrese nombre o descripción" ID="TxtBuscar" runat="server"></asp:TextBox>
            <asp:LinkButton runat="server" ID="BtnBuscar" CssClass="col-1 btn btn-dark m-1" OnClick="BtnBuscar_Click"> <i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
            <asp:LinkButton runat="server" ID="BtnLimpiarBusqueda" CssClass="col-1 btn btn-dark m-1" ><i class="fa fa-trash" aria-hidden="true"></i></asp:LinkButton>
        </div>
        <div class="row p-2 bg-white justify-content-around align-middle">

            <div class="col-2 ">
                <div class="row"> <asp:DropDownList ID="DDLEstado" runat="server" CssClass="row-2 btn btn-dark btn-sm"></asp:DropDownList> </div>
                <div class="row mt-2"> <asp:DropDownList ID="DDLCategorias" runat="server" CssClass="row-2 btn btn-dark btn-sm"></asp:DropDownList> </div>
            </div>

            <div class="col-3 justify-content-between">
                 <div class="row" style="text-align: center;">
                    <asp:Panel ID="PanelValor" runat="server">
                        <label> VALOR </label>
                        <asp:TextBox ID="tbPrecioMenor" TextMode="Number" min="0" runat="server" Text="Min"  CssClass="col-3"></asp:TextBox>
                        <label> - </label>
                        <asp:TextBox ID="tbPrecioMayor" TextMode="Number" min="0" runat="server" Text="Max"  CssClass="col-3"></asp:TextBox>
                    </asp:Panel>
                </div>
                <div class="row m-1">
                    <asp:DropDownList ID="DDLPrecios" runat="server" CssClass="btn btn-dark btn-sm"></asp:DropDownList>
                </div>
            </div>

            <div class="col-3 justify-content-between">
                <div class="row" style="text-align: center;">
                    <asp:Panel ID="PanelStock" runat="server">
                        <label> STOCK </label>
                        <asp:TextBox ID="tbStockMenor" TextMode="Number" min="0" runat="server" Text="Min" CssClass="col-3"></asp:TextBox>
                        <label> - </label>
                        <asp:TextBox ID="tbStockMayor" runat="server" TextMode="Number" min="0" Text="Max" CssClass="col-3"></asp:TextBox>
                    </asp:Panel>
                </div>
                <div class="row m-1">
                    <asp:DropDownList ID="DDLStock" runat="server" CssClass="btn btn-dark btn-sm"></asp:DropDownList>
                </div>
            </div>

            <div class="col-2" style="text-align: center;"> <asp:CheckBoxList ID="CheckBoxAtributos" runat="server" CssClass="small"></asp:CheckBoxList> </div>

            <div class="col-2">
                <div class="row"> <asp:Button runat="server" ID="BtnAplicarFiltros" CssClass="btn btn-dark btn-sm" Text="Aplicar filtros" OnClick="BtnAplicarFiltros_Click" /> </div>
                <div class="row mt-1"> <asp:Button ID="btnLimpiarFiltro" runat="server" CssClass="btn btn-dark btn-sm" Text="Limpiar filtros" OnClick="btnLimpiarFiltro_Click" /></div>
            </div>

        </div>
    <%--Fin nav filtros--%>


    <%--    Grid View Lista productos--%>
    <div class="row bg-white rounded-bottom"  style="max-height:500px; overflow-y:scroll;"> 

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
                        <asp:BoundField HeaderText="Vegano" DataField="AptoVegano" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Celiaco" DataField="AptoCeliaco" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Alcohol" DataField="Alcohol" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Stock" DataField="Stock" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Estado" DataField="Activo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Tiempo Cocción" DataField="TiempoCoccion" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />

                        <asp:TemplateField HeaderText="Stock" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button runat="server" OnClick="BtnAgregarStock_Click" ID="BtnAgregarStock" Text="+" ToolTip="Sumar Stock" CssClass="btn btn-dark" CommandArgument='<%#Eval("Id") %>' />
                                <asp:TextBox runat="server" TextMode="Number" min="0" ID="tbAgregarStock" CssClass="col-4"></asp:TextBox>
                                <asp:Button ID="BtnQuitarStock" runat="server" Text="-" ToolTip="Restar Producto" OnClick="BtnQuitarStock_Click" CommandArgument='<%#Eval("Id")%>' CssClass="btn btn-dark" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField  HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
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

            </div>

        <%--BOTONERA--%>
    <div class="col row bg-white rounded p-2 align-middle" style="margin-left: 5px; text-align: center;">

        <div class="col-12 btn btn-dark" data-bs-toggle="modal" data-bs-target="#modalProductos" title="Agregar Nuevo Producto">
           
                 <i class="row fa fa-cutlery align-items-center justify-content-center" aria-hidden="true" style="font-size: 5rem;"></i>
           <div> Nuevo producto </div>            
        </div>

        <div class="col-12 btn btn-dark mt-1" data-bs-toggle="modal" data-bs-target="#modalCategorias" title="Menú Categorías">
            <i class="row fa fa-file align-items-center justify-content-center" aria-hidden="true" style="font-size: 5rem;"></i>
            Categorias
        </div>



        <div class="col-12 btn btn-dark mt-1" title="Modificar Selección" data-bs-toggle="modal" data-bs-target="#modalEditarLote">
            <i class="row fa fa-file align-items-center justify-content-center" aria-hidden="true" style="font-size: 5rem;"></i>
            Modificar Lote
        </div>

        <div class="col-12 btn btn-dark mt-1" title="Eliminar Selección"  data-bs-toggle="modal" data-bs-target="#modalEliminarLote">
            <i class="row fa fa-trash  align-items-center justify-content-center" aria-hidden="true" style="font-size: 5rem;"></i>
              Eliminar Lote
        </div>

        </div>
        



    </div>


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
                                        <asp:TextBox CssClass="form-control" ID="NuevoProductoValor" TextMode="Number" min="0" runat="server" Style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0);"></asp:TextBox>
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
        <!-- Fin Modal Nuevo Producto -->


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



        <!-- Modal Categorias-->

        <div class="modal fade" id="modalCategorias" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="p-3 d-flex justify-content-between">
                        <h5 class="modal-title">Menu Categorías</h5>

                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>

                    </div>
                    <div class="p-2">
                        <div class="d-flex justify-content-center ">
                            <div class="col form-control bg-secondary">
                                <div class="row text-white h5">
                                    <span class="col-1 align-middle">ID</span>
                                    <span class="col align-middle">Categoría</span>
                                </div>


                                <asp:Repeater runat="server" ID="CategoriasRepetidor">
                                    <ItemTemplate>
                                        <div class="row bg-white p-1">

                                            <div class="col-6">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="tbCategoriaNombre" runat="server" AutoPostBack="true" CssClass="col align-middle bg-white border-0" Text='<%#Eval("Descripcion")%>'></asp:TextBox>
                                                    </ContentTemplate>

                                                </asp:UpdatePanel>
                                            </div>




                                            <span class="col bg-white">
                                                <asp:Button ID="btnModificarCategoria" CommandArgument='<%#Eval("Id") %>' Text="Guardar" runat="server" CssClass="col btn btn-dark pl-2" ToolTip="ModificarCategoria" OnClick="btnModificarCategoria_Click" />
                                                <asp:Button ID="btnActivarCategoria" CommandArgument='<%#Eval("Id") %>' Text="Eliminar" runat="server" CssClass="col btn btn-dark pl-2" ToolTip="Eliminar" OnClick="btnActivarCategoria_Click" />

                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <div class="row secondary text-white">
                                    <span class="row m-2" style="font-size: 12px">NUEVA CATEGORIA:</span>
                                    <div class="row d-flex justify-content-between">
                                        <div class="col">
                                            <asp:TextBox ID="tbNuevaCategoria" runat="server" CssClass="form-control" Text="Ingresar Descripción"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <asp:Button ID="btnGuardarCategoria" Text="Guardar" runat="server" CssClass="col btn btn-dark" ToolTip="Guardar Registro" OnClick="btnGuardarCategoria_Click" />
                                            <asp:Button ID="btnCancelarCategoria" Text="Cancelar" runat="server" CssClass="col btn btn-dark" ToolTip="Cancelar" OnClick="btnCancelarCategoria_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Fin Modal Categorias -->


        <%--Modal Editar lote--%>

        <div class="modal fade" id="modalEditarLote" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="p-3 d-flex justify-content-between">
                        <h5 class="modal-title">Editar lote filtrado</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div class="p-2">

                        <label style="color: red">Atención: todos los productos de la lista filtrada serán afectados.</label>

                        <div class="form-control bg-secondary p-2">
                            <div class="row m-2">

                                <div class="col">
                                    <div class="row">
                                        <label class="row text-white">Valor Por %</label>
                                        <asp:TextBox runat="server" ID="tbPorcentaje" CssClass="form col-6" type="number" min="0"></asp:TextBox>
                                        <asp:Button runat="server" ID="BtnAplicarPorcentaje" Text="+" CssClass="bg-dark text-white col-2" />
                                        <asp:Button runat="server" ID="Button1" Text="-" CssClass="bg-dark text-white col-2" />
                                    </div>
                                </div>


                                <div class="col">
                                    <div class="row">
                                        <label class="row text-white">Valor Por Monto</label>
                                        <asp:TextBox runat="server" ID="TextBox1" CssClass="form col" type="number" min="0"></asp:TextBox>
                                        <asp:Button runat="server" ID="Button2" Text="+" CssClass="bg-dark text-white col-2" />
                                        <asp:Button runat="server" ID="Button3" Text="-" CssClass="bg-dark text-white col-2" />
                                    </div>
                                </div>
                            </div>

                            <div class="row m-2">
                                   
                                        <label class="text-white">Stock</label>
                                        <asp:TextBox runat="server" ID="TextBox2" CssClass="form col" type="number" min="0"></asp:TextBox>
                                        <asp:Button runat="server" ID="Button4" Text="+" CssClass="bg-dark text-white col-1" />
                                        <asp:Button runat="server" ID="Button5" Text="-" CssClass="bg-dark text-white col-1" />
                                   
                                </div>



                            <div class="row bg-white rounded-bottom m-2">

                                <div class="col small">
                                    <div class="row bg-dark text-white">
                                        <div class="col-1">Sí</div>
                                        <div class="col-3"></div>
                                        <div class="col-1">No</div>
                                    </div>

                                    <div class="row  p-1">
                                        <asp:RadioButton runat="server" CssClass="col-1" />
                                        <div class="col-3">Apto Vegano</div>
                                        <asp:RadioButton runat="server" CssClass="col-1" />
                                        <asp:Button runat="server" Text="Aplicar" CssClass="col bg-dark text-white rounded" />
                                    </div>

                                    <div class="row p-1">
                                        <asp:RadioButton runat="server" CssClass="col-1" />
                                        <div class="col-3">Apto Celiaco</div>
                                        <asp:RadioButton runat="server" CssClass="col-1" />
                                        <asp:Button runat="server" Text="Aplicar" CssClass="col bg-dark text-white rounded" />
                                    </div>

                                     <div class="row  p-1">
                                        <asp:RadioButton runat="server" CssClass="col-1" />
                                        <div class="col-3">Cont. Alcohol</div>
                                        <asp:RadioButton runat="server" CssClass="col-1" />
                                        <asp:Button runat="server" Text="Aplicar" CssClass="col bg-dark text-white rounded" />
                                    </div>

                                    <div class="row bg-white m-2 p-2 ">
                                        <asp:Button runat="server" Text="Marcar todo en selección como Activo" CssClass="btn btn-dark"/>
                                <asp:Button runat="server" Text="Marcar todo en selección como Inactivo" CssClass="btn btn-dark mt-1"/>

 </div>
                                </div>

                            </div>

                                <asp:Button runat="server" Text="Cerrar" CssClass="btn btn-dark m-2"/>

                            
                            
                           
                        </div>


                    </div>

                </div>
            </div>
    
                        
                        </div>


        <%--Fin Modal Editar Lote--%>





       <%--Modal Eliminar Lote--%>
        
        <div class="modal fade" id="modalEliminarLote"">
            <div class="modal-dialog">
                <div class="modal-content p-3">
                    <div class="d-flex justify-content-between">
                        <h5 class="modal-title">Eliminar los productos filtrados?</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div class="row">
                        <button class="col btn btn-dark m-1"> Cancelar </button>
                        <asp:Button runat="server" Text="Eliminar Lote" CssClass="col btn btn-dark m-1" ID="BtnEliminarLote" OnClick="BtnEliminarLote_Click"/>
                    </div>
                         
                        
                    <label style="color:red;"> * Esta acción no podrá ser deshecha </label>
                        <label style="color:red;"> * Esta acción no se aplicará a los productos que estén en Menú actual</label>
                     
                </div>
            </div>
        </div>


       <%-- Fin Modal Eliminar Lote--%>








</asp:Content>
