using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.Services;

namespace SGP.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationDependency(this IServiceCollection services, IConfiguration configuration) 
        {
            #region Services
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IMantenimientoUsuarioService, MantenimientoUsuarioService>();
            services.AddTransient<IMedicoService, MedicoService>();
            services.AddTransient<IPacienteService, PacienteService>();
            services.AddTransient<IConsultorioService, ConsultorioService>();
            services.AddTransient<IPruebaLaboratorioService, PruebaLaboratorioService>();
            services.AddTransient<IResultadoLaboratorioService, ResultadoLaboratorioService>();
            services.AddTransient<ICitaService, CitaService>();
            #endregion

        }
    }
}
