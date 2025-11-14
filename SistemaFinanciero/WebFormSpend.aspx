<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormSpend.aspx.cs" Inherits="SistemaFinanciero.WebFormSpend" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function deshabilitarbtn() {
            var btn = "<%=BtnAgregar.ClientID %>";
            document.getElementById(btn).disabled = true;
            document.getElementById(btn).value = "Guardando...";
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:scriptmanager id="ScriptManager1" runat="server">
    </asp:scriptmanager>

    <div class="panel panel-primary" runat="server" id="panellista" visible="true">
        <div class="panel-heading">
            <h6>AGREGAR GASTOS ADMNISTRATIVOS</h6>
        </div>
        <div class="panel-body">
            <div class="row">
                   <div class="col-sm-3">

                    <div class="form-group">
                        <label for="Renta">Fecha del Gasto </label>
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
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="exampleInputEmail1">Descripción del Gasto</label>
                        <asp:textbox id="TxtDescripcionGasto" cssclass="form-control" runat="server"></asp:textbox>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label for="exampleInputEmail1">Monto del Gasto</label>
                        <asp:TextBox ID="TxtMontoGasto" CssClass="form-control" runat="server" TextMode="Number" step="0.02"></asp:TextBox>
                    </div>
                </div>
            </div>



            <div class="row">
                <div class="col-sm-4">
                    <asp:button id="BtnAgregar" runat="server" text="Guardar" class="btn btn-primary" style="background: #428bca"  onclientclick="deshabilitarbtn()" usesubmitbehavior="false" OnClick="BtnAgregar_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

