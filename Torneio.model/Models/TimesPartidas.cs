using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneio.model.Models
{
    public class TimesPartidas
    {
        public int Rodada { get; set; }
        public string TimeCasa { get; set; }
        public int? PlacarTime1 { get; set; }
        public int? PlacarTime2 { get; set; }
        public string TimeFora { get; set; }
        public DateTime? DataHora { get; set; }
        public int IDTorneio { get; set; }
        public string TorneioNome { get; set; }
    }
}
