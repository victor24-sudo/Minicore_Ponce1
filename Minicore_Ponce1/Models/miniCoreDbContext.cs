using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Minicore_Ponce1.Models
{
    public class miniCoreDbContext: DbContext
    {
        public DbSet<Pase> Pases { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}