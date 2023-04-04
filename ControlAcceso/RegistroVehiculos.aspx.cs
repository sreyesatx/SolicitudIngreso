using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolicitudValidar;
using SistemaSolicitudIngreso.Correo;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SistemaSolicitudIngreso.RV
{
    public partial class RegistroVehiculos : System.Web.UI.Page
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
                btnEnviar.CausesValidation = false;
                CargarGrillaIngreso();

                ddltipovehiculo.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddltipovehiculo.Items.Insert(1, new ListItem("Funcionario", "1"));
                ddltipovehiculo.Items.Insert(2, new ListItem("Carga", "2"));
                ddltipovehiculo.SelectedIndex = 0;

                //if (!listaMenu.Contains(Request.Path))
                //{
                //    Session.Contents.RemoveAll();
                //    Response.Redirect("../NoAutorizado.aspx");
                //}
            }
        }

        private void CargarGrillaIngreso()
        {
            dsMantenedor.SelectParameters.Clear();
            dsMantenedor.SelectParameters.Add("tiporeporte", "1");
            dsMantenedor.SelectParameters.Add("empresa", "0");
            DataView datos = dsMantenedor.Select(DataSourceSelectArguments.Empty) as DataView;
            if (datos != null)
            {
                if (datos.ToTable().Rows.Count > 0)
                {
                    gvContactos.DataSource = datos;
                    gvContactos.DataBind();
                }
            }

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarDatos();
        }

        private void BuscarDatos()
        {
            DataTable dtResul = new DataTable();
            dtResul = ConsultaRecurrenciaVehicular(this.txtpatente.Text.Trim(), this.txtdescripcion.Text.Trim(), this.ddlempresas.SelectedValue.ToString(), this.ddltipovehiculo.SelectedValue.ToString());
            if (dtResul != null && dtResul.Rows.Count > 0)
            {
                gvContactos.DataSource = dtResul;
                gvContactos.DataBind();
            }
        }

        private DataTable ConsultaRecurrenciaVehicular(string strPatente, string strNombre, string strEmpresa, string strEstacionamiento)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_Buscar_vehiculos_Recurrentes", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Patente", string.IsNullOrEmpty(strPatente) ? (object)DBNull.Value : strPatente);
                command.Parameters.AddWithValue("@Nombre", string.IsNullOrEmpty(strNombre) ? (object)DBNull.Value : strNombre);
                command.Parameters.AddWithValue("@empresa", string.IsNullOrEmpty(strEmpresa) ? (object)DBNull.Value : strEmpresa);
                command.Parameters.AddWithValue("@estacionamiento", string.IsNullOrEmpty(strEstacionamiento) ? (object)DBNull.Value : strEstacionamiento);
                SqlDataAdapter dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = command;
                DataTable dataTable = new DataTable();
                dataAdapt.Fill(dataTable);
                return dataTable;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtpatente.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Campo patente es obligatorio');", Title), true);
                    return;
                }
                if (txtdescripcion.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Campo Nombre es obligatorio');", Title), true);
                    return;
                }
                if (txtfechavigencia.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Campo Vigencia es obligatorio');", Title), true);
                    return;
                }
                if (ddlempresas.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Campo Empresa es obligatorio');", Title), true);
                    return;
                }
                if (ddltipovehiculo.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Campo Vehículo es obligatorio');", Title), true);
                    return;
                }

                //dsMantenedor.Insert();
                var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_insert_vehiculos", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@sre_patente", txtpatente.Text.ToUpper());
                    command.Parameters.AddWithValue("@sre_nombre", txtdescripcion.Text);
                    command.Parameters.AddWithValue("@sre_fechavigencia", txtfechavigencia.Text);
                    command.Parameters.AddWithValue("@sre_empresa", ddlempresas.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@sre_estacionamiento", ddltipovehiculo.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@sre_user_creac", Session["nombre_usuario"]);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Se agrega Registro Vehícular');", Title), true);
                CargarGrillaIngreso();
            }
            catch (Exception ex)
            {
            }
        }

        private void LimpiarCampos()
        {
            this.ddlempresas.SelectedIndex = 0;
            this.txtpatente.Text = "";
            this.txtfechavigencia.Text = "";
            this.ddltipovehiculo.SelectedIndex = 0;
            this.txtdescripcion.Text = "";
        }

        protected void ddlempresas_DataBound(object sender, EventArgs e)
        {
            ddlempresas.Items.Insert(0, "");
        }

        protected void CerrarSolicitud(object sender, EventArgs e)
        {
            mpePopUp.Hide();
        }

        protected void Enviar(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }

            try
            {
                ModificarFechaVigencia(Convert.ToInt32(txtIDRegistro.Text), txtVigenciaHasta.Text);
                Response.Redirect(Request.Url.ToString());
            }
            catch (Exception ex)
            {
                mpePopUp.Hide();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void btnOpenPopUp_Click(object sender, EventArgs e)
        {
            if (idUsuario == "0")
            {
                Response.Redirect("../IngresoSistema.aspx");
            }
            try
            {
                txtIDRegistro.Text = string.Empty;
                LinkButton obj = (LinkButton)(sender);
                string id_solicitud = obj.CommandArgument.ToString();

                txtIDRegistro.Text = id_solicitud;
                mpePopUp.Show();
            }
            catch (Exception ex)
            {
                mpePopUp.Hide();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("swal('Ha ocurrido un error : " + Validar.ValidaString(ex.Message) + "');", Title), true);
                return;
            }
        }

        protected void onDelete(object sender, EventArgs ae)
        {
            if (hiddenval.Value == "Yes")
            {
                LinkButton clickedbutton = (LinkButton)sender;
                GridViewRow row = (GridViewRow)clickedbutton.NamingContainer;
                string id = row.Cells[2].Text;
                Eliminar(Convert.ToInt32(id), NombreUsuario);
                CargarGrillaIngreso();
            }
        }

        private void Eliminar(int id, string usuario)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_elimina_vehicular", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@sre_id", id);
                command.Parameters.AddWithValue("@sre_user_creac", usuario);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void ModificarFechaVigencia(int id, string fechaVigencia)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SolicitudIngresoConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_Actualiza_Recurrencia", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@sre_id", id);
                command.Parameters.AddWithValue("@nuevFecha", fechaVigencia);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        protected void gvContactos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvContactos.PageIndex = e.NewPageIndex;
            BuscarDatos();
        }
    }
}