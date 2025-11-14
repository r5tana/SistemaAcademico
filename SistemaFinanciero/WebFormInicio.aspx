    <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormInicio.aspx.cs" Inherits="SistemaFinanciero.WebFormInicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    

    <div class="panel panel-primary" runat="server" id="panellista" visible="false">
        <div class="panel-heading">
            <h6>RESUMEN DE CREDITOS</h6>
        </div>
        <div class="panel-body">
             <div class="table-responsive">
                    <asp:GridView CssClass="table table-hover table-bordered table-striped" HeaderStyle-ForeColor="Blue" HeaderStyle-Font-Size="Medium" ID="GridDatasolicitud" runat="server" AutoGenerateColumns="False" BorderStyle="None">
                        <AlternatingRowStyle BorderStyle="None" />

                        <Columns>
                            <asp:BoundField DataField="CedulaTrabajador" HeaderText="Cedula Trabajador" SortExpression="CedulaTrabajador" />
                            <asp:BoundField DataField="NombreTrabajador" HeaderText="Nombre Trabajador" SortExpression="NombreTrabajador" />
                            <asp:BoundField DataField="Activos" HeaderText="Créditos_Activos" SortExpression="Activos" />
                            <asp:BoundField DataField="Al_Dia" HeaderText="Créditos_Al_Día" SortExpression="Al_Dia" />
                            <asp:BoundField DataField="En_Mora" HeaderText="Créditos_En_Mora" SortExpression="En_Mora" />
                            <asp:BoundField DataField="Vencidos" HeaderText="Créditos_Vencidos" SortExpression="Vencidos" />
                        </Columns>
                        <EditRowStyle BorderStyle="None" />
                        <EmptyDataRowStyle BorderStyle="None" />
                        <HeaderStyle BorderStyle="None" />
                        <RowStyle BorderStyle="None" />
                    </asp:GridView>


                </div>
        </div>
    </div>

</asp:Content>
