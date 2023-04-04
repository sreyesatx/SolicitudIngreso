using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using System.Threading.Tasks;
using System.IO;
using System.Web.UI;

namespace SistemaSolicitudIngreso.Correo
{
    public static class Correo
    {
        private static void Enviar_Correo(string destinatario, string copiaCorreo, string asunto, string body)
        {
            MailMessage mail = new MailMessage();
           // SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
            mail.From = new MailAddress("sistema.acceso@couriers.cl", "Sistemas Atrex", Encoding.UTF8);
            //Aquí ponemos el asunto del correo
            mail.Subject = asunto;
            mail.Body = body;
            mail.IsBodyHtml = true;
            // mail.Body = "Se a enviado una nueva clave: " + Clave + " Recuerde cambiarla para mayor seguridad. Puedes hacerlo visitando la siguiente URL https://192.168.45.6/Atrex/Generales/CambioClave.aspx";
            //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
            mail.To.Add(destinatario);
			mail.Bcc.Add("recibidos.atrex@couriers.cl");
            if (copiaCorreo != string.Empty)
            {
                mail.CC.Add(copiaCorreo);
            }
            //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran
            //mail.Attachments.Add(new Attachment(@"C:\Documentos\carta.docx"));
            //Configuracion del SMTP
           // SmtpServer.Port = 587; //Puerto que utiliza Gmail para sus servicios
            //Especificamos las credenciales con las que enviaremos el mail
           // SmtpServer.Credentials = new System.Net.NetworkCredential("atrexit@gmail.com", "Atrex.,2022");
           // SmtpServer.EnableSsl = true;
			
			System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("mail.couriers.cl", 25);      
            SmtpServer.Credentials = new System.Net.NetworkCredential("sistema.acceso@couriers.cl", "Atrex.,2022");
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
			
			
                       
            try
            {
                SmtpServer.Send(mail);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        SmtpServer.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {                
            }
        }

        public static void CrearCorreo(string encargado, string correo, string copiaCorreo, string mensaje, string fecha, string empresa, string visita, string motivo, string autorizar, string denegar, string posdata)
        {
            StringBuilder body = new StringBuilder(File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Correo/PlantillaCorreo.html")));
            body.Replace("@ENCARGADO", encargado);
            body.Replace("@MENSAJE", mensaje);
            body.Replace("@FECHA", fecha);
            body.Replace("@EMPRESA", empresa);
            body.Replace("@VISITAS", visita);
            body.Replace("@MOTIVO", motivo);
            body.Replace("@AUTORIZAR", autorizar);
            body.Replace("@DENEGAR", denegar);
            body.Replace("@POSDATA", posdata);
            body.Replace("@ANIO", DateTime.Now.Year.ToString());

            //ENVIA CORREO
            Enviar_Correo(correo, copiaCorreo, "Solicitud de ingreso", body.ToString());
            //Enviar_Correo("soporte@atrexchile.cl; sebastian.osorio@atrexchile.cl", "Solicitud de ingreso", body.ToString());
        }
    }
}