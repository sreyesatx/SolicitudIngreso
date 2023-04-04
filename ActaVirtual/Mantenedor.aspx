<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Home.Master" CodeFile="Mantenedor.aspx.cs" Inherits="SistemaSolicitudIngreso.Lav.Mantenedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
          </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
   
   <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
     <br />
        <div id="mainContainer" class="container">

            <div class="shadowBox">
                <div class="page-container">
                    <div class="container">
                        <div class="jumbotron">
                            <h1>Mantenedores</h1>
                        </div>
                    
                        <asp:UpdatePanel ID="upContacto" runat="server">
                            <ContentTemplate>
                                <div id="accordion">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4>
                                                <a class="card-link" data-toggle="collapse" href="#collapse1">Contacto empresa
                                                </a>
                                            </h4>
                                        </div>
                                        <div id="collapse1" class="collapse show" data-parent="#accordion">
                                            <div class="card-body">

                                                <div class="control-group row">
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                   Tipo Código  <asp:DropDownList ID="ddlTipoIngreso" runat="server" CssClass="form-control" data-style="btn-info" AutoPostBack="true" required>
                                                            <asp:ListItem Value="G">GUARDIAS</asp:ListItem>
                                                            <asp:ListItem Value="P">PUESTO</asp:ListItem>
                                                            <asp:ListItem Value="C">COURIER</asp:ListItem>
                                                            <asp:ListItem Value="A">ACONTECIMIENTO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-3 col-lg-3">
                                                     Código   <asp:TextBox ID="txtcodigo" runat="server" class="form-control" placeholder="" AutoCompleteType="Disabled" MaxLength="100" required></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-4">
                                                    Nombre  <asp:TextBox ID="txtdescripción" runat="server" class="form-control" placeholder="" AutoCompleteType="Disabled" MaxLength="100" required></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-2 col-lg-2">
                                                        <asp:SqlDataSource ID="dsMantenedor" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>" SelectCommand="sp_lista_tiposregistros" SelectCommandType="StoredProcedure" DeleteCommand="sp_elimina_tiposregistros" DeleteCommandType="StoredProcedure" InsertCommand="sp_insert_tiporegistro" InsertCommandType="StoredProcedure">
                                                            <DeleteParameters>
                                                                <asp:Parameter Name="str_codigo" Type="Int32" />
                                                            </DeleteParameters>
                                                            <InsertParameters>
                                                                <asp:ControlParameter ControlID="txtcodigo" Name="str_codigo" PropertyName="Text" Type="String" />
                                                                <asp:ControlParameter ControlID="txtdescripción" Name="str_descripcion" PropertyName="Text" Type="String" />
                                                                <asp:ControlParameter ControlID="ddlTipoIngreso" Name="str_tiporegistro" PropertyName="SelectedValue" Type="String" />
                                                                <asp:SessionParameter DefaultValue="" Name="str_user_creac" SessionField="nombre_usuario" Type="String" />
                                                            </InsertParameters>
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddlTipoIngreso" Name="TipoRegistro" PropertyName="SelectedValue" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    &nbsp;  <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="form-control btn btn-primary" OnClick="btnAgregar_Click" />
                                                        <br />
                                                    </div>
                                                </div>

                                                <br />
                                                <div class="row">
                                                    <div class="col-lg-12 ">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvContactos" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" DataKeyNames="str_idregistro" EmptyDataText="No se encontraron funcionarios." AllowPaging="True" DataSourceID="dsMantenedor">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Esta seguro que desea eliminar este item');"
                                                                                CommandName="Delete">Eliminar</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:BoundField DataField="str_idregistro" HeaderText="id" SortExpression="str_idregistro" />
                                                                    <asp:BoundField DataField="str_codigo" HeaderText="Codigo" SortExpression="str_codigo" />
                                                                    <asp:BoundField DataField="str_descripcion" HeaderText="Nombre" SortExpression="str_descripcion" />
                                                                 <%--   <asp:BoundField DataField="correo_contacto" HeaderText="Correo contacto" SortExpression="correo_contacto" />
                                                                    <asp:BoundField DataField="nombre_empresa" HeaderText="Empresa" SortExpression="nombre_empresa" />--%>
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
                    
                    </div>
                </div>
            </div>
        </div>
       </form>
</asp:Content>


