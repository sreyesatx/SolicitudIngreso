using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace SistemaSolicitudIngreso.Lav
{
    public partial class Registro : System.Web.UI.Page
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

                dsTipo.SelectParameters.Clear();
                dsTipo.SelectParameters.Add("TipoCodigo", "C");
                cargaLista(ddlcourier, dsTipo, "str_descripcion", "str_descripcion");               

                dsTipo.SelectParameters.Clear();
                dsTipo.SelectParameters.Add("TipoCodigo", "G");
                cargaLista(ddlguardia, dsTipo, "str_descripcion", "str_codigo");  

                dsTipo.SelectParameters.Clear();
                dsTipo.SelectParameters.Add("TipoCodigo", "P");
                cargaLista(ddlpuesto, dsTipo, "str_descripcion", "str_codigo");  

                dsTipo.SelectParameters.Clear();
                dsTipo.SelectParameters.Add("TipoCodigo", "A");
                cargaLista(ddltipoac, dsTipo, "str_descripcion", "str_codigo");  


                cargaTiempo(ddlhora, 23);
                cargaTiempo(ddlminuto, 59);


            }
        }
        protected void cargaLista(DropDownList lista, SqlDataSource data, string texto, string valor)
        {
            lista.DataSource = data;
            lista.DataTextField = texto;
            lista.DataValueField = valor;
            lista.DataBind();
            lista.Items.Insert(0, new ListItem("", ""));
        }

        protected void cargaTiempo(DropDownList lista, int tiempo)
        {
            for (int i = tiempo; i >= 0; i--)
            {
                lista.Items.Insert(0, new ListItem(i.ToString("00"), i.ToString("00")));
            }
            lista.Items.Insert(0, new ListItem("", ""));
        }

        protected void btnagregar_Click(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }
            try
            {
                dsOperacion.InsertParameters.Clear();
                dsOperacion.InsertParameters.Add("sac_fecha", tbfecha.Text + " " + ddlhora.SelectedItem.ToString() + ":" + ddlminuto.SelectedItem.ToString());              
                dsOperacion.InsertParameters.Add("sac_courier", ddlcourier.SelectedItem.ToString());
                dsOperacion.InsertParameters.Add("sac_observacion", tbobservacion.Text);
                dsOperacion.InsertParameters.Add("sac_tipo", ddltipoac.SelectedValue.ToString());
                dsOperacion.InsertParameters.Add("sac_puesto", ddlpuesto.SelectedItem.ToString());
                dsOperacion.InsertParameters.Add("sac_turno", ddlguardia.SelectedItem.ToString());
                dsOperacion.InsertParameters.Add("sac_user_creac", NombreUsuario);
                dsOperacion.Insert();

                tbobservacion.Text = "";
                gvContactos.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Registro exitoso');", Title), true);

            }
            catch (Exception ex)
            {
            }
        }

        protected void dslistaregistros_Deleting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@user"].Value = NombreUsuario;
        }
    }
}