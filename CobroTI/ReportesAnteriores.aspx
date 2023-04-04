<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="ReportesAnteriores.aspx.cs" Inherits="CobroTI_ReportesAnteriores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.css">
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function Confirm() {
            if (confirm("¿Seguro que desea eliminar Reporte?")) {
                document.getElementById('<%=hiddenval.ClientID%>').value = "Yes";
            } else {
                document.getElementById('<%=hiddenval.ClientID%>').value = "No";
            }
        }
    </script>
    <style type="text/css">
        .colorfondo {
            background-color: #fbe9e7 !important;
        }

        .colorfondoguia {
            background-color: #e3f2fd !important;
        }

        .shrink {
            -webkit-transform: scale(0.8);
            -moz-transform: scale(0.8);
            -ms-transform: scale(0.8);
            transform: scale(0.8);
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

        .swal-wide {
            width: 500px;
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

        #lineaSeparator {
            margin: 10px;
            padding: 10px;
            border: 1px solid;
            border-radius: 16px;
            box-sizing: inherit;
            border: 1px solid #ccc !important;
        }

        #margentituloguia {
            margin: 10px;
        }

        .the-legend {
            border-style: none;
            border-width: 0;
            font-size: 13px;
            line-height: 5px;
            margin-bottom: auto;
            width: 400px;
            padding: 5px;
            border: 0px solid #e0e0e0;
            text-align: left;
        }

        .the-fieldset {
            margin: 10px;
            padding: 10px;
            border: 1px solid;
            border-radius: 16px;
            box-sizing: inherit;
            border: 1px solid #ccc !important;
        }

        .ajustarTamano {
            margin: 10px;
            padding: 10px;
            border: 1px solid;
            border-radius: 16px;
            box-sizing: inherit;
            border: 0px solid #ccc !important;
        }

        .ajustarTamanoTitulo {
            margin: 20px;
        }

        #ajustarsubTitulo {
            margin: 1px;
        }

        .col-lg-7 {
            margin-top: 0px;
        }

        #espaciotop {
            top: 20px;
        }

        p span:not(:first-child) {
            border-left: 1px solid #000;
        }

        .space-left {
            padding-left: 10px;
        }

        tr.nowrap {
            white-space: nowrap;
        }

        .documento table {
            width: 100%;
        }

        .separadorp {
            margin-top: 0px;
            margin-bottom: 0px;
        }

        #d {
            table-layout: fixed;
            width: 100%;
            margin: 10px;
            padding: 10px;
            border: 0px solid #ffffff;
        }

        thead.line {
            border-bottom: 1px solid #ccc;
        }

        td.space {
            padding: 1px 25px 1px 1px;
        }

        #divPopUp {
            width: 50%;
            margin: 0 auto;
            padding: 20px;
        }

        .GridPager a,
        .GridPager span {
            display: inline-block;
            padding: 0px 9px;
            margin-right: 4px;
            border-radius: 3px;
            border: solid 1px #c0c0c0;
            background: #e9e9e9;
            box-shadow: inset 0px 1px 0px rgba(255,255,255, .8), 0px 1px 3px rgba(0,0,0, .1);
            font-size: .875em;
            font-weight: bold;
            text-decoration: none;
            color: #717171;
            text-shadow: 0px 1px 0px rgba(255,255,255, 1);
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
            background: #616161;
            box-shadow: inset 0px 0px 8px rgba(0,0,0, .5), 0px 1px 0px rgba(255,255,255, .8);
            color: #f0f0f0;
            text-shadow: 0px 0px 3px rgba(0,0,0, .5);
            border: 1px solid #3AC0F2;
        }

        .btn-toolbar .btn{
            margin-right: 5px;
        }
        .btn-toolbar .btn:last-child{
            margin-right: 0;
        }


    </style>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
        <br />
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
                                    <h1>Reportes Anteriores</h1>
                                </div>
                                <div class="control-group row">
                                    <div class="col-xs-auto col-sm-4 col-md-4 col-lg-2">
                                        <label for="ddlyear">Año</label>
                                        <asp:DropDownList ID="ddlyear" runat="server" placeholder="Seleccione Año" CssClass="form-control" Font-Size="Small">
                                            <asp:ListItem Value="">--</asp:ListItem>
                                            <asp:ListItem Value="2018">2018</asp:ListItem>
                                            <asp:ListItem Value="2019">2019</asp:ListItem>
                                            <asp:ListItem Value="2020">2020</asp:ListItem>
                                            <asp:ListItem Value="2021">2021</asp:ListItem>
                                            <asp:ListItem Value="2022">2022</asp:ListItem>
                                            <asp:ListItem Value="2023">2023</asp:ListItem>
                                            <asp:ListItem Value="2024">2024</asp:ListItem>
                                            <asp:ListItem Value="2025">2025</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-auto col-sm-4 col-md-4 col-lg-3">
                                        <label for="ddlmonth">Mes</label>
                                        <div style="display: flex;">
                                            <asp:DropDownList ID="ddlmonth" runat="server" placeholder="Seleccione Mes" CssClass="form-control" Font-Size="Small">
                                                <asp:ListItem Value="">--</asp:ListItem>
                                                <asp:ListItem Value="1">ENERO</asp:ListItem>
                                                <asp:ListItem Value="2">FEBRERO</asp:ListItem>
                                                <asp:ListItem Value="3">MARZO</asp:ListItem>
                                                <asp:ListItem Value="4">ABRIL</asp:ListItem>
                                                <asp:ListItem Value="5">MAYO</asp:ListItem>
                                                <asp:ListItem Value="6">JUNIO</asp:ListItem>
                                                <asp:ListItem Value="7">JULIO</asp:ListItem>
                                                <asp:ListItem Value="8">AGOSTO</asp:ListItem>
                                                <asp:ListItem Value="9">SEPTIEMBRE</asp:ListItem>
                                                <asp:ListItem Value="10">OCTUBRE</asp:ListItem>
                                                <asp:ListItem Value="11">NOVIEMBRE</asp:ListItem>
                                                <asp:ListItem Value="12">DICIEMBRE</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar" class="btn btn-primary" />  
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="control-group row">
                                    <div class="col-lg-12 ">
                                        <div class="table-responsive">
                                            <asp:HiddenField ID="hiddenval" runat="server" />	
                                            <asp:GridView ID="gvReportesAnteriores" runat="server" CssClass="table table-striped table-bordered table-hover" 
                                                AutoGenerateColumns="False"  EmptyDataText="No se encontraron solicitudes." 
                                                Style="font-size: 11px;" AllowPaging="True">
											   <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <Columns>
                                                  <asp:TemplateField HeaderText="Año">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblyear" runat="server" Text='<%#Bind("year") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Mes">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmes" runat="server" Text='<%#Bind("Mes") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Monto">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmonto" runat="server" Text='<%#Bind("monto") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Usuario Creacion">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUsuarioCreacion" runat="server" Text='<%#Bind("UsuarioCreacion") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Fecha Creacion">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaCreacion" runat="server" Text='<%#Bind("FechaCreacion") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="NumMes" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblnummes" runat="server" Text='<%#Bind("month") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText=""  Visible="true">
                                                        <ItemTemplate>                                                 
                                                            <asp:LinkButton  ID="Linkeliminar" runat="server" class="btn btn-danger" Text="Eliminar" OnClick="onDelete" OnClientClick="Confirm()" ></asp:LinkButton>                                          
                                                        </ItemTemplate>                                
                                                    </asp:TemplateField> 
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>                                
                                <br />
                                <div class="row" style="text-align: center">
                                    <div class="col-lg-12 ">
                                        <div class="table-responsive">
                                            <asp:Button ID="bntExcel" runat="server" OnClick="ExportToExcel" Text="Excel" class="btn btn-success" visible="false" /> 
                                            <asp:GridView ID="GV_modeloPonderacion" runat="server" Width="100%" CellPadding="2" ForeColor="#333333"
                                                GridLines="None" AutoGenerateColumns="False" Style="margin-top: 0px"
                                                AllowPaging="false"
                                                OnRowDataBound="GV_modeloPonderacion_RowDataBound">
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Courier">
                                                        <ItemTemplate>
                                                            <small><asp:Label ID="lblidempresa" runat="server" Text='<%#Bind("idempresa") %>'></asp:Label></small>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cantidad Repositorio">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCantRepo" runat="server" Text='<%#Bind("CantRepo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalRepo" runat="server" Text='<%#Bind("TotalRepo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cantidad Dips">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCantDIPS" runat="server" Text='<%#Bind("CantDIPS") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotDips" runat="server" Text='<%#Bind("TotDips") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cantidad Guias">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCantGuias" runat="server" Text='<%#Bind("CantGuias") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotGuias" runat="server" Text='<%#Bind("TotGuias") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cantidad Dussi">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCantDussi" runat="server" Text='<%#Bind("CantDussi") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotDussi" runat="server" Text='<%#Bind("TotDussi") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TOTAL COURIER">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalCourier" runat="server" Text='<%#Bind("TotalCourier") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>No se encontraron datos</EmptyDataTemplate>
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <br />
                                <br />
                            </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="bntExcel" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</asp:Content>
