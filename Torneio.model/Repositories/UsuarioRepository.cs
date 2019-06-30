using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torneio.model.Models;
using System.Data;
using System.Data.Entity;


namespace Torneio.model.Repositories
{
    public class UsuarioRepository
    {
        private TorneioEntities db = new TorneioEntities();

        public List<Usuarios> usuariosTime(int idTime)
        {
            return (from p in db.Usuarios join b in db.usuarios_times on p.ID equals b.IDUsuario where b.IDTime == idTime select p).ToList();
        }

    }
}
