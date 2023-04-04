using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolicitudValidar;

namespace SistemaSolicitudIngreso.Administracion
{
    public partial class Mantenedores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlEmpresaFuncionario.DataSource = sdsEmpresas;
                ddlEmpresaFuncionario.DataTextField = "nombre_empresa";
                ddlEmpresaFuncionario.DataValueField = "id_empresa";
                ddlEmpresaFuncionario.DataBind();
                ddlEmpresaFuncionario.Items.Insert(0, new ListItem("--Seleccione--", ""));
            }
        }

        protected void ddlEmpresaFuncionario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                sdsContactos.SelectParameters.Clear();
                sdsContactos.SelectParameters.Add("id_empresa", ddlEmpresaFuncionario.SelectedValue);
                DataView datos = (DataView)sdsContactos.Select(DataSourceSelectArguments.Empty);
                gvContactos.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
            }
        }

        protected void btnAgregarFuncionario_Click(object sender, EventArgs e)
        {
            try
            {
                sdsContactos.InsertParameters.Clear();
                sdsContactos.InsertParameters.Add("nombre_contacto", txtNombreFuncionario.Text.Trim());
                sdsContactos.InsertParameters.Add("correo_contacto", txtCorreoFuncionario.Text.Trim());
                sdsContactos.InsertParameters.Add("id_empresa", ddlEmpresaFuncionario.SelectedValue);
                sdsContactos.Insert();
                gvContactos.DataBind();

                txtNombreFuncionario.Text = string.Empty;
                txtCorreoFuncionario.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
            }
        }
    }
}