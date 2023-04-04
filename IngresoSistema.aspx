<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="IngresoSistema.aspx.cs" Inherits="IngresoSistema" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="contactForm" method="post">
        <section id="ingresoSistema">
            <div class="container">
                <div class="row">
                    <div class="col-lg-12 mx-auto">
                        <div style="text-align: center">
                            <img src="img/LogoAtrex.png" alt="" class="img-fluid">
                        </div>
                    </div>
                </div>
                <br />
                <br />

                <div class="row">
                    <div class="col-lg-8 mx-auto card-body">

                        <div class="control-group row">
                            <div class="form-group controls col-6 mx-auto">
                                <label for="usr">Usuario</label>
                                <input type="text" class="form-control" name="usr" placeholder="Usuario" required autofocus>
                            </div>
                        </div>
                        <div class="control-group row">
                            <div class="form-group controls col-6 mx-auto">
                                <label for="pwd">Contraseña</label>
                                <input type="password" class="form-control" name="pwd" placeholder="Contraseña" required>
                            </div>
                        </div>
                        <div class="control-group row">
                            <div class="form-group controls col-6 mx-auto">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <br>

                        <div class="control-group row">
                            <div class="form-group controls col-6 mx-auto">
                                <button class="btn btn-primary" type="submit">
                                    Iniciar sesión</button>

                            </div>
                        </div>
                        <asp:SqlDataSource ID="sdsUsuario" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_usuario_Cambio" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="usuario" Type="String"></asp:Parameter>
                                <asp:Parameter Name="pass" Type="String"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsMenu" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_menu_Cambio" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="id_usuario" Type="Int32"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>

                </div>
            </div>
        </section>
    </form>
</asp:Content>
