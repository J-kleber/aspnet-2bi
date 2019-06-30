using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Torneio.model;
using Torneio.model.Models;
using Torneio.model.Repositories;


namespace Torneio.view.Controllers
{
    public class TorneiosController : Controller
    {
        private TorneioEntities db = new TorneioEntities();
        private TorneioRepository repository = new TorneioRepository();

        // GET: Torneios
        [Authorize(Roles = "Organizador")]
        public ActionResult Index()
        {
            UsuariosController ousuario = new UsuariosController();
            int idUsuario = 0;
            if (User.Identity.IsAuthenticated)
            {
                idUsuario = ousuario.getUsuario(User.Identity.Name).ID;
            }
            //List<usuarios_torneios> idsTorneios = (from p in db.usuarios_torneios where p.IDUsuario == idUsuario select p).ToList();
            List<Torneios> listaTorneios = new List<Torneios>();
            /*foreach(var id in idsTorneios)
            {
                listaTorneios.Add((from p in db.Torneios where p.ID == id.IDTorneio select p).FirstOrDefault());
            }*/
            listaTorneios = this.selecionaTodos(idUsuario);
            return View(listaTorneios);
        }

        // GET: Torneios/Details/5
        [Authorize(Roles = "Organizador")]
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
        [Authorize(Roles = "Organizador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Torneios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
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
                PartidasController oPartidasController = new PartidasController();
                List<Partidas> partidas = oPartidasController.geraPartidas(torneios.ID, IDTime);
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
        [Authorize(Roles = "Organizador")]
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
        [Authorize(Roles = "Organizador")]
        public ActionResult Edit([Bind(Include = "ID,Nome,Premiacao,Ano,Realizador")] Torneios torneios, [Bind(Include = "IdUsuario")] usuarios_torneios usuarioTorneio, IEnumerable<int> IDTime, String GerarNova)
        {
            if (ModelState.IsValid)
            {

                if(GerarNova == "sim")
                {
                    PartidasController oPartidasController = new PartidasController();
                    oPartidasController.deletaPartidasTorneio(torneios.ID);
                    var item = db.Times.ToList();
                    db.Torneios.Add(torneios);
                    usuarioTorneio.IDTorneio = torneios.ID;
                    db.usuarios_torneios.Add(usuarioTorneio);

                    foreach (var id in IDTime)
                    {
                        Torneios_Times torneioTimes = new Torneios_Times();
                        torneioTimes.IDTorneio = torneios.ID;
                        torneioTimes.IDTime = id;
                        db.Torneios_Times.Add(torneioTimes);
                    }
                   
                    List<Partidas> partidas = oPartidasController.geraPartidas(torneios.ID, IDTime);
                    foreach (Partidas partida in partidas)
                    {
                        db.Partidas.Add(partida);
                    }
                }
              

                db.Entry(torneios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(torneios);
        }

        // GET: Torneios/Delete/5
        [Authorize(Roles = "Organizador")]
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
        [Authorize(Roles = "Organizador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Torneios torneios = db.Torneios.Find(id);
            db.Torneios.Remove(torneios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Tabela(int id)
        {
            List<Tabela> Tabela = montaTabela(id);
            return View(Tabela);
        }

        public List<Tabela> montaTabela(int idTorneio)
        {
            int k = idTorneio;
            TabelaRepository repository = new TabelaRepository();
            return repository.montaTabela(idTorneio);
        }

        public List<Torneios> selecionaTodos(int idUsuario)
        {
            return this.repository.selecionaTodos(idUsuario);
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
