using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using Torneio.model;
using Torneio.view.Models;

namespace Torneio.view.Controllers
{
    public class TabelaController : Controller
    {
        private TorneioEntities db = new TorneioEntities();
        // GET: Tabela
        public ActionResult Index()
        {
            /*List<List<Tabela>> tabela = new List<List<Tabela>>();
            tabela = db.Database.SqlQuery<List<Tabela>>(
                "select ID, Nome, sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas, sum(pontos) as pontos, sum(gols) as gols, sum(golsTomados) as golsTomados, sum(gols) - sum(golsTomados) as saldoGols from (select t1.ID, t1.Nome, sum(pontosCasa) as pontos, sum(golsCasa) as gols, sum(golsTomados) as golsTomados,sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas from (select t1.ID, sum(PlacarTime1) as golsCasa, sum(PlacarTime2) as golsTomados, (case when sum(PlacarTime1) > sum(PlacarTime2) then 3 when sum(PlacarTime1) = sum(PlacarTime2) then 1 else 0 end) as pontosCasa,(case when sum(PlacarTime1) > sum(PlacarTime2) then 1 else 0 end) as vitorias,(case when sum(PlacarTime1) = sum(PlacarTime2) then 1 else 0 end) as empates,(case when sum(PlacarTime1) < sum(PlacarTime2) then 1 else 0 end) as derrotas from Partidas inner join Times as t1 on t1.ID = Partidas.IDTime1 where Partidas.IDTorneio = 15 group by t1.ID, Rodada) as PartidasCasa inner join Times as t1 on t1.ID = PartidasCasa.ID group by t1.ID, t1.Nome union select t1.ID, t1.Nome, sum(pontosFora) as pontos, sum(golsFora) as gols, sum(golsTomados) as golsTomados, sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas from (select t1.ID, sum(PlacarTime2) as golsFora, sum(PlacarTime1) as golsTomados,(case when sum(PlacarTime2) > sum(PlacarTime1) then 3 when sum(PlacarTime2) = sum(PlacarTime1) then 1 else 0 end) as pontosFora,(case when sum(PlacarTime2) > sum(PlacarTime1) then 1 else 0 end) as vitorias,(case when sum(PlacarTime2) = sum(PlacarTime1) then 1 else 0 end) as empates,(case when sum(PlacarTime2) < sum(PlacarTime1) then 1 else 0 end) as derrotas from Partidas inner join Times as t1 on t1.ID = Partidas.IDTime2 where Partidas.IDTorneio = 15 group by t1.ID, Rodada) as PartidasFora inner join Times as t1 on t1.ID = PartidasFora.ID group by t1.ID, t1.Nome) as tabela group by ID, Nome order by pontos desc, saldoGols desc, gols desc").ToList();
            var a = tabela;*/
            using (var context = new SampleContext())
            {
                var tabela = context.Tabela.SqlQuery("select ID, Nome, sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas, sum(pontos) as pontos, sum(gols) as gols, sum(golsTomados) as golsTomados, sum(gols) - sum(golsTomados) as saldoGols from (select t1.ID, t1.Nome, sum(pontosCasa) as pontos, sum(golsCasa) as gols, sum(golsTomados) as golsTomados,sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas from (select t1.ID, sum(PlacarTime1) as golsCasa, sum(PlacarTime2) as golsTomados, (case when sum(PlacarTime1) > sum(PlacarTime2) then 3 when sum(PlacarTime1) = sum(PlacarTime2) then 1 else 0 end) as pontosCasa,(case when sum(PlacarTime1) > sum(PlacarTime2) then 1 else 0 end) as vitorias,(case when sum(PlacarTime1) = sum(PlacarTime2) then 1 else 0 end) as empates,(case when sum(PlacarTime1) < sum(PlacarTime2) then 1 else 0 end) as derrotas from Partidas inner join Times as t1 on t1.ID = Partidas.IDTime1 where Partidas.IDTorneio = 15 group by t1.ID, Rodada) as PartidasCasa inner join Times as t1 on t1.ID = PartidasCasa.ID group by t1.ID, t1.Nome union select t1.ID, t1.Nome, sum(pontosFora) as pontos, sum(golsFora) as gols, sum(golsTomados) as golsTomados, sum(vitorias) as vitorias, sum(empates) as empates, sum(derrotas) as derrotas from (select t1.ID, sum(PlacarTime2) as golsFora, sum(PlacarTime1) as golsTomados,(case when sum(PlacarTime2) > sum(PlacarTime1) then 3 when sum(PlacarTime2) = sum(PlacarTime1) then 1 else 0 end) as pontosFora,(case when sum(PlacarTime2) > sum(PlacarTime1) then 1 else 0 end) as vitorias,(case when sum(PlacarTime2) = sum(PlacarTime1) then 1 else 0 end) as empates,(case when sum(PlacarTime2) < sum(PlacarTime1) then 1 else 0 end) as derrotas from Partidas inner join Times as t1 on t1.ID = Partidas.IDTime2 where Partidas.IDTorneio = 15 group by t1.ID, Rodada) as PartidasFora inner join Times as t1 on t1.ID = PartidasFora.ID group by t1.ID, t1.Nome) as tabela group by ID, Nome order by pontos desc, saldoGols desc, gols desc").ToList();
                var a = tabela;
            }
            return View();
        }
    }
}