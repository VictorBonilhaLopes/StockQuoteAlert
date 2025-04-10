using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Intrinsics.X86;
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

        public bool ReadMail()
        {
            bool ok = false;

            string strDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string strCaminhoArqEx = Path.Combine(strDesktop, "Exemplo_ConfigMail.txt");
            using (StreamWriter writer = new StreamWriter(strCaminhoArqEx))
            {
                writer.WriteLine("Remetente: remetente@gmail.com ");
                writer.WriteLine("Senha: 123456");
                writer.WriteLine("Destinatario: destinatario@gmail.com");
                writer.WriteLine("Servidor SMTP: smtp.gmail.com(exemplo gmail)");
                writer.WriteLine("Porta: 587(exemplo gmail)");
                writer.WriteLine("EnableSsl: true ou false");
                writer.WriteLine("****Edite os dados desse arquivo se preferir e renomei-o****");
            }

            Console.WriteLine($"Arquivo criado em: {strCaminhoArqEx}");
            Process.Start("notepad.exe", strCaminhoArqEx);

            Console.WriteLine("Verifique o modelo criado para poder fazer a configuração de acesso");
            Console.WriteLine("Digite o nome do arquivo de configuração e sua extencao(.txt), localizado na área de trabalho e aperte Enter:");
            string strNomeArquivo = Console.ReadLine();
            strNomeArquivo = !strNomeArquivo.Contains(".txt") ? strNomeArquivo += ".txt" : strNomeArquivo;
            string strCaminho = Path.Combine(strDesktop, strNomeArquivo);

            if (File.Exists(strCaminho))
            {
                try
                {
                    var Campos = File.ReadAllLines(strCaminho);
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

                    if (ValidaCampos())
                    {
                        mailMessage = new MailMessage(csEmail.Remetente, csEmail.Destinatario);
                        ok = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao ler o arquivo: " + ex.Message);
                    ok = false;
                }

            }
            else
            {
                Console.WriteLine("Arquivo não encontrado.");
            }

            return ok;
        }

        private bool ValidaCampos()
        {
            return !string.IsNullOrEmpty(csEmail.Remetente)
                && !string.IsNullOrEmpty(csEmail.Senha)
                && !string.IsNullOrEmpty(csEmail.Destinatario)
                && !string.IsNullOrEmpty(csEmail.ServidorSMTP)
                && csEmail.Porta > 0;
        }

        public void SendMail(string strOperacao, string strAtivo, string strHorario, decimal decPrecoClose, string strCurrency, decimal decReferencia)
        {
            string strColor = strOperacao != "Compra" ? "#dc3545" : "#28a745";

            mailMessage.Subject = strOperacao;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>" +
                                    "<div style='max-width: 600px; margin: 0 auto; background-color: #fff; border-radius: 10px; padding: 20px; color: #333;'>" +
                                        "<h2 style='color: " + strColor + ";'>Alerta de " + strOperacao + " - Ativo Financeiro</h2>" +
                                        "<p>Foi identificado um momento oportuno para <strong>" + strOperacao.ToLower() + "</strong> do ativo <strong style='color: " + strColor + ";'>" + strAtivo + "</strong>.</p>" +
                                        "<p>Detalhes da recomendação:</p>" +
                                            "<ul>" +
                                                "<li><strong>Horário:</strong> " + strHorario + "</li>" +
                                                "<li><strong>Valor Atual:</strong> R$ " + decPrecoClose.ToString("N2") + strCurrency +"</li>" +
                                                "<li><strong>Valor de " + strOperacao + " Alvo:</strong> R$ " + decReferencia.ToString("N2") + strCurrency +"</li>" +
                                            "</ul>" +
                                        "<p>Considere realizar a " + strOperacao.ToLower() + " de acordo com sua estratégia de investimentos.</p>" +
                                    "</div>" +
                                "</div>";

            mailMessage.SubjectEncoding = Encoding.GetEncoding("UTF-8");
            mailMessage.BodyEncoding = Encoding.GetEncoding("UTF-8");

            SmtpClient smtpClient = new SmtpClient(csEmail.ServidorSMTP, csEmail.Porta);

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(csEmail.Remetente, csEmail.Senha);

            smtpClient.EnableSsl = csEmail.EnableSsl;

            smtpClient.Send(mailMessage);
        }
    }
}
