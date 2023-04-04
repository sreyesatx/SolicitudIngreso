<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="Mantenedores.aspx.cs" Inherits="SistemaSolicitudIngreso.Administracion.Mantenedores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js'></script>
	<script language="javascript" type="text/javascript">       		 		
	     function validaCorreo(source, arguments) {

            emailRegex = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return emailRegex.test(arguments.Value) ? arguments.IsValid = true : arguments.IsValid = false;
        }
	
    </script>    
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
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                                        <asp:DropDownList ID="ddlEmpresaFuncionario" runat="server" CssClass="form-control" data-style="btn-info" OnSelectedIndexChanged="ddlEmpresaFuncionario_SelectedIndexChanged" AutoPostBack="true" required></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                                        <asp:TextBox ID="txtNombreFuncionario" runat="server" class="form-control" placeholder="Nombre funcionario" AutoCompleteType="Disabled" MaxLength="100" required></asp:TextBox>
                                                        <br />
                                                    </div>                                                
													<div class="col-xs-auto col-sm-6 col-md-4 col-lg-3">
                                                        <asp:TextBox ID="txtCorreoFuncionario" runat="server" class="form-control" placeholder="Correo funcionario" AutoCompleteType="Disabled" MaxLength="100" required ></asp:TextBox>                                                        
														 <p class="help-block text-danger">
															<asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="* Correo inválido" ControlToValidate="txtCorreoFuncionario" ClientValidationFunction="validaCorreo" SetFocusOnError="True"></asp:CustomValidator>
														</p>
                                                    </div>													
                                                    <div class="col-xs-auto col-sm-6 col-md-12 col-lg-3">
                                                        <asp:Button ID="btnAgregarFuncionario" runat="server" Text="Agregar" class="btn btn-primary" OnClick="btnAgregarFuncionario_Click" />
                                                        <br />
                                                    </div>
                                                </div>

                                                <br />
                                                <div class="row">
                                                    <div class="col-lg-12 ">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvContactos" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" DataKeyNames="id_contacto" EmptyDataText="No se encontraron funcionarios." DataSourceID="sdsContactos" AllowPaging="True">
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
                                                                    <asp:BoundField DataField="id_contacto" HeaderText="Id contacto" SortExpression="id_contacto" />
                                                                    <asp:BoundField DataField="nombre_contacto" HeaderText="Nombre contacto" SortExpression="nombre_contacto" />
                                                                    <asp:BoundField DataField="correo_contacto" HeaderText="Correo contacto" SortExpression="correo_contacto" />
                                                                    <asp:BoundField DataField="nombre_empresa" HeaderText="Empresa" SortExpression="nombre_empresa" />
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
                        <asp:SqlDataSource ID="sdsContactos" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_contactos" SelectCommandType="StoredProcedure" DeleteCommand="sp_elimina_contactos" DeleteCommandType="StoredProcedure" InsertCommand="sp_insert_contactos" InsertCommandType="StoredProcedure">
                            <DeleteParameters>
                                <asp:Parameter Name="id_contacto" Type="Int32"></asp:Parameter>
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="nombre_contacto" Type="String"></asp:Parameter>
                                <asp:Parameter Name="correo_contacto" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_empresa" Type="Int32"></asp:Parameter>
                            </InsertParameters>
                            <SelectParameters>
                                <asp:Parameter Name="id_empresa" Type="Int32" DefaultValue="0"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="sdsEmpresas" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_empresas" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
