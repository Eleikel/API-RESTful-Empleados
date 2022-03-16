using API_Empleados.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Empleados
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Hacer esto antes de agregar Migracion

        public DbSet<Departamento> Departamento { get; set; }

        public DbSet<Cargo> Cargo { get; set; }

        public DbSet<Empleado> Empleado { get; set; }

        public DbSet<Usuario> Usuario { get; set; }
    }
}