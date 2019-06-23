using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Torneio.model;

namespace Torneio.view.Models
{
    public class SampleContext : DbContext
    {
        public DbSet<Tabela> Tabela { get; set; }
    }
}