using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Torneio.model;
using Torneio.model.Repositories;

namespace Torneio.view.Controllers
{
    public class PartidasController : Controller
    {
        private TorneioEntities db = new TorneioEntities();
        private PartidaRepository repository = new PartidaRepository();
        // GET: Partidas
        //[Authorize(Roles = "Organizador")]
        public ActionResult Index(int? id)
        {
            if(id > 0)
            {
                var partidas = this.partidasTorneioComID(id);
                return View(partidas.ToList());
            }
            else
            {
                var partidas = this.partidasTorneioSemID();
                return View(partidas.ToList());
            }
          
            
        }

        // GET: Partidas/Details/5
        [Authorize(Roles = "Organizador")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partidas partidas = db.Partidas.Find(id);
            if (partidas == null)
            {
                return HttpNotFound();
            }
            return View(partidas);
        }

        // GET: Partidas/Create
        [Authorize(Roles = "Organizador")]
        public ActionResult Create()
        {
            ViewBag.IDTime1 = new SelectList(db.Times, "ID", "Nome");
            ViewBag.IDTime2 = new SelectList(db.Times, "ID", "Nome");
            ViewBag.IDTorneio = new SelectList(db.Torneios, "ID", "Nome");
            return View();
        }

        // POST: Partidas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public ActionResult Create([Bind(Include = "ID,IDTorneio,IDTime1,IDTime2,Rodada,PlacarTime1,PlacarTime2,DataHora")] Partidas partidas)
        {
            if (ModelState.IsValid)
            {
                db.Partidas.Add(partidas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDTime1 = new SelectList(db.Times, "ID", "Nome", partidas.IDTime1);
            ViewBag.IDTime2 = new SelectList(db.Times, "ID", "Nome", partidas.IDTime2);
            ViewBag.IDTorneio = new SelectList(db.Torneios, "ID", "Nome", partidas.IDTorneio);
            return View(partidas);
        }

        // GET: Partidas/Edit/5
        [Authorize(Roles = "Organizador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partidas partidas = db.Partidas.Find(id);
            if (partidas == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDTime1 = new SelectList(db.Times, "ID", "Nome", partidas.IDTime1);
            ViewBag.IDTime2 = new SelectList(db.Times, "ID", "Nome", partidas.IDTime2);
            ViewBag.IDTorneio = new SelectList(db.Torneios, "ID", "Nome", partidas.IDTorneio);
            return View(partidas);
        }

        // POST: Partidas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public ActionResult Edit([Bind(Include = "ID,IDTorneio,IDTime1,IDTime2,Rodada,PlacarTime1,PlacarTime2,DataHora")] Partidas partidas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partidas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index/"+partidas.IDTorneio);
            }
            ViewBag.IDTime1 = new SelectList(db.Times, "ID", "Nome", partidas.IDTime1);
            ViewBag.IDTime2 = new SelectList(db.Times, "ID", "Nome", partidas.IDTime2);
            ViewBag.IDTorneio = new SelectList(db.Torneios, "ID", "Nome", partidas.IDTorneio);
            return RedirectToAction("Index/" +partidas.IDTorneio);
            //return View(partidas);
        }

        // GET: Partidas/Delete/5
        [Authorize(Roles = "Organizador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partidas partidas = db.Partidas.Find(id);
            if (partidas == null)
            {
                return HttpNotFound();
            }
            return View(partidas);
        }

        // POST: Partidas/Delete/5
        [Authorize(Roles = "Organizador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Partidas partidas = db.Partidas.Find(id);
            db.Partidas.Remove(partidas);
            db.SaveChanges();
            return RedirectToAction("Index/"+partidas.IDTorneio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<Partidas> partidasTorneioComID(int? idTorneio)
        {
            return this.repository.partidasTorneioComID(idTorneio);
        }

        public List<Partidas> partidasTorneioSemID()
        {
            return this.repository.partidasTorneioSemID();
        }

        public List<Partidas> geraPartidas(int idTorneio, IEnumerable<int> idTimes)
        {
            return this.repository.geraPartidas(idTorneio, idTimes);
        }

        public void deletaPartidasTorneio(int idTorneio)
        {
            this.repository.deletaPartidasTorneio(idTorneio);
        }

        public void deletaPartidasTime(int idTime)
        {
            this.repository.deletaPartidasTime(idTime);
        }
    }
}
