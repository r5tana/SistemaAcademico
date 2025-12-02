<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormStudentFee.aspx.cs" Inherits="SistemaFinanciero.WebFormStudentFee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .yellow {
            background-color: yellow !important;
        }

        .white {
            background-color: white !important;
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

                $('#ContentPlaceHolder1_GridAranceles').DataTable({
                    "scrollX": true,
                    "language": idioma,
                    dom: 'Bfrtip',
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true,
                    "order": [[0, "desc"]],
                    "rowCallback": function (row, data, index) {

                        var estado = data[4],
                            $node = this.api().row(row).nodes().to$();

                        if (estado == 'CANCELADO') {
                            $node.addClass('white');
                        }
                        else {
                            $node.addClass('yellow');
                        }
                    },
                    buttons: [
                        <%--{
                            extend: 'excel',
                            text: '<i class="fa fa-file-excel-o"></i> Exportar a Excel',
                            autoFilter: true,
                            className: 'btn btn-success',
                            title: 'Instituto Pedagógico La Salle',
                            //messageTop: 'Reporte de Aranceles por Estudiante',
                            messageTop: function () {
                                var codigo = $('#<%= txtEstudiante.ClientID %>').val();
                                var nombre = $('#<%= txtNombres.ClientID %>').val();

                                return 'Reporte de Aranceles por Estudiante' + '\n' +
                                    'Código: ' + codigo + ' | Nombre: ' + nombre;
                            },
                            filename: 'Reporte de Aranceles',
                            exportOptions: {
                                modifier: {
                                    page: 'all'
                                }
                            }

                        },--%>
                        {
                            extend: 'pdf',
                            text: 'Imprimir PDF',
                            className: 'btn btn-danger',
                            //messageTop: 'Reporte de Aranceles por Estudiante',
                            //title: 'Instituto Pedagógico La Salle',
                            filename: 'Reporte de Aranceles',
                            exportOptions: {
                                //columns: [3, 4,]
                                modifier: {
                                    page: 'all'
                                }
                            },
                            customize: function (doc) {
                                // 1. Obtener los valores de los TextBoxes
                                var codigoEstudiante = $('#<%= txtEstudiante.ClientID %>').val(); // $('#txtEstudiante').val();
                                var nombreEstudiante = $('#<%= txtNombres.ClientID %>').val(); // $('#txtNombres').val();

                                // 2. Definir los textos personalizados
                                var tituloPrincipal = 'Instituto Pedagógico La Salle';
                                var subtituloReporte = 'Reporte de Aranceles por Estudiantes';
                                var subtituloDetalle = 'Código: ' + codigoEstudiante + ' | Nombre: ' + nombreEstudiante;

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


            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="panel panel-primary" runat="server" id="panellista">
        <div class="panel-heading text-left">
            <h6>Aranceles por Estudiante</h6>
        </div>
        <div class="panel-body">

            <div class="table-responsive">

                <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%" HeaderStyle-ForeColor="Blue" HeaderStyle-Font-Size="Medium"
                    ID="GridEstudiantes" runat="server" AutoGenerateColumns="False" BorderStyle="None"
                    OnRowCommand="GridEstudiantes_RowCommand">
                    <AlternatingRowStyle BorderStyle="None" />

                    <Columns>
                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnBuscar" CommandName="BuscarArancel"
                                    runat="server" CommandArgument='<%# Eval("id_estudiante") %>'
                                    CssClass="circle btn btn-primary grow">
                             <i  class="glyphicon glyphicon-search" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </ItemTemplate>

                            <HeaderStyle Width="5px" />

                            <ItemStyle CssClass="text-center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="id_estudiante" HeaderText="Código Estudiante" SortExpression="id_estudiante" />
                        <asp:BoundField DataField="nombres" HeaderText="Nombres" SortExpression="nombres" />
                        <asp:BoundField DataField="apellidos" HeaderText="Apellidos" SortExpression="apellidos" />
                        <asp:BoundField DataField="estado" HeaderText="Estado" SortExpression="estado" />
                        <asp:BoundField DataField="seccion" HeaderText="Sección" SortExpression="seccion" />
                    </Columns>
                    <EditRowStyle BorderStyle="None" />
                    <EmptyDataRowStyle BorderStyle="None" />
                    <HeaderStyle BorderStyle="None" />
                    <RowStyle BorderStyle="None" />
                </asp:GridView>
            </div>


            <asp:Panel runat="server" ID="pnlResultado" Visible="false">
                <div class="row">

                    <div class="col-lg-2 col-md-2 col-xs-6">
                        Código Estudiante:
               
                    </div>
                    <div class="col-lg-4 col-md-4 col-xs-6">
                        <asp:TextBox ID="txtEstudiante" CssClass="form-control mayusculas" PlaceHolder="Código Estudiante" runat="server" ReadOnly="true"></asp:TextBox>

                    </div>

                </div>

                <br />

                <div class="row">

                    <div class="col-lg-2 col-md-2 col-xs-6">
                        Nombres y Apellidos:
                
                    </div>
                    <div class="col-lg-4 col-md-4 col-xs-6">
                        <asp:TextBox ID="txtNombres" CssClass="form-control mayusculas" PlaceHolder="Nombres y Apellidos" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="col-lg-2 col-md-2 col-xs-6">
                        Estado:
                
                    </div>
                    <div class="col-lg-4 col-md-4 col-xs-6">
                        <asp:TextBox ID="txtEstado" CssClass="form-control mayusculas" PlaceHolder="Estado" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>

                <br />
                <br />
                <div class="table-responsive">
                    <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%" HeaderStyle-ForeColor="Blue" HeaderStyle-Font-Size="Medium"
                        ID="GridAranceles" runat="server" AutoGenerateColumns="False" BorderStyle="None"
                        OnRowDataBound="GridAranceles_RowDataBound">
                        <AlternatingRowStyle BorderStyle="None" />

                        <Columns>
                            <asp:BoundField DataField="idtrans" HeaderText="Id transacción" SortExpression="idtrans" />
                            <asp:BoundField DataField="id_concepto" HeaderText="Concepto" SortExpression="id_concepto" />
                            <asp:BoundField DataField="descripcion" HeaderText="Descripcion" SortExpression="descripcion" />
                            <asp:BoundField DataField="total_cordobas" HeaderText="Total" SortExpression="total_cordobas" DataFormatString="{0:N2}" HtmlEncode="false" />
                            <asp:BoundField DataField="estado" HeaderText="Estado" SortExpression="estado" />
                            <asp:BoundField DataField="estadopor" HeaderText="Estado Por" SortExpression="estadopor" />
                            <asp:BoundField DataField="annio_lectivo" HeaderText="Año" SortExpression="annio_lectivo" />

                        </Columns>
                        <EditRowStyle BorderStyle="None" />
                        <EmptyDataRowStyle BorderStyle="None" />
                        <HeaderStyle BorderStyle="None" />
                        <RowStyle BorderStyle="None" />
                    </asp:GridView>
                </div>
                <br />

                <asp:Button ID="bntRetornar" CssClass="btn btn-danger" runat="server" Text="Regresar" OnClick="bntRetornar_Click" />


            </asp:Panel>




        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
