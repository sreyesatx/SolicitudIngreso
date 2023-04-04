<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Home.Master" CodeFile="Registro.aspx.cs" Inherits="SistemaSolicitudIngreso.Lav.Registro" %>

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
                            <h1>Registro</h1>
                        <%-- </div>--%>
                        <asp:UpdatePanel ID="upContacto" runat="server">
                            <ContentTemplate>
                                <div id="accordion">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4>
                                                <a class="card-link" data-toggle="collapse" href="#collapse1">Registro Acontecimientos
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapse1" class="collapse show" data-parent="#accordion">
                                            <div class="card-body">

                                                <div class="row">
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                        Ingresa Fecha y Hora                                                  
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                        <asp:TextBox ID="tbfecha" runat="server" class="form-control" placeholder="" AutoCompleteType="Disabled" MaxLength="10" required></asp:TextBox>
                                                         <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="tbfecha" Format="dd-MM-yyyy" FirstDayOfWeek="Monday" PopupPosition="TopLeft"></cc1:CalendarExtender>
   
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-2 col-lg-2">
                                                        <div class="input-group">
                                                       <span  class="input-group-addon">Hr.</span> <asp:DropDownList ID="ddlhora" runat="server" class="form-control" required></asp:DropDownList>                                                           
                                                            </div>
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-2 col-lg-2">
                                                       <div class="input-group">
                                                       <span  class="input-group-addon">Min.</span> <asp:DropDownList ID="ddlminuto" runat="server" class="form-control" required></asp:DropDownList>        
                                                            </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                        Courier
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-9 col-lg-9">
                                                         <asp:DropDownList ID="ddlcourier" class="form-control" runat="server" required></asp:DropDownList>

                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                        Observación
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-9 col-lg-9">
                                                        <asp:TextBox ID="tbobservacion" runat="server" class="form-control" placeholder="Comentarios" AutoCompleteType="Disabled" MaxLength="100" required TextMode="MultiLine"></asp:TextBox>

                                                    </div>
                                                </div>
                                                <br />
                                                  <div class="row">
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                        Tipo Acontecimiento 
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-9 col-lg-9">
                                                                  <asp:DropDownList ID="ddltipoac" class="form-control" runat="server" required></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <br />
                                                  <div class="row">
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                        Puesto Control
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-9 col-lg-9">
                                                        <asp:DropDownList ID="ddlpuesto" class="form-control" runat="server" required></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <br />
                                                 <div class="row">
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                      Guardia Turno
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-9 col-lg-9">
                                                         <asp:DropDownList ID="ddlguardia" class="form-control" runat="server" required></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row" style="text-align: center">
                                                       <div class="col-md-12 mx-auto">
                                                        <asp:Button ID="btnagregar" runat="server" Text="Agregar" class="btn btn-primary" OnClick="btnagregar_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12 ">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvContactos" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" DataKeyNames="sac_id" EmptyDataText="No existe registro para hoy." AllowPaging="True" DataSourceID="dslistaregistros">
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
                                                                <asp:BoundField DataField="sac_fecha" HeaderText="Fecha" SortExpression="sac_fecha" />
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
        <asp:SqlDataSource ID="dslistaregistros" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>" SelectCommand="sp_listar_registros_2" SelectCommandType="StoredProcedure" DeleteCommand="sp_elimina_registro" DeleteCommandType="StoredProcedure" OnDeleting="dslistaregistros_Deleting">
            <DeleteParameters>
                <asp:Parameter Name="sac_id" Type="Int32" />
                <asp:Parameter Name="user" Type="String" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter DefaultValue="1" Name="tiporeporte" Type="Int32" />
            </SelectParameters>
           
        </asp:SqlDataSource>
    </form>
</asp:Content>
