<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFIle="Vehiculos.aspx.cs" Inherits="SistemaSolicitudIngreso.AccesoVehiculos.Vehiculos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 350px;
            border: 3px solid #0DA9D0;
            border-radius: 12px;
            padding: 0;
        }

            .modalPopup .header {
                display: -ms-flexbox;
                -ms-flex-align: start;
                align-items: flex-start;
                -ms-flex-pack: justify;
                justify-content: space-between;
                padding: 1rem;
                border-bottom: 1px solid #e9ecef;
                border-top-left-radius: 0.3rem;
                border-top-right-radius: 0.3rem;
            }

            .modalPopup .body {
                position: relative;
                -ms-flex: 1 1 auto;
                flex: 1 1 auto;
                padding: 1rem;
            }

            .modalPopup .footer {
                display: -ms-flexbox;
                -ms-flex-align: center;
                align-items: center;
                -ms-flex-pack: end;
                justify-content: flex-end;
                padding: 1rem;
                border-top: 1px solid #e9ecef;
            }

            .modalPopup .yes, .modalPopup .no {
                height: 23px;
                color: White;
                line-height: 23px;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
                border-radius: 4px;
            }

            .modalPopup .yes {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
            }

            .modalPopup .no {
                background-color: #9F9F9F;
                border: 1px solid #5C5C5C;
            }
    </style>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
        <br />
        <div id="mainContainer" class="container">
            <div class="shadowBox">
                <div class="page-container">
                    <div class="container">
                        <div class="jumbotron">
                            <h1>Solicitudes de ingreso vehicular</h1>
                        </div>

                        <div>
                            <div>
                                <h3>
                                    <a>Filtros de búsqueda</a></h3>
                            </div>
                        </div>
                        <br />
                        <div class="row" style="text-align: left">
                            <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                <label for="ddlEmpresas">Empresa a visitar</label>
                                <asp:DropDownList ID="ddlBuscaEmpresas" runat="server" CssClass="form-control" data-style="btn-info"></asp:DropDownList>
                            </div>
                            <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                <label for="ddlTipoVisita">Fecha de visita</label>
                                <asp:TextBox ID="txtBuscaFechaVisita" runat="server" class="form-control" placeholder="Buscar por fecha" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBuscaFechaVisita" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                            </div>

                            <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                <label for="ddlEmpresas">Nombre solicitante</label>
                                <asp:TextBox ID="txtBuscaNombre" runat="server" class="form-control" placeholder="Buscar por nombre" AutoCompleteType="Disabled" MaxLength="100"></asp:TextBox>
                            </div>
                            <div class="col-xs-auto col-sm-6 col-md-12 col-lg-3">
                                <label for="ddlTipoVisita">Rut solicitante</label>
                                <asp:TextBox ID="txtBuscaRut" runat="server" class="form-control" AutoCompleteType="Disabled" placeholder="Buscar por rut" MaxLength="20" autofocus="autofocus"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row" style="text-align: center">
                            <div class="col-md-12 mx-auto">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-lg-12 ">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvSolicitud" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" DataKeyNames="id_solicitud" EmptyDataText="No se encontraron solicitudes." DataSourceID="sdsSolicitudes" Style="font-size: 11px;" AllowPaging="True" OnRowDataBound="gvSolicitud_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="id_solicitud" HeaderText="Solicitud" SortExpression="id_solicitud" />
                                            <asp:BoundField DataField="nombre_empresa" HeaderText="Nombre empresa" SortExpression="nombre_empresa" />
                                            <asp:BoundField DataField="descripcion_tipo_visita" HeaderText="Tipo visita" SortExpression="descripcion_tipo_visita" />
                                            <asp:BoundField DataField="fecha_visita" HeaderText="Fecha visita" SortExpression="fecha_visita"/>
                                            <asp:BoundField DataField="motivo_ingreso" HeaderText="Motivo ingreso" SortExpression="motivo_ingreso"/>
                                            <asp:BoundField DataField="nombre_solicitante" HeaderText="Nombre solicitante" SortExpression="nombre_solicitante" />
                                            <asp:BoundField DataField="rut_solicitante" HeaderText="Rut solicitante" SortExpression="rut_solicitante" />
                                            <asp:BoundField DataField="patente_vehiculo" HeaderText="Patente vehiculo" SortExpression="patente_vehiculo" />
                                            <asp:BoundField DataField="empresa_solicitante" HeaderText="Empresa solicitante" SortExpression="empresa_solicitante" />
                                            <asp:BoundField DataField="estacionamiento" HeaderText="Estacionamiento" SortExpression="estacionamiento" />
                                            <asp:TemplateField HeaderText="Registrar">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnEntrada" runat="server" Text="Entrada" Visible="False" OnClick="entrada" CommandArgument='<%# Eval("id_solicitud")%>' />
                                                    <asp:Button ID="btnSalida" runat="server" Text="Salida" Visible="False" OnClick="salida" CommandArgument='<%# Eval("id_solicitud")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="hora_ingreso" HeaderText="Hora ingreso" SortExpression="hora_ingreso" />
                                            <asp:BoundField DataField="hora_salida" HeaderText="Hora salida" SortExpression="hora_salida" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <asp:SqlDataSource ID="sdsSolicitudes" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_solicitud_vehiculo" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="Empresa" Type="String"></asp:Parameter>
                                <asp:Parameter Name="FechaVisita" Type="String"></asp:Parameter>
                                <asp:Parameter Name="NombreSolicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="RutSolicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="Solicitud" Type="String"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsEntrada" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_update_solicitud_entrada" SelectCommandType="StoredProcedure" UpdateCommand="sp_update_solicitud_entrada" UpdateCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="HoraIngreso" Type="DateTime"></asp:Parameter>
                            </SelectParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="HoraIngreso" Type="DateTime"></asp:Parameter>
                            </UpdateParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsSalida" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_update_solicitud_salida" SelectCommandType="StoredProcedure" UpdateCommand="sp_update_solicitud_salida" UpdateCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="HoraSalida" Type="DateTime"></asp:Parameter>
                            </SelectParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="HoraSalida" Type="DateTime"></asp:Parameter>
                            </UpdateParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsEmpresas" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_empresas" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </div>
                </div>
            </div>

        </div>
    </form>
</asp:Content>
