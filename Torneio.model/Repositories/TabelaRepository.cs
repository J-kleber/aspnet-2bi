using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torneio.model.Models;

namespace Torneio.model.Repositories
{
    public class TabelaRepository
    {
        public List<Tabela> montaTabela(int idTorneio)
        {
            List<Tabela> Tabela = new List<Tabela>();
            using (var tab = new TorneioEntities())
            {
                var ListTabela = tab.Database
                                .SqlQuery<Tabela>("select (row_number() over (order by sum(pontos) desc, (sum(gols) - sum(golsTomados)) desc, sum(gols) desc)) as posicao, ID, Nome, sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas, sum(pontos) as pontos, case when sum(gols) is null then '0' else sum(gols) end as gols, case when sum(golsTomados) is null then '0' else sum(golsTomados) END as golsTomados, case when sum(gols) - sum(golsTomados) is null then '0' else  sum(gols) - sum(golsTomados) end as saldoGols from (select t1.ID, t1.Nome, sum(pontosCasa) as pontos, sum(golsCasa) as gols, sum(golsTomados) as golsTomados,sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas from (select t1.ID, sum(PlacarTime1) as golsCasa, sum(PlacarTime2) as golsTomados, (case when sum(PlacarTime1) > sum(PlacarTime2) then 3 when sum(PlacarTime1) = sum(PlacarTime2) then 1 else 0 end) as pontosCasa,(case when sum(PlacarTime1) > sum(PlacarTime2) then 1 else 0 end) as vitorias,(case when sum(PlacarTime1) = sum(PlacarTime2) then 1 else 0 end) as empates,(case when sum(PlacarTime1) < sum(PlacarTime2) then 1 else 0 end) as derrotas from Partidas inner join Times as t1 on t1.ID = Partidas.IDTime1 where Partidas.IDTorneio = @id group by t1.ID, Rodada) as PartidasCasa inner join Times as t1 on t1.ID = PartidasCasa.ID group by t1.ID, t1.Nome union select t1.ID, t1.Nome, sum(pontosFora) as pontos, sum(golsFora) as gols, sum(golsTomados) as golsTomados, sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas from (select t1.ID, sum(PlacarTime2) as golsFora, sum(PlacarTime1) as golsTomados,(case when sum(PlacarTime2) > sum(PlacarTime1) then 3 when sum(PlacarTime2) = sum(PlacarTime1) then 1 else 0 end) as pontosFora,(case when sum(PlacarTime2) > sum(PlacarTime1) then 1 else 0 end) as vitorias,(case when sum(PlacarTime2) = sum(PlacarTime1) then 1 else 0 end) as empates,(case when sum(PlacarTime2) < sum(PlacarTime1) then 1 else 0 end) as derrotas from Partidas inner join Times as t1 on t1.ID = Partidas.IDTime2 where Partidas.IDTorneio = @id group by t1.ID, Rodada) as PartidasFora inner join Times as t1 on t1.ID = PartidasFora.ID group by t1.ID, t1.Nome) as tabela group by ID, Nome order by pontos desc, saldoGols desc, gols desc", new SqlParameter("@id", idTorneio))
                                .ToList();

                Tabela = ListTabela;
            }
            return Tabela;
        }
    }
}
