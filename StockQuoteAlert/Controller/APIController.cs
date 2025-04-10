using System;
using System.Globalization;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace StockQuoteAlert.Controller
{
    class APIController
    {
        public void ProcessaAtivo(MailController csMail, string strAtivo, decimal decVenda, decimal decCompra)
        {
            var strContent = GetStock(strAtivo);
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
                        Console.WriteLine("Valor no horario: " + strHorario + " R$" + decClose.ToString("N2") + " " + strCurrency);
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

        public RestResponse GetStock(string strStock)
        {
            if (!File.Exists("appsettings.json"))
            {
                Console.WriteLine("Erro ao encontrar o arquivo de configuração(appsettings.json) ");
                return null;
            }

            var json = File.ReadAllText("appsettings.json");
            var config = JObject.Parse(json);
            string strToken = config["Token"].ToString();

            var client = new RestClient(@"https://api.twelvedata.com");
            var request = new RestRequest("time_series", Method.Get);

            request.AddParameter("apikey", strToken);
            request.AddParameter("interval", "5min");
            request.AddParameter("symbol", strStock);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine("Resposta:");
            }
            else
            {
                Console.WriteLine($"Erro: {response.StatusCode} - {response.ErrorMessage}");
            }

            return response;
        }
    }
}
