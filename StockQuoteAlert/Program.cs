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
        static MailController csMail = new MailController();
        static APIController csAPI = new APIController();
        static string strAtivo = string.Empty;
        static decimal decVenda = 0;
        static decimal decCompra = 0;

        static void Main(string[] args)
        {
            if (csMail.ReadMail())
            {
                if (args.Length == 1)
                {
                    string strDados = args[0].ToString();
                    string[] parametros = strDados.Split('/');

                    if (!ValidaParametros(parametros))
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Digite os parametros para a pesquisa! Siga o exemplo abaixo:");
                    Console.WriteLine("<Ativo>/<Referencial Venda>/<Referencial Compra>");
                    Console.WriteLine("PETR4/33.40/32.01");
                    Console.WriteLine("Apos digitar aperte Enter para prosseguir...");
                    args = Convert.ToString(Console.ReadLine()).Split('/');

                    if (!ValidaParametros(args))
                    {
                        return;
                    }
                }

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
                    foreach (var item in obj["values"].Reverse()) //para simular a requisicao em tempo real, foi invertido a ordem dos dados obtidos, indo assim do mais antigo para o mais novo
                    {
                        decimal decClose = Convert.ToDecimal(item["close"].ToString().Trim(), CultureInfo.InvariantCulture);
                        string strHorario = Convert.ToDateTime(item["datetime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
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
                            Console.WriteLine("Valor no horario: " + strHorario +" R$"+ decClose.ToString("N2") + " " + strCurrency);
                        }

                        Thread.Sleep(15000); //simulando requisições espaçadas, se fosse utilizado uma API que disponibilizasse os dados em tempo real, seria refeita a requisição de minuto em minuto
                    }
                }
                else
                {
                    Console.WriteLine("Erro ao realizar pesquisa, tente novamente.");
                    return;
                }
            }
        }

        private static bool ValidaParametros(string[] parametros)
        {
            bool valida = true;

            if (parametros.Length != 3)
            {
                Console.WriteLine("Digite parametros validos para o monitoramento!");
                valida = false;
            }
            else if (!decimal.TryParse(parametros[1].ToString().Trim(), CultureInfo.InvariantCulture, out decVenda) || !decimal.TryParse(parametros[2].ToString().Trim(), CultureInfo.InvariantCulture, out decCompra))
            {
                Console.WriteLine("Digite referenciais validos para o monitoramento!");
                valida = false;
            }

            strAtivo = parametros[0].ToString().Trim();

            return valida;
        }
    }
}