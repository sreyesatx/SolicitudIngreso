<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="RespuestaCorreo.aspx.cs" Inherits="SistemaSolicitudIngreso.RespuestaCorreo" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
                <asp:UpdatePanel ID="upAdministracion" runat="server">
                        <ContentTemplate>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="padding: 10px 0 30px 0;">
                                    <table align="center" border="1" cellpadding="0" cellspacing="0" width="600" style="border-collapse: collapse;" frame="vsides" rules="none">
                                        <tr style="padding: 20px 0 20px 0;">
                                            <td align="center" bgcolor="#dbedf7" style="color: #003366; font-family: Arial, sans-serif; font-size: 18px; padding-top: 10px; padding-bottom: 10px";>
                                                <b>SISTEMA DE SOLICITUD DE INGRESO ATREX</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="padding: 20px 0 20px 0;">
                                                <div style="text-align: center">
                                                    <img src="img/LogoAtrex.png" alt="" class="img-fluid"/>
                                                </div>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="PanelMotivo" runat="server" Visible="False">
                                            <tr>                                
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <div style="text-align: center">
                                                                    <asp:TextBox ID="txtMotivo" runat="server" placeholder="Indique motivo de su decisión" Width="400" Height="50" MaxLength="100"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div style="text-align: center">
                                                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar"  OnClick="btnGuardar_Click" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td bgcolor="#ffffff" style="padding: 40px 30px 40px 30px;">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">                                
                                                    <tr>
                                                        <td style="padding: 20px 0 20px 0; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align:justify">
                                                            <asp:Label ID="lblRespuesta" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#6c757d" style="padding: 30px 30px 30px 30px;">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="color: #ffffff; font-family: Arial, sans-serif; font-size: 14px; text-align: center">
                                                            &reg; Todos los derechos reservados - Atrex <asp:Label ID="lblAnio" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:SqlDataSource ID="sdsUpdateSolicitud" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_update_solicitud" SelectCommandType="StoredProcedure" UpdateCommand="sp_update_solicitud" UpdateCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                        <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                        <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                        <asp:Parameter Name="observaciones" Type="String"></asp:Parameter>
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                        <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                        <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                        <asp:Parameter Name="observaciones" Type="String"></asp:Parameter>
                    </UpdateParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="sdsEstadoSolicitud" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_estado_solicitud" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </div>
    </form>
</body>
</html>
