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
    public class JogadoresController : Controller
    {
        private TorneioEntities db = new TorneioEntities();
        private JogadoresRepository repository = new JogadoresRepository();
        // GET: Jogadores
        [Authorize(Roles = "Times")]
        public ActionResult Index()
        {
            try
            {
                UsuariosController ousuario = new UsuariosController();
                int idUsuario = 0;
                int idTime = 0;
                if (User.Identity.IsAuthenticated)
                {
                    idUsuario = ousuario.getUsuario(User.Identity.Name).ID;
                    TimesController oTimeRepository = new TimesController();

                    idTime = oTimeRepository.getTime(idUsuario).ID;
                    //idTime = (from p in db.usuarios_times where p.IDUsuario == idUsuario select p.IDTime).FirstOrDefault();
                }

                var jogadores = db.Jogadores.Include(j => j.Times).Where(j => j.Times.ID == idTime);
                return View(jogadores.ToList());
            }catch(Exception e)
            {
                List<Jogadores> ojogadores = new List<Jogadores>();
                return View(ojogadores);
            }
           
        }

        // GET: Jogadores/Details/5
        [Authorize(Roles = "Times")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogadores jogadores = db.Jogadores.Find(id);
            if (jogadores == null)
            {
                return HttpNotFound();
            }
            return View(jogadores);
        }

        // GET: Jogadores/Create
        [Authorize(Roles = "Times")]
        public ActionResult Create()
        {
            ViewBag.IDTime = new SelectList(db.Times, "ID", "Nome");
            return View();
        }

        // POST: Jogadores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Times")]
        public ActionResult Create([Bind(Include = "ID,Nome,Idade,Nacionalidade,DataNascimento,IDTime")] Jogadores jogadores)
        {
            if (ModelState.IsValid)
            {
                db.Jogadores.Add(jogadores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDTime = new SelectList(db.Times, "ID", "Nome", jogadores.IDTime);
            return View(jogadores);
        }

        // GET: Jogadores/Edit/5
        [Authorize(Roles = "Times")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogadores jogadores = db.Jogadores.Find(id);
            if (jogadores == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDTime = new SelectList(db.Times, "ID", "Nome", jogadores.IDTime);
            return View(jogadores);
        }

        // POST: Jogadores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Times")]
        public ActionResult Edit([Bind(Include = "ID,Nome,Idade,Nacionalidade,DataNascimento,IDTime")] Jogadores jogadores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jogadores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDTime = new SelectList(db.Times, "ID", "Nome", jogadores.IDTime);
            return View(jogadores);
        }

        // GET: Jogadores/Delete/5
        [Authorize(Roles = "Times")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogadores jogadores = db.Jogadores.Find(id);
            if (jogadores == null)
            {
                return HttpNotFound();
            }
            return View(jogadores);
        }

        // POST: Jogadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Times")]
        public ActionResult DeleteConfirmed(int id)
        {
            Jogadores jogadores = db.Jogadores.Find(id);
            db.Jogadores.Remove(jogadores);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Jogadores> selecionaJogadoresTime(int idTime)
        {
            return this.repository.selecionaJogadoresTime(idTime);
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
