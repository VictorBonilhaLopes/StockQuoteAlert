using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StockQuoteAlert.Controller
{
    class Mail
    {
        MailMessage? mailMessage = null;
        string strMail = string.Empty;
        string strSenha = string.Empty;
        string strDestinatario = string.Empty;
        public void Set()
        {
            var json = File.ReadAllText("appsettings.json");
            var config = JObject.Parse(json);

            strMail = config["Email"]?["From"]?.ToString() ?? "";
            strSenha = config["Email"]?["Password"]?.ToString() ?? "";
            strDestinatario = config["Email"]?["To"]?.ToString() ?? "";
        }

        public void BuildMail(string strMail)
        {
            //this.strMail = strMail;
            mailMessage = new MailMessage(strMail, strDestinatario);

            mailMessage.Subject = "Test";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<p> Test </p>";
            mailMessage.SubjectEncoding = Encoding.GetEncoding("UTF-8");
            mailMessage.BodyEncoding = Encoding.GetEncoding("UTF-8");
        }

        public void SendMail()
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(strMail, strSenha);

            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
        }
    }
}
