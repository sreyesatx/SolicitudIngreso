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
    public partial class RegistroDiario : System.Web.UI.Page
    {
        public static string ParamTotalesVar = "";
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
                ddlBuscaEmpresas.DataSource = sdsEmpresas;
                ddlBuscaEmpresas.DataTextField = "nombre_empresa";
                ddlBuscaEmpresas.DataValueField = "id_empresa";
                ddlBuscaEmpresas.DataBind();
                ddlBuscaEmpresas.Items.Insert(0, new ListItem("--Seleccione--", "0"));


                DataTable dt = new DataTable();
                dt = ConsultaTotales();
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvTotales.DataSource = dt;
                    gvTotales.DataBind();
                }
                
                CargarGrilla();

                ddlTipBusqueda.Items.Insert(0, new ListItem("--Seleccione--", ""));
                ddlTipBusqueda.Items.Insert(1, new ListItem("Autorizados", "TotalAutorizados"));
                ddlTipBusqueda.Items.Insert(2, new ListItem("Total Salida", "TotSalida"));
                ddlTipBusqueda.Items.Insert(3, new ListItem("Total Rechazos", "TotRechaZos"));
                ddlTipBusqueda.Items.Insert(4, new ListItem("Ingreso Peatonal", "TotIngresoPeatonal"));
                ddlTipBusqueda.Items.Insert(5, new ListItem("Salida Peatonal", "TotSalidaPeatonal"));
                ddlTipBusqueda.Items.Insert(6, new ListItem("Ingreso Vehicular", "TotIngresoVehicular"));
                ddlTipBusqueda.Items.Insert(7, new ListItem("Salida Vehicular", "TotSalidaVehicular"));


                ddlTipoIngreso.Items.Insert(0, new ListItem("--Seleccione--", ""));
                ddlTipoIngreso.Items.Insert(1, new ListItem("Peatonal", "1"));
                ddlTipoIngreso.Items.Insert(2, new ListItem("Vehicular", "2"));
                ParamTotalesVar = "";
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

            LimpiarControles();
            ParamTotalesVar = TipoBusqueda;
            //                  
            DataTable dt = new DataTable();
            dt = ConsultaListaXTotales(TipoBusqueda);

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
                sdsSolicitudes.SelectParameters.Clear();
                //
                if (ddlBuscaEmpresas.SelectedIndex > 0)
                {
                    sdsSolicitudes.SelectParameters.Add("Empresa", ddlBuscaEmpresas.SelectedValue);
                }
                else
                {
                    sdsSolicitudes.SelectParameters.Add("Empresa", " ");
                }
                //
                if (this.txtBuscaRut.Text.Trim() != "")
                {
                    sdsSolicitudes.SelectParameters.Add("RutSolicitante", this.txtBuscaRut.Text.Trim());
                }
                else
                {
                    sdsSolicitudes.SelectParameters.Add("RutSolicitante", " ");
                }
                //
                if (this.txtBuscaNombre.Text.Trim() != "")
                {
                    sdsSolicitudes.SelectParameters.Add("NombreSolicitante", this.txtBuscaNombre.Text.Trim());
                }
                else
                {
                    sdsSolicitudes.SelectParameters.Add("NombreSolicitante", " ");
                }  
                //
                if (this.txtPatente.Text.Trim() != "")
                {
                    sdsSolicitudes.SelectParameters.Add("Patente", this.txtPatente.Text.Trim());
                }
                else
                {                 
                    sdsSolicitudes.SelectParameters.Add("Patente", " ");
                }                
                //
                if (ddlTipBusqueda.SelectedIndex > 0)
                {
                    sdsSolicitudes.SelectParameters.Add("TipoBusqueda", ddlTipBusqueda.SelectedValue);
                }
                else
                {
                    sdsSolicitudes.SelectParameters.Add("TipoBusqueda", " ");
                }
                //
                if (ddlTipoIngreso.SelectedIndex > 0)
                {
                    sdsSolicitudes.SelectParameters.Add("TipoIngreso", ddlTipoIngreso.SelectedValue);
                }
                else
                {
                    sdsSolicitudes.SelectParameters.Add("TipoIngreso", " ");
                }
               
                //
                DataView datos = (DataView)sdsSolicitudes.Select(DataSourceSelectArguments.Empty);
                if (datos != null)
                {
                    if (datos.Count != 0)
                    {
                        gvSolicitud.DataSource = datos;
                        gvSolicitud.DataBind();
                    }
                    else
                    {
                        gvSolicitud.DataSource = null;
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
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        string tipoIngreso = e.Row.Cells[14].Text.Trim();
                        string horaIngreso = e.Row.Cells[12].Text.Trim();
                        string horaSalida = e.Row.Cells[13].Text.Trim();
                        string estado = e.Row.Cells[16].Text.Trim();
                        Label lblcomentario = e.Row.FindControl("lblComentario") as Label;

                        if (tipoIngreso == "1")
                        {
                            if (horaIngreso == string.Empty || horaIngreso == "&nbsp;")
                            {
                            }
                            else
                            {
                                if (horaSalida == string.Empty || horaSalida == "&nbsp;")
                                {
                                }
                                else
                                {
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

        protected void ddlBuscaEmpresas_ddlChanged(object sender, EventArgs e)
        {
            if (ddlBuscaEmpresas.SelectedIndex > 0)
            {
				 ddlBuscaEmpresas.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F0E1");				         
            }
            else
            {
                  ddlBuscaEmpresas.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            }
			 CargarGrilla();  
        }
        protected void txtBuscaNombre_TxtChanged(object sender, EventArgs e)
        {
            if (this.txtBuscaNombre.Text.Trim() != "")
            {				
                txtBuscaNombre.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F0E1");		
            }
            else
            {
                txtBuscaNombre.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            }  
			CargarGrilla();
        }
        protected void txtBuscaRut_TxtChanged(object sender, EventArgs e)
        {
            if (this.txtBuscaRut.Text.Trim() != "")
            {
                txtBuscaRut.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F0E1");				
            }
            else
            {
                txtBuscaRut.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            }
			CargarGrilla();
        }
        protected void txtPatente_TxtChanged(object sender, EventArgs e)
        {
            if (this.txtPatente.Text.Trim() != "")
            {
                txtPatente.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F0E1");				
            }
            else
            {
                txtPatente.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            } 
			CargarGrilla();
        }
        protected void ddlTipBusqueda_ddlChanged(object sender, EventArgs e)
        {
            if (ddlTipBusqueda.SelectedIndex > 0)
            {
                ddlTipBusqueda.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F0E1");				
            }
            else
            {
                ddlTipBusqueda.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            }
			CargarGrilla();
        }
        protected void ddlTipoIngreso_ddlChanged(object sender, EventArgs e)
        {
            if (ddlTipoIngreso.SelectedIndex > 0)
            {
                ddlTipoIngreso.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2F0E1");				
            }
            else
            {
                ddlTipoIngreso.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            }
			CargarGrilla();
        }

        private void LimpiarControles()
        {
            ddlTipoIngreso.SelectedIndex = 0;
            ddlTipBusqueda.SelectedIndex = 0;
            txtPatente.Text = "";
            txtBuscaRut.Text = "";
            txtBuscaNombre.Text = "";
            ddlBuscaEmpresas.SelectedIndex = 0;
			ddlTipoIngreso.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
			ddlTipBusqueda.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
			txtPatente.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
			txtBuscaRut.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
			txtBuscaNombre.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
			ddlBuscaEmpresas.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        }
    }   
}