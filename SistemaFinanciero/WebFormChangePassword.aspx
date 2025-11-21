<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormChangePassword.aspx.cs" Inherits="SistemaFinanciero.WebFormChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function mostrarclave(tipo) {
            debugger;

            if (tipo == 1) // Contraseña anterior
            {
                var cambio = document.getElementById('<%=txtclaveanterior.ClientID%>');
                if (cambio.type == "password") {
                    cambio.type = "text";
                    $('.icon1').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
                else {
                    cambio.type = "password";
                    $('.icon1').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                }
            }
            else if (tipo == 2) // Contraseña nueva
            {
                var cambio = document.getElementById('<%=txtclavenueva.ClientID%>');
                if (cambio.type == "password") {
                    cambio.type = "text";
                    $('.icon2').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
                else {
                    cambio.type = "password";
                    $('.icon2').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                }
            }
            else if (tipo == 3) // Confirmar contraseña
            {

                var cambio = document.getElementById('<%=txtconfirmarclave.ClientID%>');
                if (cambio.type == "password") {
                    cambio.type = "text";
                    $('.icon3').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
                else {
                    cambio.type = "password";
                    $('.icon3').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                }

            }
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Cambiar Clave</h6>
                </div>
                <div class="card-body">
                    <div align="center">
                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Contraseña Anterior:
                            </div>
                            <div class="col-lg-6 col-md-4 col-xs-6">
                                <div class="input-group">
                                    <asp:TextBox ID="txtclaveanterior" class="form-control form-control-user" runat="server" type="password" placeholder="Contraseña anterior"></asp:TextBox>
                                    <div class="input-group-append">
                                        <button id="show_password_old" class="btn btn-primary" type="button" onclick="mostrarclave(1);">
                                            <span class="input-group-addon"><i class="fa fa-eye icon1"></i></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Contraseña Nueva:
                            </div>
                            <div class="col-lg-6 col-md-4 col-xs-6">
                                <div class="input-group">
                                    <asp:TextBox ID="txtclavenueva" class="form-control form-control-user" runat="server" type="password" placeholder="Contraseña anterior"></asp:TextBox>
                                    <div class="input-group-append">
                                        <button id="show_password_new" class="btn btn-primary" type="button" onclick="mostrarclave(2);">
                                            <span class="input-group-addon"><i class="fa fa-eye icon2"></i></span>
                                        </button>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-xs-6">
                                Confirmar Contraseña:
                            </div>
                            <div class="col-lg-6 col-md-4 col-xs-6">
                                <div class="input-group">
                                    <asp:TextBox ID="txtconfirmarclave" class="form-control form-control-user" runat="server" type="password" placeholder="Confirmar Contraseña"></asp:TextBox>
                                    <div class="input-group-append">
                                        <button id="show_password_confirm" class="btn btn-primary" type="button" onclick="mostrarclave(3);">
                                            <span class="input-group-addon"><i class="fa fa-eye icon3"></i></span>
                                        </button>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <br />
                        <br />
                        <div class="text-center">
                            <asp:Button ID="btnguardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnguardar_Click" />

                        </div>


                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
