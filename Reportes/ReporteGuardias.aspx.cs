using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolicitudValidar;

namespace SistemaSolicitudIngreso.Reportes
{
    public partial class ReporteGuardias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlBuscaGuardia.DataSource = sdsGuardias;
                ddlBuscaGuardia.DataTextField = "nombre_usuario";
                ddlBuscaGuardia.DataValueField = "nombre_usuario";
                ddlBuscaGuardia.DataBind();
                ddlBuscaGuardia.Items.Insert(0, new ListItem("--Seleccione--", ""));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            
            try
            {
                sdsReportes.SelectParameters.Clear();

                if (ddlBuscaGuardia.SelectedValue != "")
                    sdsReportes.SelectParameters.Add("Guardia", ddlBuscaGuardia.SelectedValue);

                if (txtBuscaFechaDesde.Text.Trim() != string.Empty && txtBuscaFechaHasta.Text.Trim() != string.Empty)
                {
                    sdsReportes.SelectParameters.Add("FechaDesde", Convert.ToDateTime(txtBuscaFechaDesde.Text.Trim()).ToShortDateString());
                    sdsReportes.SelectParameters.Add("FechaHasta", Convert.ToDateTime(txtBuscaFechaHasta.Text.Trim()).ToShortDateString());
                }

                if (txtBuscaNombre.Text.Trim() != string.Empty)
                    sdsReportes.SelectParameters.Add("Nombre", txtBuscaNombre.Text.Trim());

                if (txtBuscaRut.Text.Trim() != string.Empty)
                    sdsReportes.SelectParameters.Add("Rut", txtBuscaRut.Text.Trim());

                DataView datos = (DataView)sdsReportes.Select(DataSourceSelectArguments.Empty);
                if (datos != null)
                {
                    if (datos.ToTable().Rows.Count > 0)
                    {
                        gvReportes.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error','" + Validar.ValidaString(ex.Message) + "','error');", Title), true);
                return;
            }
        }
    }
}