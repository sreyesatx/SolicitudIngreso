using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace SistemaSolicitudIngreso.Reportes
{
    public partial class ReporteNPU : System.Web.UI.Page
    {
        private static string filtro = "(1,2,3,5)";


        GridView dgvExcel = new GridView();
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
                    //Session.Contents.RemoveAll();
                    //Response.Redirect("../NoAutorizado.aspx");
                }
                //
                ddlBuscaEmpresas.DataSource = sdsEmpresas;
                ddlBuscaEmpresas.DataTextField = "nombre_empresa";
                ddlBuscaEmpresas.DataValueField = "id_empresa";
                ddlBuscaEmpresas.DataBind();
                ddlBuscaEmpresas.Items.Insert(0, new ListItem("--Seleccione--", "0"));
                ddlBuscaEmpresas.SelectedValue = Session["idempresa"].ToString();
            }
        }
       
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (rblTipoIngreso.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('¡Debe seleccionar Tipo de Ingreso!');", Title), true);
                return;
            }

            if ((txtBuscaFechaDesde.Text.Trim() == "" && txtBuscaFechaHasta.Text.Trim() != "") || (txtBuscaFechaDesde.Text.Trim() != "" && txtBuscaFechaHasta.Text.Trim() == ""))
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('¡Debe seleccionar rango de fecha!');", Title), true);
                return;
            }

            string strEmpresa="";
            if (ddlBuscaEmpresas.SelectedValue.ToString() != "0") strEmpresa = ddlBuscaEmpresas.SelectedValue.ToString();

            string strTipo = "";
            if (rblTipoIngreso.SelectedValue.ToString() != "0") strTipo = rblTipoIngreso.SelectedValue.ToString();


            DataTable dt = new DataTable();
            dt = ConsultaReporte(strEmpresa, txtBuscaFechaDesde.Text.Trim(), txtBuscaFechaHasta.Text.Trim(), txtBuscaNombre.Text, txtBuscaRut.Text, strTipo, rblTop25.SelectedValue.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                gvReportes.DataSource = dt;
                gvReportes.DataBind();
            }
        } 


		protected DataTable buscar()
		{
			 DataTable dt = new DataTable();
			if (rblTipoIngreso.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('¡Debe seleccionar Tipo de Ingreso!');", Title), true);
                return dt;
            }

            if ((txtBuscaFechaDesde.Text.Trim() == "" && txtBuscaFechaHasta.Text.Trim() != "") || (txtBuscaFechaDesde.Text.Trim() != "" && txtBuscaFechaHasta.Text.Trim() == ""))
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('¡Debe seleccionar rango de fecha!');", Title), true);
                return dt;
            }

            string strEmpresa="";
            if (ddlBuscaEmpresas.SelectedValue.ToString() != "0") strEmpresa = ddlBuscaEmpresas.SelectedValue.ToString();

            string strTipo = "";
            if (rblTipoIngreso.SelectedValue.ToString() != "0") strTipo = rblTipoIngreso.SelectedValue.ToString();

            
            dt = ConsultaReporte(strEmpresa, txtBuscaFechaDesde.Text.Trim(), txtBuscaFechaHasta.Text.Trim(), txtBuscaNombre.Text, txtBuscaRut.Text, strTipo, rblTop25.SelectedValue.ToString());
			
			return dt;
		}	

        private DataTable ConsultaReporte(string strEmpresa
                                       , string strFechaDesde
                                       , string strFechaHasta
                                       , string strNombre
                                       , string strRut
                                       , string strTipo
                                       , string strTop)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_lista_reporte_npu_2", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Empresa", string.IsNullOrEmpty(strEmpresa) ? (object)DBNull.Value : strEmpresa);
                command.Parameters.AddWithValue("@FechaDesde", string.IsNullOrEmpty(strFechaDesde) ? (object)DBNull.Value : strFechaDesde);
                command.Parameters.AddWithValue("@FechaHasta", string.IsNullOrEmpty(strFechaHasta) ? (object)DBNull.Value : strFechaHasta);
                command.Parameters.AddWithValue("@Nombre", string.IsNullOrEmpty(strNombre) ? (object)DBNull.Value : strNombre);
                command.Parameters.AddWithValue("@Rut", string.IsNullOrEmpty(strRut) ? (object)DBNull.Value : strRut);
                command.Parameters.AddWithValue("@TipoIngreso", Convert.ToInt32(strTipo));
                command.Parameters.AddWithValue("@Top25", strTop);
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                DataTable dataTable = new DataTable();
                dataAdapt.Fill(dataTable);
                return dataTable;
            }
        }

        public static DataTable ConsultaDOC(string strEmpresa
                                               , string strFechaDesde
                                               , string strFechaHasta
                                               , string strNombre
                                               , string strRut
                                               , string strTipoIngreso
                                               , string strTop25)
        {
            DataTable dataTable = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_lista_reporte_npu_2", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Empresa", strEmpresa);
                command.Parameters.AddWithValue("@FechaDesde", strFechaDesde);
                command.Parameters.AddWithValue("@FechaHasta", strFechaHasta);
                command.Parameters.AddWithValue("@Nombre", strNombre);
                command.Parameters.AddWithValue("@Rut", strRut);
                command.Parameters.AddWithValue("@TipoIngreso", strTipoIngreso);
                command.Parameters.AddWithValue("@Top25", strTop25);
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                //
                dataAdapt.SelectCommand = command;
                dataAdapt.Fill(dataTable);
                //
                connection.Close();
                return dataTable;
            }
        }

        private DataTable ConsultarExportacion()
        {
            string strEmpresa = "";
            string strFechaDesde = "";
            string strFechaHasta = "";
            string strNombre = "";
            string strRut = "";
            string strTipoIngreso = "";
            string strTop25 = "";
            //
            if (ddlBuscaEmpresas.SelectedValue != "0")
            {
                strEmpresa = ddlBuscaEmpresas.SelectedValue;
            }
            if (txtBuscaFechaDesde.Text.Trim() != string.Empty)
            {
                strFechaDesde = txtBuscaFechaDesde.Text.Trim();
            }
            if (txtBuscaFechaHasta.Text.Trim() != string.Empty)
            {
                strFechaHasta = txtBuscaFechaHasta.Text.Trim();
            }
            if (txtBuscaNombre.Text.Trim() != string.Empty)
            {
                strNombre = txtBuscaNombre.Text.Trim();
            }
            if (txtBuscaRut.Text.Trim() != string.Empty)
            {
                strRut = txtBuscaRut.Text.Trim();
            }
            if (rblTipoIngreso.SelectedValue != "0")
            {
                strTipoIngreso = rblTipoIngreso.SelectedValue.ToString();
            }
            if (rblTop25.SelectedValue != "0")
            {
                strTop25 = rblTop25.SelectedValue.ToString();
            }
            //
            DataTable dt = new DataTable();
            dt = ConsultaDOC(strEmpresa
                            , strFechaDesde
                            , strFechaHasta
                            , strNombre
                            , strRut
                            , strTipoIngreso
                            , strTop25);
            return dt;
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            try
            {
                //this.gvContactos.Columns[0].Visible = false;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Reporte.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    //To Export all pages
                    this.dvexcel.AllowPaging = false;

                    DataTable dt = new DataTable();
                    dt = buscar();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dgvExcel.DataSource = dt;
                        dgvExcel.DataBind();
                    }
                    else
                    {
                        return;
                    }

                    this.dgvExcel.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in this.dgvExcel.HeaderRow.Cells)
                    {
                        cell.BackColor = this.dgvExcel.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in this.dgvExcel.Rows)
                    {
                        row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {

                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = dgvExcel.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = dgvExcel.RowStyle.BackColor;
                            }
                            cell.CssClass = "textmode";
                        }
                    }

                    dgvExcel.RenderControl(hw);

                    //style to format numbers to string
                    string style = @"<style> .textmode { } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}