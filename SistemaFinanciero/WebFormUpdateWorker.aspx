<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormUpdateWorker.aspx.cs" Inherits="SistemaFinanciero.WebFormUpdateWorker" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

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

                $('#ContentPlaceHolder1_GridDatasolicitud').DataTable({
                    "scrollX": true,
                    "language": idioma,
                    dom: 'Bfrtip',
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true,
                    "order": [[7, "asc"]],
                    buttons: [
                        {
                            extend: 'excel',
                            text: '<i class="fa fa-file-excel-o"></i> Exportar a Excel',
                            autoFilter: true,
                            className: 'btn btn-success',
                            exportOptions: {
                                columns: [3, 4, 5, 6, 7]
                            }
                        },
                    {
                        extend: 'pdf',
                        text: 'Imprimir PDF',
                        className: 'btn btn-danger',
                        messageTop: 'Trabajadores Creciendo Juntos',
                        title: 'Reporte Trabajadores',
                        exportOptions: {
                            columns: [3, 4, 5, 6, 7]
                        },
                        customize: function (doc) {
                            doc.defaultStyle.fontSize = 10;
                            doc.defaultStyle.alignment = 'center';
                            doc.styles.tableHeader.fontSize = 10;
                        }
                    }
                    ]
                });
            });
        }

    </script>

    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-sweetalert/1.0.1/sweetalert.js" />
    <style type="text/css">
        .overlay {
            position: fixed;
            z-index: 200;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #313131;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .overlayContent {
            z-index: 200;
            margin: 300px auto;
            width: 320px;
            height: 250px;
        }
    </style>
    <style>
        .Enk1 {
            text-decoration-color: blue;
            font-family: Cambria;
            font-weight: bold;
            margin-left: 20px;
            color: red;
        }

        .mayus {
            text-transform: uppercase;
        }

        hr {
            border: 0;
            height: 0;
            border-top: 4px double black;
            text-align: center;
        }

            hr:after {
                position: relative;
                top: -22px;
                content: "\25cf\25cf\25cf";
                font-size: 34px;
                line-height: 34px;
                color: black;
            }

        .dropshadowclass:hover {
            border: solid 1px #CCC;
            -moz-box-shadow: 0px 2px 7px 6px #999;
            -webkit-box-shadow: 0px 2px 7px 6px #999;
            box-shadow: 0px 2px 7px 6px #999;
        }

        p, h3 {
            text-align: justify;
            text-decoration-color: blue;
            font-family: cursive;
            font-weight: bold;
        }
    </style>


</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script src="Scripts/sweetalert.min.js"></script>

    <div class="panel panel-primary" runat="server" id="panellista" visible="false">
        <div class="panel-heading text-left">
            <h6>Listado de Trabajadores</h6>
        </div>
        <div class="panel-body">
            <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%" HeaderStyle-ForeColor="Blue" HeaderStyle-Font-Size="Medium" ID="GridDatasolicitud" runat="server" AutoGenerateColumns="False" BorderStyle="None"
                OnRowCommand="GridDatasolicitud_RowCommand" OnRowDataBound="GridDatasolicitud_RowDataBound" OnSelectedIndexChanged="GridDatasolicitud_SelectedIndexChanged">
                <AlternatingRowStyle BorderStyle="None" />

                <Columns>
                    <asp:TemplateField HeaderText="Modificar" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnBaja" CommandName="DetalleSolicitud"
                                runat="server" CommandArgument='<%# Eval("CedulaTrabajador") %>'
                                CssClass="circle btn btn-primary grow">
                                                <i  class="glyphicon glyphicon-search" aria-hidden="true"></i>
                            </asp:LinkButton>
                        </ItemTemplate>

                        <HeaderStyle Width="5px" />

                        <ItemStyle CssClass="text-center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Activar/Inactivar" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnInactivarUsuario" CommandName="InactivarUsuario"
                                runat="server" CommandArgument='<%# Eval("CedulaTrabajador") %>'
                                CssClass="circle btn btn-danger grow">
                                                <i  class="glyphicon glyphicon-ok" aria-hidden="true"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Width="5px" />
                        <ItemStyle CssClass="text-center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Resetear Contraseña" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnResetPassword" CommandName="ResetPassword"
                                runat="server" CommandArgument='<%# Eval("CedulaTrabajador") %>'
                                CssClass="circle btn btn-success">
                                                <i  class="glyphicon glyphicon-ok" aria-hidden="true"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Width="5px" />
                        <ItemStyle CssClass="text-center"></ItemStyle>
                    </asp:TemplateField>

                    <asp:BoundField DataField="NombreTrabajador" HeaderText="Nombre Trabajador" SortExpression="NombreTrabajador" />
                    <asp:BoundField DataField="CedulaTrabajador" HeaderText="Numero Cedula" SortExpression="CedulaTrabajador" />
                    <asp:BoundField DataField="Telefono1" HeaderText="Telefono" SortExpression="Telefono1" />
                    <asp:BoundField DataField="Sexo" HeaderText="Sexo" SortExpression="Sexo" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                </Columns>
                <EditRowStyle BorderStyle="None" />
                <EmptyDataRowStyle BorderStyle="None" />
                <HeaderStyle BorderStyle="None" />
                <RowStyle BorderStyle="None" />
            </asp:GridView>

        </div>
    </div>

</asp:Content>
