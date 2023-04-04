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
using System.Drawing;
using System.Diagnostics;

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



        private DataTable dtDatosConsultados
        {
            get
            {
                if (ViewState["dtDatosConsultados"] != null)
                    return (DataTable)(ViewState["dtDatosConsultados"]);
                else
                    return null;
            }
            set { ViewState["dtDatosConsultados"] = value; }
        }

        private static DataTable dtDatosConsultados_2;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            Session.Timeout = 40;

            if (!Page.IsPostBack)
            {

                dtDatosConsultados = new DataTable();
                dtDatosConsultados.Columns.Add("srv_patente", typeof(string));
                dtDatosConsultados.Columns.Add("TipoIngreso", typeof(string));
                dtDatosConsultados.Columns.Add("Autorizado", typeof(string));
                dtDatosConsultados.Columns.Add("Hora_Lectura", typeof(string));
                dtDatosConsultados.Columns.Add("Hora_Salida", typeof(string));
                dtDatosConsultados.Columns.Add("Courier", typeof(string));
                dtDatosConsultados.Columns.Add("Nombre", typeof(string));
                dtDatosConsultados.Columns.Add("Fecha_Vigencia", typeof(string));
                dtDatosConsultados.Columns.Add("Estacionamiento", typeof(string));
                dtDatosConsultados.Columns.Add("id_solicitud", typeof(string));
                dtDatosConsultados.Columns.Add("nombre_solicitante", typeof(string));
                dtDatosConsultados.Columns.Add("rut_solicitante", typeof(string));
                dtDatosConsultados.Columns.Add("correo_solicitante", typeof(string));
                dtDatosConsultados.Columns.Add("empresa", typeof(string));

                ConsultaRevistrosVehicular("", "", 1);
            }
        }

        private void ConsultaRevistrosVehicular(string patente, string fecha, int consulta)
        {
            dtDatosConsultados.Clear();
            dtDatosConsultados.Rows.Clear();

            dtDatosConsultados_2 = new DataTable();
            dtDatosConsultados_2.Clear();
            dtDatosConsultados_2.Rows.Clear();
            DataTable dt = new DataTable();
            if (consulta == 1)
            {
                dt = ConsultaIngresoVehiculos("", "", consulta);
            }
            else
            {
                dt = ConsultaIngresoVehiculos(patente, fecha, consulta);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                dtDatosConsultados_2 = dt;
                gvSolicitud.DataSource = dtDatosConsultados_2;
                gvSolicitud.DataBind();
            }
            else
            {
                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('No se encuentran registros');", Title), true);
                //ConsultaRevistrosVehicular("", "", 1);                
                gvSolicitud.DataSource = dt;
                gvSolicitud.DataBind();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }
            //
            try
            {
                if (this.txtPatente.Text != "" && this.txtBuscaFechaVisita.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Se debe ingresar una fecha de búsqueda');", Title), true);
                    return;
                }
                if (this.txtPatente.Text == "" && this.txtBuscaFechaVisita.Text == "")
                {
                    ConsultaRevistrosVehicular("", "", 1);
                }
                else
                {
                    ConsultaRevistrosVehicular(this.txtPatente.Text, this.txtBuscaFechaVisita.Text, 2);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        private DataTable ConsultaIngresoVehiculos(string str_patente
                                                , string str_Fecha
                                                , int consulta)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Sp_Consulta_Ingreso_Vehiculos", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.AddWithValue("@Patente", string.IsNullOrEmpty(str_patente) ? (object)DBNull.Value : str_patente);
                command.Parameters.AddWithValue("@fecha", string.IsNullOrEmpty(str_Fecha) ? (object)DBNull.Value : str_Fecha);
                command.Parameters.AddWithValue("@consulta", consulta);
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
            if (this.txtPatente.Text == "" && this.txtBuscaFechaVisita.Text == "")
            {
                ConsultaRevistrosVehicular("", "", 1);
            }
            else
            {
                ConsultaRevistrosVehicular(this.txtPatente.Text, this.txtBuscaFechaVisita.Text, 2);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtBuscaFechaVisita.Text = "";
            this.txtPatente.Text = "";
            gvSolicitud.DataSource = null;
            gvSolicitud.DataBind();
            ConsultaRevistrosVehicular("", "", 1);
        }

        protected void gvSolicitud_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string v = e.Row.Cells[3].Text.Replace("&nbsp;", "");
                if (v != "")
                {
                    e.Row.Cells[3].BackColor = System.Drawing.Color.Tan;
                    e.Row.Cells[4].BackColor = System.Drawing.Color.Tan;
                    e.Row.Cells[5].BackColor = System.Drawing.Color.Tan;
                    e.Row.Cells[6].BackColor = System.Drawing.Color.Tan;
                }
                //
                if (e.Row.Cells[7].Text != "0")
                {
                    e.Row.Cells[7].BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Cells[8].BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Cells[9].BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Cells[10].BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Cells[11].BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Cells[12].BackColor = System.Drawing.Color.LightBlue;
                }
                if (e.Row.Cells[2].Text.Replace("&nbsp;", "") != "" && e.Row.Cells[5].Text.Replace("&nbsp;", "") == "" && e.Row.Cells[7].Text.Replace("&nbsp;", "") == "0")
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#cfeb59");
                }
            }
        }
        protected void OnDataBound(object sender, EventArgs e)
        {

            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "Lectura Patente";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = "Registro Recurrentes";
            row.Controls.Add(cell);
            cell.BackColor = System.Drawing.Color.Tan;

            cell = new TableHeaderCell();
            cell.ColumnSpan = 6;
            cell.Text = "Solicitud Ingreso";
            row.Controls.Add(cell);
            cell.BackColor = System.Drawing.Color.LightBlue;

            row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
            gvSolicitud.HeaderRow.Parent.Controls.AddAt(0, row);

        }

    }
}