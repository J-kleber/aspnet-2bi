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
    public class CadastroController : Controller
    {
        private TorneioEntities db = new TorneioEntities();
        private UsuarioRepository repository = new UsuarioRepository();

        // GET: Usuarios/Create
        public ActionResult Index()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ID,Email,Senha,Nome,Sobrenome")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                int id = 0;
                try
                {
                    id = this.getUsuario(usuarios.Email).ID;
                }
                catch (Exception e)
                {
                    id = 0;
                }

                if (id == 0)
                {
                    usuarios.Ativo = "S";
                    usuarios.Tipo = "Organizador";
                    db.Usuarios.Add(usuarios);
                    db.SaveChanges();
                }
                return RedirectToAction("../Conta/Login");
            }

            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        public Usuarios getUsuario(string email)
        {
            return (from p in db.Usuarios where p.Email == email select p).FirstOrDefault();
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
