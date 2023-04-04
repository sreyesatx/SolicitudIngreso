using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolicitudValidar;
using SistemaSolicitudIngreso.Correo;


namespace SistemaSolicitudIngreso.Ingreso
{
    public partial class AgendarVisita : System.Web.UI.Page
    {
        private List<Acompanantes> listadoCompadres
        {
            get { return (List<Acompanantes>)Session.Contents["Acompanantes"]; }
            set { Session.Contents["Acompanantes"] = value; }
        }

        private int _Solicitud
        {
            get
            {
                if (ViewState["_Solicitud"] != null)
                    return (int)(ViewState["_Solicitud"]);
                else
                    return 0;
            }
            set { ViewState["_Solicitud"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //txtBatch.Attributes.Add("onchange", "VerificaBatch()");
                //Select1.DataSource = sdsEmpresas;
                //Select1.DataTextField = "nombre_empresa";
                //Select1.DataValueField = "id_empresa";
                //Select1.DataBind();
                //Select1.Items.Insert(0, new ListItem("--Seleccione--", ""));

                ddlEmpresa.DataSource = sdsEmpresas;
                ddlEmpresa.DataTextField = "nombre_empresa";
                ddlEmpresa.DataValueField = "id_empresa";
                ddlEmpresa.DataBind();
                ddlEmpresa.Items.Insert(0, new ListItem("--Seleccione--", ""));

                ddlTipoVisita.DataSource = sdsTipoVisita;
                ddlTipoVisita.DataTextField = "descripcion_tipo_visita";
                ddlTipoVisita.DataValueField = "id_tipo_visita";
                ddlTipoVisita.DataBind();
                ddlTipoVisita.Items.Insert(0, new ListItem("--Seleccione--", ""));

                calFechaVisita.StartDate = DateTime.Now.Date;
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string encargado = ""
                , correo = ""
                , copiaCorreo = ""
                , mensaje = ""
                , fechaVisita = ""
                , empresa = ""
                , motivo = ""
                , autorizar = ""
                , denegar = ""
                , posdata = ""
                , nombre_contacto = ""
                , visitas = ""
                , webRespuesta = ""
                , solicitud = ""
                , otro_indicar = "";
            try
            {
                if (rblTipoIngreso.SelectedValue == "2" && ddlEmpresa.Items[ddlEmpresa.SelectedIndex].Value.ToString() == "31")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('El ingreso a sala Telecom es solamente Peatonal.');", Title), true);
                    return;
                }				
                string valida = validaFormulario();
                if (valida == string.Empty)
                {
                    //
            
                    encargado = correo = copiaCorreo = mensaje = fechaVisita = empresa = motivo = autorizar = denegar = posdata = nombre_contacto = visitas = webRespuesta = solicitud = otro_indicar = string.Empty;
                    //
                    if (ddlTipoVisita.Items[ddlTipoVisita.SelectedIndex].Text.ToString() == "VISITA TÉCNICA")
                    {
                        if (rblVisitaTecnica.SelectedItem != null)
                        {
                            otro_indicar = rblVisitaTecnica.SelectedItem.Text;
                        }
                        else
                        {
                            otro_indicar = "Otro";
                        }
                    }
                    fechaVisita = txtFecha.Text.Trim() + " " + ddlHora.Items[ddlHora.SelectedIndex].Text + ":" + ddlMinutos.Items[ddlMinutos.SelectedIndex].Text;

                    webRespuesta = "<a href='https://couriers.cl/ingreso/RespuestaCorreo.aspx?nombre_contacto=";

                    sdsSolicitud.InsertParameters.Clear();
                    sdsSolicitud.InsertParameters.Add("id_empresa", ddlEmpresa.Items[ddlEmpresa.SelectedIndex].Value.ToString());
                    sdsSolicitud.InsertParameters.Add("id_tipo_visita", ddlEmpresa.Items[ddlEmpresa.SelectedIndex].Value.ToString() == "32" ? "9" : ddlTipoVisita.Items[ddlTipoVisita.SelectedIndex].Value.ToString());
                    sdsSolicitud.InsertParameters.Add("otro_indicar", otro_indicar);
                    sdsSolicitud.InsertParameters.Add("fecha_visita", fechaVisita);
                    sdsSolicitud.InsertParameters.Add("motivo_ingreso", txtMotivoIngreso.Text.Trim());
                    //sdsSolicitud.InsertParameters.Add("id_tipo_ingreso", Request.Form["tipoIngreso"].ToString());
                    sdsSolicitud.InsertParameters.Add("id_tipo_ingreso", rblTipoIngreso.SelectedValue);
                    sdsSolicitud.InsertParameters.Add("fecha_solicitud", DateTime.Now.ToString());
                    sdsSolicitud.InsertParameters.Add("nombre_solicitante", txtNombreSolicitante.Text.Trim());
                    sdsSolicitud.InsertParameters.Add("rut_solicitante", txtRutSolicitante.Text.Trim().Replace(".", ""));
                    sdsSolicitud.InsertParameters.Add("patente_vehiculo", txtPatente.Text.Trim());
                    sdsSolicitud.InsertParameters.Add("correo_solicitante", txtCorreo.Text.Trim());
                    sdsSolicitud.InsertParameters.Add("empresa_solicitante", txtEmpresaSolicitante.Text.Trim());
                    sdsSolicitud.InsertParameters.Add("cargo_solicitante", txtCargoSolicitante.Text.Trim());
                    //sdsSolicitud.InsertParameters.Add("listado_acompanantes", Request.Form["acompanante"].ToString());
                    sdsSolicitud.InsertParameters.Add("listado_acompanantes", rblAcompanante.SelectedValue);
                    sdsSolicitud.InsertParameters.Add("id_estado", "1"); //creado
                    sdsSolicitud.InsertParameters.Add("hora_ingreso", "");
                    sdsSolicitud.InsertParameters.Add("hora_salida", "");
                    sdsSolicitud.Insert();

                    visitas += (txtRutSolicitante.Text.Trim().Replace(".", "") + " " + txtNombreSolicitante.Text.Trim()) + "<br />";

                    solicitud = _Solicitud.ToString();

                    fechaVisita = "Fecha y hora visita: " + fechaVisita;

                    //if (Request.Form["acompanante"].ToString() == "1")
                    //{
                    if (rblAcompanante.SelectedValue == "1")
                    {
                        foreach (var item in listadoCompadres)
                        {
                            sdsAcompanantes.InsertParameters.Clear();
                            sdsAcompanantes.InsertParameters.Add("nombre", item.nombre);
                            sdsAcompanantes.InsertParameters.Add("rut", item.rut);
                            sdsAcompanantes.InsertParameters.Add("id_solicitud", solicitud);
                            sdsAcompanantes.Insert();

                            visitas += (item.rut + " " + item.nombre) + "<br />";
                        }
                    }


                    if (ddlEmpresa.Items[ddlEmpresa.SelectedIndex].Value == "32")
                    {
                        string[] arreglobatch = HiddenField1.Value.Split('|');
                        for (int i = 0; i < arreglobatch.Length - 1; i++)
                        {
                            string[] batch = arreglobatch[i].Split('/');
                            sdsBatchRetiro.InsertParameters.Clear();
                            sdsBatchRetiro.InsertParameters.Add("numerobatch", batch[0].ToString());
                            sdsBatchRetiro.InsertParameters.Add("id_solicitud", solicitud);
                            sdsBatchRetiro.InsertParameters.Add("resumen_batch", batch[1].ToString());
                            sdsBatchRetiro.Insert();
                        }
                    }

                    //BUSCA CONTACTOS

                    sdsContactos.SelectParameters.Clear();
                    sdsContactos.SelectParameters.Add("id_empresa", ddlEmpresa.Items[ddlEmpresa.SelectedIndex].Value.ToString());
                    DataView datos = (DataView)sdsContactos.Select(DataSourceSelectArguments.Empty);
                    if (datos != null)
                    {
                        if (datos.Count != 0)
                        {
                            //for (int i = 0; i < datos.ToTable().Rows.Count; i++)
                            //{
                            //    encargado += (datos.ToTable().Rows[0]["nombre_contacto"].ToString().Trim()) + ", ";
                            //}

                            encargado = (datos.ToTable().Rows[0]["nombre_empresa"].ToString().Trim()) + ", ";

                            for (int i = 0; i < datos.ToTable().Rows.Count; i++)
                            {
                                nombre_contacto = (datos.ToTable().Rows[i]["nombre_contacto"].ToString().Trim());
                                correo = (datos.ToTable().Rows[i]["correo_contacto"].ToString().Trim());                                
                                //
                                mensaje = "Las siguientes personas estan solicitando acceso a las instalaciones por el motivo descrito a continuación:";
                                empresa = "Empresa: " + txtEmpresaSolicitante.Text.Trim();
                                motivo = ddlTipoVisita.Items[ddlTipoVisita.SelectedIndex].Text + " - " + txtMotivoIngreso.Text.Trim();
                                autorizar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=2' target='_blank'>AUTORIZAR</a>";
                                denegar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=3' target='_blank' style='color: rgb(230,0,38)'>DENEGAR</a>";
                                posdata = "FAVOR AUTORIZAR O DENEGAR ACCESO PRESIONANDO EN LOS LINKS, NO REENVIAR ESTE CORREO.";
                                //
                                if (ddlEmpresa.Items[ddlEmpresa.SelectedIndex].Value == "32")
                                {
                                    string url = "<a target='_blank' rel='nofollow noopener' href='https://couriers.cl/PortalAduana/Batch/CierreBatch_Import.aspx?numero_batch=PTX500'> Ver</a>";

                                    motivo += "</br></br>AEROPOST CHILE S.A. ";
                                    motivo += "</br></br>N° de batch a retirar ";
                                    motivo += "</br></br>" + HiddenField1.Value.Replace("|", url);
                                    //   motivo += "<a target='_blank' rel='nofollow noopener' href='https://couriers.cl/PortalAduana/Batch/CierreBatch_Import.aspx?numero_batch=PTX500'>Ver</a>";

                                }
                                //   Correo.Correo.CrearCorreo("cesar.silva@atrexchile.cl", "cesar.silva@atrexchile.cl", copiaCorreo,mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);
                                Correo.Correo.CrearCorreo(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);
                            }

                            //ENVIAR CORREO A SEBASTIAN CON COPIA A ROMINA
                            //encargado = "Sebastian, ";
                            //nombre_contacto = "Sebastian Osorio"; //ASIGNAR ID ADMNISTRACION


                            //////////////////////////////////
                            

                            //mensaje = "Las siguientes personas estan solicitando acceso a las instalaciones por el motivo descrito a continuación:";
                            //empresa = "Empresa: " + txtEmpresaSolicitante.Text.Trim();
                            //motivo = ddlTipoVisita.Items[ddlTipoVisita.SelectedIndex].Text + " - " + txtMotivoIngreso.Text.Trim();
                            //autorizar = string.Empty;
                            //denegar = string.Empty;
                            //posdata = "Para ingresar al sistema diríjase a: http://bit.ly/Atrex";

                            //Correo.Correo.CrearCorreo(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);

                            //////////////////////////////////
                        }
                        else
                        {
                            //ENVIAR CORREO A SEBASTIAN CON COPIA A ROMINA
                            encargado = "Control Acceso Atrex, ";
                            nombre_contacto = "AtrexChile"; //ASIGNAR ID ADMNISTRACION
                            correo = "control@atrexchile.cl";

                            mensaje = "Las siguientes personas estan solicitando acceso a las instalaciones por el motivo descrito a continuación:";
                            empresa = "Empresa: " + txtEmpresaSolicitante.Text.Trim();
                            motivo = ddlTipoVisita.Items[ddlTipoVisita.SelectedIndex].Text + " - " + txtMotivoIngreso.Text.Trim();
                            autorizar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=2' target='_blank'>AUTORIZAR</a>";
                            denegar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=3' target='_blank' style='color: rgb(230,0,38)'>DENEGAR</a>";
                            posdata = "FAVOR AUTORIZAR O DENEGAR ACCESO PRESIONANDO EN LOS LINKS, NO REENVIAR ESTE CORREO. <br /> <br /> Para ingresar al sistema presione <a href='https://couriers.cl/ingreso/IngresoSistema.aspx' target='_blank'>AQUI</a>";

                            Correo.Correo.CrearCorreo(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);
                        }
                    }

                    limpiarFormulario();
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Solicitud creada correctamente, espere aprobación del Courier y Atrex vía correo electrónico antes de llegar a portería.');", Title), true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "swal", string.Format("swal('Falta completar los siguientes campos:  " + valida + "');", Title), true);
                }
            }
            catch (Exception ex)
            {
                Correo.Correo.CrearCorreo(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
            }
        }
        protected void sdsSolicitud_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            _Solicitud = Convert.ToInt32(e.Command.Parameters["@id_solicitud"].Value);
        }
        protected void sdsSolicitud_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            try
            {
                SqlParameter insertedKey2 = new SqlParameter("@id_solicitud", SqlDbType.Int);
                insertedKey2.Direction = ParameterDirection.Output;
                e.Command.Parameters.Add(insertedKey2);
            }
            catch
            {
            }
        }
        //
        protected void limpiarFormulario()
        {
            ddlEmpresa.SelectedIndex = 0;
            ddlTipoVisita.SelectedIndex = 0;
            rblVisitaTecnica.ClearSelection();
            txtFecha.Text = string.Empty;
            ddlHora.SelectedIndex = 0;
            ddlMinutos.SelectedIndex = 0;
            txtMotivoIngreso.Text = string.Empty;
            rblTipoIngreso.ClearSelection();
            txtNombreSolicitante.Text = string.Empty;
            txtRutSolicitante.Text = string.Empty;
            txtPatente.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtEmpresaSolicitante.Text = string.Empty;
            txtCargoSolicitante.Text = string.Empty;
            rblAcompanante.ClearSelection();
            lbNomina.Items.Clear();
        }
        //
        protected string validaFormulario()
        {

            string valida = string.Empty;

            string dato = ddlEmpresa.Items[ddlEmpresa.SelectedIndex].Value.ToString();

            if (this.listadoCompadres != null)
                this.listadoCompadres.Clear();

            if (ddlEmpresa.SelectedIndex.ToString() == "")
            {
                valida += "- Seleccione empresa.";
            }
            if (dato != "32")
            {
                if (ddlTipoVisita.SelectedIndex.ToString() == "")
                {
                    valida += "- Seleccione tipo de visita.";
                }
            }
            else
            {

                if (HiddenField1.Value == "")
                {
                    valida += " Ingrese Batch a retirar.";
                }
            }
            //if (ddlTipoVisita.SelectedIndex.ToString() == "5")
            //{
            //    if (txtOtroTipo.Text.Trim() == string.Empty)
            //        valida += "- Indique otro tipo de visita.";
            //}
            if (txtFecha.Text.Trim() == string.Empty)
            {
                valida += "- Ingrese fecha visita.";
            }
            if (ddlHora.SelectedIndex.ToString() == "")
            {
                valida += "- Seleccione hora.";
            }
            if (ddlMinutos.SelectedIndex.ToString() == "")
            {
                valida += "- Seleccione minutos.";
            }
            if (txtMotivoIngreso.Text.Trim() == string.Empty)
            {
                valida += "- Ingrese motivo de ingreso.";
            }
            //if (Request.Form["tipoIngreso"] == null)
            //{
            //    valida += "- Seleccione tipo de ingreso.";
            //}
            if (rblTipoIngreso.SelectedItem == null)
            {
                valida += " Seleccione tipo de ingreso,";
            }
            if (txtNombreSolicitante.Text.Trim() == string.Empty)
            {
                valida += "- Ingrese nombre y apellido solicitante.";
            }
            if (txtRutSolicitante.Text.Trim() == string.Empty)
            {
                valida += "- Ingrese rut solicitante.";
            }
            //if (!Validar.ValidaRut(txtRutSolicitante.Text.Trim()))
            //{
            //    valida += "- el rut de " + txtNombreSolicitante.Text.Trim() + " no es válido.";
            //}
            //if (Request.Form["tipoIngreso"].ToString() == "2")
            //{
            //    if (txtPatente.Text.Trim() == string.Empty)
            //        valida += "- Ingrese patente vehículo.";
            //}
            if (rblTipoIngreso.SelectedValue == "2")
            {
                if (txtPatente.Text.Trim() == string.Empty)
                    valida += " Ingrese patente vehículo,";
            }
            if (txtCorreo.Text.Trim() == string.Empty)
            {
                valida += "- Ingrese correo solicitante.";
            }
            if (txtEmpresaSolicitante.Text.Trim() == string.Empty)
            {
                valida += "- Ingrese empresa solicitante.";
            }
            if (txtCargoSolicitante.Text.Trim() == string.Empty)
            {
                valida += "- Ingrese cargo solicitante.";
            }
            //if (Request.Form["acompanante"] == null)
            //{
            //    valida += "- Seleccione listado de acompañantes.";
            //}
            if (rblAcompanante.SelectedItem == null)
            {
                valida += " Seleccione listado de acompañantes,";
            }
            //if (Request.Form["acompanante"].ToString() == "1")
            //{
            //    if (Request.Form["confirm_value"] == null || Request.Form["confirm_value"] == "")
            //        valida += "- Ingrese acompañantes.";
            if (rblAcompanante.SelectedValue == "1")
            {
                if (Request.Form["confirm_value"] == null)
                    valida += " Ingrese acompañantes.";
                else
                {
                    string confirmValue = Request.Form["confirm_value"];
                    string[] lista = Request.Form["confirm_value"].Split(',');

                    foreach (var item in lista)
                    {
                        string[] compadre = item.ToString().Split('/');
                        string rut = compadre[0].Trim();
                        string nombre = compadre[1].Trim();

                        if (!Validar.ValidaRut(rut.Trim()))
                        {
                            valida += "- el rut de " + nombre + " no es válido.";
                        }
                        else
                        {
                            var colega = new Acompanantes();
                            colega.ID = Guid.NewGuid();
                            colega.rut = rut;
                            colega.nombre = nombre;

                            if (this.listadoCompadres == null)
                            {
                                this.listadoCompadres = new List<Acompanantes> { colega };
                            }
                            else
                            {
                                this.listadoCompadres.Add(colega);
                            }
                        }
                    }
                }
            }
            return valida;
        }
        [WebMethod]
        public static string busca_batch(string batch, string empresa)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            string Respuesta = "NO EXISTE BATCH PARA RETIRO";
            //
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_consulta_batch", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@batch", batch));
                //cmd.Parameters.Add(new SqlParameter("@dde_idproducto", empresa));
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (Convert.ToBoolean(rdr["estado"]) == true)
                        Respuesta = "BATCH YA FUE ENTREGADO|";
                    else
                        Respuesta = "OK|" + rdr["totalguia"].ToString() + "|" + rdr["totalpeso"].ToString();

                    //SqlParameter outParam = new SqlParameter();
                    //outParam.SqlDbType = System.Data.SqlDbType.Int;
                    //outParam.ParameterName = "@id";
                    //outParam.Direction = System.Data.ParameterDirection.Output;
                    //cmd.Parameters.Add(outParam);
                    //cmd.ExecuteNonQuery();
                    //int outval = (int)cmd.Parameters["@id"].Value;
                    //return outval.ToString();
                }
                return Respuesta;
            }
            catch
            {
                return string.Format("BATCH NO ESTA DISPONIBLE|");
            }
            // return string.Format("Name: {0}{2}Age: {1}", batch, empresa, Environment.NewLine);
        }
        /////////////////////////////////////////////////////////////
        //
        //
        [WebMethod]
        public static string ConsultaUltimoIngreso_Dia(string rut)
        {
            try
            {
                rut = rut.Replace(".", "");
                string respuesta = "";
                var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_consulta_registro_dia_App", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@rut", rut);
                    SqlDataAdapter dataAdapt = new SqlDataAdapter();
                    dataAdapt.SelectCommand = command;
                    DataTable dataTable = new DataTable();
                    dataAdapt.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        respuesta = "OK|" + dataTable.Rows[0]["nombre_solicitante"].ToString() + "|" + dataTable.Rows[0]["FechaVisita"].ToString() + "|" + dataTable.Rows[0]["id_empresa"].ToString() + "|" + dataTable.Rows[0]["id_estado"].ToString() + "|" + dataTable.Rows[0]["nom_empresa"].ToString() + "|" + dataTable.Rows[0]["HoraVisita"].ToString();
                    }
                }
                return respuesta;
            }
            catch
            {
                return string.Format("NO|");
            }
        }
        //
        [WebMethod]
        public static string ConsultaUltimoIngreso(string rut)
        {
            try
            {
                rut = rut.Replace(".", "");
                string respuesta = "";
                var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_consulta_ultimo_registro_App", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@rut", rut);
                    SqlDataAdapter dataAdapt = new SqlDataAdapter();
                    dataAdapt.SelectCommand = command;
                    DataTable dataTable = new DataTable();
                    dataAdapt.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        respuesta = "OK|" + dataTable.Rows[0]["nombre_solicitante"].ToString() + "|" + dataTable.Rows[0]["correo_solicitante"].ToString();
                    }
                }
                return respuesta;
            }
            catch
            {
                return string.Format("NO|");
            }
        }
    }
}