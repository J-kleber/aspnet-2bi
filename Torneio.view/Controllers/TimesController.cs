using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Torneio.model;
using Torneio.model.Repositories;
using System.Security.Cryptography;

namespace Torneio.view.Controllers
{
    public class TimesController : Controller
    {
        private TorneioEntities db = new TorneioEntities();
        private TimeRepository repository = new TimeRepository();

        // GET: Times
        [Authorize(Roles = "Organizador")]
        public ActionResult Index()
        {
            UsuariosController ousuario = new UsuariosController();
            int idUsuario = 0;
            if (User.Identity.IsAuthenticated)
            {
                idUsuario = ousuario.getUsuario(User.Identity.Name).ID;
            }
            //List<usuarios_times> idsTimes = (from p in db.usuarios_times where p.IDUsuario == idUsuario select p).ToList();
            //List<Times> listaTimes = new List<Times>();
            /*foreach (var id in idsTimes)
            {
                listaTimes.Add((from p in db.Times where p.ID == id.IDTime select p).FirstOrDefault());
            }*/

            List<Times> listaTimes = new List<Times>();
            listaTimes = this.selecionaTodos(idUsuario);

            return View(listaTimes);
            //return View(db.Times.ToList());
        }

        public ActionResult Jogadores(int id)
        {
            JogadoresController oJogadoresController = new JogadoresController();
            List<Jogadores> jogadores = oJogadoresController.selecionaJogadoresTime(id);
            return View(jogadores);
        }

        // GET: Times/Details/5
        [Authorize(Roles = "Organizador")]
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
        [Authorize(Roles = "Organizador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Times/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public ActionResult Create([Bind(Include = "ID,Nome,Sigla")] Times times, [Bind(Include = "IdUsuario")] usuarios_times usuarioTime, HttpPostedFileBase Emblema)
        {
            if (ModelState.IsValid)
            {
                if (Emblema != null && Emblema.ContentLength > 0)
                {
                    string data = DateTime.Now.ToString().Replace('/', '-').Replace(':', '-').Replace(' ', '-');
                    string path = Server.MapPath("~/Imagens");
                    //Emblema.SaveAs(path);
                    Emblema.SaveAs(Path.Combine(path, data + Emblema.FileName));
                    times.Emblema = "Imagens/" + data + Emblema.FileName;
                }

                db.Times.Add(times);
                usuarioTime.IDTime = times.ID;
                db.usuarios_times.Add(usuarioTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(times);
        }

        // GET: Times/Edit/5
        [Authorize(Roles = "Organizador")]
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
        [Authorize(Roles = "Organizador")]
        public ActionResult Edit([Bind(Include = "ID,Nome,Emblema,Sigla")] Times times, HttpPostedFileBase Emblema)
        {
            if (ModelState.IsValid)
            {
                if (Emblema != null && Emblema.ContentLength > 0)
                {
                    string data = DateTime.Now.ToString().Replace('/', '-').Replace(':', '-').Replace(' ', '-');
                    string path = Server.MapPath("~/Imagens");
                    //Emblema.SaveAs(path);
                    Emblema.SaveAs(Path.Combine(path, data + Emblema.FileName));
                    times.Emblema = "Imagens/" + data + Emblema.FileName;
                }

                db.Entry(times).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(times);
        }

        // GET: Times/Delete/5
        [Authorize(Roles = "Organizador")]
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
        [Authorize(Roles = "Organizador")]
        public ActionResult DeleteConfirmed(int id)
        {
            Times times = db.Times.Find(id);
            PartidasController oPartidaController = new PartidasController();
            oPartidaController.deletaPartidasTime(id);
            db.Times.Remove(times);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Times> selecionaTodos(int idUsuario)
        {
            return this.repository.selecionaTodos(idUsuario);
        }

        public Times getTime(int idUsuario)
        {
            return this.repository.getTime(idUsuario);
        }

        [Authorize(Roles = "Organizador")]
        public ActionResult CreateUsuario(int id)
        {
            UsuariosController oUsuariosController = new UsuariosController();
            return oUsuariosController.Create();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public ActionResult CreateUsuario([Bind(Include = "ID,Email,Senha,Nome,Sobrenome")] Usuarios usuarios, int id)
        {
            usuarios.Tipo = "Times";
            usuarios.Ativo = "S";
            UsuariosController oUsuariosController = new UsuariosController();
            oUsuariosController.Create(usuarios);
            usuarios_times oUsuariosTimes = new usuarios_times();
            oUsuariosTimes.IDTime = id;
            oUsuariosTimes.IDUsuario = usuarios.ID;
            oUsuariosController.InsereUsuarioTime(oUsuariosTimes);
            return RedirectToAction("Index");
        }

        public ActionResult Usuarios(int id)
        {
            UsuariosController oUsuariosController = new UsuariosController();
            return View(oUsuariosController.usuariosTime(id));
        }

        public ActionResult EditUsuario(int? id)
        {
            UsuariosController oUsuariosController = new UsuariosController();
            return oUsuariosController.Edit(id);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsuario([Bind(Include = "ID,Email,Senha,Tipo,Ativo,Nome,Sobrenome")] Usuarios usuarios)
        {
            UsuariosController oUsuariosController = new UsuariosController();
            return oUsuariosController.Edit(usuarios);
        }

        public ActionResult DeleteUsuario(int? id)
        {
            UsuariosController oUsuariosController = new UsuariosController();
            return oUsuariosController.Delete(id);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("DeleteUsuario")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUsuario(int id)
        {
            UsuariosController oUsuariosController = new UsuariosController();
            return oUsuariosController.DeleteConfirmed(id);
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
