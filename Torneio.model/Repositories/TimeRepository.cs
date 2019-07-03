using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneio.model.Repositories
{
    public class TimeRepository
    {
        private TorneioEntities db = new TorneioEntities();

        public Times getTime(int idUsuario)
        {
            int idTime = (from p in db.usuarios_times where p.IDUsuario == idUsuario select p.IDTime).FirstOrDefault();
            return (from p in db.Times where p.ID == idTime select p).FirstOrDefault();
        }

        public List<Times> selecionaTodos(int idUsuario)
        {
            return (from p in db.Times join a in db.usuarios_times on p.ID equals a.IDTime where a.IDUsuario == idUsuario select p).ToList();
        }

    }
}
