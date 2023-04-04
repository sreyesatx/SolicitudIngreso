using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Mail;
using SolicitudValidar;
using System.Configuration;
using System.Data.SqlClient;

namespace SistemaSolicitudIngreso.Administracion
{
    public partial class Historial : System.Web.UI.Page
    {
        private static string filtro = "4,6,7,8,9,10,11";

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

        private static DataTable dtDatosConsultados;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            Session.Timeout = 40;

            if (!Page.IsPostBack)
            {


                ddlEstado.DataSource = sdsEstados;
                ddlEstado.DataTextField = "descripcion_estado";
                ddlEstado.DataValueField = "id_estado";
                ddlEstado.DataBind();
                ddlEstado.Items.Insert(0, new ListItem("--", "0"));

                ddlBuscaEmpresas.DataSource = sdsEmpresas;
                ddlBuscaEmpresas.DataTextField = "nombre_empresa";
                ddlBuscaEmpresas.DataValueField = "id_empresa";
                ddlBuscaEmpresas.DataBind();
                ddlBuscaEmpresas.Items.Insert(0, new ListItem("--Seleccione--", "0"));

                calFechaVisita.StartDate = DateTime.Now.Date;

                /*
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
                */
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
                txtFecha.Text = string.Empty;
                ddlHora.ClearSelection();
                ddlMinutos.ClearSelection();
                ddlEstado.ClearSelection();

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
                            ddlEstacionamiento.Enabled = false;
                        else
                            ddlEstacionamiento.Enabled = true;

                        lblSolicitud.Text = id_solicitud;

                        string estacionamiento = datos.ToTable().Rows[0]["estacionamiento"].ToString().Trim();

                        if (estacionamiento != null && estacionamiento != "&nbsp;" && estacionamiento != "")
                        {
                            ddlEstacionamiento.SelectedValue = estacionamiento;
                        }

                        txtFecha.Text = Convert.ToDateTime(datos.ToTable().Rows[0]["fecha_visita"].ToString()).ToShortDateString();
                        DateTime hor = Convert.ToDateTime(datos.ToTable().Rows[0]["fecha_visita"].ToString());
                        string[] horas = hor.ToString("HH:mm").Split(':');
                        string hora = horas[0].Trim();
                        string minutos = horas[1].Trim();

                        ddlHora.SelectedValue = hora ?? "--";
                        ddlMinutos.SelectedValue = minutos ?? "--";

                        ddlEstado.SelectedValue = (datos.ToTable().Rows[0]["id_estado"].ToString().Trim()) ?? "0";

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
                string fechaVisita = txtFecha.Text.Trim() + " " + ddlHora.SelectedValue + ":" + ddlMinutos.SelectedValue;

                sdsUpdateSolicitud.UpdateParameters.Clear();
                sdsUpdateSolicitud.UpdateParameters.Add("nombre_solicitante", NombreUsuario);
                sdsUpdateSolicitud.UpdateParameters.Add("id_estado", ddlEstado.SelectedValue);
                sdsUpdateSolicitud.UpdateParameters.Add("id_solicitud", lblSolicitud.Text);
                sdsUpdateSolicitud.UpdateParameters.Add("fecha_visita", fechaVisita);
                sdsUpdateSolicitud.UpdateParameters.Add("estacionamiento", ddlEstacionamiento.SelectedValue);
                sdsUpdateSolicitud.Update();

                if (chkcorreo.Checked == true)
                {
                    sdsSolicitud.SelectParameters.Clear();
                    sdsSolicitud.SelectParameters.Add("Solicitud", lblSolicitud.Text);
                    DataView datos = (DataView)sdsSolicitud.Select(DataSourceSelectArguments.Empty);
                    if (datos != null)
                    {
                        if (datos.Count != 0)
                        {

                            string encargado, correo, copiaCorreo, mensaje, empresa, motivo, autorizar, denegar, posdata, visitas, tipoIngreso;
                            //    string fechaVisita = String.Empty;
                            encargado = correo = copiaCorreo = mensaje = empresa = motivo = autorizar = denegar = posdata = visitas = tipoIngreso = string.Empty;

                            encargado = datos.ToTable().Rows[0]["nombre_solicitante"].ToString().Trim();
                            //nombre_contacto = "Sebastian Osorio"; //ASIGNAR ID ADMNISTRACION
                            correo = datos.ToTable().Rows[0]["correo_solicitante"].ToString().Trim();//COPIAR CORREO NUEVO PUDAHUEL                            

                            tipoIngreso = datos.ToTable().Rows[0]["id_tipo_ingreso"].ToString().Trim();

                            fechaVisita = Convert.ToDateTime(datos.ToTable().Rows[0]["fecha_visita"].ToString()).ToString("dd/MM/yyyy HH:mm");

                            /////////////////////////////////////////
                            mensaje = "Su solicitud de acceso a las instalaciones ha sido " + ddlEstado.SelectedItem.Text.ToUpper();

                            //SI ES AUTORIZADO
                            if (ddlEstado.SelectedItem.Text == "Autorizado")
                            {
                                if (datos.ToTable().Rows[0]["id_empresa"].ToString() == "5")
                                {
                                    copiaCorreo = "soporte@atrexchile.cl";
                                }
                                else
                                {
                                    copiaCorreo = "";
                                }
                                //copiaCorreo = "sebatian.reyes@atrexchile.cl";
                                fechaVisita = "Fecha y hora visita: " + fechaVisita;

                                if (tipoIngreso == "1")
                                {
                                    empresa = "Favor presentar su cedula de identidad en porteria de acceso peatonal";
                                }
                                else
                                {
                                    if (ddlEstacionamiento.SelectedIndex == 0)
                                    {
                                        empresa = "Favor presentar su cedula de identidad en porteria y estacionar en los espacios exclusivos";
                                    }
                                    else
                                    {
                                        empresa = "Favor presentar su cedula de identidad en porteria y estacionar en los espacios exclusivos de " + ddlEstacionamiento.SelectedValue.ToUpper();
                                    }									
                                    //empresa = "Favor presentar su cedula de identidad en porteria y estacionar en los espacios exclusivos de " + ddlEstacionamiento.SelectedValue.ToUpper();
                                }

                                if (txtMensajeCorreo.Text.Trim() != string.Empty)
                                {
                                    empresa += "<br /> <br /> " + txtMensajeCorreo.Text.Trim();
                                }

                                motivo = "Cabe señalar que estamos en un recinto aduanero y en zona primaria, por ende, aduana tiene potestad para abrir, revisar y retener carga que no este debidamente justificada.";
                                //autorizar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=2' target='_blank'>AUTORIZAR</a>";
                                //denegar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=3' target='_blank' style='color: rgb(230,0,38)'>DENEGAR</a>";
                                posdata = "Copio a Nuevo Pudahuel para su conocimiento.";


                                CrearCorreoHistorial(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);
                            }
                            else if (ddlEstado.SelectedItem.Text == "Anulado" || ddlEstado.SelectedItem.Text == "Denegado")
                            {
                                fechaVisita = string.Empty;
                                empresa = txtMensajeCorreo.Text.Trim();

                                CrearCorreoHistorial(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);
                            }
                        }
                    }
                }

                gvSolicitud.DataBind();
            }
            catch (Exception ex)
            {
                mpePopUp.Hide();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void CerrarColegas(object sender, EventArgs e)
        {
            colegasPopUp.Hide();
        }

        protected void CerrarSolicitud(object sender, EventArgs e)
        {
            mpePopUp.Hide();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.ddlBuscaEmpresas.SelectedIndex = 0;
            this.txtBuscaFechaVisita.Text = "";
            this.txtBuscaNombre.Text = "";
            this.txtBuscaRut.Text = "";
            this.txtPatente.Text = "";
            gvSolicitud.DataSource = null;
            gvSolicitud.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
            {
                string strEmpresa = string.Empty, strFechaVisita = string.Empty, strNombreSolicitante = string.Empty, strRutSolicitante = string.Empty, strSolicitud = string.Empty, strFiltro = string.Empty, strPatente = string.Empty;

                if (ddlBuscaEmpresas.SelectedValue != "0")
                {
                    strEmpresa = ddlBuscaEmpresas.SelectedValue;
                }
                strFechaVisita = txtBuscaFechaVisita.Text.Trim();
                strNombreSolicitante = txtBuscaNombre.Text.Trim();
                strRutSolicitante = txtBuscaRut.Text.Trim();
                strFiltro = filtro;
                strPatente = txtPatente.Text.Trim();


                if (string.IsNullOrEmpty(strEmpresa) &&
                   string.IsNullOrEmpty(strFechaVisita) &&
                   string.IsNullOrEmpty(strNombreSolicitante) &&
                   string.IsNullOrEmpty(strRutSolicitante) &&
                    string.IsNullOrEmpty(strPatente))
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Favor ingresar al menos un filtro');", Title), true);
                    return;
                }

                DataTable dt = new DataTable();
                dt = ConsultaProcedures(strEmpresa, strFechaVisita, strNombreSolicitante, strRutSolicitante, strSolicitud, strFiltro, strPatente);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dtDatosConsultados = dt;
                    gvSolicitud.DataSource = dt;
                    gvSolicitud.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        private DataTable ConsultaProcedures(string strEmpresa
                                , string strFechaVisita
                                , string strNombreSolicitante
                                , string strRutSolicitante
                                , string strSolicitud
                                , string strFiltro
                                , string strPatente)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_lista_solicitud_4", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.AddWithValue("@Empresa", string.IsNullOrEmpty(strEmpresa) ? (object)DBNull.Value : strEmpresa);
                command.Parameters.AddWithValue("@FechaVisita", string.IsNullOrEmpty(strFechaVisita) ? (object)DBNull.Value : strFechaVisita);
                command.Parameters.AddWithValue("@NombreSolicitante", string.IsNullOrEmpty(strNombreSolicitante) ? (object)DBNull.Value : strNombreSolicitante);
                command.Parameters.AddWithValue("@RutSolicitante", string.IsNullOrEmpty(strRutSolicitante) ? (object)DBNull.Value : strRutSolicitante);
                command.Parameters.AddWithValue("@Solicitud", string.IsNullOrEmpty(strSolicitud) ? (object)DBNull.Value : strSolicitud);
                command.Parameters.AddWithValue("@Filtro", string.IsNullOrEmpty(strFiltro) ? (object)DBNull.Value : strFiltro);
                command.Parameters.AddWithValue("@Patente", string.IsNullOrEmpty(strPatente) ? (object)DBNull.Value : strPatente);
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                DataTable dataTable = new DataTable();
                dataAdapt.Fill(dataTable);
                return dataTable;
            }
        }


        private static void CrearCorreoHistorial(string encargado, string correo, string copiaCorreo, string mensaje, string fecha, string empresa, string visita, string motivo, string autorizar, string denegar, string posdata)
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
            Enviar_Correo_historial(correo, copiaCorreo, "Solicitud de ingreso", body.ToString());
        }
        private static void Enviar_Correo_historial(string destinatario, string copiaCorreo, string asunto, string body)
        {
            MailMessage mail = new MailMessage();
            //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
            mail.From = new MailAddress("sistemas.atrex@couriers.cl", "Sistemas Atrex", Encoding.UTF8);
            //Aquí ponemos el asunto del correo
            mail.Subject = asunto;
            mail.Body = body;
            mail.IsBodyHtml = true;
            // mail.Body = "Se a enviado una nueva clave: " + Clave + " Recuerde cambiarla para mayor seguridad. Puedes hacerlo visitando la siguiente URL https://192.168.45.6/Atrex/Generales/CambioClave.aspx";
            //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
            mail.To.Add(destinatario);
            if (copiaCorreo != string.Empty)
            {
                mail.CC.Add(copiaCorreo);
            }



            System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("mail.couriers.cl", 25);
            SmtpServer.Credentials = new System.Net.NetworkCredential("sistemas.atrex@couriers.cl", "Atrex.,2022");
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.Send(mail);

        }

        protected void gvSolicitud_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSolicitud.PageIndex = e.NewPageIndex;
            this.gvSolicitud.DataSource = dtDatosConsultados;
            this.gvSolicitud.DataBind();
        }
    }
}