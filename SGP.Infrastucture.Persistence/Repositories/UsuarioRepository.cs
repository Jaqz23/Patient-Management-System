using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Helpers;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Entities;
using SGP.Core.Domain.Enums;
using SGP.Infrastucture.Persistence.Contexts;


namespace SGP.Infrastucture.Persistence.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationContext _dbContext;

        public UsuarioRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<Usuario> AddAsync(Usuario entity)
        {
            entity.Contraseña = PasswordEncryptation.ComputeSha256Hash(entity.Contraseña);
            entity.Rol = RolUsuario.Administrador;
            await base.AddAsync(entity);
            return entity;
        }

        public async Task<Usuario> LoginAsync(LoginViewModel loginVm)
        {
            string passwordEncrypt = PasswordEncryptation.ComputeSha256Hash(loginVm.Contraseña);
            Usuario usuario = await _dbContext.Set<Usuario>().FirstOrDefaultAsync(u => u.NombreUsuario == loginVm.NombreUsuario && u.Contraseña == passwordEncrypt);
            return usuario;
        }

        public async Task<bool> ExistsByNombreUsuario(string nombreUsuario)
        {
            return await _dbContext.Usuarios.AnyAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<bool> ExistsByCorreo(string correo)
        {
            return await _dbContext.Usuarios.AnyAsync(c => c.Correo == correo);
        }
    }
}
