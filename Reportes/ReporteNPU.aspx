<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="ReporteNPU.aspx.cs" Inherits="SistemaSolicitudIngreso.Reportes.ReporteNPU" EnableEventValidation = "false" %>

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
                            <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportar" />
                            </Triggers>
                            <ContentTemplate>
                                <div id="accordion">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4>
                                                <a class="card-link" data-toggle="collapse" href="#collapse1">Reporte nuevo pudahuel
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapse1" class="collapse show" data-parent="#accordion">
                                            <div class="card-body">
                                                <div class="control-group row">
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                                        <asp:RadioButtonList ID="rblTipoIngreso" runat="server" RepeatLayout="Table" required RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem Value="1">Peatonal</asp:ListItem>
                                                            <asp:ListItem Value="2">Vehicular</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                                        <asp:RadioButtonList ID="rblTop25" runat="server" RepeatLayout="Table" required RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem Value="1">Top 25</asp:ListItem>
                                                            <asp:ListItem Value="0" Selected="True">Todos</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                                        <asp:TextBox ID="txtBuscaFechaDesde" runat="server" class="form-control" placeholder="Fecha desde" AutoCompleteType="Disabled" MaxLength="10" required></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBuscaFechaDesde" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                                        <asp:TextBox ID="txtBuscaFechaHasta" runat="server" class="form-control" placeholder="Fecha hasta" AutoCompleteType="Disabled" MaxLength="10" required></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtBuscaFechaHasta" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-4">
                                                        <asp:DropDownList ID="ddlBuscaEmpresas" runat="server" CssClass="form-control" data-style="btn-info"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-4">
                                                        <asp:TextBox ID="txtBuscaNombre" runat="server" class="form-control" placeholder="Buscar nombre" AutoCompleteType="Disabled" MaxLength="100"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-4">
                                                        <asp:TextBox ID="txtBuscaRut" runat="server" class="form-control" placeholder="Buscar rut" AutoCompleteType="Disabled" MaxLength="100"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                </div>                                                
                                                <div class="row" style="text-align: center">
                                                        <div class="col-xs-auto col-sm-1 col-md-2 col-lg-1">
                                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click" />
                                                        </div>
                                                        <div class="col-xs-auto col-sm-1 col-md-2 col-lg-1">
                                                            <asp:Button ID="btnExportar" runat="server" Text="Exportar Excel" class="btn btn-primary" OnClick="ExportToExcel"   />
                                                        </div>
                                                    </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-lg-12 ">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvReportes" runat="server" 
                                                                CssClass="table table-striped table-bordered table-hover" 
                                                                AutoGenerateColumns="False" DataKeyNames="ID_SOLICITUD" 
                                                                EmptyDataText="No se encontraron registros." 
                                                                AllowPaging="True" Font-Size="X-Small" PageSize="30">
                                                                
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID_SOLICITUD" HeaderText="Solicitud" SortExpression="ID_SOLICITUD" />
                                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" />
                                                                    <asp:BoundField DataField="RUT_SOLICITANTE" HeaderText="Rut" SortExpression="RUT_SOLICITANTE" />
                                                                    <asp:BoundField DataField="PATENTE" HeaderText="Patente" SortExpression="PATENTE" />
                                                                    <asp:BoundField DataField="FECHA VISITA" HeaderText="Fecha visita" SortExpression="FECHA VISITA" />
                                                                    <asp:BoundField DataField="FECHA INGRESO" HeaderText="Fecha ingreso" SortExpression="FECHA INGRESO" />
                                                                    <asp:BoundField DataField="HORA INGRESO" HeaderText="Hora ingreso" SortExpression="HORA INGRESO" />
                                                                    <asp:BoundField DataField="FECHA SALIDA" HeaderText="Fecha salida" SortExpression="FECHA SALIDA" />
                                                                    <asp:BoundField DataField="HORA SALIDA" HeaderText="Hora salida" SortExpression="HORA SALIDA" />
                                                                    <asp:BoundField DataField="TIEMPO PERMANENCIA" HeaderText="Tiempo permanencia" SortExpression="TIEMPO PERMANENCIA" />
                                                                    <asp:BoundField DataField="EMPRESA_VISITA" HeaderText="Empresa a visitar" SortExpression="EMPRESA_VISITA" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>

                                                        <div class="table-responsive" visible ="false">
                                                            <asp:GridView ID="dvexcel" runat="server" 
                                                                CssClass="table table-striped table-bordered table-hover" 
                                                                AutoGenerateColumns="False" DataKeyNames="ID_SOLICITUD" 
                                                                EmptyDataText="No se encontraron registros." 
                                                                AllowPaging="True" Font-Size="X-Small" 
                                                                PageSize="30" Visible ="false">
                                                                
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID_SOLICITUD" HeaderText="Solicitud" SortExpression="ID_SOLICITUD" />
                                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" />
                                                                    <asp:BoundField DataField="RUT_SOLICITANTE" HeaderText="Rut" SortExpression="RUT_SOLICITANTE" />
                                                                    <asp:BoundField DataField="PATENTE" HeaderText="Patente" SortExpression="PATENTE" />
                                                                    <asp:BoundField DataField="FECHA VISITA" HeaderText="Fecha visita" SortExpression="FECHA VISITA" />
                                                                    <asp:BoundField DataField="DIA_VISITA" HeaderText="Fecha visita" SortExpression="DIA VISITA" />
                                                                    <asp:BoundField DataField="MES_VISITA" HeaderText="Fecha visita" SortExpression="MES VISITA" />
                                                                    <asp:BoundField DataField="ANO_VISITA" HeaderText="Fecha visita" SortExpression="AÑO VISITA" />
                                                                    <asp:BoundField DataField="HORA_VISITA" HeaderText="Fecha visita" SortExpression="HORA VISITA" />

                                                                    <asp:BoundField DataField="FECHA INGRESO" HeaderText="Fecha ingreso" SortExpression="FECHA INGRESO" />
                                                                    <asp:BoundField DataField="DIA_INGRESO" HeaderText="Fecha visita" SortExpression="DIA INGRESO" />
                                                                    <asp:BoundField DataField="MES_INGRESO" HeaderText="Fecha visita" SortExpression="MES INGRESO" />
                                                                    <asp:BoundField DataField="ANO_INGRESO" HeaderText="Fecha visita" SortExpression="AÑO INGRESO" />
                                                                    <asp:BoundField DataField="HORA_INGRESO" HeaderText="Fecha visita" SortExpression="HORA INGRESO" />
                                                                    
                                                                    <asp:BoundField DataField="FECHA SALIDA" HeaderText="Fecha salida" SortExpression="FECHA SALIDA" />
                                                                    <asp:BoundField DataField="DIA_SALIDA" HeaderText="Fecha visita" SortExpression="DIA SALIDA" />
                                                                    <asp:BoundField DataField="MES_SALIDA" HeaderText="Fecha visita" SortExpression="MES SALIDA" />
                                                                    <asp:BoundField DataField="ANO_SALIDA" HeaderText="Fecha visita" SortExpression="AÑO SALIDA" />
                                                                    <asp:BoundField DataField="HORA_SALIDA" HeaderText="Fecha visita" SortExpression="HORA SALIDA" />                                                                    
                                                                    <asp:BoundField DataField="TIEMPO PERMANENCIA" HeaderText="Tiempo permanencia" SortExpression="TIEMPO PERMANENCIA" />
                                                                    <asp:BoundField DataField="EMPRESA_VISITA" HeaderText="Empresa a visitar" SortExpression="EMPRESA_VISITA" />
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
                        <asp:SqlDataSource ID="sdsReportes" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_reporte_npu" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="Empresa" Type="String"></asp:Parameter>
                                <asp:Parameter Name="FechaDesde" Type="String"></asp:Parameter>
                                <asp:Parameter Name="FechaHasta" Type="String"></asp:Parameter>
                                <asp:Parameter Name="Nombre" Type="String"></asp:Parameter>
                                <asp:Parameter Name="Rut" Type="String"></asp:Parameter>
                                <asp:Parameter Name="TipoIngreso" Type="String"></asp:Parameter>
                                <asp:Parameter Name="Top25" Type="String"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsEmpresas" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_empresas" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>
    </form>    
</asp:Content>
