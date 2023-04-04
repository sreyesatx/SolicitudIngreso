using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SistemaSolicitudIngreso.Lav
{
    public partial class Consolidado : System.Web.UI.Page
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

            Session.Timeout = 40;


            if (!Page.IsPostBack)
            {
                if (!listaMenu.Contains(Request.Path))
                {
                    Session.Contents.RemoveAll();
                    Response.Redirect("../NoAutorizado.aspx");
                }
            }
        }

        protected void dslistaregistros_Deleting(object sender, SqlDataSourceCommandEventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvContactos.DataBind();
        }

        protected void dslistaregistros_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        protected void ddlBuscaEmpresas_DataBound(object sender, EventArgs e)
        {
            ddlBuscaEmpresas.Items.Insert(0, "");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            try
            {
                this.gvContactos.Columns[0].Visible = false;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Reporte.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    //To Export all pages
                    this.gvContactos.AllowPaging = false;
                    this.gvContactos.DataBind();

                    this.gvContactos.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in this.gvContactos.HeaderRow.Cells)
                    {
                        cell.BackColor = this.gvContactos.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in this.gvContactos.Rows)
                    {
                        row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            
                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = this.gvContactos.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = this.gvContactos.RowStyle.BackColor;
                            }
                            cell.CssClass = "textmode";
                        }
                    }

                    this.gvContactos.RenderControl(hw);

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