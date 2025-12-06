<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormReports.aspx.cs" Inherits="SistemaFinanciero.WebFormReports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .ColumnaOculta {
            display: none;
        }
    </style>

    <script>
        function deshabilitarbtn() {
            var btn = "<%= btnBuscar.ClientID %>";
            document.getElementById(btn).disabled = true;
            document.getElementById(btn).value = "Cargando...";
        }

    </script>

    <script type="text/javascript">

        function cargarFechasMaxima() {
            debugger;

            var fechaFinal = "";
            const dia = 31;
            const date = new Date();
            const today = date.toISOString().split('T')[0];
            const mesActual = `${(date.getMonth() + 1).toString().padStart(2, '0')}`;
            const anio = `${date.getFullYear()}`;




            var fechaActual = `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${(date.getDate()).toString().padStart(2, '0')}`;
            var horas = `${(date.getHours()).toString().padStart(2, '0')}`;
            var minutos = `${(date.getMinutes()).toString().padStart(2, '0')}`;




            fechaFinal = fechaActual;


            document.getElementById('txtFechaInicio').setAttribute('max', fechaFinal);
            document.getElementById('txtFechaFin').setAttribute('max', fechaFinal);

        }
    </script>

    <script type="text/javascript">

        function pageLoad() {
            $(document).ready(function () {

                var idioma = {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
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
                        "sLast": "Último",
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

                $('#ContentPlaceHolder1_GridRecibo').DataTable({
                    "scrollX": true,
                    "language": idioma,
                    dom: 'Bfrtip',
                    deferRender: true,
                    scrollY: 300,
                    scrollCollapse: true,
                    scroller: true,
                    buttons: [

                    ]
                });



                //$('#ContentPlaceHolder1_GridDetalleRecibo').DataTable({

                $('#ContentPlaceHolder1_GridDetalleRecibo').append('<tfoot><tr><th colspan="3"></th><th></th><th></th></tr></tfoot>'),
                 $('#ContentPlaceHolder1_GridDetalleRecibo').prepend($("<thead></thead>").append($('#ContentPlaceHolder1_GridDetalleRecibo').find("tr:first"))).DataTable({

                        "scrollX": true,
                        "language": idioma,
                        dom: 'Bfrtip',
                        deferRender: true,
                        scrollY: 300,
                        scrollCollapse: true,
                        scroller: true,
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api(), data;

                            // Remove the formatting to get integer data for summation
                            var intVal = function (i) {
                                return typeof i === 'string' ?
                                    i.replace(/[\$,]/g, '') * 1 :
                                    typeof i === 'number' ?
                                        i : 0;
                            };

                            // Total over all pages
                            total = api
                                .column(4)
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);

                            // Total over this page
                            pageTotal = api
                                .column(4, { page: 'current' })
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);

                            // Update footer
                            $(api.column(3).footer()).html("TOTAL");
                            $(api.column(4).footer()).html('C$ ' + total);

                        },
                        buttons: [
                            {
                                extend: 'pdf',
                                text: 'Imprimir PDF',
                                className: 'btn btn-danger',
                                //messageTop: 'Reporte de Aranceles por Estudiante',
                                //title: 'Instituto Pedagógico La Salle',
                                filename: 'Detalle Recibo',
                                exportOptions: {
                                    //columns: [3, 4,]
                                    modifier: {
                                        page: 'all'
                                    }
                                },
                                customize: function (doc) {
                                    // 1. Obtener los valores de los TextBoxes
                                    var recibo = $('#<%= txtNumRecibo.ClientID %>').val();
                                var nombre = $('#<%= txtNombreFactura.ClientID %>').val();

                                    // 2. Definir los textos personalizados
                                    var tituloPrincipal = 'Instituto Pedagógico La Salle';
                                    var subtituloReporte = 'Detalle de Recibo';
                                    var subtituloDetalle = 'No. Recibo: ' + recibo + '  |  A Nombre De: ' + nombre;

                                    // 3. Crear el nuevo contenido de encabezado
                                    var newHeader = [
                                        {
                                            text: tituloPrincipal,
                                            style: 'headerStyle',
                                            alignment: 'center',
                                            margin: [0, 0, 0, 5] // Ajuste de margen inferior
                                        },
                                        {
                                            text: subtituloReporte,
                                            style: 'subheaderStyle',
                                            alignment: 'center',
                                            margin: [0, 0, 0, 5]
                                        },
                                        {
                                            text: subtituloDetalle,
                                            style: 'detailStyle',
                                            alignment: 'left',
                                            margin: [0, 0, 0, 15] // Margen inferior para separar de la tabla
                                        }
                                    ];

                                    // 4. Reemplazar el contenido actual (que incluye el título por defecto)
                                    // doc.content es un array de objetos. El primer elemento es el título y los siguientes son la tabla.
                                    // Insertamos el nuevo encabezado antes de la tabla (que está en doc.content[0] después de que DataTables ha procesado el PDF por defecto)
                                    doc.content.splice(0, 1); // Elimina el título por defecto de DataTables

                                    // Añadir el nuevo encabezado al inicio del array de contenido
                                    doc.content = newHeader.concat(doc.content);

                                    // 5. Definir estilos personalizados
                                    doc.styles = doc.styles || {};

                                    doc.styles.headerStyle = {
                                        fontSize: 14, // Tamaño más grande para el título principal
                                        bold: true,
                                        alignment: 'center'
                                    };

                                    doc.styles.subheaderStyle = {
                                        fontSize: 12,
                                        bold: true,
                                        alignment: 'center'
                                    };

                                    doc.styles.detailStyle = {
                                        fontSize: 10,
                                        bold: false,
                                        alignment: 'left' // Alineado a la izquierda según tu descripción
                                    };

                                    // 6. Aplicar estilos generales para la tabla
                                    doc.defaultStyle.fontSize = 10;
                                    doc.styles.tableHeader.fontSize = 10;
                                    //doc.styles.tableHeader.fillColor = '#CCCCCC'; // Opcional: fondo gris para el encabezado de la tabla
                                }
                            }
                        ]
                    });
            });
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>

    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="panel panel-primary" runat="server" id="panellista">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        <asp:Label ID="lblcomprobante" runat="server" Text="Búsqueda de Recibos"></asp:Label>
                    </h3>
                </div>
                <div class="panel-body">

                    <div class="row" runat="server" id="divBusqueda">
                        <div class="col-lg-3 col-md-3 col-xs-3">
                            <label>Tipo de Búsqueda:</label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-xs-6">
                            <asp:DropDownList ID="ddlTipoSeleccionBusqueda" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTipoSeleccionBusqueda_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                <asp:ListItem Value="1">No. Recibo</asp:ListItem>
                                <asp:ListItem Value="2">Rango de Fechas</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />

                    <asp:Panel ID="pnlRecibo" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-xs-3">
                                <asp:Label ID="lblNumeroRecibo" runat="server" Text="No. Recibo:"></asp:Label>
                            </div>
                            <div class="col-lg-6 col-md-6 col-xs-6">
                                <asp:TextBox ID="txtNumeroRecibo" runat="server" ClientIDMode="Static" CssClass="form-control" placeholder="Dígite No. Recibo" />

                            </div>

                        </div>
                        <br />
                    </asp:Panel>

                    <asp:Panel ID="pnlRangoFecha" runat="server" Visible="false">

                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-xs-3">
                                <asp:Label ID="lblRangoFecha" runat="server" Text="Rango de fecha:"></asp:Label>
                            </div>
                            <div class="col-lg-3 col-md-3 col-xs-3">
                                <p>Fecha inicio</p>
                                <div class="form-group">
                                    <div style="font-weight: bold; font-size: large;" class='input-group date datepicker' name="datepicker">
                                        <asp:TextBox ID="txtFechaInicio" runat="server" type="date" ClientIDMode="Static" CssClass="form-control " placeholder="dd/mm/aaaa" />

                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 col-xs-3">
                                <p>Fecha fin</p>
                                <div class="form-group">
                                    <div style="font-weight: bold; font-size: large;" class='input-group date datepicker' name="datepicker">
                                        <asp:TextBox ID="txtFechaFin" runat="server" type="date" ClientIDMode="Static" CssClass="form-control" placeholder="dd/mm/aaaa" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                    </asp:Panel>


                    <div class="text-center">

                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" Visible="false"
                            AutopoPostBack="true" UseSubmitBehavior="false" OnClientClick="deshabilitarbtn()" />
                    </div>

                    <br />

                    <div class="table-responsive">

                        <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%" DataKeyNames="id_factura"
                            HeaderStyle-ForeColor="#007bff" HeaderStyle-Font-Size="small" ID="GridRecibo" runat="server" AutoGenerateColumns="False"
                            OnRowCommand="GridRecibo_RowCommand" BorderStyle="None">
                            <AlternatingRowStyle BorderStyle="None" />
                            <Columns>
                                <asp:TemplateField HeaderText="Ver Detalle" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="btnDetalle" CommandName="Detalle" runat="server" CommandArgument='<%# Eval("id_factura") %>'
                                            CssClass="circle btn btn-success grow">
                                            <i  class="fa fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>--%>

                                        <asp:LinkButton ID="btnDetalle" CommandName="Detalle"
                                            runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                            CssClass="circle btn btn-success grow">
                                            <i  class="fa fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>


                                    </ItemTemplate>
                                    <HeaderStyle Width="5px" />

                                    <ItemStyle CssClass="text-center"></ItemStyle>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Imprimir Recibo" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="btnImprimir" CommandName="ImpirmirRecibo" runat="server" CommandArgument='<%# Eval("id_factura") %>'
                                            CssClass="circle btn btn-danger grow">
                                             <i  class="fa fa-print" aria-hidden="true"></i>
                                        </asp:LinkButton>--%>

                                        <asp:LinkButton ID="btnImprimir" CommandName="ImpirmirRecibo"
                                            runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                            CssClass="circle btn btn-danger grow">
                                            <i  class="fa fa-print" aria-hidden="true"></i>
                                        </asp:LinkButton>

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="id_factura" HeaderText="Número Recibo" />
                                <asp:BoundField DataField="anombrede" HeaderText="A Nombre" />
                                <asp:BoundField DataField="forma_pago" HeaderText="Forma Pago " />
                                <asp:BoundField DataField="estado" HeaderText="Estado" />
                                <asp:BoundField DataField="id_caja" HeaderText="Id Caja" />
                                <asp:BoundField DataField="estado_por" HeaderText="Estado Por" />
                                <asp:BoundField DataField="total_cordobas" HeaderText="Total Córdoba" />
                                <asp:BoundField DataField="total_dolares" HeaderText="Total Dolares" />
                                <asp:BoundField DataField="cliente_paga" HeaderText="Cliente Paga" />
                                <asp:BoundField DataField="cliente_cambio" HeaderText="Cambio Cliente" />
                                <asp:BoundField DataField="fecha_cancela" HeaderText="Fecha" DataFormatString="{0:dd-MM-yyyy}" />


                            </Columns>
                            <EditRowStyle BorderStyle="None" />
                            <EmptyDataRowStyle BorderStyle="None" />
                            <HeaderStyle BorderStyle="None" />
                            <RowStyle BorderStyle="None" />

                        </asp:GridView>


                    </div>
                    <asp:Panel ID="pnlDetalleFactura" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:Label ID="lblRecibo" runat="server" Text="No. Recibo:"></asp:Label>
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:TextBox ID="txtNumRecibo" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" />

                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-2">
                                <asp:Label ID="lblNombreFactura" runat="server" Text="A Nombre De:"></asp:Label>
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-4">
                                <asp:TextBox ID="txtNombreFactura" runat="server" ClientIDMode="Static" CssClass="form-control" ReadOnly="true" />

                            </div>
                        </div>
                        <br />

                        <div class="table-responsive">

                            <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%"
                                HeaderStyle-ForeColor="#007bff" HeaderStyle-Font-Size="small" ID="GridDetalleRecibo" runat="server"
                                AutoGenerateColumns="False"
                                BorderStyle="None">
                                <AlternatingRowStyle BorderStyle="None" />
                                <Columns>

                                    <asp:BoundField DataField="id" HeaderText="IdFacturaDetalle" ItemStyle-CssClass="ColumnaOculta" HeaderStyle-CssClass="ColumnaOculta" />
                                    <%--<asp:BoundField DataField="id_factura" HeaderText="Número Recibo" ItemStyle-CssClass="ColumnaOculta" HeaderStyle-CssClass="ColumnaOculta" />--%>
                                    <asp:BoundField DataField="nombre_item" HeaderText="Item" />
                                    <asp:BoundField DataField="tipo_item" HeaderText="Tipo Item" />
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="preciounitario" HeaderText="Precio" />
                                    <asp:BoundField DataField="subtotal" HeaderText="Subtotal" />

                                </Columns>
                                <EditRowStyle BorderStyle="None" />
                                <EmptyDataRowStyle BorderStyle="None" />
                                <HeaderStyle BorderStyle="None" />
                                <RowStyle BorderStyle="None" />

                            </asp:GridView>

                        </div>

                        <br />

                        <div class="text-center">

                            <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="btn btn-danger" OnClick="btnRegresar_Click" Visible="false"
                                AutopoPostBack="true" />
                        </div>

                        <br />
                    </asp:Panel>

                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnRegresar" />
            <asp:PostBackTrigger ControlID="btnBuscar" />
            <asp:PostBackTrigger ControlID="GridRecibo" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
