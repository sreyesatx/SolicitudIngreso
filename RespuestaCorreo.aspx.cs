using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaSolicitudIngreso;
using SolicitudValidar;
using System.Configuration;
using System.Data.SqlClient;

namespace SistemaSolicitudIngreso
{
    public partial class RespuestaCorreo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblAnio.Text = DateTime.Now.Year.ToString();
                string respuesta = Request.QueryString["respuesta"].ToString();

                if (respuesta == "3")
                {
                    PanelMotivo.Visible = true;
                }
                else
                {
                    PanelMotivo.Visible = false;
                    actualizar();
                }
            }
        }
        //
        protected void actualizar()
        {
            try
            {
                string encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, motivo, autorizar, denegar, posdata, nombre_contacto, visitas, webRespuesta, solicitud;
                encargado = correo = copiaCorreo = mensaje = fechaVisita = empresa = motivo = autorizar = denegar = posdata = nombre_contacto = visitas = webRespuesta = solicitud = string.Empty;

                nombre_contacto = Request.QueryString["nombre_contacto"].ToString();
                solicitud = Request.QueryString["solicitud"].ToString();
                string respuesta = Request.QueryString["respuesta"].ToString();


                sdsEstadoSolicitud.SelectParameters.Clear();
                sdsEstadoSolicitud.SelectParameters.Add("id_solicitud", solicitud);
                DataView datos = (DataView)sdsEstadoSolicitud.Select(DataSourceSelectArguments.Empty);
                if (datos != null && datos.Count != 0)
                {
                    string estado = datos.ToTable().Rows[0]["id_estado"].ToString().Trim();

                    if (estado == "1")
                    {
                        sdsUpdateSolicitud.UpdateParameters.Clear();
                        sdsUpdateSolicitud.UpdateParameters.Add("nombre_solicitante", nombre_contacto);
                        sdsUpdateSolicitud.UpdateParameters.Add("id_estado", respuesta);
                        sdsUpdateSolicitud.UpdateParameters.Add("id_solicitud", solicitud);
                        sdsUpdateSolicitud.UpdateParameters.Add("observaciones", txtMotivo.Text.Trim());
                        sdsUpdateSolicitud.Update();

                        encargado = "Control Acceso Atrex, ";
                        correo = "control@atrexchile.cl";
                        

                        mensaje = "El usuario " + nombre_contacto + " a respondido la solicitud N° " + solicitud;
                        empresa = string.Empty;
                        motivo = txtMotivo.Text.Trim(); // string.Empty; se actualizó este dato 09-08-2019 11:30
                        autorizar = string.Empty;
                        denegar = string.Empty;
                        posdata = "Para ingresar al sistema presione <a href='https://couriers.cl/ingreso/IngresoSistema.aspx' target='_blank'>AQUI</a>";

                        Correo.Correo.CrearCorreo(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);

                        PanelMotivo.Visible = false;
                        lblRespuesta.ForeColor = System.Drawing.Color.Black;
                        lblRespuesta.Text = "Su respuesta fue guardada correctamente, nos comunicaremos con el solicitante para informar su decisión, muchas gracias!";
                    }
                    else
                    {
                        PanelMotivo.Visible = false;
                        lblRespuesta.ForeColor = System.Drawing.Color.Black;
                        lblRespuesta.Text = "Esta solicitud ya tiene una respuesta, actualmente se encuentra en estado " + ConsultaEstado(estado) + ".";
                    }
                }
            }
            catch (Exception ex)
            {
                lblRespuesta.ForeColor = System.Drawing.Color.Red;
                lblRespuesta.Text = "Ha ocurrido un error : " + Validar.ValidaString(ex.Message.ToString());
            }

        }
        private static string ConsultaEstado(string estado)
        {
            string resp = "";
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Select descripcion_estado from EstadoSolicitud Where id_estado = @param", connection);
                command.Parameters.AddWithValue("@param", estado);
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                DataTable dataTable = new DataTable();
                dataAdapt.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    resp = dataTable.Rows[0][0].ToString();
                }
            }

            return resp;
        }		

        //
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtMotivo.Text.Trim() != "")
            {
                actualizar();
            }
            else
            {
                lblRespuesta.ForeColor = System.Drawing.Color.Red;
                lblRespuesta.Text = "Debe ingresar motivo";
            }
        }
    }
}