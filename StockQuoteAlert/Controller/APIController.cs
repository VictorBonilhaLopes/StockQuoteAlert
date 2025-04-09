using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace StockQuoteAlert.Controller
{
    class APIController
    {
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
