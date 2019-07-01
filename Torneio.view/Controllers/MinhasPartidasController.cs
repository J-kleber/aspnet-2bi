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
using Torneio.model.Models;


namespace Torneio.view.Controllers
{
    public class MinhasPartidasController : Controller
    {
        private TorneioEntities db = new TorneioEntities();
        private PartidaRepository repository = new PartidaRepository();
        // GET: Partidas
        [Authorize(Roles = "Times")]
        public ActionResult Index()
        {
            UsuariosController ousuario = new UsuariosController();
            int idTime = 0;
            if (User.Identity.IsAuthenticated)
            {
                int idUsuario = ousuario.getUsuario(User.Identity.Name).ID;
                TimesController oTimes = new TimesController();
                idTime = oTimes.getTime(idUsuario).ID;
            }
            List<TimesPartidas> Partidas = new List<TimesPartidas>();
            Partidas = this.repository.minhasPartidas(idTime);
            return View(Partidas);
        }
    }
}