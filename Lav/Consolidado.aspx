<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Home.Master" CodeFile="Consolidado.aspx.cs" Inherits="SistemaSolicitudIngreso.Lav.Consolidado" %>


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
                        <br />
                        <br />
                        <%--   <div class="jumbotron">--%>
                        <h1>Listado Registros</h1>
                        <%-- </div>--%>
                        <asp:UpdatePanel ID="upContacto" runat="server">
                            <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportar" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row" style="text-align: left">
                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                        <label for="ddlEmpresas">Por empresa</label>
                                        <asp:DropDownList ID="ddlBuscaEmpresas" runat="server" CssClass="form-control" data-style="btn-info" DataSourceID="dsMantenedor" DataTextField="str_descripcion" DataValueField="str_descripcion" OnDataBound="ddlBuscaEmpresas_DataBound"></asp:DropDownList>
                                           <asp:SqlDataSource ID="dsMantenedor" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>" SelectCommand="sp_lista_tiposregistros" SelectCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:Parameter DefaultValue="C" Name="TipoRegistro" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                    </div>
                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                        <label for="ddlTipoVisita">Fecha desde</label>
                                        <asp:TextBox ID="txtBuscadesde" runat="server" class="form-control" placeholder="Buscar por fecha desde" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBuscadesde" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                    </div>

                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                        <label for="ddlEmpresas">Fecha Hasta</label>
                                        <asp:TextBox ID="txtBuscahasta" runat="server" class="form-control" placeholder="Buscar por fecha hasta" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtBuscahasta" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                                    </div>
                                </div>
                                <p></p>
                                <div class="row" style="text-align: left">
                                    <div class="col-xs-auto col-sm-2 col-md-2 col-lg-2 form-group">                                         
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary form-control" OnClick="btnBuscar_Click" />  
                                    </div>
                                    <div class="col-xs-auto col-sm-2 col-md-2 col-lg-2 form-group">
                                        <asp:Button ID="btnExportar" runat="server" Text="Exportar Excel" class="btn btn-primary form-control" OnClick="ExportToExcel" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12 ">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvContactos" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" DataKeyNames="sac_id" EmptyDataText="No existe registro para hoy." AllowPaging="True" DataSourceID="dslistaregistros"  PageSize="15">                                                                                                
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server"
                                                                OnClientClick="return confirm('Esta seguro de querer eliminar este contacto?');"
                                                                CommandName="Delete">Eliminar</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:BoundField DataField="sac_id" HeaderText="Id" SortExpression="sac_id" />
                                                    <asp:BoundField DataField="sac_fecha_dia" HeaderText="Día" SortExpression="sac_fecha_dia" />
                                                    <asp:BoundField DataField="sac_fecha_mes" HeaderText="Mes" SortExpression="sac_fecha_mes" />
                                                    <asp:BoundField DataField="sac_fecha_ano" HeaderText="Año" SortExpression="sac_fecha_ano" />
                                                    <asp:BoundField DataField="sac_fecha_hora" HeaderText="Hora" SortExpression="sac_fecha_hora" />
                                                    <asp:BoundField DataField="sac_courier" HeaderText="Courier" SortExpression="sac_courier" />
                                                    <asp:BoundField DataField="sac_observacion" HeaderText="Observación" SortExpression="sac_observacion" />
                                                    <asp:BoundField DataField="sac_tipo" HeaderText="Tipo" SortExpression="sac_tipo" />
                                                    <asp:BoundField DataField="sac_puesto" HeaderText="Puesto Control" SortExpression="sac_puesto" />
                                                    <asp:BoundField DataField="sac_turno" HeaderText="Guardia Turno" SortExpression="sac_turno" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:SqlDataSource ID="dsTipo" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>" SelectCommand="sp_listar_codigos" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Name="TipoCodigo" Type="String" DefaultValue="C" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsOperacion" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>" SelectCommand="sp_listar_codigos" SelectCommandType="StoredProcedure" InsertCommand="sp_insert_registro" InsertCommandType="StoredProcedure">
            <InsertParameters>
                <asp:Parameter Name="sac_fecha" Type="DateTime" />
                <asp:Parameter Name="sac_tipocourier" Type="String" />
                <asp:Parameter Name="sac_courier" Type="String" />
                <asp:Parameter Name="sac_observacion" Type="String" />
                <asp:Parameter Name="sac_tipo" Type="String" />
                <asp:Parameter Name="sac_puesto" Type="String" />
                <asp:Parameter Name="sac_turno" Type="String" />
                <asp:Parameter Name="sac_user_creac" Type="String" />
            </InsertParameters>
            <SelectParameters>
                <asp:Parameter Name="TipoCodigo" Type="String" DefaultValue="A" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dslistaregistros" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>" SelectCommand="sp_listar_registros" SelectCommandType="StoredProcedure" DeleteCommand="sp_elimina_registro" DeleteCommandType="StoredProcedure" OnDeleting="dslistaregistros_Deleting" OnSelecting="dslistaregistros_Selecting">
            <DeleteParameters>
                <asp:Parameter Name="sac_id" Type="Int32" />
                <asp:Parameter Name="user" Type="String" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter DefaultValue="2" Name="tiporeporte" Type="Int32" />
                <asp:ControlParameter ControlID="txtBuscadesde" Name="FechaDesde" PropertyName="Text" Type="DateTime" DefaultValue="01/01/1999" />
                <asp:ControlParameter ControlID="txtBuscahasta" Name="FechaHasta" PropertyName="Text" Type="DateTime" DefaultValue="01/01/2019" />
                <asp:ControlParameter ControlID="ddlBuscaEmpresas" DefaultValue="0" Name="empresa" PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</asp:Content>
