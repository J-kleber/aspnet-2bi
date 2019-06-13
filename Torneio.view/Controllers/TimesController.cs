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
    public class TimesController : Controller
    {
        private TorneioEntities db = new TorneioEntities();

        // GET: Times
        public ActionResult Index()
        {
            List<usuarios_times> idsTimes = (from p in db.usuarios_times where p.IDUsuario == 1 select p).ToList();
            List<Times> listaTimes = new List<Times>();
            foreach (var id in idsTimes)
            {
                listaTimes.Add((from p in db.Times where p.ID == id.IDTime select p).FirstOrDefault());
            }



            return View(listaTimes);
            //return View(db.Times.ToList());
        }

        // GET: Times/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Times times = db.Times.Find(id);
            if (times == null)
            {
                return HttpNotFound();
            }
            return View(times);
        }

        // GET: Times/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Times/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Emblema,Sigla")] Times times, [Bind(Include = "IdUsuario")] usuarios_times usuarioTime)
        {
            if (ModelState.IsValid)
            {
                
                db.Times.Add(times);
                usuarioTime.IDTime = times.ID;
                db.usuarios_times.Add(usuarioTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(times);
        }

        // GET: Times/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Times times = db.Times.Find(id);
            if (times == null)
            {
                return HttpNotFound();
            }
            return View(times);
        }

        // POST: Times/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Emblema,Sigla")] Times times)
        {
            if (ModelState.IsValid)
            {
                db.Entry(times).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(times);
        }

        // GET: Times/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Times times = db.Times.Find(id);
            if (times == null)
            {
                return HttpNotFound();
            }
            return View(times);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Times times = db.Times.Find(id);
            db.Times.Remove(times);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public Times getTime(int idUsuario)
        {
            int idTime = (from p in db.usuarios_times where p.IDUsuario == idUsuario select p.IDTime).FirstOrDefault();
            return (from p in db.Times where p.ID == idTime select p).FirstOrDefault();
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
