<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormPrinterCountCash.aspx.cs" Inherits="SistemaFinanciero.WebFormPrinterCountCash" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="panel panel-primary" runat="server" id="panellista" visible="true">
        <div class="panel-heading">
            <h6>Ejemplo de impresión</h6>
        </div>
        <div class="panel-body">
            <div class="row">

            </div>

            <div class="row">
                <div class="col-sm-4">
                    <asp:Button ID="BtnImprimir" runat="server" Text="Imprimir" class="btn btn-primary" Style="background: #428bca" OnClick="BtnImprimir_Click" />
                </div>
            </div>
        </div>
    </div>


</asp:Content>
