using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolicitudValidar;
using SistemaSolicitudIngreso.Correo;

namespace SistemaSolicitudIngreso.Empresas
{
    public partial class Empresas : System.Web.UI.Page
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

            Session.Timeout = 20;

            if (!Page.IsPostBack)
            {
                /*
                if (!listaMenu.Contains(Request.Path))
                {
                    Session.Contents.RemoveAll();
                    Response.Redirect("../NoAutorizado.aspx");
                }
                */

                ddlEstado.Items.Insert(0, new ListItem("--", ""));

                ddlBuscaEmpresas.DataSource = sdsEmpresas;
                ddlBuscaEmpresas.DataTextField = "nombre_empresa";
                ddlBuscaEmpresas.DataValueField = "id_empresa";
                ddlBuscaEmpresas.DataBind();
                ddlBuscaEmpresas.Items.Insert(0, new ListItem("--Seleccione--", "0"));
                ddlBuscaEmpresas.SelectedValue = Session["idempresa"].ToString();

                txtBuscaFechaVisita.Text = DateTime.Now.ToString("dd/MM/yyyy");

                ddlBuscaEmpresas.Enabled = false;
                CalVigenciaHasta.StartDate = DateTime.Now.Date.AddDays(+1);

                sdsSolicitudes.SelectParameters.Clear();
                sdsSolicitudes.SelectParameters.Add("Empresa", Session["idempresa"].ToString());
                sdsSolicitudes.SelectParameters.Add("FechaVisita", DateTime.Now.ToString("dd/MM/yyyy"));
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

                txtMensaje.Text = string.Empty;

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
                sdsUpdateSolicitud.UpdateParameters.Clear();
                sdsUpdateSolicitud.UpdateParameters.Add("nombre_solicitante", NombreUsuario);
                sdsUpdateSolicitud.UpdateParameters.Add("id_estado", ddlEstado.SelectedValue);
                sdsUpdateSolicitud.UpdateParameters.Add("id_solicitud", lblSolicitud.Text);

                if (CheckBoxRecurrente.Checked)
                {
                    sdsUpdateSolicitud.UpdateParameters.Add("vigencia_hasta", txtVigenciaHasta.Text.Trim());
                }

                sdsUpdateSolicitud.UpdateParameters.Add("estacionamiento", ddlEstacionamiento.SelectedValue);
                sdsUpdateSolicitud.UpdateParameters.Add("observaciones", txtMensaje.Text.Trim());
                sdsUpdateSolicitud.Update();

                gvSolicitud.DataBind();

                //MANDAR CORREO
                string encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, motivo, autorizar, denegar, posdata, nombre_contacto, visitas, webRespuesta, solicitud;
                encargado = correo = copiaCorreo = mensaje = fechaVisita = empresa = motivo = autorizar = denegar = posdata = nombre_contacto = visitas = webRespuesta = solicitud = string.Empty;

                encargado = "Control Acceso Atrex, ";
                correo = "control@atrexchile.cl";
                //correo = "sebastian.reyes@atrexchile.cl";

                mensaje = "El usuario " + nombre_contacto + " a respondido la solicitud N° " + solicitud;
                empresa = string.Empty;
                motivo = txtMensaje.Text.Trim(); // string.Empty; se actualizó este dato 09-08-2019 11:30
                autorizar = string.Empty;
                denegar = string.Empty;
                posdata = "Para ingresar al sistema presione <a href='https://couriers.cl/ingreso/IngresoSistema.aspx' target='_blank'>AQUI</a>";

                Correo.Correo.CrearCorreo(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);

                //PanelMotivo.Visible = false;
                //lblRespuesta.ForeColor = System.Drawing.Color.Black;
                //lblRespuesta.Text = "Su respuesta fue guardada correctamente, nos comunicaremos con el solicitante para informar su decisión, muchas gracias!";
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

    }
}