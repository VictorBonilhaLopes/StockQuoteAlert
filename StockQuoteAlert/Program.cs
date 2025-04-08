using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using Newtonsoft.Json.Linq;
using StockQuoteAlert.Controller;
using static StockQuoteAlert.Controller.API;
using static StockQuoteAlert.Controller.Mail;

namespace StockQuoteAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            Mail csMail = new Mail();
            API csAPI = new API();
            string strAtivo = string.Empty;
            decimal decVenda = 0;
            decimal decCompra = 0;

            Console.WriteLine("Digite os parametros para a pesquisa! Siga e exemplo abaixo:");
            Console.WriteLine("<Ativo> <Referencial Venda> <Referencial Compra>");
            args = Convert.ToString(Console.ReadLine()).Split(' ');

            strAtivo = args[0].ToString();
            decimal.TryParse(args[1].ToString(), CultureInfo.InvariantCulture, out decVenda);
            decimal.TryParse(args[2].ToString(), CultureInfo.InvariantCulture, out decCompra);

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

                foreach (var item in obj["values"])
                {
                    if (decimal.Parse(item["close"].ToString(), CultureInfo.InvariantCulture) > decVenda)
                    {
                        Console.WriteLine("no horario " + item["datetime"].ToString() + " Vende");
                    }
                    else if (decimal.Parse(item["close"].ToString(), CultureInfo.InvariantCulture) < decCompra)
                    {
                        Console.WriteLine("no horario " + item["datetime"].ToString() + " Compra");
                    }
                    else
                    {
                        Console.WriteLine(item["close"].ToString());
                    }
                }
            }


            //csMail.BuildMail("");
            //csMail.SendMail();
        }
    }
}