<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="ReporteGuardias.aspx.cs" Inherits="SistemaSolicitudIngreso.Reportes.ReporteGuardias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
        <br />
        <div id="mainContainer" class="container">

            <div class="shadowBox">
                <div class="page-container">
                    <div class="container">
                        <div class="jumbotron">
                            <h1>Reportes</h1>
                        </div>
                        <asp:UpdatePanel ID="upContacto" runat="server">
                            <ContentTemplate>
                                <div id="accordion">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4>
                                                <a class="card-link" data-toggle="collapse" href="#collapse1">Reporte guardias
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapse1" class="collapse show" data-parent="#accordion">
                                            <div class="card-body">
                                                <div class="control-group row">
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-2">
                                                        <asp:TextBox ID="txtBuscaFechaDesde" runat="server" class="form-control" placeholder="Fecha desde" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBuscaFechaDesde" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-2">
                                                        <asp:TextBox ID="txtBuscaFechaHasta" runat="server" class="form-control" placeholder="Fecha hasta" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtBuscaFechaHasta" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-2">
                                                        <asp:DropDownList ID="ddlBuscaGuardia" runat="server" CssClass="form-control" data-style="btn-info"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                                        <asp:TextBox ID="txtBuscaNombre" runat="server" class="form-control" placeholder="Nombre solicitante" AutoCompleteType="Disabled" MaxLength="100"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-2">
                                                        <asp:TextBox ID="txtBuscaRut" runat="server" class="form-control" placeholder="Rut solicitante" AutoCompleteType="Disabled" MaxLength="100"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div class="row" style="text-align: center">
                                                        <div class="col-xs-auto col-sm-12 col-md-12 col-lg-12">
                                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click" />
                                                        </div>
                                                    </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-lg-12 ">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvReportes" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" DataKeyNames="ID_SOLICITUD" EmptyDataText="No se encontraron registros." DataSourceID="sdsReportes" AllowPaging="True" Font-Size="X-Small" PageSize="30">
                                                                
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID_SOLICITUD" HeaderText="Solicitud" SortExpression="ID_SOLICITUD" />
                                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" />
                                                                    <asp:BoundField DataField="RUT_SOLICITANTE" HeaderText="Rut" SortExpression="RUT_SOLICITANTE" />
                                                                    <asp:BoundField DataField="USUARIO" HeaderText="Guardia" SortExpression="USUARIO" />
                                                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" />
                                                                    <asp:BoundField DataField="HORA" HeaderText="Hora" SortExpression="HORA" />
                                                                    <asp:BoundField DataField="MARCAJE" HeaderText="Marcaje" SortExpression="MARCAJE" />
                                                                </Columns>

                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                        <asp:SqlDataSource ID="sdsReportes" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_reporte_guardias" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="FechaDesde" Type="String"></asp:Parameter>
                                <asp:Parameter Name="FechaHasta" Type="String"></asp:Parameter>
                                <asp:Parameter Name="Nombre" Type="String"></asp:Parameter>
                                <asp:Parameter Name="Rut" Type="String"></asp:Parameter>
                                <asp:Parameter Name="Guardia" Type="String"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="sdsGuardias" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_guardias" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                    </div>
                </div>
            </div>
        </div>
    </form>

</asp:Content>
