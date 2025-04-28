using Microsoft.EntityFrameworkCore;
using SGP.Core.Domain.Entities;

namespace SGP.Infrastucture.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Consultorio> Consultorios { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<PruebaLaboratorio> PruebasLaboratorio { get; set; }
        public DbSet<ResultadoLaboratorio> ResultadosLaboratorio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tables
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Consultorio>().ToTable("Consultorios");
            modelBuilder.Entity<Medico>().ToTable("Medicos");
            modelBuilder.Entity<Paciente>().ToTable("Pacientes");
            modelBuilder.Entity<Cita>().ToTable("Citas");
            modelBuilder.Entity<PruebaLaboratorio>().ToTable("PruebasLaboratorio");
            modelBuilder.Entity<ResultadoLaboratorio>().ToTable("ResultadosLaboratorio");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Consultorio>().HasKey(c => c.Id);
            modelBuilder.Entity<Medico>().HasKey(m => m.Id);
            modelBuilder.Entity<Paciente>().HasKey(p => p.Id);
            modelBuilder.Entity<Cita>().HasKey(c => c.Id);
            modelBuilder.Entity<PruebaLaboratorio>().HasKey(pl => pl.Id);
            modelBuilder.Entity<ResultadoLaboratorio>().HasKey(rl => rl.Id);
            #endregion

            #region Relationships

            // Relacion Consultorio (1:N) Usuario
            modelBuilder.Entity<Consultorio>()
                .HasMany<Usuario>(c => c.Usuarios)
                .WithOne(u => u.Consultorio)
                .HasForeignKey(u => u.ConsultorioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Consultorio (1:N) Medico
            modelBuilder.Entity<Consultorio>()
                .HasMany<Medico>(c => c.Medicos)
                .WithOne(m => m.Consultorio)
                .HasForeignKey(m => m.ConsultorioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Consultorio (1:N) Paciente
            modelBuilder.Entity<Consultorio>()
                .HasMany<Paciente>(c => c.Pacientes)
                .WithOne(p => p.Consultorio)
                .HasForeignKey(p => p.ConsultorioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Consultorio (1:N) Cita
            modelBuilder.Entity<Consultorio>()
                .HasMany<Cita>(c => c.Citas)
                .WithOne(c => c.Consultorio)
                .HasForeignKey(c => c.ConsultorioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Consultorio (1:N) PruebaLaboratorio
            modelBuilder.Entity<Consultorio>()
                .HasMany<PruebaLaboratorio>(c => c.PruebasLaboratorio)
                .WithOne(pl => pl.Consultorio)
                .HasForeignKey(pl => pl.ConsultorioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Médico (1:N) Cita
            modelBuilder.Entity<Medico>()
                .HasMany<Cita>(m => m.Citas)
                .WithOne(c => c.Medico)
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Paciente (1:N) Cita
            modelBuilder.Entity<Paciente>()
                .HasMany<Cita>(p => p.Citas)
                .WithOne(c => c.Paciente)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion PruebaLaboratorio (1:N) ResultadoLaboratorio
            modelBuilder.Entity<PruebaLaboratorio>()
                .HasMany<ResultadoLaboratorio>(pl => pl.Resultados)
                .WithOne(rl => rl.PruebaLaboratorio)
                .HasForeignKey(rl => rl.PruebaLaboratorioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Paciente (1:N) ResultadoLaboratorio
            modelBuilder.Entity<Paciente>()
                .HasMany<ResultadoLaboratorio>(p => p.Resultados)
                .WithOne(rl => rl.Paciente)
                .HasForeignKey(rl => rl.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Cita (1:N) ResultadoLaboratorio
            modelBuilder.Entity<Cita>()
                .HasMany<ResultadoLaboratorio>(c => c.Resultados)
                .WithOne(rl => rl.Cita)
                .HasForeignKey(rl => rl.CitaId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Property Configurations

            // Usuario
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Correo)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.NombreUsuario)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Contraseña)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasConversion<string>();

            // Paciente
            modelBuilder.Entity<Paciente>()
                .Property(p => p.Cedula)
                .IsRequired()
                .HasMaxLength(11);

            modelBuilder.Entity<Paciente>()
                .HasIndex(p => p.Cedula) // Cedula unica
                .IsUnique();

            modelBuilder.Entity<Paciente>()
                .Property(p => p.FechaNacimiento)
                .IsRequired();

            modelBuilder.Entity<Paciente>()
                .Property(p => p.Telefono)
                .IsRequired()
                 .HasMaxLength(10);

            modelBuilder.Entity<Paciente>()
                .Property(p => p.Direccion)
                .IsRequired();

            modelBuilder.Entity<Paciente>()
                .Property(p => p.Foto)
                .IsRequired(false);

            // Medico
            modelBuilder.Entity<Medico>()
                .Property(m => m.Cedula)
                .IsRequired()
                .HasMaxLength(11);
            
            modelBuilder.Entity<Medico>()
                .HasIndex(m => m.Cedula) 
                .IsUnique();

            modelBuilder.Entity<Medico>()
                .Property(m => m.Correo)
                .IsRequired();

            modelBuilder.Entity<Medico>()
                .Property(m => m.Telefono)
                .IsRequired()
                .HasMaxLength(10);
           
            modelBuilder.Entity<Medico>()
                .Property(m => m.Foto)
                .IsRequired(false);

            // Cita
            modelBuilder.Entity<Cita>()
                .Property(c => c.Fecha)
                .IsRequired();

            modelBuilder.Entity<Cita>()
                .Property(c => c.Hora)
                .IsRequired();

            modelBuilder.Entity<Cita>()
                .Property(c => c.Causa)
                .IsRequired()
                .HasMaxLength(300);
           
            modelBuilder.Entity<Cita>()
                .Property(c => c.Estado)
                .HasConversion<string>();

            // Prueba de laboratorio
            modelBuilder.Entity<PruebaLaboratorio>()
                .Property(pl => pl.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            // Resultado de laboratorio
            modelBuilder.Entity<ResultadoLaboratorio>()
                .Property(rl => rl.Resultado)
                .HasMaxLength(200);

            modelBuilder.Entity<ResultadoLaboratorio>()
                .Property(rl => rl.Estado)
                .HasConversion<string>();

            #endregion
        }
    }
}
