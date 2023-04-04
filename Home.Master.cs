using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


    public partial class Home : System.Web.UI.MasterPage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            lblAnio.Text = DateTime.Now.Year.ToString();
            lblMenu.Text = imprimeMenu();
        }
		/*
        public string imprimeMenu() 
        {
            string menu = string.Empty;

            menu += "<div class='collapse navbar-collapse' id='navbarResponsive'>";
            menu += "<ul class='navbar-nav ml-auto'>";            

            if (idUsuario != "0")
            {
                sdsMenu.SelectParameters.Clear();
                sdsMenu.SelectParameters.Add("id_usuario", idUsuario);
                sdsMenu.SelectParameters.Add("main_menu", "Administracion");
                DataView menuAdmin = (DataView)sdsMenu.Select(DataSourceSelectArguments.Empty);

                sdsMenu.SelectParameters.Clear();
                sdsMenu.SelectParameters.Add("id_usuario", idUsuario);
                sdsMenu.SelectParameters.Add("main_menu", "Normal");
                DataView menuNormal = (DataView)sdsMenu.Select(DataSourceSelectArguments.Empty);

                sdsMenu.SelectParameters.Clear();
                sdsMenu.SelectParameters.Add("id_usuario", idUsuario);
                sdsMenu.SelectParameters.Add("main_menu", "LAV");
                DataView menuLav = (DataView)sdsMenu.Select(DataSourceSelectArguments.Empty);

                sdsMenu.SelectParameters.Clear();
                sdsMenu.SelectParameters.Add("id_usuario", idUsuario);
                sdsMenu.SelectParameters.Add("main_menu", "Reportes");
                DataView menuReportes = (DataView)sdsMenu.Select(DataSourceSelectArguments.Empty);

                if (menuAdmin != null && menuAdmin.Count != 0)
                {
                    menu += "<li class='nav-item dropdown'>";
                    menu += "<a class='nav-link dropdown-toggle' href='#' id='" + menuAdmin.ToTable().Rows[0]["main_menu"].ToString().Trim() + "' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" + menuAdmin.ToTable().Rows[0]["main_menu"].ToString().Trim() + "</a>";
                    menu += "<div class='dropdown-menu' aria-labelledby='" + menuAdmin.ToTable().Rows[0]["main_menu"].ToString().Trim() + "'>";

                    for (int i = 0; i < menuAdmin.Count; i++)
                    {
                        menu += "<a class='dropdown-item' href='../" + menuAdmin.ToTable().Rows[i]["direccion_menu"].ToString().Trim() + "'>" + menuAdmin.ToTable().Rows[i]["nombre_menu"].ToString().Trim() + "</a>";
                    }

                    menu += "</div>";
                    menu += "</li>";
                }

                if (menuNormal != null && menuNormal.Count != 0)
                {
                    for (int i = 0; i < menuNormal.Count; i++)
                    {
                        menu += "<li class='nav-item'>";
                        menu += "<a class='nav-link js-scroll-trigger' href='../" + menuNormal.ToTable().Rows[i]["direccion_menu"].ToString().Trim() + "'>" + menuNormal.ToTable().Rows[i]["nombre_menu"].ToString().Trim() + "</a>";
                        menu += "</li>";
                    }
                }

                if (menuLav != null && menuLav.Count != 0)
                {
                    menu += "<li class='nav-item dropdown'>";
                    menu += "<a class='nav-link dropdown-toggle' href='#' id='" + menuLav.ToTable().Rows[0]["main_menu"].ToString().Trim() + "' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" + menuLav.ToTable().Rows[0]["main_menu"].ToString().Trim() + "</a>";
                    menu += "<div class='dropdown-menu' aria-labelledby='" + menuLav.ToTable().Rows[0]["main_menu"].ToString().Trim() + "'>";

                    for (int i = 0; i < menuLav.Count; i++)
                    {
                        menu += "<a class='dropdown-item' href='../" + menuLav.ToTable().Rows[i]["direccion_menu"].ToString().Trim() + "'>" + menuLav.ToTable().Rows[i]["nombre_menu"].ToString().Trim() + "</a>";
                    }

                    menu += "</div>";
                    menu += "</li>";
                }

                if (menuReportes != null && menuReportes.Count != 0)
                {
                    menu += "<li class='nav-item dropdown'>";
                    menu += "<a class='nav-link dropdown-toggle' href='#' id='" + menuReportes.ToTable().Rows[0]["main_menu"].ToString().Trim() + "' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" + menuReportes.ToTable().Rows[0]["main_menu"].ToString().Trim() + "</a>";
                    menu += "<div class='dropdown-menu' aria-labelledby='" + menuReportes.ToTable().Rows[0]["main_menu"].ToString().Trim() + "'>";

                    for (int i = 0; i < menuReportes.Count; i++)
                    {
                        menu += "<a class='dropdown-item' href='../" + menuReportes.ToTable().Rows[i]["direccion_menu"].ToString().Trim() + "'>" + menuReportes.ToTable().Rows[i]["nombre_menu"].ToString().Trim() + "</a>";
                    }

                    menu += "</div>";
                    menu += "</li>";
                }



                menu += "<li class='nav-item'>";
                menu += "<a class='nav-link js-scroll-trigger' href='../CerrarSesion.aspx'>Cerrar sesión</a>";
                menu += "</li>";

            }
            else
            {
                //menu += "<li class='nav-item'>";
                //menu += "<a class='nav-link js-scroll-trigger' href='../ingreso/QuienesSomos.aspx'>Quienes somos</a>";
                //menu += "</li>";

                //para probar en local host quitar carpeta "/ingreso" y para publicar agregar

                menu += "<li class='nav-item'>";
                menu += "<a class='nav-link js-scroll-trigger' href='../ingreso/AgendarVisita.aspx'>Agendar visita</a>";
                menu += "</li>";
                menu += "<li class='nav-item'>";
                menu += "<a class='nav-link js-scroll-trigger' href='../ingreso/IngresoSistema.aspx'>Ingreso al sistema</a>";
                menu += "</li>";
            }

            menu += "</ul>";
            menu += "</div>";

            return menu;
        }
		*/
		
 public string imprimeMenu() 
        {
            string menu = string.Empty;

            menu += "<div class='collapse navbar-collapse' id='navbarResponsive'>";
            menu += "<ul class='navbar-nav ml-auto'>";            

            if (idUsuario != "0")
            {
                sdsMenu.SelectParameters.Clear();
                sdsMenu.SelectParameters.Add("id_usuario", idUsuario);
                sdsMenu.SelectParameters.Add("main_menu", "Control Acceso");
                DataView menuControlAcceso = (DataView)sdsMenu.Select(DataSourceSelectArguments.Empty);

                sdsMenu.SelectParameters.Clear();
                sdsMenu.SelectParameters.Add("id_usuario", idUsuario);
                sdsMenu.SelectParameters.Add("main_menu", "Acta Virtual");
                DataView menuActaVirtual = (DataView)sdsMenu.Select(DataSourceSelectArguments.Empty);

                sdsMenu.SelectParameters.Clear();
                sdsMenu.SelectParameters.Add("id_usuario", idUsuario);
                sdsMenu.SelectParameters.Add("main_menu", "Cobro TI");
                DataView menuCobroTI = (DataView)sdsMenu.Select(DataSourceSelectArguments.Empty);

                if (menuControlAcceso != null && menuControlAcceso.Count != 0)
                {
                    menu += "<li class='nav-item dropdown'>";
                    menu += "<a class='nav-link dropdown-toggle' href='#' id='" + menuControlAcceso.ToTable().Rows[0]["main_menu"].ToString().Trim() + "' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" + menuControlAcceso.ToTable().Rows[0]["main_menu"].ToString().Trim() + "</a>";
                    menu += "<div class='dropdown-menu' aria-labelledby='" + menuControlAcceso.ToTable().Rows[0]["main_menu"].ToString().Trim() + "'>";

                    for (int i = 0; i < menuControlAcceso.Count; i++)
                    {
                        menu += "<a class='dropdown-item' href='../" + menuControlAcceso.ToTable().Rows[i]["direccion_menu"].ToString().Trim() + "'>" + menuControlAcceso.ToTable().Rows[i]["nombre_menu"].ToString().Trim() + "</a>";
                    }

                    menu += "</div>";
                    menu += "</li>";
                }

                if (menuActaVirtual != null && menuActaVirtual.Count != 0)
                {
                    menu += "<li class='nav-item dropdown'>";
                    menu += "<a class='nav-link dropdown-toggle' href='#' id='" + menuActaVirtual.ToTable().Rows[0]["main_menu"].ToString().Trim() + "' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" + menuActaVirtual.ToTable().Rows[0]["main_menu"].ToString().Trim() + "</a>";
                    menu += "<div class='dropdown-menu' aria-labelledby='" + menuActaVirtual.ToTable().Rows[0]["main_menu"].ToString().Trim() + "'>";

                    for (int i = 0; i < menuActaVirtual.Count; i++)
                    {
                        menu += "<a class='dropdown-item' href='../" + menuActaVirtual.ToTable().Rows[i]["direccion_menu"].ToString().Trim() + "'>" + menuActaVirtual.ToTable().Rows[i]["nombre_menu"].ToString().Trim() + "</a>";
                    }
                    menu += "</div>";
                    menu += "</li>";
                }

                if (menuCobroTI != null && menuCobroTI.Count != 0)
                {
                    menu += "<li class='nav-item dropdown'>";
                    menu += "<a class='nav-link dropdown-toggle' href='#' id='" + menuCobroTI.ToTable().Rows[0]["main_menu"].ToString().Trim() + "' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" + menuCobroTI.ToTable().Rows[0]["main_menu"].ToString().Trim() + "</a>";
                    menu += "<div class='dropdown-menu' aria-labelledby='" + menuCobroTI.ToTable().Rows[0]["main_menu"].ToString().Trim() + "'>";

                    for (int i = 0; i < menuCobroTI.Count; i++)
                    {
                        menu += "<a class='dropdown-item' href='../" + menuCobroTI.ToTable().Rows[i]["direccion_menu"].ToString().Trim() + "'>" + menuCobroTI.ToTable().Rows[i]["nombre_menu"].ToString().Trim() + "</a>";
                    }

                    menu += "</div>";
                    menu += "</li>";
                }



                menu += "<li class='nav-item'>";
                menu += "<a class='nav-link js-scroll-trigger' href='../CerrarSesion.aspx'>Cerrar sesión</a>";
                menu += "</li>";

            }
            else
            {
                //menu += "<li class='nav-item'>";
                //menu += "<a class='nav-link js-scroll-trigger' href='../ingreso/QuienesSomos.aspx'>Quienes somos</a>";
                //menu += "</li>";

                //para probar en local host quitar carpeta "/ingreso" y para publicar agregar

                menu += "<li class='nav-item'>";
                menu += "<a class='nav-link js-scroll-trigger' href='../ingreso/AgendarVisita.aspx'>Agendar visita</a>";
                menu += "</li>";
                menu += "<li class='nav-item'>";
                menu += "<a class='nav-link js-scroll-trigger' href='../ingreso/IngresoSistema.aspx'>Ingreso al sistema</a>";
                menu += "</li>";
            }

            menu += "</ul>";
            menu += "</div>";

            return menu;
        }	
    }
