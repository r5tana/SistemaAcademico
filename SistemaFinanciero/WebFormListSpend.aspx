<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormListSpend.aspx.cs" Inherits="SistemaFinanciero.WebFormListSpend" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

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

                $('#ContentPlaceHolder1_GridGastos').DataTable({
                    "scrollX": true,
                    "language": idioma,
                    dom: 'Bfrtip',
                    deferRender: true,
                    scrollY: 200,
                    scrollCollapse: true,
                    scroller: true,
                    buttons: [
                        //'copyHtml5',
                        //'excelHtml5',
                        //'csvHtml5',
                        //'pdfHtml5'
                        {
                            extend: 'excel',
                            text: '<i class="fa fa-file-excel-o"></i> Exportar a Excel',
                            autoFilter: true,
                            className: 'btn btn-success',
                            exportOptions: {
                                modifier: {
                                    page: 'all'
                                }
                            }
                        },
                    {
                        //extend: 'pdf',
                        //orientation: 'landscape',
                        //pageSize: 'LEGAL',
                        //title: 'FINESA',
                        //filename: 'Reporte ',
                        //text: '<i class="fa fa-file-pdf-o"></i> Exportar PDF',
                        //className: 'btn btn-info',
                        //customize: function (doc) {
                        //    doc.defaultStyle.fontSize = 6;
                        //    doc.styles.tableHeader.fontSize = 6;
                        //    doc.pageMargins = [10, 10, 10, 10];

                        //}

                        extend: 'pdf',
                        text: 'Imprimir PDF',
                        className: 'btn btn-danger',
                        messageTop: 'Creciendo Juntos',
                        title: 'Reporte de Gastos',
                        customize: function (doc) {
                            doc.defaultStyle.fontSize = 13;
                            doc.defaultStyle.alignment = 'center';
                            doc.styles.tableHeader.fontSize = 16;
                        }
                    }
                    ]
                });
            });
        }

    </script>
</asp:Content>
<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

       <div class="panel panel-primary" runat="server" id="panellista" visible="true">
        <div class="panel-heading">
            <h6>Gastos</h6>
        </div>
        <div class="panel-body">
            <div class="row">
                    <div class="col-sm-3">

                    <div class="form-group">
                        <label for="Renta">FECHA INICIO </label>
                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" Width="220px"></asp:TextBox>
                        <asp:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                            CultureDecimalPlaceholder="" CultureName="es-NI" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio"
                            UserDateFormat="DayMonthYear"></asp:MaskedEditExtender>
                        <asp:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" Enabled="True"
                            Format="dd/MM/yyyy" TargetControlID="txtFechaInicio"></asp:CalendarExtender>
                    </div>

                </div>

                <div class="col-sm-3">

                    <div class="form-group">
                        <label for="Renta">FECHA FIN </label>
                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" Width="220px"></asp:TextBox>
                        <asp:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                            CultureDecimalPlaceholder="" CultureName="es-NI" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin"
                            UserDateFormat="DayMonthYear"></asp:MaskedEditExtender>
                        <asp:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True"
                            Format="dd/MM/yyyy" TargetControlID="txtFechaFin"></asp:CalendarExtender>
                    </div>

                </div>


            </div>

            <div class="row">
                <div class="col-sm-4">
                    <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" class="btn btn-primary" Style="background: #428bca" OnClick="BtnBuscar_Click"  />
                </div>
            </div>
            <br />
            <br />
            <div class="overflow-auto" style="max-width: inherit; max-height: inherit;">
            <div class="table-responsive">

                <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%" HeaderStyle-ForeColor="#428bca" HeaderStyle-Font-Size="Small" ID="GridGastos" runat="server" AutoGenerateColumns="False" BorderStyle="None" OnRowDataBound="GridGastos_RowDataBound" OnSelectedIndexChanged="GridGastos_SelectedIndexChanged" Visible="false">
                    <AlternatingRowStyle BorderStyle="None" />

                    <Columns>

                        <asp:BoundField DataField="DescripcionGasto" HeaderText="Gasto" SortExpression="DescripcionGasto" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:yyyy/M/dd}" />
                        <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto"  />
                    </Columns>
                    <EditRowStyle BorderStyle="None" />
                    <EmptyDataRowStyle BorderStyle="None" />
                    <HeaderStyle BorderStyle="None" />
                    <RowStyle BorderStyle="None" />
                </asp:GridView>
            </div>
                </div>
            </div>
           </div>

</asp:content>
