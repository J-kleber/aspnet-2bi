using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torneio.model.Models;
using Torneio.model.Repositories;
using System.Data;
using System.Data.Entity;


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

        public List<Partidas> partidasTorneioComID(int? idTorneio)
        {
            return db.Partidas.Include(p => p.Times).Include(p => p.Times1).Include(p => p.Torneios).Where(p => p.IDTorneio == idTorneio).ToList();
        }

        public List<Partidas> partidasTorneioSemID()
        {
            return db.Partidas.Include(p => p.Times).Include(p => p.Times1).Include(p => p.Torneios).ToList();
        }

        public List<Partidas> geraPartidas(int idTorneio, IEnumerable<int> idTimes)
        {
            List<int> times = new List<int>();
            int index = 0;
            foreach (var id in idTimes)
            {
                times.Insert(index, id);
                index++;
            }

            if (times.Count % 2 == 1)
            {
                times.Insert(0, 0);
            }
            int t = times.Count;
            int m = times.Count / 2;

            List<Partidas> Partidas = new List<Partidas>();

            for (int i = 0; i < t - 1; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Partidas p = new Partidas();
                    p.IDTorneio = idTorneio;
                    p.Rodada = i + 1;

                    //Clube está de fora nessa rodada?
                    if (times[j] == 0)
                    {
                        continue;
                    }

                    //Teste para ajustar o mando de campo
                    if (j % 2 == 1 || i % 2 == 1 && j == 0)
                    {
                        p.IDTime1 = times[t - j - 1];
                        p.IDTime2 = times[j];
                    }
                    else
                    {
                        p.IDTime1 = times[j];
                        p.IDTime2 = times[t - j - 1];
                    }
                    int ads = p.IDTime1;
                    int dasd = p.IDTime2;
                    Partidas.Add(p);
                }
                //Gira os clubes no sentido horário, mantendo o primeiro no lugar
                int asa = times.Count - 1;
                int k = times[asa];
                times.Remove(times[asa]);
                times.Insert(1, k);
            }
            return Partidas;
        }

        public void deletaPartidasTorneio(int idTorneio)
        {
            List<Partidas> ListaPartidas = (from p in db.Partidas where p.IDTorneio == idTorneio select p).ToList();
            foreach (var par in ListaPartidas)
            {
                Partidas partidas = db.Partidas.Find(par.ID);
                db.Partidas.Remove(partidas);
                db.SaveChanges();
            }
        }

        public void deletaPartidasTime(int idTime)
        {
            List<Partidas> ListaPartidas = (from p in db.Partidas join b in db.Times on p.IDTime1 equals b.ID join c in db.Times on p.IDTime2 equals c.ID where (p.IDTime1 == idTime || p.IDTime2 == idTime) select p).ToList();
            foreach (var par in ListaPartidas)
            {
                Partidas partidas = db.Partidas.Find(par.ID);
                db.Partidas.Remove(partidas);
                db.SaveChanges();
            }
        }
    }
}
