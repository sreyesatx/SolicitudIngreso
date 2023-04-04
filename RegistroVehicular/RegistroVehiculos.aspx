<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Home.Master" CodeFile="RegistroVehiculos.aspx.cs" Inherits="SistemaSolicitudIngreso.RV.RegistroVehiculos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

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
<script type="text/javascript">   

        function Confirm() {
            if (confirm("¿Seguro que desea quitar vehículo?")) {
                document.getElementById('<%=hiddenval.ClientID%>').value = "Yes";
            } else {
                document.getElementById('<%=hiddenval.ClientID%>').value = "No";
            }
        }
    </script>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
        <br />
        <div id="mainContainer" class="container">
            <asp:UpdatePanel ID="upContacto" runat="server">
                <ContentTemplate>
                    <div class="shadowBox">
                        <div class="page-container">
                            <div class="container">
                                <div class="jumbotron">
                                    <h1>Mantenedores</h1>
                                </div>
                                <div id="accordion">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4>
                                                <a class="card-link" data-toggle="collapse" href="#collapse1">Registro Vehícular
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapse1" class="collapse show" data-parent="#accordion">
                                            <div class="card-body">
                                                <div class="control-group row">
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-4">
                                                        Empresa<asp:DropDownList ID="ddlempresas" runat="server" CssClass="form-control" data-style="btn-info" AutoPostBack="true" DataSourceID="dsempresas" DataTextField="nombre_empresa" DataValueField="sigla" OnDataBound="ddlempresas_DataBound">
                                                        </asp:DropDownList>
                                                        <asp:SqlDataSource ID="dsempresas" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>" SelectCommand="sp_lista_empresas_sigla" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                        Patente  
                                                        <asp:TextBox ID="txtpatente" runat="server" class="form-control" placeholder="" AutoCompleteType="Disabled" MaxLength="6" ></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                        Vigencia  
                                                        <asp:TextBox ID="txtfechavigencia" runat="server" class="form-control" placeholder="" AutoCompleteType="Disabled" MaxLength="10" ></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfechavigencia" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div class="control-group row">
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-4">
                                                        Nombre 
                                                        <asp:TextBox ID="txtdescripcion" runat="server" class="form-control" placeholder="" AutoCompleteType="Disabled" MaxLength="100" ></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-4">
                                                        Vehículo 
                                                        <asp:DropDownList ID="ddltipovehiculo" runat="server" CssClass="form-control" data-style="btn-info" AutoPostBack="true" >                                                            
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-2 col-lg-2">
                                                        <asp:SqlDataSource ID="dsMantenedor" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>" SelectCommand="sp_listar_vehiculos" SelectCommandType="StoredProcedure" DeleteCommandType="StoredProcedure" InsertCommandType="StoredProcedure" InsertCommand="sp_insert_vehiculos" DeleteCommand="sp_elimina_vehicular">
                                                            <DeleteParameters>
                                                                <asp:Parameter Name="sre_id" Type="Int32" />
                                                                <asp:SessionParameter Name="sre_user_creac" SessionField="nombre_usuario" Type="String" />
                                                            </DeleteParameters>
                                                            <InsertParameters>
                                                                <asp:ControlParameter ControlID="txtpatente" Name="sre_patente" PropertyName="Text" Type="String" />
                                                                <asp:ControlParameter ControlID="txtdescripcion" Name="sre_nombre" PropertyName="Text" Type="String" />
                                                                <asp:ControlParameter ControlID="txtfechavigencia" Name="sre_fechavigencia" PropertyName="Text" Type="DateTime" />
                                                                <asp:ControlParameter ControlID="ddlempresas" Name="sre_empresa" PropertyName="SelectedValue" Type="String" />
                                                                <asp:ControlParameter ControlID="ddltipovehiculo" Name="sre_estacionamiento" PropertyName="SelectedValue" Type="Int16" />
                                                                <asp:SessionParameter Name="sre_user_creac" SessionField="nombre_usuario" Type="String" />
                                                            </InsertParameters>
                                                            <SelectParameters>
                                                                <asp:Parameter DefaultValue="1" Name="tiporeporte" Type="Int32" />
                                                                <asp:ControlParameter ControlID="ddlempresas" DefaultValue="0" Name="empresa" PropertyName="SelectedValue" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div class="control-group row">
                                                    <div class="col-xs-auto col-sm-6 col-md-2 col-lg-2">
                                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="form-control btn btn-success" OnClick="btnBuscar_Click" />                                                        
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-2 col-lg-2">
                                                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="form-control btn btn-primary" OnClick="btnAgregar_Click" />
                                                        <asp:HiddenField ID="hiddenval" runat="server" />	
                                                    </div>
                                                </div>

                                                <br />
                                                <div class="row">
                                                    <div class="col-lg-12 ">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvContactos" runat="server"
                                                                CssClass="table table-striped table-bordered table-hover"
                                                                AutoGenerateColumns="False" DataKeyNames="sre_id"
                                                                EmptyDataText="No se encontraron funcionarios."
                                                                AllowPaging="True"
                                                                Style="font-size: 11px;"
                                                                OnPageIndexChanging="gvContactos_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-Width="30px" >
                                                                        <ItemTemplate>                                                 
                                                                            <asp:LinkButton  ID="LinkEliminar" runat="server" class="btn btn-outline-danger btn-sm" Text="Eliminar" OnClick="onDelete" OnClientClick="Confirm()" ></asp:LinkButton>                                          
                                                                        </ItemTemplate>   
                                                                        <HeaderStyle Width="20" />
                                                                        <ItemStyle Width="20" />
                                                                    </asp:TemplateField>                                  
                                                                    <asp:TemplateField ItemStyle-Width="30px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="linkSolicitud" runat="server" class="btn btn-outline-warning btn-sm" OnClick="btnOpenPopUp_Click" Text="Modificar" CommandArgument='<%# Eval("sre_id")%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="20" />
                                                                        <ItemStyle Width="20" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="sre_id" HeaderText="id" SortExpression="sre_id" />
                                                                    <asp:BoundField DataField="sre_patente" HeaderText="Patente" SortExpression="sre_patente" />
                                                                    <asp:BoundField DataField="sre_nombre" HeaderText="Nombre" SortExpression="sre_nombre" />
                                                                    <asp:BoundField DataField="sre_fechavigencia" HeaderText="Vigencia" SortExpression="sre_fechavigencia" />
                                                                    <asp:BoundField DataField="sre_empresa" HeaderText="Empresa" SortExpression="sre_empresa" />
                                                                    <asp:BoundField DataField="ste_descripcion" HeaderText="Vehículo" SortExpression="ste_descripcion" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
                    <cc1:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

                    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                            <h4 class="modal-title">Actualizar Fecha Vigencia
                                <asp:Label ID="lblSolicitud" runat="server" Text=""></asp:Label></h4>
                        </div>
                        <div class="body">
                            <div class="control-group row">
                                <div class="col-xs-auto col-sm-5 col-md-5 col-lg-5">
                                    <label for="txtIDRegistro">ID Registro:</label>
                                    <asp:TextBox ID="txtIDRegistro" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    <br />
                                </div>
                            </div>
                            <div class="control-group row">
                                <div class="col-xs-auto col-sm-12 col-md-12 col-lg-12">
                                    <label for="txtFecha">Nueva Fecha Vigencia:</label>
                                    <asp:TextBox ID="txtVigenciaHasta" runat="server" CssClass="form-control" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalVigenciaHasta" runat="server" TargetControlID="txtVigenciaHasta" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div class="footer">
                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" class="btn btn-primary" OnClick="Enviar" CausesValidation="false" UseSubmitBehavior="False" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-secondary" OnClick="CerrarSolicitud" formnovalidate="formnovalidate" />
                        </div>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="upContacto" EnableViewState="False">
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</asp:Content>


