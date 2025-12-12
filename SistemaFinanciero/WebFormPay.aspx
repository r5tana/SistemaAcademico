<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormPay.aspx.cs" Inherits="SistemaFinanciero.WebFormPay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function deshabilitarbtn() {
            var btn = "<%= btnAbrirModalEstudiante.ClientID %>";
            document.getElementById(btn).disabled = true;
            document.getElementById(btn).value = "Cargando...";
        }

        function showModalConfirm() {
            $('#ModalConfirm').modal('show');
        }

        function closeModalConfirm() {
            $('#ModalConfirm').modal('hide');
        }

        function showModal() {
            debugger;
            $('#ModalStudent').modal('show');

            $('#ModalStudent').on('shown.bs.modal', function () {
                $('#ContentPlaceHolder1_GridEstudiantes').DataTable().columns.adjust();
            });
        }

        function closeModal() {
            $('#ModalStudent').modal('hide');
            //$('.modal-backdrop').hide();
        };

        function RemoveBackDrop() {
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open');
        }
    </script>



    <style type="text/css">
        .modal-body-scroll {
            max-height: 70vh;
            overflow-y: auto;
        }

        .ColumnaOculta {
            display: none;
        }

        .mayusculas {
            text-transform: uppercase;
        }
    </style>

    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {

                var idioma = {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ning�n dato disponible en esta tabla",
                    "sInfo": "Mostrando _START_ al _END_ de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando _START_ al _END_ de _TOTAL_ registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Filtrar por:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "�ltimo",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    },
                    buttons: {
                        copyTitle: 'Los datos fueron copiados',
                        copyInfo: {
                            _: 'Copiados %d filas al portapapeles',
                            1: 'Copiado 1 fila al portapapeles',
                        }
                    }
                }


                $('#ContentPlaceHolder1_GridEstudiantes').DataTable({
                    "scrollX": true,
                    "language": idioma,
                    dom: 'Bfrtip',
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true,
                    buttons: [

                    ]
                });

                $('#ContentPlaceHolder1_GridProductos').DataTable({
                    "scrollX": true,
                    "language": idioma,
                    dom: 'Bfrtip',
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true,
                    buttons: [

                    ]
                });


            });
        }

    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-lg-12 ">
                    <div style="color: #007bff; text-align: center; font-size: large; font-weight: bold" class="alert alert-secondary">
                        Gestión de pagos
                    </div>
                </div>
            </div>

            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Recibo</h6>
                </div>
                <div class="card-body">
                    <div align="center">

                        <div class="row">
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Número:
 
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtRecibo" CssClass="form-control mayusculas" PlaceHolder="" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                A nombre:
                            </div>
                            <div class="col-lg-5 col-md-5 col-xs-5">
                                <asp:TextBox ID="txtNombreFactura" CssClass="form-control mayusculas" PlaceHolder="Nombres y Apellidos" runat="server" Enabled="false"></asp:TextBox>
                            </div>

                            <div class="col-lg-1 col-md-1 col-xs-1">
                                <asp:LinkButton ID="btnAbrirModalEstudiante" runat="server" Text=""
                                    CssClass="btn btn-primary" OnClick="btnAbrirModalEstudiante_Click"
                                    UseSubmitBehavior="false" OnClientClick="deshabilitarbtn()">
                                    <i class="glyphicon glyphicon-search"></i>
                                </asp:LinkButton>
                            </div>

                        </div>
                        <br />

                        <div class="row">
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Fecha:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtFecha" CssClass="form-control mayusculas" PlaceHolder="Fecha" type="date" ReadOnly="true" runat="server"></asp:TextBox>

                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                Forma de Pago:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="EFECTIVO">EFECTIVO</asp:ListItem>
                                    <asp:ListItem Value="CHEQUE">CHEQUE</asp:ListItem>
                                    <asp:ListItem Value="TRANSFERENCIA">TARJETA</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Estado:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control" Enabled="false">
                                    <asp:ListItem Value="CANCELADO">CANCELADO</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                        <br />

                        <div class="row">
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Caja:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtCaja" CssClass="form-control mayusculas" PlaceHolder="Caja" runat="server" ReadOnly="true"></asp:TextBox>

                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                Estado por:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtUsuarioProcesa" CssClass="form-control mayusculas" PlaceHolder="Estado por" runat="server" ReadOnly="true"></asp:TextBox>

                            </div>
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                T/C:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtTipoCambio" CssClass="form-control mayusculas" PlaceHolder="T/C" runat="server" TextMode="Number" step="0.01" ReadOnly="true"></asp:TextBox>

                            </div>

                        </div>
                        <br />

                        <div class="row">


                            <div class="col-lg-1 col-md-1 col-xs-1">
                            </div>

                        </div>
                        <br />

                        <div class="row">
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Total C$:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtTotalCordoba" CssClass="form-control mayusculas" PlaceHolder="Total C$" TextMode="Number" step="0.01" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                Total $:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtTotalDolar" CssClass="form-control mayusculas" PlaceHolder="Total $" TextMode="Number" step="0.01" runat="server" ReadOnly="true"></asp:TextBox>

                            </div>

                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Ingreso:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtIngreso" CssClass="form-control mayusculas" PlaceHolder="Ingreso" TextMode="Number" step="0.01" runat="server" Enabled="false" AutoPostBack="true" OnTextChanged="txtIngreso_TextChanged"></asp:TextBox>
                            </div>

                        </div>
                        <br />

                        <div class="row">
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Cambio:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtCambio" CssClass="form-control mayusculas" PlaceHolder="Cambio" runat="server" TextMode="Number" step="0.01" ReadOnly="true"></asp:TextBox>

                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtCodEstudiante" CssClass="form-control mayusculas" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtIdCaja" CssClass="form-control mayusculas" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtSerie" CssClass="form-control mayusculas" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>

                            </div>


                        </div>
                    </div>

                </div>
            </div>

            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Detalle</h6>
                </div>
                <div class="card-body">
                    <div align="center">

                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-xs-3">
                                <asp:RadioButton ID="rbtProducto" runat="server" GroupName="Opciones" AutoPostBack="true"
                                    OnCheckedChanged="RbtTodos_CheckedChanged" />
                                <strong>Producto</strong>
                            </div>
                            <div class="col-lg-3 col-md-3 col-xs-3">
                                <asp:RadioButton ID="rbtConcepto" runat="server" GroupName="Opciones" AutoPostBack="true"
                                    OnCheckedChanged="RbtTodos_CheckedChanged" />
                                <strong>Concepto</strong>
                            </div>
                            <div class="col-lg-3 col-md-3 col-xs-3">
                                <asp:RadioButton ID="rbtTransacciones" runat="server" GroupName="Opciones" AutoPostBack="true"
                                    OnCheckedChanged="RbtTodos_CheckedChanged" />
                                <strong>Transacciones</strong>
                            </div>
                            <div class="col-lg-3 col-md-3 col-xs-3">
                                <asp:RadioButton ID="rbtOtros" runat="server" GroupName="Opciones" AutoPostBack="true"
                                    OnCheckedChanged="RbtTodos_CheckedChanged" />
                                <strong>Otros/Varios</strong>
                            </div>
                        </div>

                        <br />
                        <div class="row">
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Nombre: 
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-4">
                                <asp:DropDownList ID="ddlNombreTransaccion" runat="server" CssClass="form-control" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlNombreTransaccion_SelectedIndexChanged">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="TxtOtraTransaccion" CssClass="form-control mayusculas" PlaceHolder="Otra transacción" runat="server" Visible="false"></asp:TextBox>

                            </div>
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Valor:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtPrecio" CssClass="form-control mayusculas" PlaceHolder="Valor" TextMode="Number" step="0.01" runat="server" Enabled="false"></asp:TextBox>
                                <%--  <asp:RegularExpressionValidator ID="revMonto" runat="server" ControlToValidate="txtPrecio" ForeColor="Red"
                                    ErrorMessage="Ingrese un número válido con máximo 2 decimales" ValidationExpression="^\d+(\.\d{1,2})?$">
                                </asp:RegularExpressionValidator>--%>
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                Cantidad:
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtCantidad" CssClass="form-control mayusculas" PlaceHolder="Cantidad" type="number" runat="server" Enabled="false"></asp:TextBox>
                            </div>

                        </div>
                        <br />

                        <div class="row" id="divStock" runat="server" visible="false">
                            <div class="col-lg-1 col-md-1 col-xs-1">
                                Stock: 
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-4">
                                <asp:TextBox ID="txtStock" CssClass="form-control mayusculas" PlaceHolder="Stock" type="number" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="text-center">
                            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-primary" Enabled="false" OnClick="btnAgregar_Click" />

                        </div>

                        <br />


                        <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
                            <div class="table-responsive">
                                <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%" HeaderStyle-ForeColor="Blue" HeaderStyle-Font-Size="Medium"
                                    ID="GridProductos" runat="server" AutoGenerateColumns="False" BorderStyle="None" OnRowDeleting="GridProductos_RowDeleting">
                                    <AlternatingRowStyle BorderStyle="None" />

                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="ColumnaOculta" HeaderStyle-CssClass="ColumnaOculta" />
                                        <asp:BoundField DataField="Item" HeaderText="Descripción" />
                                        <asp:BoundField DataField="Precio" HeaderText="Precio" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" />
                                        <asp:BoundField DataField="Stock" HeaderText="Stock" ItemStyle-CssClass="ColumnaOculta" HeaderStyle-CssClass="ColumnaOculta" />
                                        <%-- Se llena unicamente si es un producto --%>
                                        <asp:CommandField ShowDeleteButton="true" HeaderText="Eliminar" DeleteText="Quitar" />
                                    </Columns>
                                    <EditRowStyle BorderStyle="None" />
                                    <EmptyDataRowStyle BorderStyle="None" />
                                    <HeaderStyle BorderStyle="None" />
                                    <RowStyle BorderStyle="None" />
                                </asp:GridView>
                            </div>
                            <div class="text-center">
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" Enabled="false" OnClick="btnGuardar_Click" />
                            </div>

                        </asp:Panel>
                    </div>


                </div>
            </div>




            <%-- MODAL DEL GRID DE ESTUDIANTES --%>
            <div class="modal fade" id="ModalStudent" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header" style="text-align: center; font-weight: bold">
                            <div class="text-center">
                                <div style="text-align: center; font-size: large; font-weight: bold">
                                    Estudiantes
                                </div>
                            </div>
                        </div>

                        <div class="modal-body modal-body-scroll">
                            <br />

                            <div class="table-responsive">

                                <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%"
                                    HeaderStyle-ForeColor="Blue" DataKeyNames="CodigoEstudiante"
                                    ID="GridEstudiantes" runat="server" AutoGenerateColumns="False" BorderStyle="None"
                                    OnRowCommand="GridEstudiantes_RowCommand">
                                    <AlternatingRowStyle BorderStyle="None" />

                                    <Columns>
                                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnBuscar" CommandName="Buscar"
                                                    runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                    CssClass="circle btn btn-primary grow">
                             <i  class="glyphicon glyphicon-search" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>

                                            <HeaderStyle Width="5px" />

                                            <ItemStyle CssClass="text-center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CodigoEstudiante" HeaderText="Código Estudiante" SortExpression="CodigoEstudiante" />
                                        <asp:BoundField DataField="Nombres" HeaderText="Nombres" SortExpression="Nombres" />
                                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" SortExpression="Apellidos" />
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                                    </Columns>
                                    <EditRowStyle BorderStyle="None" />
                                    <EmptyDataRowStyle BorderStyle="None" />
                                    <HeaderStyle BorderStyle="None" />
                                    <RowStyle BorderStyle="None" />
                                </asp:GridView>

                            </div>
                            <br />
                            <br />
                        </div>

                        <div class="modal-footer">
                            <button class="btn btn-danger" type="button" data-dismiss="modal" id="cerrar">Cancelar</button>

                        </div>
                    </div>
                </div>
            </div>



            <%-- MODAL CONFIRMACIÓN --%>
            <div class="modal fade" id="ModalConfirm" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">Confirmar</h5>
                        </div>
                        <div class="modal-body">¿Guardar la factura?</div>
                        <div class="modal-footer">
                            <button class="btn btn-danger" type="button" data-dismiss="modal" id="salir">No</button>
                            <asp:Button ID="btnConfirmar" runat="server" Text="Si" CssClass="btn btn-primary" OnClick="btnConfirmar_Click" />
                        </div>
                    </div>
                </div>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnAbrirModalEstudiante" />
            <asp:PostBackTrigger ControlID="btnAgregar" />
            <asp:PostBackTrigger ControlID="btnGuardar" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
