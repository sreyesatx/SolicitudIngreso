<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="IngresoVehicular.aspx.cs" Inherits="SistemaSolicitudIngreso.Administracion.Historial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.css">
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <script type="text/javascript">

    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
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

        .modalPopup {
            background-color: #FFFFFF;
            width: 350px;
            border: 3px solid #0DA9D0;
            border-radius: 12px;
            padding: 0;
        }

        .centrado img {
            height: 93px;
            width: 128px;
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

        <div id="mainContainer" class="container">
            <asp:UpdatePanel ID="upAuditoria" runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="upAuditoria" EnableViewState="False">
                        <ProgressTemplate>
                            <div class="modal2">
                                <div class="centrado">
                                    <img src="<%= ResolveUrl("~/img/AvionCargando.gif") %>" /><br />
                                    Un momento...
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div class="shadowBox">
                        <div class="page-container">
                            <div class="container">
                                <div class="jumbotron">
                                    <h1>Ingreso Vehicular</h1>
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
                                        <label for="ddlTipoVisita">Fecha de visita</label>
                                        <asp:TextBox ID="txtBuscaFechaVisita" runat="server" class="form-control" placeholder="Buscar por fecha" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBuscaFechaVisita" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                    </div>
                                    <div class="col-xs-auto col-sm-6 col-md-12 col-lg-3">
                                        <label for="ddlPatente">Patente</label>
                                        <asp:TextBox ID="txtPatente" runat="server" class="form-control" AutoCompleteType="Disabled" placeholder="Buscar por patente" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row" style="text-align: left">
                                    <div class="col-xs-auto col-sm-1 col-md-2 col-lg-1">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click" OnClientClick="javascript:showWaitGrabar();" />
                                    </div>
                                    <div class="col-xs-auto col-sm-2 col-md-2 col-lg-2">
                                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn-success" OnClick="btnLimpiar_Click" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12 ">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvSolicitud" runat="server" CssClass="table table-striped table-bordered table-hover" PageSize="50" HeaderStyle-BackColor="#d5dbe3" HeaderStyle-ForeColor="#636363"
                                                AutoGenerateColumns="False" EmptyDataText="No se encontraron registros." Style="font-size: 11px;" AllowPaging="False" 
                                                OnPageIndexChanging="gvSolicitud_PageIndexChanging" OnRowDataBound="gvSolicitud_RowDataBound" OnDataBound = "OnDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="srv_patente" HeaderText="Patente" SortExpression="srv_patente" />
                                                    <asp:BoundField DataField="Hora_Lectura" HeaderText="Fecha Lectura" SortExpression="Hora_Lectura" />
                                                    <asp:BoundField DataField="Hora_Salida" HeaderText="Fecha Salida" SortExpression="Hora_Salida" />
                                                    <asp:BoundField DataField="Courier" HeaderText="Courier" SortExpression="Courier" />
                                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                                    <asp:BoundField DataField="Fecha_Vigencia" HeaderText="Fecha Vigencia" SortExpression="Fecha_Vigencia" />
                                                    <asp:BoundField DataField="Estacionamiento" HeaderText="Estacionamiento" SortExpression="Estacionamiento" />
                                                    <asp:BoundField DataField="id_solicitud" HeaderText="ID Solcitud" SortExpression="id_solicitud" />   
                                                    <asp:BoundField DataField="Estado" HeaderText="Estado Solicitud" SortExpression="Estado" />                                                   
                                                    <asp:BoundField DataField="empresa" HeaderText="Courier Solicitud" SortExpression="empresa" />  
                                                    <asp:BoundField DataField="nombre_solicitante" HeaderText="Nombre Solicitante" SortExpression="nombre_solicitante" />
                                                    <asp:BoundField DataField="rut_solicitante" HeaderText="Rut Solicitante" SortExpression="rut_solicitante" />
                                                    <asp:BoundField DataField="correo_solicitante" HeaderText="Correo Solicitante" SortExpression="correo_solicitante" />                                                                                                  
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>                               
                            </div>
                        </div>
                    </div>

                    <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
                    <cc1:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>
                   
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnBuscar" />
                     <asp:PostBackTrigger ControlID="btnLimpiar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</asp:Content>
