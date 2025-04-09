using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using Newtonsoft.Json.Linq;
using StockQuoteAlert.Controller;

namespace StockQuoteAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            MailController csMail = new MailController();
            APIController csAPI = new APIController();
            string strAtivo = string.Empty;
            decimal decVenda = 0;
            decimal decCompra = 0;

            if (csMail.ReadMail())
            {
                Console.WriteLine("Digite os parametros para a pesquisa! Siga o exemplo abaixo:");
                Console.WriteLine("<Ativo>/<Referencial Venda>/<Referencial Compra>");
                Console.WriteLine("PETR4/33.40/32.01");
                Console.WriteLine("Apos digitar aperte Enter para prosseguir...");
                args = Convert.ToString(Console.ReadLine()).Split('/');

                if (args.Length != 3)
                {
                    Console.WriteLine("Digite parametros validos para o monitoramento!");
                    return;
                }
                else if(!decimal.TryParse(args[1].ToString().Trim(), CultureInfo.InvariantCulture, out decVenda) || !decimal.TryParse(args[2].ToString().Trim(), CultureInfo.InvariantCulture, out decCompra))
                {
                    Console.WriteLine("Digite referenciais validos para o monitoramento!");
                    return;
                }

                strAtivo = args[0].ToString().Trim();

                var strContent = csAPI.GetStock(strAtivo);
                if (strContent.StatusCode == HttpStatusCode.OK)
                {
                    JObject obj = JObject.Parse(strContent.Content);

                    if (obj.ContainsKey("code"))
                    {
                        if (obj["code"].ToString() != "200")
                        {
                            Console.WriteLine("Ativo nao encontrado! Realize novamente a pesquisa.");
                            return;
                        }
                    }

                    string strCurrency = obj["meta"]["currency"].ToString();
                    foreach (var item in obj["values"])
                    {
                        decimal decClose = Convert.ToDecimal(item["close"].ToString().Trim(), CultureInfo.InvariantCulture);
                        string strHorario = item["datetime"].ToString();
                        if (decClose > decVenda)
                        {
                            csMail.SendMail("Venda", strAtivo, strHorario, decClose, strCurrency, decVenda);
                            Console.WriteLine("Venda");
                        }
                        else if (decClose < decCompra)
                        {
                            csMail.SendMail("Compra", strAtivo, strHorario, decClose, strCurrency, decCompra);
                            Console.WriteLine("Compra");
                        }
                        else
                        {
                            Console.WriteLine(decClose + " " + strCurrency);
                        }

                        Thread.Sleep(15000);
                    }
                }
                else
                {
                    Console.WriteLine("Erro ao realizar pesquisa, tente novamente.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Erro ao tentar ler arquivo de Email! Tente novamente.");
            }
        }
    }
}