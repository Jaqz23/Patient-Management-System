using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Domain.Entities;
using SGP.Infrastucture.Persistence.Contexts;

namespace SGP.Infrastucture.Persistence.Repositories
{
    public class MantenimientoUsuarioRepository : GenericRepository<Usuario>, IMantenimientoUsuarioRepository
    {
        private readonly ApplicationContext _dbContext;

        public MantenimientoUsuarioRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Usuario>> GetUsuariosByConsultorioAsync(int consultorioId)
        {
            return await _dbContext.Usuarios
                .Where(u => u.ConsultorioId == consultorioId)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario)
        {
            return await _dbContext.Usuarios.AnyAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<bool> ExistsByCorreoAsync(string correo)
        {
            return await _dbContext.Usuarios.AnyAsync(u => u.Correo == correo);
        }
    }
}
