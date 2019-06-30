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
    public class JogadoresRepository
    {
        private TorneioEntities db = new TorneioEntities();

        public List<Jogadores> selecionaJogadoresTime(int idTime)
        {
            return (from p in db.Jogadores where p.IDTime == idTime select p).ToList();
        }

    }
}
