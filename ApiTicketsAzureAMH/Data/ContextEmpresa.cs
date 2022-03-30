using ApiTicketsAzureAMH.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTicketsAzureAMH.Data
{
    public class ContextEmpresa: DbContext
    {
        public ContextEmpresa(DbContextOptions<ContextEmpresa> options) : base(options) { }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
