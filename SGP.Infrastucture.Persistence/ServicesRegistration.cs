using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Infrastucture.Persistence.Contexts;
using SGP.Infrastucture.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGP.Infrastucture.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region "DB config"
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(connectionString, m=> m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IMantenimientoUsuarioRepository, MantenimientoUsuarioRepository>();
            services.AddTransient<IConsultorioRepository, ConsultorioRepository>();
            services.AddTransient<IMedicoRepository, MedicoRepository>();
            services.AddTransient<IPacienteRepository, PacienteRepository>();
            services.AddTransient<ICitaRepository, CitaRepository>();
            services.AddTransient<IPruebaLaboratorioRepository, PruebaLaboratorioRepository>();
            services.AddTransient<IResultadoLaboratorioRepository, ResultadoLaboratorioRepository>();
            #endregion
        }

    }
}
