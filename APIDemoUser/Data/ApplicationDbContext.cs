using APIDemoUser.Models;
using Microsoft.EntityFrameworkCore;

namespace APIDemoUser.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        
        public DbSet<Area> Areas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Motivo> Motivos { get; set; }
        public DbSet<Incidencia> Incidencias { get; set; }
        public DbSet<Autorizacion> Autorizaciones { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Incidencia>()
                .HasOne(i => i.Usuario)
                .WithMany(t => t.Incidencias)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incidencia>()
                .HasOne(i => i.Area)
                .WithMany(a => a.Incidencias)
                .HasForeignKey(i => i.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incidencia>()
                .HasOne(i => i.Categoria)
                .WithMany(c => c.Incidencias)
                .HasForeignKey(i => i.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incidencia>()
                .HasOne(i => i.Motivo)
                .WithMany(m => m.Incidencias)
                .HasForeignKey(i => i.MotivoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Autorizacion>()
                .HasOne(a => a.Usuario)
                .WithMany(t => t.Autorizaciones)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Autorizacion>()
                .HasOne(a => a.Area)
                .WithMany(at => at.Autorizaciones)
                .HasForeignKey(a => a.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Autorizacion>()
                .HasOne(a => a.Categoria)
                .WithMany(c => c.Autorizaciones)
                .HasForeignKey(a => a.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
