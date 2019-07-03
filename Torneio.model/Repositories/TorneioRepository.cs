using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            Usuarios usuarios = db.Usuarios.Find(idUsuario);
            if(usuarios.Tipo == "Organizador")
            {
                return (from p in db.Torneios join b in db.usuarios_torneios on p.ID equals b.IDTorneio where b.IDUsuario == idUsuario select p).ToList();
            }else if(usuarios.Tipo == "Times")
            {
                List<Torneios> Torneios = new List<Torneios>();
                using (var tab = new TorneioEntities())
                {
                    var ListaTorneios = tab.Database
                                    .SqlQuery<Torneios>("select a.* from torneios as a join Torneios_Times as b on a.ID = b.IDTorneio join Times as c on c.ID = b.IDTime join usuarios_times as d on d.IDTime = b.IDTime where d.IDUsuario = @id group by a.ID, a.Nome, a.Premiacao, a.Realizador, a.Ano", new SqlParameter("@id", idUsuario)).ToList();

                    Torneios = ListaTorneios;
                }
                return Torneios;
            }
            return (from p in db.Torneios join b in db.usuarios_torneios on p.ID equals b.IDTorneio where b.IDUsuario == idUsuario select p).ToList();

        }


    }
}
