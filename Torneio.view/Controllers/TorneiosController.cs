using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Torneio.model;

namespace Torneio.view.Controllers
{
    public class TorneiosController : Controller
    {
        private TorneioEntities db = new TorneioEntities();

        // GET: Torneios
        public ActionResult Index()
        {
            List<usuarios_torneios> idsTorneios = (from p in db.usuarios_torneios where p.IDUsuario == 1 select p).ToList();
            List<Torneios> listaTorneios = new List<Torneios>();
            foreach(var id in idsTorneios)
            {
                listaTorneios.Add((from p in db.Torneios where p.ID == id.IDTorneio select p).FirstOrDefault());
            }



            return View(listaTorneios);
        }

        // GET: Torneios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Torneios torneios = db.Torneios.Find(id);
            if (torneios == null)
            {
                return HttpNotFound();
            }
            return View(torneios);
        }

        // GET: Torneios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Torneios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Premiacao,Ano,Realizador")] Torneios torneios, [Bind(Include = "IdUsuario")] usuarios_torneios usuarioTorneio, IEnumerable<int> IDTime)
        {
            if (ModelState.IsValid)
            {
                var item = db.Times.ToList();
                db.Torneios.Add(torneios);
                usuarioTorneio.IDTorneio = torneios.ID;
                db.usuarios_torneios.Add(usuarioTorneio);
              
                foreach(var id in IDTime)
                {
                    Torneios_Times torneioTimes = new Torneios_Times();
                    torneioTimes.IDTorneio = torneios.ID;
                    torneioTimes.IDTime = id;
                    db.Torneios_Times.Add(torneioTimes);
                }

                List<Partidas> partidas = geraPartidas(torneios.ID, IDTime);
                foreach(Partidas partida in partidas)
                {
                    db.Partidas.Add(partida);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(torneios);
        }

        // GET: Torneios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Torneios torneios = db.Torneios.Find(id);
            if (torneios == null)
            {
                return HttpNotFound();
            }
            return View(torneios);
        }

        // POST: Torneios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Premiacao,Ano,Realizador")] Torneios torneios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(torneios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(torneios);
        }

        // GET: Torneios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Torneios torneios = db.Torneios.Find(id);
            if (torneios == null)
            {
                return HttpNotFound();
            }
            return View(torneios);
        }

        // POST: Torneios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Torneios torneios = db.Torneios.Find(id);
            db.Torneios.Remove(torneios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private List<Partidas> geraPartidas(int idTorneio, IEnumerable<int> idTimes)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
