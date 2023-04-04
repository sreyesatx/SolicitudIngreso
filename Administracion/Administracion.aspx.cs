using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolicitudValidar;
//using SolicitudCorreos;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Mail;
namespace SistemaSolicitudIngreso.Administracion
{
    public partial class Administracion : System.Web.UI.Page
    {
        private static string filtro = "(1,2,3,5)";

        //private static string fechaVisita = string.Empty;

        private List<string> listaMenu
        {
            get { return (List<string>)Session.Contents["listaMenu"]; }
            set { Session.Contents["listaMenu"] = value; }
        }

        private string idUsuario
        {
            get
            {
                if (Session["idUsuario"] != null)
                    return (string)(Session["idUsuario"]);
                else
                    return "0";
            }
            set { Session["idUsuario"] = value; }
        }

        private string NombreUsuario
        {
            get
            {
                if (Session["nombre_usuario"] != null)
                    return (string)(Session["nombre_usuario"]);
                else
                    return "";
            }
            set { Session["nombre_usuario"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            Session.Timeout = 40;

            if (!Page.IsPostBack)
            {
                if (!listaMenu.Contains(Request.Path))
                {
                   Session.Contents.RemoveAll();
                   Response.Redirect("../NoAutorizado.aspx");
                }

                ddlEstado.DataSource = sdsEstados;
                ddlEstado.DataTextField = "descripcion_estado";
                ddlEstado.DataValueField = "id_estado";
                ddlEstado.DataBind();
                ddlEstado.Items.Insert(0, new ListItem("--", ""));


                //nuevo DropDownList para Masivo
                ddlEstado_1.DataSource = sdsEstados;
                ddlEstado_1.DataTextField = "descripcion_estado";
                ddlEstado_1.DataValueField = "id_estado";
                ddlEstado_1.DataBind();
                ddlEstado_1.Items.Insert(0, new ListItem("--", ""));

                ddlBuscaEmpresas.DataSource = sdsEmpresas;
                ddlBuscaEmpresas.DataTextField = "nombre_empresa";
                ddlBuscaEmpresas.DataValueField = "id_empresa";
                ddlBuscaEmpresas.DataBind();
                ddlBuscaEmpresas.Items.Insert(0, new ListItem("--Seleccione--", "0"));

                CalVigenciaHasta.StartDate = DateTime.Now.Date.AddDays(+1);

                sdsSolicitudes.SelectParameters.Clear();
                sdsSolicitudes.SelectParameters.Add("Filtro", filtro);
                DataView datos = (DataView)sdsSolicitudes.Select(DataSourceSelectArguments.Empty);
                if (datos != null)
                {
                    if (datos.Count != 0)
                    {
                        gvSolicitud.DataBind();
                    }
                }
            }
        }

        private DataTable ConsultaSolicitud(int strEmpresa
                                           , string strFechaVisita
                                           , string strNombreSolicitante
                                           , string strRutSolicitante
                                           , string strSolicitud)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Sp_lista_solicitud_2", connection);
                command.CommandType = CommandType.StoredProcedure;
                if (strEmpresa == 0)
                {
                    command.Parameters.AddWithValue("@Empresa", null);
                }
                else
                {
                    command.Parameters.AddWithValue("@Empresa", strEmpresa);
                }
                command.Parameters.AddWithValue("@FechaVisita", string.IsNullOrEmpty(strFechaVisita) ? (object)DBNull.Value : strFechaVisita);
                command.Parameters.AddWithValue("@NombreSolicitante", string.IsNullOrEmpty(strNombreSolicitante) ? (object)DBNull.Value : strNombreSolicitante);
                command.Parameters.AddWithValue("@RutSolicitante", string.IsNullOrEmpty(strRutSolicitante) ? (object)DBNull.Value : strRutSolicitante);
                command.Parameters.AddWithValue("@Solicitud", string.IsNullOrEmpty(strSolicitud) ? (object)DBNull.Value : strSolicitud);
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                DataTable dataTable = new DataTable();
                dataAdapt.Fill(dataTable);
                return dataTable;
            }
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            mpePopUp_1.Show();
        }

        protected void btnEnviar_1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlEstado_1.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('¡Debe seleccionar un estado a modificar!');", Title), true);
                    return;
                }
                foreach (GridViewRow row in gvSolicitud.Rows)
                {
                    CheckBox chkEnviar = (CheckBox)(row.FindControl("chkEnviar"));

                    LinkButton Solicitud = (LinkButton)row.FindControl("linkSolicitud");

                    if (chkEnviar.Checked)
                    {
                        string correo = gvSolicitud.Rows[row.RowIndex].Cells[13].Text;
                        string solicitud = Solicitud.Text;
                        string estacionamiento = gvSolicitud.Rows[row.RowIndex].Cells[19].Text.Replace("&nbsp;", "").Replace("--", "");
                        EnviarMasivo(ddlEstado_1.SelectedValue, solicitud, estacionamiento, this.txtMensaje_1.Text.Trim());
                        mpePopUp_1.Hide();
                        CargarGrilla();
                    }
                }
            }
            catch (Exception ex)
            {
                mpePopUp_1.Hide();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void EnviarMasivo(string strEstado, string strSolicitud, string strEstacionamiento, string strMensajeCorreo)
        {
            try
            {
                sdsUpdateSolicitud.UpdateParameters.Clear();
                sdsUpdateSolicitud.UpdateParameters.Add("nombre_solicitante", "Romina Moya");
                sdsUpdateSolicitud.UpdateParameters.Add("id_estado", strEstado);
                sdsUpdateSolicitud.UpdateParameters.Add("id_solicitud", strSolicitud);

                /*
                if (CheckBoxRecurrente.Checked)
                {
                    sdsUpdateSolicitud.UpdateParameters.Add("vigencia_hasta", txtVigenciaHasta.Text.Trim());
                }
                */

                sdsUpdateSolicitud.UpdateParameters.Add("estacionamiento", strEstacionamiento);
                sdsUpdateSolicitud.UpdateParameters.Add("observacion_administrador", strMensajeCorreo);
                sdsUpdateSolicitud.Update();

                gvSolicitud.DataBind();

                //MANDAR CORREO
                if (CheckBoxEnvioCorreo.Checked)
                {
                    sdsSolicitud.SelectParameters.Clear();
                    sdsSolicitud.SelectParameters.Add("Solicitud", strSolicitud);
                    DataView datos = (DataView)sdsSolicitud.Select(DataSourceSelectArguments.Empty);
                    if (datos != null)
                    {
                        if (datos.Count != 0)
                        {
                            string encargado, correo, copiaCorreo, mensaje, empresa, motivo, autorizar, denegar, posdata, visitas, tipoIngreso, fechaVisita;
                            encargado = correo = copiaCorreo = mensaje = empresa = motivo = autorizar = denegar = posdata = visitas = tipoIngreso = fechaVisita = string.Empty;

                            encargado = datos.ToTable().Rows[0]["nombre_solicitante"].ToString().Trim();
                            //nombre_contacto = "Sebastian Osorio"; //ASIGNAR ID ADMNISTRACION
                            correo = datos.ToTable().Rows[0]["correo_solicitante"].ToString().Trim();//COPIAR CORREO NUEVO PUDAHUEL							

                            tipoIngreso = datos.ToTable().Rows[0]["id_tipo_ingreso"].ToString().Trim();

                            fechaVisita = Convert.ToDateTime(datos.ToTable().Rows[0]["fecha_visita"].ToString()).ToString("dd/MM/yyyy HH:mm");

                            /*
                            if (CheckBoxRecurrente.Checked)
                            {
                                fechaVisita += " Hasta el " + txtVigenciaHasta.Text.Trim();
                            }
                            */
                            /////////////////////////////////////////
                            mensaje = "Su solicitud de acceso a las instalaciones ha sido " + ddlEstado_1.SelectedItem.Text.ToUpper();

                            //SI ES AUTORIZADO
                            if (ddlEstado_1.SelectedItem.Text == "Autorizado")
                            {
                                copiaCorreo = "control@atrexchile.cl"; 
																
                                fechaVisita = "Fecha y hora visita: " + fechaVisita;

                                if (tipoIngreso == "1")
                                {
                                    empresa = "Favor presentar su cedula de identidad en porteria de acceso peatonal";
                                }
                                else
                                {
                                    empresa = "Favor presentar su cedula de identidad en porteria y estacionar en los espacios exclusivos de " + ddlEstacionamiento.SelectedValue.ToUpper();
                                }

                                motivo = "Cabe señalar que estamos en un recinto aduanero y en zona primaria, por ende, aduana tiene potestad para abrir, revisar y retener carga que no este debidamente justificada.";
                                //autorizar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=2' target='_blank'>AUTORIZAR</a>";
                                //denegar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=3' target='_blank' style='color: rgb(230,0,38)'>DENEGAR</a>";
                                posdata = "Copio a Nuevo Pudahuel para su conocimiento.";

                                CrearCorreoadmin(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, "", "", posdata, strMensajeCorreo, "NO");
                            }
                            else if (ddlEstado_1.SelectedItem.Text == "Anulado" || ddlEstado_1.SelectedItem.Text == "Denegado")
                            {
                                fechaVisita = string.Empty;
                                empresa = strMensajeCorreo;

                                CrearCorreoadmin(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, "", "", posdata, strMensajeCorreo, "NO");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                mpePopUp.Hide();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void btnOpenPopUp_Click(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
            {
                lblSolicitud.Text = string.Empty;
                ddlEstacionamiento.ClearSelection();
                //fechaVisita = string.Empty;
                //ddlHora.ClearSelection();
                //ddlMinutos.ClearSelection();
                ddlEstado.ClearSelection();
                CheckBox1.Checked = true;
                txtMensajeCorreo.Text = string.Empty;

                LinkButton obj = (LinkButton)(sender);
                string id_solicitud = obj.CommandArgument.ToString();

                sdsSolicitud.SelectParameters.Clear();
                sdsSolicitud.SelectParameters.Add("Solicitud", id_solicitud);
                sdsSolicitud.SelectParameters.Add("Filtro", filtro);
                DataView datos = (DataView)sdsSolicitud.Select(DataSourceSelectArguments.Empty);
                if (datos != null)
                {
                    if (datos.Count != 0)
                    {
                        string tipoIngreso = datos.ToTable().Rows[0]["id_tipo_ingreso"].ToString();

                        if (tipoIngreso == "1")
                        {
                            ddlEstacionamiento.Enabled = false;
                            CalVigenciaHasta.EndDate = DateTime.Now.Date.AddDays(+7);
                        }
                        else
                        {
                            ddlEstacionamiento.Enabled = true;
                            CalVigenciaHasta.EndDate = DateTime.Now.Date.AddDays(+365);
                        }

                        lblSolicitud.Text = id_solicitud;

                        string estacionamiento = datos.ToTable().Rows[0]["estacionamiento"].ToString().Trim();

                        if (estacionamiento != null && estacionamiento != "&nbsp;" && estacionamiento != "")
                        {
                            ddlEstacionamiento.SelectedValue = estacionamiento;
                        }

                        //fechaVisita = Convert.ToDateTime(datos.ToTable().Rows[0]["fecha_visita"].ToString()).ToShortDateString();
                        //DateTime hor = Convert.ToDateTime(datos.ToTable().Rows[0]["fecha_visita"].ToString());
                        //string[] horas = hor.ToString("HH:mm").Split(':');
                        //string hora = horas[0].Trim();
                        //string minutos = horas[1].Trim();

                        //ddlHora.SelectedValue = hora ?? "--";
                        //ddlMinutos.SelectedValue = minutos ?? "--";

                        //ddlEstado.SelectedValue = (datos.ToTable().Rows[0]["id_estado"].ToString().Trim()) ?? "0";

                        mpePopUp.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                mpePopUp.Hide();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void buscarColegas_Click(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
            {
                LinkButton obj = (LinkButton)(sender);
                if (obj.Text != "NO")
                {
                    string id_solicitud = obj.CommandArgument.ToString();

                    sdsColegas.SelectParameters.Clear();
                    sdsColegas.SelectParameters.Add("Solicitud", id_solicitud);
                    DataView datos = (DataView)sdsColegas.Select(DataSourceSelectArguments.Empty);
                    if (datos != null)
                    {
                        if (datos.Count != 0)
                        {
                            gvColegas.DataBind();
                            colegasPopUp.Show();
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                colegasPopUp.Hide();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void Enviar(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
            {
                if (CheckBoxRecurrente.Checked && txtVigenciaHasta.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('¡Ingrese fecha vigencia!');", Title), true);
                    return;
                }

                if (ddlEstacionamiento.Enabled == true && ddlEstado.SelectedValue == "6" && ddlEstacionamiento.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('¡Seleccione estacionamiento!');", Title), true);
                    return;
                }
                sdsUpdateSolicitud.UpdateParameters.Clear();
                sdsUpdateSolicitud.UpdateParameters.Add("nombre_solicitante", NombreUsuario);
                sdsUpdateSolicitud.UpdateParameters.Add("id_estado", ddlEstado.SelectedValue);
                sdsUpdateSolicitud.UpdateParameters.Add("id_solicitud", lblSolicitud.Text);

                if (CheckBoxRecurrente.Checked)
                {
                    sdsUpdateSolicitud.UpdateParameters.Add("vigencia_hasta", txtVigenciaHasta.Text.Trim());
                }

                sdsUpdateSolicitud.UpdateParameters.Add("estacionamiento", ddlEstacionamiento.SelectedValue);
                sdsUpdateSolicitud.UpdateParameters.Add("observacion_administrador", txtMensajeCorreo.Text.Trim());
                sdsUpdateSolicitud.Update();

                gvSolicitud.DataBind();

                //MANDAR CORREO
                if (CheckBox1.Checked)
                {
                    sdsSolicitud.SelectParameters.Clear();
                    sdsSolicitud.SelectParameters.Add("Solicitud", lblSolicitud.Text);
                    DataView datos = (DataView)sdsSolicitud.Select(DataSourceSelectArguments.Empty);
                    if (datos != null)
                    {
                        if (datos.Count != 0)
                        {
                            string encargado, correo, copiaCorreo, mensaje, empresa, motivo, autorizar, denegar, posdata, visitas, tipoIngreso, fechaVisita;
                            encargado = correo = copiaCorreo = mensaje = empresa = motivo = autorizar = denegar = posdata = visitas = tipoIngreso = fechaVisita = string.Empty;

                            encargado = datos.ToTable().Rows[0]["nombre_solicitante"].ToString().Trim();
                            //nombre_contacto = "Sebastian Osorio"; //ASIGNAR ID ADMNISTRACION
                            correo = datos.ToTable().Rows[0]["correo_solicitante"].ToString().Trim();//COPIAR CORREO NUEVO PUDAHUEL

                           // correo ="cesar.silva";//COPIAR CORREO NUEVO PUDAHUEL

                            tipoIngreso = datos.ToTable().Rows[0]["id_tipo_ingreso"].ToString().Trim();

                            fechaVisita = Convert.ToDateTime(datos.ToTable().Rows[0]["fecha_visita"].ToString()).ToString("dd/MM/yyyy HH:mm");

                            if (CheckBoxRecurrente.Checked)
                            {
                                fechaVisita += " Hasta el " + txtVigenciaHasta.Text.Trim();
                            }

                            /////////////////////////////////////////
                            mensaje = "Su solicitud de acceso a las instalaciones ha sido " + ddlEstado.SelectedItem.Text.ToUpper();

                            //SI ES AUTORIZADO
                            if (ddlEstado.SelectedItem.Text == "Autorizado")
                            {
                                copiaCorreo = "control@atrexchile.cl";
                             //   copiaCorreo = "cesar.silva@atrexchile.cl";

                                fechaVisita = "Fecha y hora visita: " + fechaVisita;

                                if (tipoIngreso == "1")
                                {
                                    empresa = "Favor presentar su cedula de identidad en porteria de acceso peatonal";
                                }
                                else
                                {
                                    empresa = "Favor presentar su cedula de identidad en porteria y estacionar en los espacios exclusivos de " + ddlEstacionamiento.SelectedValue.ToUpper();
                                }

                                motivo = "Cabe señalar que estamos en un recinto aduanero y en zona primaria, por ende, aduana tiene potestad para abrir, revisar y retener carga que no este debidamente justificada.";
                                //autorizar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=2' target='_blank'>AUTORIZAR</a>";
                                //denegar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=3' target='_blank' style='color: rgb(230,0,38)'>DENEGAR</a>";
                                posdata = "Copio a Nuevo Pudahuel para su conocimiento.";


                                CrearCorreoadmin(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, "", "", posdata, txtMensajeCorreo.Text.Trim(), "NO");
                            }
                            else if (ddlEstado.SelectedItem.Text == "Anulado" || ddlEstado.SelectedItem.Text == "Denegado")
                            {
                                fechaVisita = string.Empty;
                                empresa = txtMensajeCorreo.Text.Trim();

                                CrearCorreoadmin(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, "", "", posdata, "", "NO");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mpePopUp.Hide();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }
        private static void CrearCorreoadmin(string encargado, string correo, string copiaCorreo, string mensaje, string fecha, string empresa, string visita, string motivo, string autorizar, string denegar, string posdata,string nose, string nose2)
        {
            StringBuilder body = new StringBuilder(File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Correo/PlantillaCorreo.html")));
            body.Replace("@ENCARGADO", encargado);
            body.Replace("@MENSAJE", mensaje);
            body.Replace("@FECHA", fecha);
            body.Replace("@EMPRESA", empresa);
            body.Replace("@VISITAS", visita);
            body.Replace("@MOTIVO", motivo);
            body.Replace("@AUTORIZAR", autorizar);
            body.Replace("@DENEGAR", denegar);
            body.Replace("@POSDATA", posdata);
            body.Replace("@ANIO", DateTime.Now.Year.ToString());

            //ENVIA CORREO
            Enviar_Correo_admin(correo, copiaCorreo, "Solicitud de ingreso", body.ToString());
        }

        private static void Enviar_Correo_admin(string destinatario, string copiaCorreo, string asunto, string body)
        {
            MailMessage mail = new MailMessage();
          //  SmtpClient SmtpServer = new SmtpClient("mail.couriers.cl");
            //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
            mail.From = new MailAddress("notificaciones@couriers.cl", "Sistemas Atrex", Encoding.UTF8);
            //Aquí ponemos el asunto del correo
            mail.Subject = asunto;
            mail.Body = body;
            mail.IsBodyHtml = true;
            // mail.Body = "Se a enviado una nueva clave: " + Clave + " Recuerde cambiarla para mayor seguridad. Puedes hacerlo visitando la siguiente URL https://192.168.45.6/Atrex/Generales/CambioClave.aspx";
            //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
            mail.To.Add(destinatario);
			mail.Bcc.Add("recibidos.atrex@couriers.cl");
            if (copiaCorreo != string.Empty)
            {
                mail.CC.Add(copiaCorreo);
            }
            //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran
            //mail.Attachments.Add(new Attachment(@"C:\Documentos\carta.docx"));
            //Configuracion del SMTP
           // SmtpServer.Port = 25; //Puerto que utiliza Gmail para sus servicios
            //Especificamos las credenciales con las que enviaremos el mail
          //  SmtpServer.Credentials = new System.Net.NetworkCredential("notificaciones@couriers.cl", "Atrex.,2022");
			// SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
			 
			   System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("mail.couriers.cl", 25);

      //  ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        // SmtpServer.EnableSsl = true;
        //      SmtpServer.UseDefaultCredentials = true;
        // Crear Credencial de Autenticacion
        SmtpServer.Credentials = new System.Net.NetworkCredential("notificaciones@couriers.cl", "Atrex.,2022");

        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
			 
			 
			 
			 
            //SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

        }
        
        protected void CerrarColegas(object sender, EventArgs e)
        {
            colegasPopUp.Hide();
        }

        protected void CerrarSolicitud(object sender, EventArgs e)
        {
            mpePopUp.Hide();
        }
        protected void CerrarSolicitud_1(object sender, EventArgs e)
        {
            mpePopUp_1.Hide();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
            {
                CargarGrilla();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        private void CargarGrilla()
        {
            sdsSolicitudes.SelectParameters.Clear();

            if (ddlBuscaEmpresas.SelectedValue != "0")
            {
                sdsSolicitudes.SelectParameters.Add("Empresa", ddlBuscaEmpresas.SelectedValue);
            }
            if (txtBuscaFechaVisita.Text.Trim() != string.Empty)
            {
                sdsSolicitudes.SelectParameters.Add("FechaVisita", txtBuscaFechaVisita.Text.Trim());
            }
            if (txtBuscaNombre.Text.Trim() != string.Empty)
            {
                sdsSolicitudes.SelectParameters.Add("NombreSolicitante", txtBuscaNombre.Text.Trim());
            }
            if (txtBuscaRut.Text.Trim() != string.Empty)
            {
                sdsSolicitudes.SelectParameters.Add("RutSolicitante", txtBuscaRut.Text.Trim());
            }
            sdsSolicitudes.SelectParameters.Add("Filtro", filtro);
            DataView datos = (DataView)sdsSolicitudes.Select(DataSourceSelectArguments.Empty);
            if (datos != null)
            {
                if (datos.Count != 0)
                {
                    //gvSolicitud.DataSource = datos;
                    gvSolicitud.DataBind();
                }
            }
        }

    }
}