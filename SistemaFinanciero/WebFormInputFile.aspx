<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormInputFile.aspx.cs" Inherits="SistemaFinanciero.WebFormInputFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function deshabilitarbtn() {
            var btn = "<%= btnGuardar.ClientID %>";
            document.getElementById(btn).disabled = true;
            document.getElementById(btn).value = "Cargando...";
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

                    $('#ContentPlaceHolder1_GridDetalle').DataTable({
                        "scrollX": true,
                        "language": idioma,
                        dom: 'Bfrtip',
                        deferRender: true,
                        scrollY: 300,
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
                                className: 'btn btn-success',
                                exportOptions: {
                                    modifier: {
                                        page: 'all'
                                    }
                                }
                            },
                            {
                                extend: 'pdf',
                                orientation: 'landscape',
                                pageSize: 'LETTER',
                                title: 'Detalle Pagos',
                                filename: 'Reporte ',
                                text: '<i class="fa fa-file-pdf-o"></i> Exportar PDF',
                                className: 'btn btn-info',
                                customize: function (doc) {
                                    doc.defaultStyle.fontSize = 6;
                                    doc.styles.tableHeader.fontSize = 6;
                                    doc.pageMargins = [10, 10, 10, 10];

                                }
                            }
                        ]
                    });
                });

            }


    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="panel panel-primary">
        <div class="panel-heading">
            <h3 class="panel-title">
                <asp:Label ID="lblcomprobante" runat="server" Text="Cargar Comprobante"></asp:Label>
            </h3>
        </div>
        <div class="panel-body">
            <br />


            <asp:Panel runat="server" ID="pnlCargaArchivo" Visible="true">

                <div class="row">
                    <div class="form-group col-md-6">
                        <label class="col-md-4 control-label" name="FileUpload1">Archivo: </label>

                        <div class="col-md-8">
                            <asp:FileUpload ID="FileUpload2" multiple="true" CssClass="btn btn-default" runat="server" />
                        </div>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="col-md-5 col-lg-4 control-label" name="dtFecha">Fecha: </label>

                        <div class="col-md-7 col-lg-8">
                            <asp:TextBox ID="txtFecha" CssClass="form-control mayusculas" PlaceHolder="Fecha" type="date" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:Label ID="lblsms" runat="server" Text="La fecha es boligatoria" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>

                <br />

                <div class="text-center">

                    <asp:Button ID="btnAgregar" runat="server" Text="Cargar Datos" CssClass="btn btn-primary" OnClick="btnAgregar_Click"
                        AutopoPostBack="true" />

                </div>
            </asp:Panel>

            <br />
            <br />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="table-responsive">

                        <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%" HeaderStyle-ForeColor="Blue" HeaderStyle-Font-Size="Medium"
                            ID="GridDetalle" runat="server" AutoGenerateColumns="False" BorderStyle="None">
                            <AlternatingRowStyle BorderStyle="None" />

                            <Columns>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" NullDisplayText="-" HtmlEncode="False" />
                                <asp:BoundField DataField="NoRef" HeaderText="Referencia" />
                                <asp:BoundField DataField="CodAlum" HeaderText="Codigo Estudiante" />
                                <asp:BoundField DataField="Tipo" HeaderText="Categoria" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Arancel" />
                                <asp:BoundField DataField="Numero" HeaderText="Concepto" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:N2}" HtmlEncode="false" />
                            </Columns>
                        </asp:GridView>

                    </div>

                    <div class="text-center">
                        <asp:Button ID="btnGuardar" CssClass="btn btn-primary" runat="server" Text="Guardar" OnClick="btnGuardar_Click"
                            AutopoPostBack="true" Visible="false"  UseSubmitBehavior="false" OnClientClick="deshabilitarbtn()"  />
                        <asp:Button ID="btnCancelar" CssClass="btn btn-danger" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" Visible="false" AutopoPostBack="true" />
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGuardar" />
                    <asp:PostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
