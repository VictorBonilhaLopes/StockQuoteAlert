using System;
using System.Globalization;
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

                csAPI.ProcessaAtivo(csMail, strAtivo, decVenda, decCompra);
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