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

namespace SistemaSolicitudIngreso.Administracion
{
    public partial class Historial : System.Web.UI.Page
    {
        private static string filtro = "(4,6,7,8,9,10,11)";

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
                            //correo = ".e@gmail.com";

                            tipoIngreso = datos.ToTable().Rows[0]["id_tipo_ingreso"].ToString().Trim();

                            fechaVisita = Convert.ToDateTime(datos.ToTable().Rows[0]["fecha_visita"].ToString()).ToString("dd/MM/yyyy HH:mm");

                            /////////////////////////////////////////
                            mensaje = "Su solicitud de acceso a las instalaciones ha sido " + ddlEstado.SelectedItem.Text.ToUpper();

                            //SI ES AUTORIZADO
                            if (ddlEstado.SelectedItem.Text == "Autorizado")
                            {
                                copiaCorreo = "control@atrexchile.cl";
                                //copiaCorreo = "sebatian.reyes@atrexchile.cl";
                                fechaVisita = "Fecha y hora visita: " + fechaVisita;

                                if (tipoIngreso == "1")
                                {
                                    empresa = "Favor presentar su cedula de identidad en porteria de acceso peatonal";
                                }
                                else
                                {
                                    empresa = "Favor presentar su cedula de identidad en porteria y estacionar en los espacios exclusivos de " + ddlEstacionamiento.SelectedValue.ToUpper();
                                }

                                if (txtMensajeCorreo.Text.Trim() != string.Empty)
                                {
                                    empresa += "<br /> <br /> " + txtMensajeCorreo.Text.Trim();
                                }

                                motivo = "Cabe señalar que estamos en un recinto aduanero y en zona primaria, por ende, aduana tiene potestad para abrir, revisar y retener carga que no este debidamente justificada.";
                                //autorizar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=2' target='_blank'>AUTORIZAR</a>";
                                //denegar = webRespuesta + nombre_contacto + "&solicitud=" + solicitud + "&respuesta=3' target='_blank' style='color: rgb(230,0,38)'>DENEGAR</a>";
                                posdata = "Copio a Nuevo Pudahuel para su conocimiento.";


                                Correo.Correo.CrearCorreo(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);
                            }
                            else if (ddlEstado.SelectedItem.Text == "Anulado" || ddlEstado.SelectedItem.Text == "Denegado")
                            {
                                fechaVisita = string.Empty;
                                empresa = txtMensajeCorreo.Text.Trim();

                                Correo.Correo.CrearCorreo(encargado, correo, copiaCorreo, mensaje, fechaVisita, empresa, visitas, motivo, autorizar, denegar, posdata);
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
                if (txtPatente.Text.Trim() != string.Empty)
                {                   
                    sdsSolicitudes.SelectParameters.Add("Patente", txtPatente.Text.Trim());
                }				
                DataView datos = (DataView)sdsSolicitudes.Select(DataSourceSelectArguments.Empty);
                if (datos != null)
                {
                    if (datos.Count != 0)
                    {
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