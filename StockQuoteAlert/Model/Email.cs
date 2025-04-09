using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockQuoteAlert.Model
{
    internal class Email
    {
        public string Remetente { get; set; }
        public string Senha { get; set; }
        public string Destinatario { get; set; }
        public string ServidorSMTP { get; set; }
        public int Porta { get; set; }
        public bool EnableSsl { get; set; }
    }
}
