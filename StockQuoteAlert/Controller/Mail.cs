using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StockQuoteAlert.Controller
{
    class Mail
    {
        MailMessage? mailMessage = null;
        string strMail = string.Empty;

        public void BuildMail(string strMail)
        {
            this.strMail = strMail;
            mailMessage = new MailMessage(strMail, "MAIL_TO");

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
            smtpClient.Credentials = new NetworkCredential(strMail, "SENHA");

            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
        }
    }
}
