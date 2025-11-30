<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormAddWorker.aspx.cs" Inherits="SistemaFinanciero.WebFormAddWorker" %>

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

                $('#ContentPlaceHolder1_GridTrabajadores').DataTable({
                    "scrollX": true,
                    "language": idioma,
                    dom: 'Bfrtip',
                    deferRender: true,
                    scrollY: 300,
                    scrollCollapse: true,
                    scroller: true,
                    "rowCallback": function (row, data, index) {

                        var estado = data[10],
                            $node = this.api().row(row).nodes().to$();

                        if (estado == 'ACTIVO' | estado == 'ACTIVA') {
                            $node.addClass('white');
                        }
                        else {
                            $node.addClass('yellow');
                        }
                    },

                    buttons: [
                        //'copyHtml5',
                        //'excelHtml5',
                        //'csvHtml5',
                        //'pdfHtml5'
                        {

                            extend: 'excel',
                            text: '<i class="fa fa-file-excel-o"></i> Exportar a Excel',
                            messageTop: 'Lista de usuarios',
                            autoFilter: true,
                            className: 'btn btn-success',
                            exportOptions: {
                                columns: [4, 5, 6, 7, 8, 9, 10, 11, 12]
                                //modifier: {
                                //    page: 'all'


                                //}
                            }
                        },
                        {
                            extend: 'pdf',
                            orientation: 'landscape',
                            pageSize: 'LETTER', //LEGAL
                            title: 'Reporte Usuario',
                            exportOptions: {
                                columns: [4, 5, 6, 7, 8, 9, 10, 11, 12]
                            },
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>


            <div class="panel panel-primary" runat="server" id="panellista">
                <div class="panel-heading">
                    <h3>Usuarios</h3>
                </div>
                <div class="panel-body">

                    <asp:Panel ID="pnlDatos" runat="server" Visible="false">


                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Usuario:
                                       
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-6">
                                <asp:TextBox ID="txtIdUsuarioActual" CssClass="form-control mayusculas" ReadOnly="true" PlaceHolder="Id" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtlogin" CssClass="form-control mayusculas" ReadOnly="true" PlaceHolder="Usuario" runat="server"></asp:TextBox>
                                <asp:Label ID="lblsms" runat="server" Text="El usuario será generado automáticamente" ForeColor="Green" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <br />
                        <div class="row">

                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Nombres:
                                       
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-6">
                                <asp:TextBox ID="txtNombres" CssClass="form-control mayusculas" PlaceHolder="Nombres" runat="server"></asp:TextBox>
                            </div>

                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Apellidos:
                                       
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-6">
                                <asp:TextBox ID="txtApellidos" CssClass="form-control mayusculas" PlaceHolder="Apellidos" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">

                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Cargo:
                                       
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-6">
                                <asp:TextBox ID="txtCargo" CssClass="form-control mayusculas" PlaceHolder="Apellidos" runat="server" ReadOnly="true"></asp:TextBox>

                                <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCargo_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="N/A">SELECCIONE...</asp:ListItem>
                                    <asp:ListItem Value="ADMINISTRADOR">ADMINISTRADOR</asp:ListItem>
                                    <asp:ListItem Value="CAJERO">CAJERO</asp:ListItem>
                                    <asp:ListItem Value="CONTADORA">CONTADORA</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Estado:
                                       
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-6">
                                <asp:TextBox ID="txtEstado" CssClass="form-control mayusculas" PlaceHolder="Estado" runat="server" Text="Activo" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>

                        <br />

                        <div class="row" id="divSerie" runat="server" visible="false">
                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Serie Caja:
           
                            </div>
                            <div class="col-lg-4 col-md-4 col-xs-6">
                                <asp:DropDownList ID="ddlSerie" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">SELECCIONE...</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <br />

                        <div class="text-center">
                            <asp:Button ID="btnActualizar" CssClass="btn btn-primary" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" Visible="false" />
                            <asp:Button ID="btnGuardar" CssClass="btn btn-primary" runat="server" Text="Guardar" OnClick="btnGuardar_Click" Visible="false" />
                            <asp:Button ID="btnCancelar" CssClass="btn btn-danger" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                        </div>

                    </asp:Panel>

                    <asp:Panel ID="pnlGrid" runat="server">

                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar Usuario" CssClass="btn btn-primary" OnClick="btnAgregar_Click" />
                        <br />
                        <br />

                        <br />

                        <div class="table-responsive">

                            <asp:GridView CssClass="table table-hover table-bordered table-striped" Style="width: 100%" DataKeyNames="id_usuario"
                                HeaderStyle-ForeColor="#007bff" HeaderStyle-Font-Size="small" ID="GridTrabajadores" runat="server" AutoGenerateColumns="False"
                                OnRowCommand="GridTrabajadores_RowCommand" OnRowDataBound="GridTrabajadores_RowDataBound" BorderStyle="None">
                                <AlternatingRowStyle BorderStyle="None" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Editar" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" CommandName="ModificarUsuario" runat="server" CommandArgument='<%# Eval("id_usuario") %>'
                                                CssClass="circle btn btn-success grow">
                                            <i  class="glyphicon glyphicon-pencil" aria-hidden="true"></i>
                                            </asp:LinkButton>


                                        </ItemTemplate>
                                        <HeaderStyle Width="5px" />

                                        <ItemStyle CssClass="text-center"></ItemStyle>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Activar/Inactivar" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnInactivar" CommandName="InactivarUsuario" runat="server" CommandArgument='<%# Eval("id_usuario") %>'
                                                CssClass="circle btn btn-danger grow">
                                            <i  class="glyphicon glyphicon-ok" aria-hidden="true"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Resetear Clave" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnResetearClave" CommandName="ResetearClave" runat="server" CommandArgument='<%# Eval("id_usuario") %>'
                                                CssClass="circle btn btn-info grow">
                                            <i class="glyphicon glyphicon-refresh" aria-hidden="true"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="id_usuario" HeaderText="ID" ItemStyle-CssClass="ColumnaOculta" HeaderStyle-CssClass="ColumnaOculta" />
                                    <asp:BoundField DataField="usuario" HeaderText="Login" />
                                    <asp:BoundField DataField="nombres" HeaderText="Nombres" />
                                    <asp:BoundField DataField="apellidos" HeaderText="Apellidos" />
                                    <asp:BoundField DataField="cargo" HeaderText="Cargo" />
                                    <asp:BoundField DataField="fecha_inicio" HeaderText="Fecha Inicio" />
                                    <asp:BoundField DataField="fecha_fin" HeaderText="Fecha Fin" />
                                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                                    <asp:BoundField DataField="nivel" HeaderText="Nivel" />
                                    <asp:BoundField DataField="sesion" HeaderText="Sesión" />

                                </Columns>
                                <EditRowStyle BorderStyle="None" />
                                <EmptyDataRowStyle BorderStyle="None" />
                                <HeaderStyle BorderStyle="None" />
                                <RowStyle BorderStyle="None" />
                            </asp:GridView>

                        </div>

                    </asp:Panel>
                    <br />

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
