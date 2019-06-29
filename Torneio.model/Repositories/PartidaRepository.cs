using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torneio.model.Models;

namespace Torneio.model.Repositories
{
    public class PartidaRepository
    {
        private TorneioEntities db = new TorneioEntities();

        public List<TimesPartidas> minhasPartidas(int idTime)
        {
            List<TimesPartidas> Tabela = new List<TimesPartidas>();
            using (var tab = new TorneioEntities())
            {
                var ListTabela = tab.Database
                                .SqlQuery<TimesPartidas>("select Rodada, t1.Nome as TimeCasa, PlacarTime1, PlacarTime2, t2.Nome as TimeFora, Partidas.DataHora, Torneios.ID as IDTorneio, Torneios.Nome as TorneioNome from Partidas inner join Times as t1 on t1.ID = Partidas.IDTime1 inner join Times as t2 on t2.ID = Partidas.IDTime2 inner join Torneios on Torneios.ID = Partidas.IDTorneio where(t1.ID = @id or t2.ID = @id)", new SqlParameter("@id", idTime)).ToList();

                Tabela = ListTabela;
            }
            return Tabela;
        }
    }
}
