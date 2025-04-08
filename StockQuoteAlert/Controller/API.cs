using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace StockQuoteAlert.Controller
{
    class API
    {
        public RestResponse GetStock(string strStock)
        {
            var client = new RestClient(@"https://api.twelvedata.com");
            var request = new RestRequest("time_series", Method.Get);

            request.AddParameter("apikey", "TOKEN");   
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
