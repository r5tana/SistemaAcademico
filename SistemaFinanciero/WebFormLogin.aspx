<%@ Page Title="" Language="C#" MasterPageFile="~/Ejmplo.Master" AutoEventWireup="true" CodeBehind="WebFormLogin.aspx.cs" Inherits="SistemaFinanciero.WebFormLogin" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div align="center">
        <div class="panel panel-primary" style="height: auto; text-align: center;">
            <div class="panel-heading">
                Inicio de Sesión 
            </div>
            <div class="panel-body ">
                <div class="input-group">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                    <asp:TextBox ID="txtLogin" type="text" CssClass="form-control" placeholder="Usuario"
                        runat="server"></asp:TextBox>
                </div>
                <br />
                <div class="input-group">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                    <asp:TextBox ID="txtPassword" type="text" CssClass="form-control" placeholder="Contraseña"
                        runat="server" TextMode="Password"></asp:TextBox>
                </div>
                <br />

                <div class="row">
                    <label>Escriba el texto en la imagen:</label>
                </div>

                <div class="row">

                    <cc1:captchacontrol id="Captcha1" runat="server" captchabackgroundnoise="Low" captchalength="4"
                        captchaheight="60" captchawidth="300" captchamintimeout="5" captchamaxtimeout="240"
                        fontcolor="#337AB7" noisecolor="#B1B1B1" height="30px" />

                   <%-- <asp:ImageButton ID="ImageButton1" ImageUrl="http://placehold.jp/150x150.png>" runat="server" CausesValidation="false" Height="65px" Width="80px" required="required" />--%>
                </div>
                <br />
                <br />

                <div class="row">
                    <div class="col-md-12">
                        <div class="input-group">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-check"></i></span>
                            <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control" placeholder="Escriba el texto en la imagen:"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <br />
                 <div class="col-lg-12">
                        <asp:Button ID="btnAcceder" runat="server" Text="Acceder" CssClass="btn btn-primary" OnClick="btnAcceder_Click" />
                   
                    <asp:Button ID="btnDesbloquear" runat="server" Text="Desbloquear" CssClass="btn btn-danger" OnClick="btnDesbloquear_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
