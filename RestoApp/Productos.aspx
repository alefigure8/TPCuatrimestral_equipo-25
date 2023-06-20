<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="RestoApp.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ContentProductos" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <h4 class="text-gray-100 small">Hola, <%= usuario?.Nombres %> <%= usuario?.Apellidos %> (<%= usuario?.Tipo %>)</h4>

    <div class="row bg-white rounded m-2 p-4 small">

        <div class="row p-2">
            <div class="col-2">
                <p class="h2">Filtrar lista</p>
            </div>
            <asp:TextBox CssClass="col m-1 form-control" placeholder="Buscar" ID="TxtBuscar" runat="server"></asp:TextBox>
            <asp:LinkButton runat="server" CssClass="col-2 btn btn-dark m-1"> <i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
        </div>

        <br />
        <br />

        <div class="row">

            <div class="col-2 ">

                <div class="row">
                    <asp:DropDownList ID="DDLEstado" runat="server" CssClass="row-2 btn btn-dark btn-sm" OnSelectedIndexChanged="DDLEstado_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>

                <div class="row mt-2">
                    <asp:DropDownList ID="DDLCategorias" runat="server" CssClass="row-2 btn btn-dark btn-sm"></asp:DropDownList>
                </div>

            </div>



            <div class="col-2">
                <div class="row-2 ">
                    <asp:Panel ID="PanelValor" runat="server">
                        <label>VALOR</label>
                        <asp:TextBox ID="tbPrecioMenor" runat="server" CssClass="col-md-3"></asp:TextBox>
                        <label>- </label>
                        <asp:TextBox ID="tbPrecioMayor" runat="server" CssClass="col-md-3"></asp:TextBox>
                    </asp:Panel>
                </div>
                <div class="row-2 mt-2">
                    <asp:DropDownList ID="DDLPrecios" runat="server" CssClass="btn btn-dark btn-sm"></asp:DropDownList>
                </div>
            </div>
            <div class="col-2">
                <div class="row-2 ">
                    <asp:Panel ID="PanelStock" runat="server">
                        <label>STOCK</label>
                        <asp:TextBox ID="tbStockMenor" runat="server" CssClass="col-md-2"></asp:TextBox>
                        <label>- </label>
                        <asp:TextBox ID="tbStockMayor" runat="server" CssClass="col-md-2"></asp:TextBox>
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
                    <asp:Button runat="server" CssClass="btn btn-dark" Text="Aplicar filtros" />
                </div>
                <div class="row pt-1">
                    <asp:Button ID="btnLimpiarFiltro" runat="server" CssClass="btn btn-dark" Text="Limpiar filtros" OnClick="btnLimpiarFiltro_Click" />
                </div>
            </div>


        </div>

    </div>

    <div class="row bg-white rounded m-2 p-4">
 
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
            <asp:GridView ID="GVProductos" runat="server" AutoGenerateColumns="false"
                OnDataBound="GVProductos_DataBound"
                OnRowDataBound="GVProductos_RowDataBound"
                CssClass="table small">
                <headerstyle horizontalalign="Left" verticalalign="Middle" cssclass="small" />
                <rowstyle horizontalalign="Left" verticalalign="Middle" cssclass="small" />
                <columns>
                    <asp:BoundField HeaderText="Nombre" DataField="Nombre" HeaderStyle-CssClass="columna-grilla" ItemStyle-CssClass="columna-grilla" />
                    <asp:BoundField HeaderText="Categoria" DataField="Categoria" HeaderStyle-CssClass="columna-grilla" ItemStyle-CssClass="columna-grilla" />
                    <asp:BoundField HeaderText="Valor" DataFormatString="{0:C}" DataField="Valor" HeaderStyle-CssClass="columna-grilla-sm" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="30px" />
                    <asp:BoundField HeaderText="Vegano" ItemStyle-CssClass="columna-grilla-sm" HeaderStyle-CssClass="columna-grilla-sm" DataField="AptoVegano" />
                    <asp:BoundField HeaderText="Celiaco" ItemStyle-CssClass="columna-grilla-sm" HeaderStyle-CssClass="columna-grilla-sm" DataField="AptoCeliaco" />
                    <asp:BoundField HeaderText="Alcohol" ItemStyle-CssClass="columna-grilla-sm" HeaderStyle-CssClass="columna-grilla-sm" DataField="Alcohol" />
                    <asp:BoundField HeaderText="Stock" ItemStyle-CssClass="columna-grilla-sm" HeaderStyle-CssClass="columna-grilla-sm" DataField="Stock" />
                    <asp:BoundField HeaderText="Estado" ItemStyle-CssClass="columna-grilla-sm" HeaderStyle-CssClass="columna-grilla-sm" DataField="Activo" />
                    <asp:BoundField HeaderText="Tiempo Cocción" DataField="TiempoCoccion" HeaderStyle-CssClass="columna-grilla-sm" ItemStyle-CssClass="columna-grilla-sm" />
                    <asp:ButtonField runat="server" ControlStyle-CssClass="btn btn-dark" Text="🖍" ItemStyle-CssClass="columna-grilla-btn" HeaderStyle-CssClass="columna-grilla-btn" />
                    <asp:ButtonField runat="server" ControlStyle-CssClass="btn btn-dark" Text="🗑" ItemStyle-CssClass="columna-grilla-btn" HeaderStyle-CssClass="columna-grilla-btn" />

                </columns>

            </asp:GridView>
                </ContentTemplate>
        </asp:UpdatePanel>


    </div>


    <%--BOTONERA--%>
    <div class="row bg-white rounded p-2  m-2 rounded justify-content-center">

        <div class="col-2 me-2 btn btn-dark">
            <i class="row fa fa-cutlery align-items-center justify-content-center" style="font-size: 3rem;" aria-hidden="true"></i>

            <asp:LinkButton ID="LBtnNuevoPlato" runat="server"
                CssClass="h5">
                        <label>Nuevo Plato</label>
            </asp:LinkButton>

        </div>

        <div class="col-2 me-2 btn btn-dark">
            <i class="row fa fa-wine-glass align-items-center justify-content-center" style="font-size: 3rem;" aria-hidden="true"></i>

            <asp:LinkButton ID="LBtnNuevaBebida" runat="server"
                CssClass="h5">
                       <label>Nueva Bebida</label>
            </asp:LinkButton>
        </div>

        <div class="col-2 me-2 btn btn-dark">
            <i class="row fa fa-file align-items-center justify-content-center" aria-hidden="true" style="font-size: 3rem;"></i>

            <asp:LinkButton ID="LBtnCategorías" runat="server"
                CssClass="h5">
                        <label>Categorías</label>
            </asp:LinkButton>
        </div>

        <div class="col-2 me-2 btn btn-dark">
            <i class="row fa fa-file align-items-center justify-content-center" aria-hidden="true" style="font-size: 3rem;"></i>
            <asp:LinkButton ID="LinkButton1" runat="server"
                CssClass="h5">
                        <label>Lote</label>
            </asp:LinkButton>
        </div>

        <div class="col-2 me-2 btn btn-dark">
            <i class="row fa fa-trash  align-items-center justify-content-center" aria-hidden="true" style="font-size: 3rem;"></i>
            <asp:LinkButton ID="LinkButton2" runat="server"
                CssClass="h5">
                        <label>Lote</label>
            </asp:LinkButton>
        </div>


    </div>






</asp:Content>
