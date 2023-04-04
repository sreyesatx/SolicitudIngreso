using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CobroTI_Auditoria : System.Web.UI.Page
{
    private DataTable dtTablaTotales
    {
        get
        {
            if (ViewState["dtTablaTotales"] != null)
                return (DataTable)(ViewState["dtTablaTotales"]);
            else
                return null;
        }
        set { ViewState["dtTablaTotales"] = value; }
    }

    private DataTable dtTablaGrilla
    {
        get
        {
            if (ViewState["dtTablaGrilla"] != null)
                return (DataTable)(ViewState["dtTablaGrilla"]);
            else
                return null;
        }
        set { ViewState["dtTablaGrilla"] = value; }
    }

    private DataTable dtTablaType
    {
        get
        {
            if (ViewState["dtTablaGrilla"] != null)
                return (DataTable)(ViewState["dtTablaType"]);
            else
                return null;
        }
        set { ViewState["dtTablaType"] = value; }
    }

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
    public static GridView gvExcel = new GridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (idUsuario == "0")
        {
            Response.Redirect("../IngresoSistema.aspx");
        }

        Session.Timeout = 40;

        if (!Page.IsPostBack)
        {
            /*
            if (!listaMenu.Contains(Request.Path))
            {
                Session.Contents.RemoveAll();
                Response.Redirect("../NoAutorizado.aspx");
            }
            */

            dtTablaTotales = new DataTable();
            dtTablaTotales.Columns.Add("idempresa", typeof(string));
            dtTablaTotales.Columns.Add("Cant_import", typeof(int));
            dtTablaTotales.Columns.Add("Peso_import", typeof(int));
            dtTablaTotales.Columns.Add("Fob_Import", typeof(int));
            dtTablaTotales.Columns.Add("Cant_export", typeof(int));
            dtTablaTotales.Columns.Add("Peso_export", typeof(int));
            dtTablaTotales.Columns.Add("Fob_export", typeof(int));
            dtTablaTotales.Columns.Add("Total_Cant", typeof(int));
            dtTablaTotales.Columns.Add("Total_Peso", typeof(int));
            dtTablaTotales.Columns.Add("Total_Fob", typeof(int));
            dtTablaTotales.Columns.Add("CantDIPS", typeof(int));
            dtTablaTotales.Columns.Add("CantGuias", typeof(int));
            dtTablaTotales.Columns.Add("CantDussi", typeof(int));
            dtTablaTotales.Columns.Add("CantRepositorio", typeof(int));

            dtTablaGrilla = new DataTable();
            dtTablaGrilla.Columns.Add("idempresa", typeof(string));
            dtTablaGrilla.Columns.Add("CantRepo", typeof(string));
            dtTablaGrilla.Columns.Add("TotalRepo", typeof(string));
            dtTablaGrilla.Columns.Add("CantDIPS", typeof(string));
            dtTablaGrilla.Columns.Add("TotDips", typeof(string));
            dtTablaGrilla.Columns.Add("CantGuias", typeof(string));
            dtTablaGrilla.Columns.Add("TotGuias", typeof(string));
            dtTablaGrilla.Columns.Add("CantDussi", typeof(string));
            dtTablaGrilla.Columns.Add("TotDussi", typeof(string));
            dtTablaGrilla.Columns.Add("TotalCourier", typeof(string));

            dtTablaType = new DataTable();
            dtTablaType.Columns.Add("idempresa", typeof(string));
            dtTablaType.Columns.Add("CantRepo", typeof(int));
            dtTablaType.Columns.Add("TotalRepo", typeof(int));
            dtTablaType.Columns.Add("CantDIPS", typeof(int));
            dtTablaType.Columns.Add("TotDips", typeof(int));
            dtTablaType.Columns.Add("CantGuias", typeof(int));
            dtTablaType.Columns.Add("TotGuias", typeof(int));
            dtTablaType.Columns.Add("CantDussi", typeof(int));
            dtTablaType.Columns.Add("TotDussi", typeof(int));
            dtTablaType.Columns.Add("TotalCourier", typeof(int));

            this.bntExcel.Visible = false;
            this.btnGrabar.Visible = false;

        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlmonth.SelectedIndex == 0 && ddlyear.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Se debe seleccionar Mes y Año');", Title), true);
                return;
            }
            if (this.txtMonto.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Se debe ingresar monto');", Title), true);
                return;
            }
            ConsultaCargaGrilla();
            //
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + ex.Message + "');", Title), true);
            return;
        }
    }

    private void ConsultaCargaGrilla()
    {
        this.GV_modeloPonderacion.DataSource = null;
        this.GV_modeloPonderacion.DataBind();
        dtTablaGrilla.Clear();
        DataTable dt = new DataTable();
        dt = ConsultaTotales(this.ddlyear.SelectedValue.ToString(), this.ddlmonth.SelectedValue.ToString(), this.txtMonto.Text);

        if (dt != null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow fila = dtTablaGrilla.NewRow();
                fila[0] = dt.Rows[i]["idempresa"].ToString();
                fila[1] = Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["CantRepo"]))); ;
                fila[2] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["TotalRepo"])));
                fila[3] = Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["CantDIPS"])));
                fila[4] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["TotDips"])));
                fila[5] = Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["CantGuias"])));
                fila[6] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["TotGuias"])));
                fila[7] = Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["CantDussi"])));
                fila[8] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["TotDussi"])));
                fila[9] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", Convert.ToInt32(dt.Rows[i]["TotalCourier"])));
                dtTablaGrilla.Rows.Add(fila);
            }

            int CantRepo = 0, TotalRepo = 0, CantDIPS = 0, TotDips = 0, CantGuias = 0, TotGuias = 0, CantDussi = 0, TotDussi = 0, TotalCourier = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CantRepo = CantRepo + Convert.ToInt32(dt.Rows[i]["CantRepo"]);
                TotalRepo = TotalRepo + Convert.ToInt32(dt.Rows[i]["TotalRepo"]);
                CantDIPS = CantDIPS + Convert.ToInt32(dt.Rows[i]["CantDIPS"]);
                TotDips = TotDips + Convert.ToInt32(dt.Rows[i]["TotDips"]);
                CantGuias = CantGuias + Convert.ToInt32(dt.Rows[i]["CantGuias"]);
                TotGuias = TotGuias + Convert.ToInt32(dt.Rows[i]["TotGuias"]);
                CantDussi = CantDussi + Convert.ToInt32(dt.Rows[i]["CantDussi"]);
                TotDussi = TotDussi + Convert.ToInt32(dt.Rows[i]["TotDussi"]);
                TotalCourier = TotalCourier + Convert.ToInt32(dt.Rows[i]["TotalCourier"]);
            }

            DataRow fila1 = dtTablaGrilla.NewRow();
            fila1[0] = "";
            fila1[1] = Convert.ToString(string.Format("{0:##,#0.##}", CantRepo));
            fila1[2] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", TotalRepo));
            fila1[3] = Convert.ToString(string.Format("{0:##,#0.##}", CantDIPS));
            fila1[4] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", TotDips));
            fila1[5] = Convert.ToString(string.Format("{0:##,#0.##}", CantGuias));
            fila1[6] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", TotGuias));
            fila1[7] = Convert.ToString(string.Format("{0:##,#0.##}", CantDussi));
            fila1[8] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", TotDussi));
            fila1[9] = "$" + Convert.ToString(string.Format("{0:##,#0.##}", TotalCourier));
            dtTablaGrilla.Rows.Add(fila1);

            this.bntExcel.Visible = true;
            this.btnGrabar.Visible = true;
            this.GV_modeloPonderacion.DataSource = dtTablaGrilla;
            this.GV_modeloPonderacion.DataBind();

            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            GV_modeloPonderacion.GridLines = GridLines.Both;
            GV_modeloPonderacion.HeaderStyle.Font.Bold = true;
            GV_modeloPonderacion.RenderControl(htmltextwrtter);



        }
    }

    private DataTable ConsultaTotales(string year
                                    , string month
                                    , string monto)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["EstadisticaConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("sp_CobroMes_Calculo", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 6000;
            command.Parameters.AddWithValue("@year", string.IsNullOrEmpty(year) ? (object)DBNull.Value : year);
            command.Parameters.AddWithValue("@mes", string.IsNullOrEmpty(month) ? (object)DBNull.Value : month);
            command.Parameters.AddWithValue("@monto", string.IsNullOrEmpty(monto) ? (object)DBNull.Value : monto);
            SqlDataAdapter dataAdapt = new SqlDataAdapter();
            dataAdapt.SelectCommand = command;
            DataTable dataTable = new DataTable();
            dataAdapt.Fill(dataTable);
            return dataTable;
        }
    }

    protected void GV_modeloPonderacion_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblidempresa = (Label)e.Row.FindControl("lblidempresa");
            if (lblidempresa.Text != "" && e.Row.RowIndex >= 0)
            {
                e.Row.Cells[0].BackColor = System.Drawing.Color.Khaki;
                e.Row.Cells[2].BackColor = System.Drawing.Color.LightBlue;
                e.Row.Cells[4].BackColor = System.Drawing.Color.LightBlue;
                e.Row.Cells[6].BackColor = System.Drawing.Color.LightBlue;
                e.Row.Cells[8].BackColor = System.Drawing.Color.LightBlue;
                e.Row.Cells[9].BackColor = System.Drawing.Color.LightGreen;
            }
            if (lblidempresa.Text == "" && e.Row.RowIndex > 0)
            {
                e.Row.Cells[1].BackColor = System.Drawing.Color.Tan;
                e.Row.Cells[2].BackColor = System.Drawing.Color.Tan;
                e.Row.Cells[3].BackColor = System.Drawing.Color.Tan;
                e.Row.Cells[4].BackColor = System.Drawing.Color.Tan;
                e.Row.Cells[5].BackColor = System.Drawing.Color.Tan;
                e.Row.Cells[6].BackColor = System.Drawing.Color.Tan;
                e.Row.Cells[7].BackColor = System.Drawing.Color.Tan;
                e.Row.Cells[8].BackColor = System.Drawing.Color.Tan;
                e.Row.Cells[9].BackColor = System.Drawing.Color.Tan;
            }
        }
    }


    protected void ExportToExcel(object sender, EventArgs e)
    {
        try
        {
            string FileName = "Reporte CobroTI" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvExcel.AllowPaging = false;


                if (dtTablaGrilla != null && dtTablaGrilla.Rows.Count > 0)
                {
                    gvExcel.DataSource = dtTablaGrilla;
                    gvExcel.DataBind();
                }
                else
                {
                    return;
                }

                gvExcel.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvExcel.HeaderRow.Cells)
                {
                    cell.BackColor = gvExcel.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvExcel.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvExcel.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvExcel.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvExcel.RenderControl(hw);

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

    private int GrabarTabla(int year, int month, int monto, string usuario)
    {
        dtTablaType.Clear();
        string strerror = "";
        int strvalidaIngreso = 0;        
        //dt = ConsultaTotales(this.ddlyear.SelectedValue.ToString(), this.ddlmonth.SelectedValue.ToString(), this.txtMonto.Text);
        for (int i = 0; i < dtTablaGrilla.Rows.Count; i++)
        {
            if (dtTablaGrilla.Rows[i]["idempresa"].ToString() != "")
            {
                DataRow fila = dtTablaType.NewRow();
                fila[0] = dtTablaGrilla.Rows[i]["idempresa"].ToString();
                fila[1] = Convert.ToInt32(dtTablaGrilla.Rows[i]["CantRepo"].ToString().Replace("$", "").Replace(".", "")); ;
                fila[2] = Convert.ToInt32(dtTablaGrilla.Rows[i]["TotalRepo"].ToString().Replace("$", "").Replace(".", ""));
                fila[3] = Convert.ToInt32(dtTablaGrilla.Rows[i]["CantDIPS"].ToString().Replace("$", "").Replace(".", ""));
                fila[4] = Convert.ToInt32(dtTablaGrilla.Rows[i]["TotDips"].ToString().Replace("$", "").Replace(".", ""));
                fila[5] = Convert.ToInt32(dtTablaGrilla.Rows[i]["CantGuias"].ToString().Replace("$", "").Replace(".", ""));
                fila[6] = Convert.ToInt32(dtTablaGrilla.Rows[i]["TotGuias"].ToString().Replace("$", "").Replace(".", ""));
                fila[7] = Convert.ToInt32(dtTablaGrilla.Rows[i]["CantDussi"].ToString().Replace("$", "").Replace(".", ""));
                fila[8] = Convert.ToInt32(dtTablaGrilla.Rows[i]["TotDussi"].ToString().Replace("$", "").Replace(".", ""));
                fila[9] = Convert.ToInt32(dtTablaGrilla.Rows[i]["TotalCourier"].ToString().Replace("$", "").Replace(".", ""));
                dtTablaType.Rows.Add(fila);
            }
        }


        if (dtTablaType != null && dtTablaType.Rows.Count > 0)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["EstadisticaConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_Ingreso_Reporte_CobroTI", connection);
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ano", year);
                command.Parameters.AddWithValue("@mes", month);
                command.Parameters.AddWithValue("@monto", monto);
                command.Parameters.AddWithValue("@usuario", usuario);
                //
                SqlParameter param = new SqlParameter("@listadoTotalesEmpresas", SqlDbType.Structured);
                param.Value = dtTablaType;
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "dbo.AUD_Reporte_CobroTI_Type";
                command.Parameters.Add(param);
                //
                command.Parameters.AddWithValue("@InsertValida", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@ErrorMsgValida", SqlDbType.VarChar, 300);
                command.Parameters["@ErrorMsgValida"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                strvalidaIngreso = Convert.ToInt32(command.Parameters["@InsertValida"].Value);
                strerror = Convert.ToString(command.Parameters["@ErrorMsgValida"].Value);

                if (strvalidaIngreso == 5)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + strerror + "');", Title), true);
                    return 0;
                }
                return strvalidaIngreso;
            }
        }
        return strvalidaIngreso;
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        if (ddlmonth.SelectedIndex == 0 && ddlyear.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Se debe seleccionar Mes y Año');", Title), true);
            return;
        }
        if (this.txtMonto.Text == "")
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Se debe ingresar monto');", Title), true);
            return;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        int strResp = GrabarTabla(Convert.ToInt32(this.ddlyear.SelectedValue), Convert.ToInt32(this.ddlmonth.SelectedValue), Convert.ToInt32(this.txtMonto.Text), NombreUsuario);
        if (strResp == 3)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Reporte ya existe en sistema ');", Title), true);
        }

        if (strResp == 1)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Reporte Grabado Existosamente');", Title), true);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}