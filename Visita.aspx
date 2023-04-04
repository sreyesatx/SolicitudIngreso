 <%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="Visita.aspx.cs" Inherits="SistemaSolicitudIngreso.Ingreso.Visita" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>--%>
    <script type="text/javascript" src="js/sweetalert.js"></script>
    <script type="text/javascript">

        function validar_rut(source, arguments) {

            var rut = arguments.Value; suma = 0; mul = 2; i = 0;

            for (i = rut.length - 3; i >= 0; i--) {
                suma = suma + parseInt(rut.charAt(i)) * mul;
                mul = mul == 7 ? 2 : mul + 1;
            }

            var dvr = '' + (11 - suma % 11);
            if (dvr == '10') dvr = 'K'; else if (dvr == '11') dvr = '0';

            if (rut.charAt(rut.length - 2) != "-" || rut.charAt(rut.length - 1).toUpperCase() != dvr)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function validaCorreo(source, arguments) {

            emailRegex = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

            return emailRegex.test(arguments.Value) ? arguments.IsValid = true : arguments.IsValid = false;

        }

        function llenaNomina() {

            var nombre = document.getElementById('ContentPlaceHolder1_txtNombreNomina');
            var rut = document.getElementById('ContentPlaceHolder1_txtRutNomina');

            if (rut.value != "" && nombre.value != "") {

                var opt = document.createElement("option");
                document.getElementById("ContentPlaceHolder1_lbNomina").options.add(opt);
                opt.text = rut.value + ' / ' + nombre.value;
                rut.value = nombre.value = "";
            }
            else {

                swal('Ingrese nombre y rut');
                return;
            }
        }
        function enviarNomina() {

            var valida = new String("");
            valida.text = "";

            var ddlEmpresas = document.getElementById("ContentPlaceHolder1_ddlEmpresa").selectedIndex;

            //var txtOtroTipo = document.getElementById("ContentPlaceHolder1_txtOtroTipo").value;
          //  var fechaVisita = document.getElementById("ContentPlaceHolder1_txtFecha").value;
         //   var ddlHora = document.getElementById("ContentPlaceHolder1_ddlHora").selectedIndex;
         //   var ddlMinutos = document.getElementById("ContentPlaceHolder1_ddlMinutos").selectedIndex;
            var txtMotivoIngreso = document.getElementById("ContentPlaceHolder1_txtMotivoIngreso").value;
            var rblTipoIngreso = $("[id$='rblTipoIngreso']").find(":checked").val();

            var txtNombreSolicitante = document.getElementById("ContentPlaceHolder1_txtNombreSolicitante").value;
            var txtRutSolicitante = document.getElementById("ContentPlaceHolder1_txtRutSolicitante").value;
            var txtPatente = document.getElementById("ContentPlaceHolder1_txtPatente").value;
            var txtCorreo = document.getElementById("ContentPlaceHolder1_txtCorreo").value;
            var txtEmpresaSolicitante = document.getElementById("ContentPlaceHolder1_txtEmpresaSolicitante").value;
           

            var rblAcompanante = $("[id$='rblAcompanante']").find(":checked").val();
            var lista = document.getElementById('ContentPlaceHolder1_lbNomina');

            if (ddlEmpresas == 0) {

                valida.text += "- Seleccione empresa a visitar.\n";
            }

            if (document.getElementById("ContentPlaceHolder1_ddlEmpresa").value != "32") {
                var ddlTipoVisita = document.getElementById("ContentPlaceHolder1_ddlTipoVisita").selectedIndex;
                if (ddlTipoVisita == 0) {

                    valida.text += "- Seleccione tipo visita.\n";
                }

            }

            else {
                if (document.getElementById("ContentPlaceHolder1_lstbatch").innerText == "") {

                    valida.text += "- Digite N° Batch para retiro.\n";
                }
                else {

                    var listabatch = document.getElementById("ContentPlaceHolder1_lstbatch");
                    for (var i = 0; i < listabatch.length; i++) {
                        var opt = listabatch[i];
                        document.getElementById("ContentPlaceHolder1_HiddenField1").value += opt.text + "|";

                    }
                }
            }

            //if (ddlTipoVisita == 5) {
            //    if (txtOtroTipo == "") {
            //        valida.text += "- Indique otro tipo de visita.\n";
            //    }
            //}

           

            if (txtMotivoIngreso == "") {

                valida.text += "- Ingrese motivo de ingreso.\n";
            }

            if (rblTipoIngreso == null) {

                valida.text += "- Seleccione tipo de ingreso.\n";
            }
            else {

                if (txtNombreSolicitante == "") {

                    valida.text += "- Ingrese nombre solicitante.\n";
                }
                if (txtRutSolicitante == "") {

                    valida.text += "- Ingrese rut solicitante.\n";
                }
                if (txtCorreo == "") {

                    valida.text += "- Ingrese correo.\n";
                }
                else {
                    emailRegex = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

                    if (emailRegex.test(txtCorreo) != true)
                        valida.text += "- Correo inválido.\n";
                }

                if (txtEmpresaSolicitante == "") {

                    valida.text += "- Ingrese empresa.\n";
                }
             

                if (rblTipoIngreso == 2) {

                    if (txtPatente == "") {

                        valida.text += "- Ingrese patente vehículo.\n";
                    }
                }
            }

            if (rblAcompanante == null) {

                valida.text += "- Seleccione listado de acompañantes.\n";
            }
            else {

                if (rblAcompanante == "1") {

                    if (lista.length == 0) {

                        valida.text += "- Ingrese acompañantes.\n";
                    }
                }
            }
            //**** Validación del batch ******//
            //if (document.getElementById("ContentPlaceHolder1_ddlEmpresas").value == "32") {



            //}

            //*******************************//
            if (valida.text == "") {

                var envio = new String("");
                envio.text = "";


                for (var i = 0; i < lista.length; i++) {
                    var opt = lista[i];
                    envio.text += opt.text + ",";

                }

                envio.text = envio.text.slice(0, -1);

                var confirm_value;
                confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                confirm_value.value = envio.text;
                document.forms[0].appendChild(confirm_value);

  


                espera();

                return true;
            }
            else {

                swal('Falta completar los siguientes campos: \n\n' + valida.text);
                return false;
            }
        }

        function borrarSeleccion() {

            $.each($('#<%=lbNomina.ClientID%> :selected'), function () {

                $(this).remove();

            });
        }
        function borrarSeleccionbatch() {

            $.each($('#<%=lstbatch.ClientID%> :selected'), function () {

                $(this).remove();

            });
        }

        function tipoIngreso() {

            //var prueba = $('input[name=tipoIngreso]:checked', '#contactForm').val();
            var selectedVal = $("[id$='rblTipoIngreso']").find(":checked").val();
            var nombre = document.getElementById("ContentPlaceHolder1_txtNombreSolicitante");
            var rut = document.getElementById("ContentPlaceHolder1_txtRutSolicitante");
            var patente = document.getElementById("ContentPlaceHolder1_txtPatente");

            if (selectedVal == "1") {
                document.getElementById('<%=PanelPeaton.ClientID%>').style.display = 'block';
                document.getElementById('<%=PanelPatente.ClientID%>').style.display = 'none';

                nombre.placeholder = "Nombre y apellido";
                rut.placeholder = "Rut o pasaporte, ej: 12345678-9";
                patente.required = false;
            }
            else {
                document.getElementById('<%= PanelPeaton.ClientID %>').style.display = 'block';
                document.getElementById('<%=PanelPatente.ClientID%>').style.display = 'block';

                nombre.placeholder = "Nombre y apellido conductor";
                rut.placeholder = "Rut conductor o pasaporte, ej: 12345678-9";
                patente.required = true;
            }
        }

        function acompanante() {

            var selectedVal = $("[id$='rblAcompanante']").find(":checked").val();

            if (selectedVal == "1") {
                document.getElementById('<%=PanelAcompañante.ClientID%>').style.display = 'block';
            }
            else {
                document.getElementById('<%=PanelAcompañante.ClientID%>').style.display = 'none';
            }

        }

        function otroTipo() {

            var cod = document.getElementById("ContentPlaceHolder1_ddlTipoVisita").value;
            //var otro = document.getElementById("ContentPlaceHolder1_txtOtroTipo");
            switch (cod) {
                case "5":
                    document.getElementById('<%=PanelOtroTipo.ClientID%>').style.display = 'block';
                    break;
                case "9":
                    document.getElementById('<%=PanelTransportista.ClientID%>').style.display = 'block';
                    break;
                default:
                    {
                        document.getElementById('<%=PanelOtroTipo.ClientID%>').style.display = 'none';
                        document.getElementById('<%=PanelTransportista.ClientID%>').style.display = 'none';
                    }

            }
        }

        function zonaPrimaria() {

            var cod = document.getElementById("ContentPlaceHolder1_ddlEmpresa").value;
            var motivo = document.getElementById("ContentPlaceHolder1_txtMotivoIngreso");

            if (cod == "32") {
                motivo.placeholder = "Especificar motivo ingreso y empresas a visitar";
                document.getElementById('<%=PanelTransportista.ClientID%>').style.display = 'block';
                document.getElementById('<%=PanelOtroTipo.ClientID%>').style.display = 'none';
                document.getElementById("ContentPlaceHolder1_ddlTipoVisita").style.display = 'none';
                document.getElementById("lbltipovisita").innerText = 'Indique n° batch retirar';
                document.getElementById("ContentPlaceHolder1_ddlTipoVisita").required = false;

            }
            else {
                motivo.placeholder = "Especificar motivo ingreso";
                document.getElementById('<%=PanelTransportista.ClientID%>').style.display = 'none';
                document.getElementById("ContentPlaceHolder1_ddlTipoVisita").style.display = 'block';
                document.getElementById("ContentPlaceHolder1_ddlTipoVisita").required = true;
                document.getElementById("lbltipovisita").innerText = 'Seleccione Tipo de Visita';
            }
        }


        function espera() {

            $get('upssConsulta').style.display = 'block';
        }

        function rutPasaporte(e) {
            var key = window.Event ? e.which : e.keyCode
            return (key >= 48 && key <= 57) || (key == 13) || (key == 45) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122)

        }


        function VerificaBatch() {


            var obj = {};
            obj.batch = document.getElementById('ContentPlaceHolder1_txtBatch').value;
            obj.empresa = 'ATREX';

            $.ajax({
                type: "POST",
                url: "AgendarVisita.aspx/busca_batch",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (data) {
                    var arrVal = data.d.split('|');
                    var labelbatch = document.getElementById("ContentPlaceHolder1_lblBatch");
                    labelbatch.innerText = "";
                    var textbatch = document.getElementById("ContentPlaceHolder1_txtBatch");
                    if (arrVal[0] != "OK") {

                        //    labelbatch.innerText = arrVal[0];
                        //    labelbatch.style = "color: Red";

                        var opt = document.createElement("option");
                        document.getElementById("ContentPlaceHolder1_lstbatch").options.add(opt);
                        opt.text = textbatch.value + "/" + arrVal[0];
                        textbatch.value = "";

                    }
                    else {
                        var resumenbatch = "Total Guías: " + arrVal[1] + "   Total Peso: " + arrVal[2];
                        //labelbatch.innerText = "Total Guías: " + arrVal[1] + "   Total Peso: " + arrVal[2];
                        //labelbatch.style = "color: Green";   


                        var opt = document.createElement("option");
                        document.getElementById("ContentPlaceHolder1_lstbatch").options.add(opt);
                        opt.text = textbatch.value + "/" + resumenbatch;
                        textbatch.value = "";

                    }

                    //  document.getElementById("ContentPlaceHolder1_HiddenField1").value = document.getElementById("ContentPlaceHolder1_lstbatch").innerText;
                },
                error: function (result) {
                    alert(result);

                }
            });
        }
        function listarPropiedades(obj) {
            html = ""
            for (var i in obj) {
                html += ('propiedad: ' + i + ' valor: ' + obj[i]) + "<br>";
            }
            popup = window.open('');
            popup.document.open();
            popup.document.write(html);
            popup.document.focus();
            popup.document.close();
        }


        //$('ContentPlaceHolder1_txtBatch').on('change', function () {
        //    alert("La acción se puede lanzar aquí, ¿por qué no? " + this.value);
        //})
    </script>

    <style type="text/css">
        .PanelOculto {
            display: none;
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

        .colorletra {
            color: green;
        }
    </style>

    <section id="agendarVisita">

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
                <div class="col-lg-8 mx-auto">
                    <h1>Agendar visita</h1>
                    <p class="lead text-justify">Para agendar una visita a nuestras instalaciones, favor completar el siguiente formulario:</p>
                    <br />
                    <form id="contactForm" runat="server">
                        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
                        <br />



                        <asp:SqlDataSource ID="sdsEmpresas" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_empresas" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsTipoVisita" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_tipo_visita" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsSolicitud" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' InsertCommand="sp_insert_solicitud" InsertCommandType="StoredProcedure" SelectCommand="sp_insert_solicitud" SelectCommandType="StoredProcedure" OnInserted="sdsSolicitud_Inserted" OnInserting="sdsSolicitud_Inserting">
                            <InsertParameters>
                                <asp:Parameter Name="id_empresa" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="id_tipo_visita" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="otro_indicar" Type="String"></asp:Parameter>
                                <asp:Parameter Name="fecha_visita" Type="DateTime"></asp:Parameter>
                                <asp:Parameter Name="motivo_ingreso" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_tipo_ingreso" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="fecha_solicitud" Type="DateTime"></asp:Parameter>
                                <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="rut_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="patente_vehiculo" Type="String"></asp:Parameter>
                                <asp:Parameter Name="correo_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="empresa_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="cargo_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="listado_acompanantes" Type="Boolean"></asp:Parameter>
                                <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="hora_ingreso" Type="DateTime"></asp:Parameter>
                                <asp:Parameter Name="hora_salida" Type="DateTime"></asp:Parameter>
                                <asp:Parameter Direction="InputOutput" Name="id_solicitud" Type="Int32"></asp:Parameter>
                            </InsertParameters>
                            <SelectParameters>
                                <asp:Parameter Name="id_empresa" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="id_tipo_visita" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="otro_indicar" Type="String"></asp:Parameter>
                                <asp:Parameter Name="fecha_visita" Type="DateTime"></asp:Parameter>
                                <asp:Parameter Name="motivo_ingreso" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_tipo_ingreso" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="fecha_solicitud" Type="DateTime"></asp:Parameter>
                                <asp:Parameter Name="nombre_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="rut_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="patente_vehiculo" Type="String"></asp:Parameter>
                                <asp:Parameter Name="correo_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="empresa_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="cargo_solicitante" Type="String"></asp:Parameter>
                                <asp:Parameter Name="listado_acompanantes" Type="Boolean"></asp:Parameter>
                                <asp:Parameter Name="id_estado" Type="Int32"></asp:Parameter>
                                <asp:Parameter Name="hora_ingreso" Type="DateTime"></asp:Parameter>
                                <asp:Parameter Name="hora_salida" Type="DateTime"></asp:Parameter>
                                <asp:Parameter Direction="InputOutput" Name="id_solicitud" Type="Int32"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsBatchRetiro" runat="server" ConnectionString="<%$ ConnectionStrings:SolicitudIngresoConnectionString %>" InsertCommand="sp_insert_batchretiro" InsertCommandType="StoredProcedure" ProviderName="<%$ ConnectionStrings:SolicitudIngresoConnectionString.ProviderName %>">
                            <InsertParameters>
                                <asp:Parameter Name="numerobatch" Type="String" />
                                <asp:Parameter Name="id_solicitud" Type="Int32" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsAcompanantes" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' InsertCommand="sp_insert_acompanante" InsertCommandType="StoredProcedure" SelectCommand="sp_insert_acompanante" SelectCommandType="StoredProcedure">
                            <InsertParameters>
                                <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
                                <asp:Parameter Name="rut" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                            </InsertParameters>
                            <SelectParameters>
                                <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
                                <asp:Parameter Name="rut" Type="String"></asp:Parameter>
                                <asp:Parameter Name="id_solicitud" Type="Int32"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sdsContactos" runat="server" ConnectionString='<%$ ConnectionStrings:SolicitudIngresoConnectionString %>' SelectCommand="sp_lista_contactos" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="" Name="id_empresa" Type="Int32"></asp:Parameter>
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <div>
                            <div>
                                <h3>
                                    <a>Datos ingreso</a></h3>
                            </div>
                        </div>
                        <br />

                        <div class="control-group row">
                            <div class="form-group controls col-md-6 mx-auto">
                                <label for="ddlEmpresa">Seleccione empresa a visitar:</label>
                                <select id="ddlEmpresa" class="browser-default custom-select" required onclick="zonaPrimaria();"
                                    name="ddlEmpresa"
                                    runat="server">
                                </select>
                            </div>
                            <div class="form-group controls col-md-6 mx-auto">
                                <label id="lbltipovisita" for="ddlTipoVisita">Seleccione el tipo de visita: </label>
                                <select id="ddlTipoVisita" class="browser-default custom-select" required onclick="otroTipo();"
                                    name="ddlTipoVisita"
                                    runat="server">
                                </select>
                                <asp:Panel ID="PanelTransportista" runat="server" class="PanelOculto">
                                    <div class="control-group row">

                                        <div class="input-group form-group controls">
                                            <asp:TextBox ID="txtBatch" runat="server" class="form-control" placeholder="Especificar el N° batch a retirar" AutoCompleteType="Disabled" MaxLength="100"></asp:TextBox>
                                            <span class="input-group-addon">
                                                <input id="Button2" type="button" class="btn btn-success" value="Buscar" onclick="VerificaBatch()" />
                                            </span>

                                        </div>
                                        <div class="control-group">
                                            <div class="form-group controls">
                                                <p class="help-block text-danger">
                                                    <asp:Label ID="lblBatch" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                </p>
                                            </div>
                                        </div>
                                        <div class="control-group col-md-12 mx-auto">
                                            <div class="form-group controls">
                                                <asp:ListBox ID="lstbatch" runat="server" CssClass="form-control" onchange="borrarSeleccionbatch();" ToolTip="Click para eliminar"></asp:ListBox>
                                            </div>

                                        </div>
                                    </div>

                                </asp:Panel>
                            </div>
                        </div>

                        <asp:Panel ID="PanelOtroTipo" runat="server" class="PanelOculto">
                            <div class="control-group row">
                                <div class="form-group controls col-md-6">
                                </div>
                                <div class="form-group controls col-md-6">

                                    <asp:RadioButtonList ID="rblVisitaTecnica" runat="server" RepeatLayout="OrderedList">
                                        <%--<asp:ListItem Value="1">Ingreso sala telecomunicaciones</asp:ListItem>--%>
                                        <asp:ListItem Value="2">Ingreso tablero eléctrico</asp:ListItem>
                                        <asp:ListItem Value="3">Otro</asp:ListItem>
                                    </asp:RadioButtonList>

                                </div>
                            </div>
                        </asp:Panel>


                     
                        <div class="control-group">
                            <div class="form-group controls">
                                <asp:TextBox ID="txtMotivoIngreso" runat="server" class="form-control" placeholder="Especificar motivo ingreso" AutoCompleteType="Disabled" required data-validation-required-message="Ingrese." MaxLength="100"></asp:TextBox>
                                <p class="help-block text-danger"></p>
                            </div>
                        </div>
                        <br />

                        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                            <%--<Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rblTipoIngreso"
                                    EventName="onclick" />
                            </Triggers>--%>
                            <ContentTemplate>

                                <div>
                                    <div>
                                        <h3>
                                            <a>Tipo de ingreso</a></h3>
                                    </div>
                                </div>
                                <br />
                                <div class="control-group row">
                                    <div class="form-group controls col-md-12 mx-auto">
                                        <%--<div class="radio" onclick="tipoIngreso();" id="rblTipoIngreso">
                                            <div>
                                                <label>
                                                    <input type="radio" name="tipoIngreso" required value="1">
                                                    Ingreso peatonal
                                                </label>
                                            </div>

                                            <div>
                                                <label>
                                                    <input type="radio" name="tipoIngreso" required value="2">
                                                    Ingreso vehicular
                                                </label>
                                            </div>
                                        </div>--%>

                                        <asp:RadioButtonList ID="rblTipoIngreso" runat="server" RepeatLayout="OrderedList" onclick="tipoIngreso();" required>
                                            <asp:ListItem Value="1">Ingreso peatonal</asp:ListItem>
                                            <asp:ListItem Value="2">Ingreso vehicular</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>

                                </div>
                                <div>
                                </div>
                                <br />

                                <asp:Panel ID="PanelPeaton" runat="server" class="PanelOculto">

                                    <div>
                                        <div>
                                            <h3>
                                                <a>Datos solicitante</a></h3>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="control-group">
                                        <div class="form-group controls">
                                            <asp:TextBox ID="txtNombreSolicitante" runat="server" class="form-control" placeholder="Nombre y apellido" required MaxLength="100"></asp:TextBox>
                                            <p class="help-block text-danger"></p>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="form-group controls">
                                            <asp:TextBox ID="txtRutSolicitante" runat="server" class="form-control" placeholder="Rut o pasaporte, ej: 12345678-9" MaxLength="10" required onKeyPress="return rutPasaporte(event)"></asp:TextBox>
                                            <p class="help-block text-danger">
                                                <%--<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="* Rut inválido" ControlToValidate="txtRutSolicitante" ClientValidationFunction="validar_rut" SetFocusOnError="True"></asp:CustomValidator>--%>
                                            </p>
                                        </div>
                                    </div>
                                    <asp:Panel ID="PanelPatente" runat="server" class="PanelOculto">
                                        <div class="control-group">
                                            <div class="form-group controls">
                                                <asp:TextBox ID="txtPatente" runat="server" class="form-control" placeholder="Patente vehículo, ej: ATRX19" MaxLength="6"></asp:TextBox>
                                                <p class="help-block text-danger"></p>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div class="control-group">
                                        <div class="form-group controls">
                                            <asp:TextBox ID="txtCorreo" runat="server" class="form-control" placeholder="Correo" pattern="[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*@[a-zA-Z0-9_-]+([.][a-zA-Z0-9_-]+)*[.][a-zA-Z]{1,5}" required MaxLength="50"></asp:TextBox>
                                            <p class="help-block text-danger">
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" Display="Dynamic" ErrorMessage="* Correo inválido" ControlToValidate="txtCorreo" ClientValidationFunction="validaCorreo" SetFocusOnError="True"></asp:CustomValidator>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="form-group controls">
                                            <asp:TextBox ID="txtEmpresaSolicitante" runat="server" class="form-control" placeholder="Empresa" required MaxLength="50"></asp:TextBox>
                                            <p class="help-block text-danger"></p>
                                        </div>
                                    </div>
                                  <br />
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <%--<Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rblTipoIngreso"
                                    EventName="onclick" />
                            </Triggers>--%>
                            <ContentTemplate>
                                <div>
                                    <div>
                                        <h3>
                                            <a>Listado de acompañantes</a></h3>
                                    </div>
                                </div>
                                <br />

                                <div class="control-group row">
                                    <div class="form-group controls col-md-12 mx-auto">
                                        <%--<div class="radio" onclick="acompanante();" id="rblAcompanante">
                                    <div>
                                        <label>
                                            <input type="radio" name="acompanante" required value="1">
                                            Sí
                                        </label>
                                    </div>

                                    <div>
                                        <label>
                                            <input type="radio" name="acompanante" required value="0">
                                            No
                                        </label>
                                    </div>
                                </div>--%>

                                        <asp:RadioButtonList ID="rblAcompanante" runat="server" RepeatLayout="OrderedList" onclick="acompanante();" required>
                                            <asp:ListItem Value="1">Sí</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>

                                </div>

                                <asp:Panel ID="PanelAcompañante" runat="server" class="PanelOculto">
                                    <div>
                                        <div>
                                            <h3>
                                                <a>Ingrese acompañantes</a></h3>
                                        </div>
                                    </div>
                                    <br />
                                    <div class=" row">
                                        <div class="form-group controls col-md-12 mx-auto">
                                            <label>Todos los acompañantes deben ingresar por acceso peatonal.</label>
                                        </div>
                                    </div>
                                    <div class=" row">
                                        <div class="form-group controls col-md-6 mx-auto">
                                            <asp:TextBox ID="txtNombreNomina" runat="server" class="form-control" placeholder="Nombre y apellido" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="form-group controls col-md-6 mx-auto">
                                            <asp:TextBox ID="txtRutNomina" runat="server" class="form-control" placeholder="Rut o pasaporte, ej: 12345678-9" MaxLength="10" onKeyPress="return rutPasaporte(event)"></asp:TextBox>
                                            <p class="help-block text-danger">
                                                <%--<asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="* Rut inválido" ControlToValidate="txtRutNomina" ClientValidationFunction="validar_rut" SetFocusOnError="True"></asp:CustomValidator>--%>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="form-group">
                                            <button type="button" class="btn btn-default" id="btnAgregarNomina" onclick="llenaNomina();">Agregar</button>
                                        </div>
                                    </div>

                                    <div>
                                        <div class="form-group controls">
                                            <asp:ListBox ID="lbNomina" runat="server" CssClass="form-control" onchange="borrarSeleccion();" ToolTip="Click para eliminar"></asp:ListBox>
                                        </div>
                                    </div>
                                </asp:Panel>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <br />
                        <div id="success"></div>
                        <div class="form-group">
                            <asp:UpdatePanel ID="upSolicitud" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:UpdateProgress ID="upssConsulta" runat="server" AssociatedUpdatePanelID="upSolicitud" DisplayAfter="100" DynamicLayout="true" ClientIDMode="Static">
                                        <ProgressTemplate>
                                            <div class="modal2">
                                                <div class="centrado">
                                                    <img src="<%= ResolveUrl("~/img/AvionCargando.gif") %>" /><br />
                                                    Un momento...
                                                </div>
                                            </div>
                                        </ProgressTemplate>


                                    </asp:UpdateProgress>

                                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar solicitud" class="btn btn-primary" OnClick="btnEnviar_Click" OnClientClick="return enviarNomina();" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnEnviar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
