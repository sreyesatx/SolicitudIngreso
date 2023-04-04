<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="AgendarVisita.aspx.cs" Inherits="SistemaSolicitudIngreso.Ingreso.AgendarVisita" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>--%>
    <script type="text/javascript" src="js/sweetalert.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            var input = document.getElementById("ContentPlaceHolder1_rutIngreso").focus();
        }
        function deleteDashesOnKeyUp() {

            var tb = document.getElementById("ContentPlaceHolder1_rutIngreso");
            var key = event.which || event.keyCode || event.charCode;

            if ((tb.value.length == 0) && (key == 8) || (key == 46)) {
                document.getElementById("ContentPlaceHolder1_rutIngreso").value = "";
                limpiarControles();
                document.getElementById("ContentPlaceHolder1_rutIngreso").focus;
            }
        }

        function focusCampo() {
            var inputField = document.getElementById("ContentPlaceHolder1_rutIngreso");
            if (inputField != null && inputField.value.length != 0) {
                if (inputField.createTextRange) {
                    var FieldRange = inputField.createTextRange();
                    FieldRange.moveStart('character', inputField.value.length);
                    FieldRange.collapse();
                    FieldRange.select();
                    alert("a");
                } else if (inputField.selectionStart || inputField.selectionStart == '0') {
                    var elemLen = inputField.value.length;
                    inputField.selectionStart = elemLen;
                    inputField.selectionEnd = elemLen;
                    inputField.focus();
                    alert("b");
                }
            } else {
                inputField.focus();
            }
        }

        $("#btnvalida").click(function () {
            if (Fn.validaRut($("#txt_rut").val())) {
                $("#msgerror").html("El rut ingresado es válido :D");
            } else {
                $("#msgerror").html("El Rut no es válido :'( ");
            }
        });

        function ObtenerDatosSolicitante() {
            var obj = {};
            obj.rut = document.getElementById("ContentPlaceHolder1_rutIngreso").value;
            $.ajax({
                type: "POST",
                url: "AgendarVisita.aspx/ConsultaUltimoIngreso",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (data) {
                    var arrVal = data.d.split('|');
                    if (arrVal[0] == "OK") {
                        document.getElementById("ContentPlaceHolder1_txtNombreSolicitante").value = arrVal[1];
                        document.getElementById("ContentPlaceHolder1_txtCorreo").value = arrVal[2];
                        document.getElementById("ContentPlaceHolder1_txtRutSolicitante").value = document.getElementById("ContentPlaceHolder1_rutIngreso").value;
                        var txt1 = document.getElementById("<%=txtNombreSolicitante.ClientID%>");
                        var txt2 = document.getElementById("<%=txtCorreo.ClientID%>");
                        var txt3 = document.getElementById("<%=txtRutSolicitante.ClientID%>");
                        txt1.style.background = "#f5f6ff";
                        txt2.style.background = "#f5f6ff";
                        txt3.style.background = "#f5f6ff";
                    }
                    else {
                        document.getElementById("ContentPlaceHolder1_txtRutSolicitante").value = document.getElementById("ContentPlaceHolder1_rutIngreso").value;
                        var txt1 = document.getElementById("<%=txtRutSolicitante.ClientID%>");
                        txt1.style.background = "#f5f6ff";
                    }
                },
                error: function (result) {
                    alert(result);
                }
            });
        }

        function ObtenerDatosSolicitante_DIa() {
            var obj = {};
            obj.rut = document.getElementById("ContentPlaceHolder1_rutIngreso").value;
            $.ajax({
                type: "POST",
                url: "AgendarVisita.aspx/ConsultaUltimoIngreso_Dia",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (data) {
                    var arrVal = data.d.split('|');

                    document.getElementById("lblFechaSolicitud").innerHTML = "";
                    document.getElementById("lblEmpesasolicitud").innerHTML = "";
                    document.getElementById("lblnomSolicitante").innerHTML = "";
                    document.getElementById("lblhoraSolicitud").innerHTML = "";
                    if (arrVal[0] == "OK") {                        
                        var trid = document.getElementById("esperaCourier_tr");
                        if (trid != null) {
                            trid.style.display = 'none';
                        }
                        if (arrVal[4] == "Creado") {
                            document.getElementById("lblEstadoSolicitud").innerHTML = "En espera de aprobación Courier";
                            var trid = document.getElementById("esperaCourier_tr");
                            if (trid != null) {
                                trid.style.display = 'block';
                            }
                        }
                        if (arrVal[4] == "Aceptado") {
                            document.getElementById("lblEstadoSolicitud").innerHTML = "Aprobado por Courier en espera de autorización de Atrex";
                        }
                        if (arrVal[4] == "Rechazado") {
                            document.getElementById("lblEstadoSolicitud").innerHTML = "Ingreso rechazado por Courier";                            
                        }
                        if (arrVal[4] == "Anulado") {
                            document.getElementById("lblEstadoSolicitud").innerHTML = "Ingreso anulado por Atrex";                            
                        }
                        if (arrVal[4] == "Autorizado") {
                            document.getElementById("lblEstadoSolicitud").innerHTML = "Ingreso Autorizado";
                        }
                        if (arrVal[4] == "Denegado") {
                            document.getElementById("lblEstadoSolicitud").innerHTML = "Ingreso rechazado por Atrex";
                        }
                        if (arrVal[4] == "Terminado") {
                            document.getElementById("lblEstadoSolicitud").innerHTML = "Ingreso del día actual en estado Terminado";
                        }
                        if (arrVal[4] == "Ingresado") {
                            document.getElementById("lblEstadoSolicitud").innerHTML = "Ingresado";
                        }
                        document.getElementById("lblFechaSolicitud").innerHTML = arrVal[2];
                        document.getElementById("lblEmpesasolicitud").innerHTML = arrVal[5];
                        document.getElementById("lblnomSolicitante").innerHTML = arrVal[1];
                        document.getElementById("lblhoraSolicitud").innerHTML = arrVal[6];

                        document.getElementById('<%=panelSolicitud_Dia.ClientID%>').style.display = 'block';
                        document.getElementById('<%=panelOcultarCampos.ClientID%>').style.display = 'none';
                        window.scrollTo(0, 380);
                    }
                    else {
                        ObtenerDatosSolicitante();                        
                        document.getElementById('<%=panelSolicitud_Dia.ClientID%>').style.display = 'none';
                        document.getElementById('<%=panelOcultarCampos.ClientID%>').style.display = 'block';
                        window.scrollTo(0, 570);
                    }
                },
                error: function (result) {
                    alert(result);
                }
            });

        }

        function habilitaFormulario() {
            if (document.getElementById("ContentPlaceHolder1_rutIngreso").value == "")
            {
                swal('¡Debe ingresar número de documento!');
                limpiarControles();
                return;
            }
            document.getElementById("ContentPlaceHolder1_txtNombreSolicitante").value = "";
            document.getElementById("ContentPlaceHolder1_txtCorreo").value = "";
            document.getElementById("ContentPlaceHolder1_txtRutSolicitante").value = "";

            var txt1 = document.getElementById("<%=txtNombreSolicitante.ClientID%>");
            var txt2 = document.getElementById("<%=txtCorreo.ClientID%>");
            var txt3 = document.getElementById("<%=txtRutSolicitante.ClientID%>");
            txt1.style.background = "#fff";
            txt2.style.background = "#fff";
            txt3.style.background = "#fff";

            if (document.getElementById('radioRut').checked) {
                if (checkRut(document.getElementById("ContentPlaceHolder1_rutIngreso").value)) {
                    ObtenerDatosSolicitante_DIa();
                    return;
                }
                else {
                    swal('¡Rut ingresado no es Válido!');
                    document.getElementById("ContentPlaceHolder1_rutIngreso").value = "";
                    var input = document.getElementById("ContentPlaceHolder1_rutIngreso").focus();
                    document.getElementById('<%=panelOcultarCampos.ClientID%>').style.display = 'none';
                }
            }
            else {
                ObtenerDatosSolicitante_DIa();
                
            }
        }
        //
        function habilitanuevaSolicitud() {
            ObtenerDatosSolicitante();
            document.getElementById('<%=panelSolicitud_Dia.ClientID%>').style.display = 'none';
            document.getElementById('<%=panelOcultarCampos.ClientID%>').style.display = 'block';
            window.scrollTo(0, 570);
        }

        function ModifyPlaceHolder(element) {
            limpiarControles();
            var data = '';
            switch (element) {
                case 'radioRut':
                    data = 'Ingrese Rut ej: 12.345.678-9';
                    break;
                case 'radioPasaporte':
                    data = 'Ingrese Pasaporte';
                    break;
            }
            document.getElementById("ContentPlaceHolder1_rutIngreso").value = "";
            document.getElementById("ContentPlaceHolder1_rutIngreso").placeholder = data;
            document.getElementById("ContentPlaceHolder1_rutIngreso").removeAttribute("readonly");
            document.getElementById("ContentPlaceHolder1_rutIngreso").focus();
        }

        function limpiarControles() {
            document.getElementById("ContentPlaceHolder1_txtNombreSolicitante").value = "";
            document.getElementById("ContentPlaceHolder1_txtCorreo").value = "";
            document.getElementById("ContentPlaceHolder1_txtRutSolicitante").value = "";

            var txt1 = document.getElementById("<%=txtNombreSolicitante.ClientID%>");
            var txt2 = document.getElementById("<%=txtCorreo.ClientID%>");
            var txt3 = document.getElementById("<%=txtRutSolicitante.ClientID%>");
            txt1.style.background = "#fff";
            txt2.style.background = "#fff";
            txt3.style.background = "#fff";

            document.getElementById("lblFechaSolicitud").innerHTML = "";
            document.getElementById("lblEmpesasolicitud").innerHTML = "";
            document.getElementById("lblnomSolicitante").innerHTML = "";
            document.getElementById("lblhoraSolicitud").innerHTML = "";

            document.getElementById('<%=panelSolicitud_Dia.ClientID%>').style.display = 'none';
            document.getElementById('<%=panelOcultarCampos.ClientID%>').style.display = 'none';
        }

        function formateaRut(rut) {           
            if (document.getElementById('radioRut').checked) {
                var actual = rut.value.replace(/^0+/, "");
                if (actual != '' && actual.length > 1) {
                    var sinPuntos = actual.replace(/\./g, "");
                    var actualLimpio = sinPuntos.replace(/-/g, "");
                    var inicio = actualLimpio.substring(0, actualLimpio.length - 1);
                    var rutPuntos = '';
                    var i = 0;
                    var j = 1;
                    for (i = inicio.length - 1; i >= 0; i--) {
                        var letra = inicio.charAt(i);
                        rutPuntos = letra + rutPuntos;
                        if (j % 3 == 0 && j <= inicio.length - 1) {
                            rutPuntos = "." + rutPuntos;
                        }
                        j++;
                    }
                    var dv = actualLimpio.substring(actualLimpio.length - 1);
                    rutPuntos = rutPuntos + "-" + dv;
                }
                if (typeof rutPuntos !== "undefined") {
                    document.getElementById("ContentPlaceHolder1_rutIngreso").value = rutPuntos;                    
                }                
                //document.getElementById("ContentPlaceHolder1_rutIngreso").focus();
               // focusCampo();

                var searchInput = $("#ContentPlaceHolder1_rutIngreso");

                searchInput
                  .putCursorAtEnd() // should be chainable
                  .on("focus", function () { // could be on any event
                      searchInput.putCursorAtEnd()
                  });
            }
        }

        function formatCliente(txtRutEntrada) {
            txtRutEntrada.value = txtRutEntrada.value.replace(/[.-]/g, '')
            .replace(/^(\d{1,2})(\d{3})(\d{3})(\w{1})$/, '$1.$2.$3-$4')
        }

        function checkRut(rut) {
            rut = rut.replace('.', '');
            rut = rut.replace('.', '');
            rut = rut.replace('-', '');

            var valor = rut;
            cuerpo = valor.slice(0, -1);
            dv = valor.slice(-1).toUpperCase();

            suma = 0;
            multiplo = 2;
            for (i = 1; i <= cuerpo.length; i++) {
                index = multiplo * valor.charAt(cuerpo.length - i);
                suma = suma + index;
                if (multiplo < 7) {
                    multiplo = multiplo + 1;
                } else {
                    multiplo = 2;
                }
            }
            dvEsperado = 11 - (suma % 11);

            dv = dv == "K" ? 10 : dv;
            dv = dv == 0 ? 11 : dv;

            if (dvEsperado != dv) {
                return false;
            } else {
                return true;
            }
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
            var fechaVisita = document.getElementById("ContentPlaceHolder1_txtFecha").value;
            var ddlHora = document.getElementById("ContentPlaceHolder1_ddlHora").selectedIndex;
            var ddlMinutos = document.getElementById("ContentPlaceHolder1_ddlMinutos").selectedIndex;
            var txtMotivoIngreso = document.getElementById("ContentPlaceHolder1_txtMotivoIngreso").value;
            var rblTipoIngreso = $("[id$='rblTipoIngreso']").find(":checked").val();

            var txtNombreSolicitante = document.getElementById("ContentPlaceHolder1_txtNombreSolicitante").value;
            var txtRutSolicitante = document.getElementById("ContentPlaceHolder1_txtRutSolicitante").value;
            var txtPatente = document.getElementById("ContentPlaceHolder1_txtPatente").value;
            var txtCorreo = document.getElementById("ContentPlaceHolder1_txtCorreo").value;
            var txtEmpresaSolicitante = document.getElementById("ContentPlaceHolder1_txtEmpresaSolicitante").value;
            var txtCargoSolicitante = document.getElementById("ContentPlaceHolder1_txtCargoSolicitante").value;

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

            if (fechaVisita == "") {

                valida.text += "- Ingrese fecha de visita.\n";
            }

            if (ddlHora == 0) {

                valida.text += "- Seleccione hora visita.\n";
            }
            if (ddlMinutos == 0) {

                valida.text += "- Seleccione minutos.\n";
            }

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
                if (txtCargoSolicitante == "") {

                    valida.text += "- Ingrese cargo.\n";
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
        //
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
                var ddlEmpresas = document.getElementById("ContentPlaceHolder1_ddlEmpresa").selectedIndex;
                if (ddlEmpresas != "20" && selectedVal == "2") {
                    document.getElementById('<%= PanelPeaton.ClientID %>').style.display = 'block';
                    document.getElementById('<%=PanelPatente.ClientID%>').style.display = 'block';

                    nombre.placeholder = "Nombre y apellido conductor";
                    rut.placeholder = "Rut conductor o pasaporte, ej: 12345678-9";
                    patente.required = true;                   
                }
                else{                  
                    nombre.placeholder = "Nombre y apellido conductor";
                    rut.placeholder = "Rut conductor o pasaporte, ej: 12345678-9";
                    patente.required = false;

                    
                    var rad = document.getElementById('<%=rblTipoIngreso.ClientID %>');
                    var radio = rad.getElementsByTagName("input");
                    if (radio[1].checked) {
                        radio[0].checked = true;
                    }
                                    
                    swal({
                        title: 'El ingreso a sala Telecom es solamente Peatonal. ',
                        showCancelButton: true,
                        showConfirmButton: false,
                        showDenyButton: true,
                        cancelButtonText: 'Cancelar',
                        cancelButtonClass: 'btn btn-danger',
                        type: 'warning',
                        buttonsStyling: false
                    });
                }
            }
        }
        //
        function acompanante() {

            var selectedVal = $("[id$='rblAcompanante']").find(":checked").val();

            if (selectedVal == "1") {
                document.getElementById('<%=PanelAcompañante.ClientID%>').style.display = 'block';
            }
            else {
                document.getElementById('<%=PanelAcompañante.ClientID%>').style.display = 'none';
            }

        }
        //
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
		
			document.getElementById('<%=PanelPeaton.ClientID%>').style.display = 'block';
			document.getElementById('<%=PanelPatente.ClientID%>').style.display = 'none';

			nombre.placeholder = "Nombre y apellido";
			rut.placeholder = "Rut o pasaporte, ej: 12345678-9";
			patente.required = false;		

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
        //
        function espera() {

            $get('upssConsulta').style.display = 'block';
        }
        //
        function rutPasaporte(e) {
            var key = window.Event ? e.which : e.keyCode
            return (key >= 48 && key <= 57) || (key == 13) || (key == 45) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122)
        }
        //
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

        jQuery.fn.putCursorAtEnd = function () {

            return this.each(function () {

                // Cache references
                var $el = $(this),
                    el = this;

                // Only focus if input isn't already
                if (!$el.is(":focus")) {
                    $el.focus();
                }

                // If this function exists... (IE 9+)
                if (el.setSelectionRange) {

                    // Double the length because Opera is inconsistent about whether a carriage return is one character or two.
                    var len = $el.val().length * 2;

                    // Timeout seems to be required for Blink
                    setTimeout(function () {
                        el.setSelectionRange(len, len);
                    }, 1);

                } else {

                    // As a fallback, replace the contents with itself
                    // Doesn't work in Chrome, but Chrome supports setSelectionRange
                    $el.val($el.val());

                }

                // Scroll to the bottom, in case we're in a tall textarea
                // (Necessary for Firefox and Chrome)
                this.scrollTop = 999999;

            });

        };
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

        .radio-toolbar input[type="radio"] {
          opacity: 0;
          position: fixed;
          width: 0;
        }
        .radio-toolbar label {
            display: inline-block;
            background-color: #4f66f7;
            padding: 10px 20px;
            font-family: sans-serif, Arial;
            font-size: 14px;
            color: #f7f9ff;
            border: 0px solid #444;
            border-radius: 5px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.1), 0 4px 5px 0 rgba(0, 0, 0, 0.19);
            height: 40px;
            -webkit-user-select: none; /* Chrome all / Safari all */
            -moz-user-select: none;    /* Firefox all             */
            -ms-user-select: none;     /* Internet Explorer  10+  */
            user-select: none;         /* Likely future           */
        }
        .radio-toolbar input[type="radio"]:checked + label {
            background-color:#48c248;
            border-color: #4c4;
            box-shadow: 0 2px 2px rgba(0, 0, 0, 0.3);
        }
        .radio-toolbar input[type="radio"]:focus + label {
            border: 0px solid #444;
        }
        .radio-toolbar label:hover {
          background-color: #3553cc;
           cursor:pointer;
        }
        #frame1 {
          border: 0px solid;
          padding: 10px;
          box-shadow: 0 4px 9px 0 rgba(0, 0, 0, 0.1), 0 4px 6px 0 rgba(0, 0, 0, 0.19);
          background-color: #fafafa;                 
        }
        #margenText {
          margin-top: 10px;
          margin-left: 40px;                
        }
        #margenText2 {
          margin-top: 10px;
          margin-left: 180px;                
        }


        table {
            border: 0px ;
            border-collapse: collapse;
            width:100%;
        }

        table td {
            border: 0px ;
        }

        table td.shrink {
            white-space:nowrap
               
        }
        table td.expand {
            width: 99%
        }

        #orderName {
            position: absolute;
            margin: 0;
            left: 50%;
            margin-right: -50%;
            transform: translate(-50%, -50%);
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
                        <asp:Panel ID="panelControlesInicio" runat="server" >
                            <div class="control-group">
                                <div class="form-group controls">
                                    <label>Seleccione tipo de documento:</label>
                                    <div class="radio-toolbar">                                     
                                        <input type="radio" id="radioRut" value="rut" onclick="ModifyPlaceHolder(this.id)" name="s.cmd" checked>
                                        <label for="radioRut">RUT</label>
                                        <input type="radio" id="radioPasaporte" value="pasaporte" onclick="ModifyPlaceHolder(this.id)" name="s.cmd">
                                        <label for="radioPasaporte">Pasaporte</label>
                                    </div>
                                    <asp:TextBox ID="rutIngreso" placeholder="Ingrese Rut ej: 12.345.678-9"  runat="server" class="form-control"  MaxLength="12" required oninput="formateaRut(this)" onkeyup="deleteDashesOnKeyUp(this)" ></asp:TextBox>
                                </div>
                                <input id="btnConsultar" type="button" class="btn btn-success" value="Consultar o ingresar nueva solicitud" Onclick="habilitaFormulario()"/>
                           </div>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="panelSolicitud_Dia" runat="server" class="PanelOculto" >
                            <div id="frame1" style="overflow-x:auto;">
                                <br />
                                <label id="orderName"><u><b>Detalle de solicitud</b></u></label>
                                <br />
                                <table>
                                  <tr>
                                    <td class="shrink"><label><b>Nombre Solicitante:&nbsp</b></label></td>
                                    <td class="expand"><label id="lblnomSolicitante"></label></td>
                                  </tr>
                                </table>
                                <br />
                                <table >
                                  <tr>                                      
                                    <td class="shrink"><label><b>Fecha Visita:&nbsp</b></label></td>
                                    <td class="shrink"><label id="lblFechaSolicitud"></label></td>
                                  </tr>
                                  <tr>                                      
                                    <td class="shrink"><label><b>Hora Visita:&nbsp</b></label></td>
                                    <td class="expand"><label id="lblhoraSolicitud"></label></td>
                                  </tr>

                                  <tr>
                                    <td class="shrink"><label><b>Estado Solicitud:&nbsp</b></label></td>
                                    <td class="shrink"><label id="lblEstadoSolicitud"></label></td>
                                  </tr>
                                </table>              
                                <table id="esperaCourier_tr">
                                  <tr>
                                    <td ><label><b>Esperando Autorización de Courier:&nbsp</b></label></td>
                                    <td class="expand"><label id="lblEmpesasolicitud"></label></td>
                                  </tr>
                                </table>                                                
                            </div>
                            <br />
                            <input id="btnNuevaSolicitud" type="button" class="btn btn-success" value="Ingresar nueva solicitud" Onclick="habilitanuevaSolicitud()"/> 
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="panelOcultarCampos" runat="server" class="PanelOculto">
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
                        <div class="control-group row">
                            <div class="form-group controls col-md-6 mx-auto">
                                <label for="txtFecha">Fecha visita</label>
                                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" AutoCompleteType="Disabled" required data-validation-required-message="Ingrese." MaxLength="10" pattern="^([0-2][0-9]|(3)[0-1])(-)(((0)[0-9])|((1)[0-2]))(-)\d{4}$"></asp:TextBox>
                                <cc1:CalendarExtender ID="calFechaVisita" runat="server" TargetControlID="txtFecha" Format="dd-MM-yyyy" FirstDayOfWeek="Monday"></cc1:CalendarExtender>
                            </div>
                            <div class="form-group controls col-md-3 mx-auto">
                                <label for="ddlHora">Hora</label>
                                <select id="ddlHora" class="browser-default custom-select" required
                                    name="ddlHora"
                                    runat="server">
                                    <option value="">--</option>
                                    <option value="00">00</option>
                                    <option value="01">01</option>
                                    <option value="02">02</option>
                                    <option value="03">03</option>
                                    <option value="04">04</option>
                                    <option value="05">05</option>
                                    <option value="06">06</option>
                                    <option value="07">07</option>
                                    <option value="08">08</option>
                                    <option value="09">09</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                    <option value="13">13</option>
                                    <option value="14">14</option>
                                    <option value="15">15</option>
                                    <option value="16">16</option>
                                    <option value="17">17</option>
                                    <option value="18">18</option>
                                    <option value="19">19</option>
                                    <option value="20">20</option>
                                    <option value="21">21</option>
                                    <option value="22">22</option>
                                    <option value="23">23</option>
                                </select>
                            </div>
                            <div class="form-group controls col-md-3 mx-auto">
                                <label for="ddlMinutos">Minutos</label>
                                <select id="ddlMinutos" class="browser-default custom-select" required
                                    name="ddlMinutos"
                                    runat="server">
                                    <option value="">--</option>
                                    <option value="00">00</option>
                                    <option value="15">15</option>
                                    <option value="30">30</option>
                                    <option value="45">45</option>
                                </select>
                            </div>
                        </div>
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
                                            <asp:ListItem Value="1" Selected="True">Ingreso peatonal</asp:ListItem>
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
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="* Correo inválido" ControlToValidate="txtCorreo" ClientValidationFunction="validaCorreo" SetFocusOnError="True"></asp:CustomValidator>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="form-group controls">
                                            <asp:TextBox ID="txtEmpresaSolicitante" runat="server" class="form-control" placeholder="Empresa" required MaxLength="50"></asp:TextBox>
                                            <p class="help-block text-danger"></p>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="form-group controls">
                                            <asp:TextBox ID="txtCargoSolicitante" runat="server" class="form-control" placeholder="Cargo" required MaxLength="50"></asp:TextBox>
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
                                            <asp:ListItem Value="0">No</asp:ListItem>
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
                        </asp:Panel>
                    </form>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
