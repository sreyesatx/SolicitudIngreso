using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolicitudValidar;


public partial class IngresoSistema : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        buscar();
    }

    protected void buscar()
    {
        try
        {
            string usuario = Request.Form["usr"] ?? "";
            string pass = Request.Form["pwd"] ?? "";

            if (usuario.Trim() != "" && pass.Trim() != "")
            {
                sdsUsuario.SelectParameters.Clear();
                sdsUsuario.SelectParameters.Add("usuario", usuario);
                sdsUsuario.SelectParameters.Add("pass", pass);
                DataView datos = (DataView)sdsUsuario.Select(DataSourceSelectArguments.Empty);
                if (datos != null)
                {
                    if (datos.Count != 0)
                    {
                        string id_usuario = datos.ToTable().Rows[0]["id_usuario"].ToString();
                        string nombre_usuario = datos.ToTable().Rows[0]["nombre_usuario"].ToString();
                        string id_empresa = datos.ToTable().Rows[0]["idempresa"].ToString();

                        sdsMenu.SelectParameters.Clear();
                        sdsMenu.SelectParameters.Add("id_usuario", id_usuario);
                        DataView menu = (DataView)sdsMenu.Select(DataSourceSelectArguments.Empty);
                        if (menu != null)
                        {
                            if (menu.Count != 0)
                            {
                                Session["idUsuario"] = id_usuario;
                                Session["nombre_usuario"] = nombre_usuario;
                                Session["idempresa"] = id_empresa;
                                List<string> lista = new List<string>();

                                for (int i = 0; i < menu.Count; i++)
                                {
                                    //para probar en local host quitar carpeta "/ingreso" y para publicar agregar
                                    //lista.Add("/" + menu.ToTable().Rows[i]["direccion_menu"].ToString());
                                    lista.Add("/ingreso/" + menu.ToTable().Rows[i]["direccion_menu"].ToString());
                                }
                                Session["listaMenu"] = lista;

                                string plantilla = menu.ToTable().Rows[0]["direccion_menu"].ToString();
                                Response.Redirect(plantilla);
                            }
                        }
                    }
                    else
                        lblError.Text = "Usuario o contraseña incorrecta";
                        
                }
                else
                    lblError.Text = "Usuario o contraseña no son validos";
            }

        }
        catch (Exception ex)
        {
            lblError.Text = Validar.ValidaString(ex.Message);
            return;
        }
    }
}
