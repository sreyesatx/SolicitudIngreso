using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolicitudValidar;

namespace SistemaSolicitudIngreso.AccesoVehiculos
{
    public partial class Vehiculos : System.Web.UI.Page
    {
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
                if (!listaMenu.Contains(Request.Path))
                {
                    Session.Contents.RemoveAll();
                    Response.Redirect("../NoAutorizado.aspx");
                }

                ddlBuscaEmpresas.DataSource = sdsEmpresas;
                ddlBuscaEmpresas.DataTextField = "nombre_empresa";
                ddlBuscaEmpresas.DataValueField = "id_empresa";
                ddlBuscaEmpresas.DataBind();
                ddlBuscaEmpresas.Items.Insert(0, new ListItem("--Seleccione--", "0"));

                sdsSolicitudes.SelectParameters.Clear();
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

        protected void entrada(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
            {
                txtBuscaRut.Text = string.Empty;
                txtBuscaRut.Focus();

                Button obj = (Button)(sender);
                string id_solicitud = obj.CommandArgument.ToString();

                sdsEntrada.UpdateParameters.Clear();
                sdsEntrada.UpdateParameters.Add("nombre_solicitante", NombreUsuario);
                sdsEntrada.UpdateParameters.Add("id_estado", "9");
                sdsEntrada.UpdateParameters.Add("id_solicitud", id_solicitud);
                sdsEntrada.UpdateParameters.Add("HoraIngreso", DateTime.Now.ToString());
                sdsEntrada.Update();

                gvSolicitud.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void salida(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
            {
                txtBuscaRut.Text = string.Empty;
                txtBuscaRut.Focus();

                Button obj = (Button)(sender);
                string id_solicitud = obj.CommandArgument.ToString();

                sdsSalida.UpdateParameters.Clear();
                sdsSalida.UpdateParameters.Add("nombre_solicitante", NombreUsuario);
                sdsSalida.UpdateParameters.Add("id_estado", "8");
                sdsSalida.UpdateParameters.Add("id_solicitud", id_solicitud);
                sdsSalida.UpdateParameters.Add("HoraSalida", DateTime.Now.ToString());
                sdsSalida.Update();

                gvSolicitud.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void gvSolicitud_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string horaIngreso = e.Row.Cells[11].Text.Trim();
                string horaSalida = e.Row.Cells[12].Text.Trim();
                Button btnEntrada = e.Row.FindControl("btnEntrada") as Button;
                Button btnSalida = e.Row.FindControl("btnSalida") as Button;

                if (horaIngreso == string.Empty || horaIngreso == "&nbsp;")
                {
                    btnEntrada.Visible = true;
                    btnSalida.Visible = false;
                }
                else
                {
                    btnEntrada.Visible = false;

                    if (horaSalida == string.Empty || horaSalida == "&nbsp;")
                    {
                        btnSalida.Visible = true;
                        btnEntrada.Visible = false;
                    }
                    else
                    {
                        btnSalida.Visible = false;
                        btnEntrada.Visible = false;
                    }
                }
            }
        }

    } 
}