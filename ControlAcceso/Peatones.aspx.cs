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
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;

namespace SistemaSolicitudIngreso.AccesoPeatones
{
    public partial class Peatones : System.Web.UI.Page
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

        public static string ParamTotalesVar;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }
            Session.Timeout = 20;

            if (!Page.IsPostBack)
            {
                /*if (!listaMenu.Contains(Request.Path))
                {
                    Session.Contents.RemoveAll();
                    Response.Redirect("../NoAutorizado.aspx");
                }*/

                ddlBuscaEmpresas.DataSource = sdsEmpresas;
                ddlBuscaEmpresas.DataTextField = "nombre_empresa";
                ddlBuscaEmpresas.DataValueField = "id_empresa";
                ddlBuscaEmpresas.DataBind();
                ddlBuscaEmpresas.Items.Insert(0, new ListItem("--Seleccione--", "0"));

                ParamTotalesVar = "";

                DataTable dt = new DataTable();
                dt = ConsultaTotales();
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvTotales.DataSource = dt;
                    gvTotales.DataBind();
                }

                txtBuscaFechaVisita.Text = DateTime.Now.ToString("dd/MM/yyyy");
                CargarGrilla();
            }
        }

        private DataTable ConsultaTotales()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Sp_Contado_Visitas", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                DataTable dataTable = new DataTable();
                dataAdapt.Fill(dataTable);
                return dataTable;
            }
        }

        protected void gvSolicitud_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSolicitud.PageIndex = e.NewPageIndex;
            if (ParamTotalesVar == "")
            {
                CargarGrilla();
            }
            else
            {
                CargarGrillaSolicitudXTotales();
            }
        }

        private void CargarGrillaSolicitudXTotales()
        {
            DataTable dt = new DataTable();
            dt = ConsultaListaXTotales(ParamTotalesVar);

            if (dt != null && dt.Rows.Count > 0)
            {
                gvSolicitud.DataSource = dt;
                gvSolicitud.DataBind();
            }
            else
            {
                gvSolicitud.DataSource = null;
                gvSolicitud.DataBind();
            }
        }

        protected void buscarTotalesXItem_Click(object sender, EventArgs e)
        {
            LinkButton obj = (LinkButton)(sender);
            string TipoBusqueda = obj.CommandArgument.ToString();

            DVLabel.Visible = true;
            if (TipoBusqueda == "TotIngresos") { lblTipoBusqueda.InnerText = "Total Ingresos"; }
            if (TipoBusqueda == "TotalAutorizados") { lblTipoBusqueda.InnerText = "Total Autorizados"; }
            if (TipoBusqueda == "TotSalida") { lblTipoBusqueda.InnerText = "Total Salida"; }
            if (TipoBusqueda == "TotRechaZos") { lblTipoBusqueda.InnerText = "Total Rechazo"; }
            if (TipoBusqueda == "TotIngresoPeatonal") { lblTipoBusqueda.InnerText = "Total Ingreso Peatonal"; }
            if (TipoBusqueda == "TotSalidaPeatonal") { lblTipoBusqueda.InnerText = "Total Salida Peatonal"; }
            if (TipoBusqueda == "TotIngresoVehicular") { lblTipoBusqueda.InnerText = "Total Ingreso Vehicular"; }
            if (TipoBusqueda == "TotSalidaVehicular") { lblTipoBusqueda.InnerText = "Total Salida Vehicular"; }
            //                  
            DataTable dt = new DataTable();
            dt = ConsultaListaXTotales(TipoBusqueda);
            ParamTotalesVar = TipoBusqueda;

            if (dt != null && dt.Rows.Count > 0)
            {
                gvSolicitud.DataSource = dt;
                gvSolicitud.DataBind();
            }
            else
            {
                gvSolicitud.DataSource = null;
                gvSolicitud.DataBind();
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LinkButton lb = e.CommandSource as LinkButton;
                GridViewRow gvr = lb.Parent.Parent as GridViewRow;
                gvr.Cells[0].BackColor = System.Drawing.Color.White;
                gvr.Cells[1].BackColor = System.Drawing.Color.White;
                gvr.Cells[2].BackColor = System.Drawing.Color.White;
                gvr.Cells[3].BackColor = System.Drawing.Color.White;
                gvr.Cells[4].BackColor = System.Drawing.Color.White;
                gvr.Cells[5].BackColor = System.Drawing.Color.White;
                gvr.Cells[6].BackColor = System.Drawing.Color.White;
                gvr.Cells[7].BackColor = System.Drawing.Color.White;
                if (e.CommandArgument.Equals("TotIngresos")) { gvr.Cells[0].BackColor = System.Drawing.Color.DarkSalmon; }
                if (e.CommandArgument.Equals("TotalAutorizados")) { gvr.Cells[1].BackColor = System.Drawing.Color.DarkSalmon; }
                if (e.CommandArgument.Equals("TotSalida")) { gvr.Cells[2].BackColor = System.Drawing.Color.DarkSalmon; }
                if (e.CommandArgument.Equals("TotRechaZos")) { gvr.Cells[3].BackColor = System.Drawing.Color.DarkSalmon; }
                if (e.CommandArgument.Equals("TotIngresoPeatonal")) { gvr.Cells[4].BackColor = System.Drawing.Color.DarkSalmon; }
                if (e.CommandArgument.Equals("TotSalidaPeatonal")) { gvr.Cells[5].BackColor = System.Drawing.Color.DarkSalmon; }
                if (e.CommandArgument.Equals("TotIngresoVehicular")) { gvr.Cells[6].BackColor = System.Drawing.Color.DarkSalmon; }
                if (e.CommandArgument.Equals("TotSalidaVehicular")) { gvr.Cells[7].BackColor = System.Drawing.Color.DarkSalmon; }
            }
            catch (Exception ex)
            {
            }
        }

        private DataTable ConsultaListaXTotales(string strTipoBusqueda)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Sp_consulta_ListaXTotales", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TipoBusqueda", strTipoBusqueda);
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                DataTable dataTable = new DataTable();
                dataAdapt.Fill(dataTable);
                return dataTable;
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
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
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
                DVLabel.Visible = false;
                CargarGrilla();

                ParamTotalesVar = "";

                DataTable dt = new DataTable();
                dt = ConsultaTotales();
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvTotales.DataSource = dt;
                    gvTotales.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        private void CargarGrilla()
        {
            try
            {
				 string[] cortado = txtBuscaRut.Text.Split('¿');
                if (cortado.Length > 1)
                    txtBuscaRut.Text = cortado[1].Substring(0, 7);
               
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
                        gvSolicitud.DataSource = datos;
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

        protected void CerrarColegas(object sender, EventArgs e)
        {
            colegasPopUp.Hide();
        }

        protected void gvSolicitud_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (gvSolicitud.Rows.Count >= 0)
                {
                    e.Row.Cells[14].Visible = false;
                    e.Row.Cells[15].Visible = false;
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        string tipoIngreso = e.Row.Cells[14].Text.Trim();
                        string horaIngreso = e.Row.Cells[12].Text.Trim();
                        string horaSalida = e.Row.Cells[13].Text.Trim();
                        string estado = e.Row.Cells[15].Text.Trim();
                        Button btnEntrada = e.Row.FindControl("btnEntrada") as Button;
                        Button btnSalida = e.Row.FindControl("btnSalida") as Button;
                        Label lblcomentario = e.Row.FindControl("lblComentario") as Label;

                        if (tipoIngreso == "1")
                        {
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
                        else
                        {
                            if (tipoIngreso == "3")
                                lblcomentario.Text = "Acompañante";
                        }

                        if ((estado == "1") || (estado == "2") || (estado == "3") || (estado == "7") || (estado == "5"))
                        {
                            btnSalida.Visible = false;
                            btnEntrada.Visible = false;

                            e.Row.BackColor = System.Drawing.Color.Bisque;
                            if (estado == "1")
                            {
                                lblcomentario.Text = "A la espera de aprobación de Empresa";
                            }
                            if (estado == "2")
                            {
                                lblcomentario.Text = "A la espera de aprobación de Atrex";
                            }
                            if (estado == "3")
                            {
                                lblcomentario.Text = "Rechazado por Empresa";
                            }
                            if (estado == "5")
                            {
                                lblcomentario.Text = "Pendiente";
                            }
                            if (estado == "7")
                            {
                                lblcomentario.Text = "Rechazado por Atrex";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}