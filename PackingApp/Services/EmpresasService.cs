using Microsoft.EntityFrameworkCore;
using PackingApp.Core;
using PackingApp.Data;

namespace PackingApp.Services
{
    public class EmpresasService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmpresasService> _logger;

        public EmpresasService(ApplicationDbContext context, ILogger<EmpresasService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaginatedResult<EmpresaDto>> GetEmpresasAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _context.Empresas.CountAsync();
                
                var empresas = await _context.Empresas
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new EmpresaDto
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        RazonSocial = $"{e.Rut}-{e.Dv}",
                        Email = string.Empty,
                        Telefono = string.Empty,
                        FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync();

                return new PaginatedResult<EmpresaDto>
                {
                    Items = empresas,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresas");
                throw;
            }
        }

        public async Task<EmpresaDto?> GetEmpresaByIdAsync(int id)
        {
            try
            {
                var empresa = await _context.Empresas.FindAsync(id);
                if (empresa == null) return null;

                return new EmpresaDto
                {
                    Id = empresa.Id,
                    Nombre = empresa.Nombre,
                    RazonSocial = $"{empresa.Rut}-{empresa.Dv}",
                    Email = string.Empty,
                    Telefono = string.Empty,
                    FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresa {EmpresaId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteEmpresaAsync(int id)
        {
            try
            {
                var empresa = await _context.Empresas.FindAsync(id);
                if (empresa == null) return false;

                _context.Empresas.Remove(empresa);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar empresa {EmpresaId}", id);
                throw;
            }
        }

        public async Task<bool> CreateEmpresaAsync(string nombre, int rut, string dv)
        {
            try
            {
                var empresa = new Empresa
                {
                    Nombre = nombre,
                    Rut = rut,
                    Dv = dv
                };

                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empresa");
                throw;
            }
        }
    }

    public class EmpresaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string FechaCreacion { get; set; } = string.Empty;
    }
}

