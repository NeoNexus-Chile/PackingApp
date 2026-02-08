using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PackingApp.Data;

namespace PackingApp.Services
{
    public class UsuariosService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsuariosService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosService(ApplicationDbContext context, ILogger<UsuariosService> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<PaginatedResult<UsuarioDto>> GetUsuariosAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _context.Users.CountAsync();
                
                var usuarios = await _context.Users
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UsuarioDto
                    {
                        Id = u.Id,
                        Email = u.Email ?? string.Empty,
                        NombreUsuario = u.UserName ?? string.Empty,
                        EstaActivo = u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow,
                        FechaRegistro = u.EmailConfirmed ? "Confirmado" : "Pendiente"
                    })
                    .ToListAsync();

                return new PaginatedResult<UsuarioDto>
                {
                    Items = usuarios,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                throw;
            }
        }

        public async Task<UsuarioDto?> GetUsuarioByIdAsync(string id)
        {
            try
            {
                var usuario = await _context.Users.FindAsync(id);
                if (usuario == null) return null;

                return new UsuarioDto
                {
                    Id = usuario.Id,
                    Email = usuario.Email ?? string.Empty,
                    NombreUsuario = usuario.UserName ?? string.Empty,
                    EstaActivo = usuario.LockoutEnd == null || usuario.LockoutEnd <= DateTimeOffset.UtcNow,
                    FechaRegistro = usuario.EmailConfirmed ? "Confirmado" : "Pendiente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario {UserId}", id);
                throw;
            }
        }

        public async Task<bool> CreateUsuarioAsync(string email, string nombreUsuario, string password)
        {
            try
            {
                var usuario = new ApplicationUser
                {
                    Email = email,
                    UserName = nombreUsuario,
                    EmailConfirmed = false
                };

                var result = await _userManager.CreateAsync(usuario, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Error al crear usuario: {errors}");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                throw;
            }
        }

        public async Task<bool> DeleteUsuarioAsync(string id)
        {
            try
            {
                var usuario = await _context.Users.FindAsync(id);
                if (usuario == null) return false;

                _context.Users.Remove(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario {UserId}", id);
                throw;
            }
        }
    }

    public class UsuarioDto
    {
        public string Id { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
        public string FechaRegistro { get; set; } = string.Empty;
    }

    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
