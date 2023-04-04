<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="Administracion.aspx.cs" Inherits="SistemaSolicitudIngreso.Administracion.Administracion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.css">
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <script type="text/javascript">

        function validaDatos() {

            var valida = new String("");
            valida.text = "";

            var ddlEstacionamiento = document.getElementById("ContentPlaceHolder1_ddlEstacionamiento");
            var ddlEstado = document.getElementById("ContentPlaceHolder1_ddlEstado").selectedIndex;
            var lblMensajes = document.getElementById("ContentPlaceHolder1_lblMensajes");
            var CheckBoxRecurrente = document.getElementById('ContentPlaceHolder1_CheckBoxRecurrente');
            var txtVigenciaHasta = document.getElementById("ContentPlaceHolder1_txtVigenciaHasta").value;

            lblMensajes.innerText = "";

            if (ddlEstacionamiento.disabled == false) {

                if (ddlEstado == 6) {

                    if (ddlEstacionamiento.selectedIndex == 0) {
                        valida.text += "* Seleccione estacionamiento.\n";
                    }
                }
            }

            if (CheckBoxRecurrente.checked == true) {

                if (txtVigenciaHasta == "") {

                    valida.text += "* Ingrese fecha vigencia.\n";
                }
            }

            if (valida.text == "")
            {

                return true;
            }
            else
            {

                lblMensajes.innerText = valida.text;
                return false;
            }
        }

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


        .modal2 {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 2px;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.70;
            -moz-opacity: 0.8;
            left: 0px;
        }

        .centrado {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .centrado img {
                height: 93px;
                width: 128px;
            }

        #Tabla_Superior {
            border-radius: 20px 0;
            border: 2px solid #3399FF;
        }
    </style>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
        <br />
        <div id="mainContainer" class="container">

            <asp:UpdatePanel ID="upAdministracion" runat="server">
                <ContentTemplate>

                    <div class="shadowBox">
                        <div class="page-container">
                            <div class="container">
                                <div class="jumbotron">
                                    <h1>Administración de solicitudes de ingreso</h1>
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
                                        <asp:TextBox ID="txtBuscaRut" runat="server" class="form-control" AutoCompleteType="Disabled" placeholder="Buscar por rut" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row" style="text-align: center">
                                    <div class="col-md-12 mx-auto">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click" formnovalidate="formnovalidate" />
                                    </div>
                                </div>
                                <br />
                                <br />
                                <div class="row" style="text-align: left">
                                    <div class="col-md-12 mx-auto">
                                        <asp:Button ID="btnAsignar" runat="server" Text="Asignar Estado" class="btn btn-primary" OnClick="btnAsignar_Click" formnovalidate="formnovalidate" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12 ">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvSolicitud" runat="server" CssClass="table table-striped table-bordered table-hover" 
                                                AutoGenerateColumns="False" DataKeyNames="id_solicitud" EmptyDataText="No se encontraron solicitudes." 
                                                DataSourceID="sdsSolicitudes" Style="font-size: 11px;" AllowPaging="True">
											<RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Seleccionar">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkEnviar" runat="server" />                                                    
														</ItemTemplate>
                                                    </asp:TemplateField>												
                                                    <asp:TemplateField HeaderText="Solicitud">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkSolicitud" runat="server" OnClick="btnOpenPopUp_Click" Text='<%# Eval("id_solicitud") %>' CommandArgument='<%# Eval("id_solicitud")%>' ToolTip="Administrar"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="descripcion_estado" HeaderText="Estado" SortExpression="descripcion_estado" />
                                                    <asp:BoundField DataField="nombre_empresa" HeaderText="Nombre empresa" SortExpression="nombre_empresa" />
                                                    <asp:BoundField DataField="descripcion_tipo_visita" HeaderText="Tipo visita" SortExpression="descripcion_tipo_visita" />
                                                    <asp:BoundField DataField="otro_indicar" HeaderText="Visita técnica" SortExpression="otro_indicar" />
                                                    <asp:BoundField DataField="fecha_visita" HeaderText="Fecha visita" SortExpression="fecha_visita" />
                                                    <asp:BoundField DataField="motivo_ingreso" HeaderText="Motivo ingreso" SortExpression="motivo_ingreso" />
                                                    <asp:BoundField DataField="descripcion_tipo_ingreso" HeaderText="Tipo ingreso" SortExpression="descripcion_tipo_ingreso" />
                                                    <asp:BoundField DataField="fecha_solicitud" HeaderText="Fecha solicitud" SortExpression="fecha_solicitud" />
                                                    <asp:BoundField DataField="nombre_solicitante" HeaderText="Nombre solicitante" SortExpression="nombre_solicitante" />
                                                    <asp:BoundField DataField="rut_solicitante" HeaderText="Rut solicitante" SortExpression="rut_solicitante" />
                                                    <asp:BoundField DataField="patente_vehiculo" HeaderText="Patente vehiculo" SortExpression="patente_vehiculo" />
                                                    <asp:BoundField DataField="correo_solicitante" HeaderText="Correo solicitante" SortExpression="correo_solicitante" />
                                                    <asp:BoundField DataField="empresa_solicitante" HeaderText="Empresa solicitante" SortExpression="empresa_solicitante" />
                                                    <asp:BoundField DataField="cargo_solicitante" HeaderText="Cargo solicitante" SortExpression="cargo_solicitante" />
                                                    <asp:TemplateField HeaderText="Listado acompañantes">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkAcompanantes" runat="server" OnClick="buscarColegas_Click" Text='<%# Eval("listado_acompanantes") %>' CommandArgument='<%# Eval("id_solicitud")%>' ToolTip="Ver listado"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="observaciones" HeaderText="Observaciones" SortExpression="observaciones" />
                                                    <asp:BoundField DataField="estacionamiento" HeaderText="Estacionamiento" SortExpression="estacionamiento" />
                                                    <asp:BoundField DataField="hora_ingreso" HeaderText="Hora ingreso" SortExpression="hora_ingreso" />
                                                    <asp:BoundField DataField="hora_salida" HeaderText="Hora salida" SortExpression="hora_salida" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <br />
                                <asp:SqlDataSource ID="sdsSolicitudes" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_solicitud" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Name="Empresa" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="FechaVisita" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="NombreSolicitante" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="RutSolicitante" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="Solicitud" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="Filtro" Type="String"></asp:Parameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="sdsSolicitud" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_solicitud" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Name="Empresa" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="FechaVisita" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="NombreSolicitante" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="RutSolicitante" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="Solicitud" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="Filtro" Type="String"></asp:Parameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="sdsEstados" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_estados_admin" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                <asp:SqlDataSource ID="sdsUpdateSolicitud" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_update_solicitud_admin" SelectCommandType="StoredProcedure" UpdateCommand="sp_update_solicitud_admin_2" UpdateCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                                        <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                                        <asp:Parameter Name="fecha_visita" Type="DateTime"></asp:Parameter>
                                        <asp:Parameter Name="estacionamiento" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="observacion_administrador" Type="String"></asp:Parameter>
                                    </SelectParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                                        <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                                        <asp:Parameter Name="vigencia_hasta" Type="DateTime"></asp:Parameter>
                                        <asp:Parameter Name="estacionamiento" Type="String"></asp:Parameter>
                                        <asp:Parameter Name="observacion_administrador" Type="String"></asp:Parameter>
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="sdsEmpresas" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_empresas" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                <asp:SqlDataSource ID="sdsColegas" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_acompanantes" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Name="Solicitud" Type="Int32"></asp:Parameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </div>
					
					<asp:Label ID="lblHidden_1" runat="server" Text=""></asp:Label>
                    <cc1:ModalPopupExtender ID="mpePopUp_1" runat="server" TargetControlID="lblHidden_1" PopupControlID="pnlPopup_1" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>
                    <asp:Panel ID="pnlPopup_1" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                            <h4 class="modal-title">Cambiar estado Masivo
                                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                            </h4>
                        </div>
                        <div class="body" >
                            <br />
								<div class="control-group row">
                                 <div class="col-xs-auto col-sm-6 col-md-6 col-lg-6">                                    
                                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control" data-style="btn-info" Visible="false">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-xs-auto col-sm-6 col-md-6 col-lg-6">
                                        <label for="ddlEstado_1">Cambiar estado: </label>
                                        <asp:DropDownList ID="ddlEstado_1" runat="server" CssClass="form-control" required data-style="btn-info">
                                        </asp:DropDownList>     
                                     <br />                     
                                </div>
       
                                <div class="col-xs-auto col-sm-12 col-md-12 col-lg-12">
                                    <asp:CheckBox ID="CheckBoxEnvioCorreo" runat="server" Text="Enviar correo" />
                                    <asp:TextBox ID="txtMensaje_1" runat="server" class="form-control" placeholder="Mensaje correo" AutoCompleteType="Disabled" MaxLength="500"></asp:TextBox>                             
                                </div>
							</div>
                        </div>
                        <br />
                        <div class="footer">
                            <asp:Button ID="btnEnviar_1" runat="server" Text="Enviar" class="btn btn-primary" OnClick="btnEnviar_1_Click" CausesValidation="false" UseSubmitBehavior="False" />
                            <asp:Button ID="btnCancelar_1" runat="server" Text="Cancelar" class="btn btn-secondary" OnClick="CerrarSolicitud_1" formnovalidate="formnovalidate" />
                        </div>

                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="upAdministracion" EnableViewState="False">
                            <ProgressTemplate>
                                <div class="modal2">
                                    <div class="centrado">
                                        <img src="<%= ResolveUrl("~/img/AvionCargando.gif") %>" /><br />
                                        Un momento...
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </asp:Panel>					

                    <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
                    <cc1:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

                    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                            <h4 class="modal-title">Solicitud:
                                <asp:Label ID="lblSolicitud" runat="server" Text=""></asp:Label></h4>
                        </div>
                        <div class="body">
                            <div class="control-group row">
                                <div class="col-xs-auto col-sm-6 col-md-6 col-lg-6">
                                    <label for="ddlEstacionamiento">Estacionamiento: </label>
                                    <asp:DropDownList ID="ddlEstacionamiento" runat="server" CssClass="form-control" data-style="btn-info">
                                        <asp:ListItem>--</asp:ListItem>
                                        <asp:ListItem>Aduana</asp:ListItem>
                                        <asp:ListItem>Atrex</asp:ListItem>
                                        <asp:ListItem>Dhl</asp:ListItem>
                                        <asp:ListItem>Discapacitados</asp:ListItem>
                                        <asp:ListItem>Fedex</asp:ListItem>
                                        <asp:ListItem>Isp</asp:ListItem>
                                        <asp:ListItem>Sag</asp:ListItem>
                                        <asp:ListItem>Sernapesca</asp:ListItem>
                                        <asp:ListItem>Ups</asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>

                                <div class="col-xs-auto col-sm-6 col-md-6 col-lg-6">
                                    <label for="ddlEstado">Cambiar estado: </label>
                                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control" required data-style="btn-info">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-xs-auto col-sm-12 col-md-12 col-lg-12">

                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Enviar correo" />
                                    <asp:TextBox ID="txtMensajeCorreo" runat="server" class="form-control" placeholder="Mensaje correo" AutoCompleteType="Disabled" MaxLength="500"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-xs-auto col-sm-6 col-md-6 col-lg-6">
                                    <asp:CheckBox ID="CheckBoxRecurrente" runat="server" Text="Recurrente" />
                                    </div>
                                <div class="col-xs-auto col-sm-6 col-md-6 col-lg-6">
                                    <label for="txtFecha">Vigencia hasta:</label>
                                    <asp:TextBox ID="txtVigenciaHasta" runat="server" CssClass="form-control" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalVigenciaHasta" runat="server" TargetControlID="txtVigenciaHasta" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                </div>
                                <div class="col-xs-auto col-sm-12 col-md-12 col-lg-12">
                                    <asp:Label ID="lblMensajes" runat="server" Text="" class="text-danger"></asp:Label>
                                </div>
                            </div>

                        </div>
                        <div class="footer">
                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" class="btn btn-primary" OnClick="Enviar" CausesValidation="false" UseSubmitBehavior="False"  />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-secondary" OnClick="CerrarSolicitud" formnovalidate="formnovalidate" />
                        </div>

                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="upAdministracion" EnableViewState="False">
                            <ProgressTemplate>
                                <div class="modal2">
                                    <div class="centrado">
                                        <img src="<%= ResolveUrl("~/img/AvionCargando.gif") %>" /><br />
                                        Un momento...
                                    </div>
                                </div>

                            </ProgressTemplate>
                        </asp:UpdateProgress>

                    </asp:Panel>




                    <asp:Label ID="lblColegasPopUp" runat="server" Text=""></asp:Label>
                    <cc1:ModalPopupExtender ID="colegasPopUp" runat="server" TargetControlID="lblColegasPopUp" PopupControlID="PanelColegasPopUp" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

                    <asp:Panel ID="PanelColegasPopUp" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                            <h4 class="modal-title">Listado de acompañantes</h4>
                        </div>
                        <div class="body">
                            <asp:GridView ID="gvColegas" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" DataKeyNames="id_acompanante" DataSourceID="sdsColegas" EmptyDataText="No se encontraron datos." Font-Size="Small">
                                <Columns>
                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre"></asp:BoundField>
                                    <asp:BoundField DataField="rut" HeaderText="Rut" SortExpression="rut"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="footer">
                            <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" class="btn btn-secondary" OnClick="CerrarColegas" formnovalidate="formnovalidate" />
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</asp:Content>
