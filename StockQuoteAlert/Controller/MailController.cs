using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StockQuoteAlert.Model;

namespace StockQuoteAlert.Controller
{
    class MailController
    {
        Email csEmail = new Email();
        MailMessage? mailMessage = null;

        public void Set()
        {
            var json = File.ReadAllText("appsettings.json");
            var config = JObject.Parse(json);

            csEmail.Remetente = config["Email"]?["From"]?.ToString() ?? "";
            csEmail.Senha = config["Email"]?["Password"]?.ToString() ?? "";
            csEmail.Destinatario = config["Email"]?["To"]?.ToString() ?? "";
        }

        public bool ReadMail()
        {
            bool ok = false;

            if (string.IsNullOrEmpty((csEmail.Remetente)))
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "Exemplo_ConfigMail.txt");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Remetente: remetente@gmail.com ");
                    writer.WriteLine("Senha: 123456");
                    writer.WriteLine("Destinatario: destinatario@gmail.com");
                    writer.WriteLine("Servidor SMTP: smtp.gmail.com(exemplo gmail)");
                    writer.WriteLine("Porta: 587(exemplo gmail)");
                    writer.WriteLine("EnableSsl: true ou false");
                    writer.WriteLine("****Edite os dados desse arquivo se preferir e renomei-o****");
                }

                Console.WriteLine($"Arquivo criado em: {filePath}");
            }


            string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Console.WriteLine("Verifique o modelo criado para poder fazer a configuração de acesso");
            Console.WriteLine("Digite o nome do arquivo de configuração localizado na área de trabalho:");
            string nomeArquivo = Console.ReadLine();
            string caminho = Path.Combine(Desktop, nomeArquivo);

            if (File.Exists(caminho))
            {
                try
                {
                    var Campos = File.ReadAllLines(caminho);
                    int intPorta = 0;
                    bool booEnableSsl;

                    foreach (var Campo in Campos)
                    {
                        if (Campo.StartsWith("Remetente:"))
                        {
                            csEmail.Remetente = Campo.Replace("Remetente:", "").Trim();
                        }
                        else if (Campo.StartsWith("Senha:"))
                        {
                            csEmail.Senha = Campo.Replace("Senha:", "").Trim();
                        }
                        else if (Campo.StartsWith("Destinatario:"))
                        {
                            csEmail.Destinatario = Campo.Replace("Destinatario:", "").Trim();
                        }
                        else if (Campo.StartsWith("Servidor SMTP:"))
                        {
                            csEmail.ServidorSMTP = Campo.Replace("Servidor SMTP:", "").Trim();
                        }
                        else if (Campo.StartsWith("Porta:"))
                        {
                            int.TryParse(Campo.Replace("Porta:", "").Trim(), out intPorta);
                            csEmail.Porta = intPorta;
                        }
                        else if (Campo.StartsWith("EnableSsl:"))
                        {
                            bool.TryParse(Campo.Replace("EnableSsl:", "").Trim(), out booEnableSsl);
                            csEmail.EnableSsl = booEnableSsl;
                        }
                    }

                    mailMessage = new MailMessage(csEmail.Remetente, csEmail.Destinatario);
                    ok = true;
                }
                catch (Exception)
                {
                    ok = false;
                }
                
            }
            else
            {
                Console.WriteLine("Arquivo não encontrado.");
            }

            return ok;
        }

        public void SendMailVenda(string strBody)
        {
            mailMessage.Subject = "Venda";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<p> " + strBody + "</p>";
            mailMessage.SubjectEncoding = Encoding.GetEncoding("UTF-8");
            mailMessage.BodyEncoding = Encoding.GetEncoding("UTF-8");

            SendMail();
        }

        public void SendMailCompra(string strBody)
        {
            mailMessage.Subject = "Compra";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<p> " + strBody + "</p>";
            mailMessage.SubjectEncoding = Encoding.GetEncoding("UTF-8");
            mailMessage.BodyEncoding = Encoding.GetEncoding("UTF-8");

            SendMail();
        }

        private void SendMail()
        {
            SmtpClient smtpClient = new SmtpClient(csEmail.ServidorSMTP, csEmail.Porta);

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(csEmail.Remetente, csEmail.Senha);

            smtpClient.EnableSsl = csEmail.EnableSsl;

            smtpClient.Send(mailMessage);
        }
    }
}
