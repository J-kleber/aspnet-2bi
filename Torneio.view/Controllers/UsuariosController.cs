﻿using System;
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
    public class UsuariosController : Controller
    {
        private TorneioEntities db = new TorneioEntities();
        private UsuarioRepository repository = new UsuarioRepository();

        // GET: Usuarios
        [Authorize(Roles = "Organizador")]
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        [Authorize(Roles = "Organizador")]
        public ActionResult Details(int? id)
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

        // GET: Usuarios/Create
        [Authorize(Roles = "Organizador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public ActionResult Create([Bind(Include = "ID,Email,Senha,Nome,Sobrenome")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                int id = 0;
                try
                {
                    id = this.getUsuario(usuarios.Email).ID;
                }catch(Exception e)
                {
                    id = 0;
                }
               
                if (id == 0)
                {
                    usuarios.Tipo = "Organizador";
                    usuarios.Ativo = "S";
                    db.Usuarios.Add(usuarios);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "Organizador")]
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

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public ActionResult Edit([Bind(Include = "ID,Email,Senha,Nome,Sobrenome")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                usuarios.Tipo = "Organizador";
                usuarios.Ativo = "S";
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        [Authorize(Roles = "Organizador")]
        public ActionResult Delete(int? id)
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

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarios);
            db.SaveChanges();
            return RedirectToAction("Index");
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

        public void InsereUsuarioTime(usuarios_times oUsuariosTimes)
        {
            db.usuarios_times.Add(oUsuariosTimes);
            db.SaveChanges();
        }

        public List<Usuarios> usuariosTime(int idTime)
        {
            return this.repository.usuariosTime(idTime);
        }
    }
}
