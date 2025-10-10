using System.Collections.Generic;
using System.Reflection.Emit;
using Api_PPS;
using Microsoft.EntityFrameworkCore;

namespace Api_PPS
{
    public class ContextoApi : DbContext
    {
        public ContextoApi(DbContextOptions<ContextoApi> options) : base(options)
        {
        }

        // Cambiado a DbSet<Videojuego>
        public DbSet<Videojuego> Videojuegos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Videojuego>(entity =>
            {
                entity.HasIndex(v => v.Nombre);
                entity.HasIndex(v => v.Genero);
                entity.HasIndex(v => v.Desarrollador);
                entity.HasIndex(v => v.AnioLanzamiento);
            });
        }
    }
}

