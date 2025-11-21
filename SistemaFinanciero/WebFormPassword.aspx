<%@ Page Title="" Language="C#" MasterPageFile="~/Ejmplo.Master" AutoEventWireup="true" CodeBehind="WebFormPassword.aspx.cs" Inherits="SistemaFinanciero.WebFormPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div align="center">
                <div class="panel panel-primary" style="height: auto; text-align: center;">
                    <div class="panel-heading">
                        Cambiar Contraseña
                    </div>
                    <div class="panel-body ">
                        <div class="input-group">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                            <asp:TextBox ID="txtClaveAnterior" type="text" CssClass="form-control" placeholder="Contraseña Temporal"
                                runat="server" TextMode="Password"></asp:TextBox>
                        </div>

                        <br />

                        <div class="input-group">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                            <asp:TextBox ID="txtClaveNueva" type="text" CssClass="form-control" placeholder="Contraseña Nueva"
                                runat="server" TextMode="Password"></asp:TextBox>
                        </div>

                        <br />

                        <div class="input-group">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-pencil"></i></span>
                            <asp:TextBox ID="txtConfirmarClave" type="text" CssClass="form-control" placeholder="Confirmar Contraseña"
                                runat="server" TextMode="Password"></asp:TextBox>
                        </div>

                        <br />

                        <br />
                        <div class="col-lg-12">
                            <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CssClass="btn btn-primary" OnClick="btnActualizar_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
