<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormErrorGeneral.aspx.cs" Inherits="SistemaFinanciero.WebFormErrorGeneral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container d-flex justify-content-center align-items-center" style="height: 100vh;">
        <div class="col-md-6">
            <div class="card shadow border-left-danger">
                <div class="card-body text-center">
                    <h1 class="display-4 text-danger">¡Ups!</h1>
                    <h4 class="text-dark mb-3">Ha ocurrido un error inesperado</h4>
                    <p class="mb-4">Por favor vuelve a intentar o contacta al administrador.</p>
                    <a href="WebFormInicio.aspx" class="btn btn-primary">Regresar al Inicio</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
