using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneio.model.Repositories
{
    public class TorneioRepository
    {
        private TorneioEntities db = new TorneioEntities();

       public List<Torneios> selecionaTodos(int idUsuario)
       {
            return (from p in db.Torneios join b in db.usuarios_torneios on p.ID equals b.IDTorneio where b.IDUsuario == idUsuario select p).ToList();
       }

    }
}
