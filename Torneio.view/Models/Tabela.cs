using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Torneio.view.Models
{
    public class Tabela
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int vitorias { get; set; }
        public int empates { get; set; }
        public int derrotas { get; set; }
        public int pontos { get; set; }
        public int gols { get; set; }
        public int golsTomados { get; set; }
        public int saldoGols { get; set; }
    }
}