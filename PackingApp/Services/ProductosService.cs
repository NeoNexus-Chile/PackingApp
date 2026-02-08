using Microsoft.EntityFrameworkCore;
using PackingApp.Core;
using PackingApp.Data;

namespace PackingApp.Services
{
    public class ProductosService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductosService> _logger;

        public ProductosService(ApplicationDbContext context, ILogger<ProductosService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaginatedResult<ProductoDto>> GetProductosAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _context.Productos.CountAsync();
                
                var productos = await _context.Productos
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new ProductoDto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Descripcion = $"Formato: {p.Formato.Nombre}, Presentación: {p.Presentacion.Nombre}",
                        CodigoProducto = p.Id.ToString(),
                        FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync();

                return new PaginatedResult<ProductoDto>
                {
                    Items = productos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                throw;
            }
        }

        public async Task<PaginatedResult<FormatoDto>> GetFormatosAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _context.Formatos.CountAsync();
                
                var formatos = await _context.Formatos
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(f => new FormatoDto
                    {
                        Id = f.Id,
                        Nombre = f.Nombre,
                        Descripcion = string.Empty,
                        FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync();

                return new PaginatedResult<FormatoDto>
                {
                    Items = formatos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener formatos");
                throw;
            }
        }

        public async Task<PaginatedResult<PresentacionDto>> GetPresentacionesAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _context.Presentaciones.CountAsync();
                
                var presentaciones = await _context.Presentaciones
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new PresentacionDto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Descripcion = string.Empty,
                        FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync();

                return new PaginatedResult<PresentacionDto>
                {
                    Items = presentaciones,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener presentaciones");
                throw;
            }
        }

        public async Task<PaginatedResult<GrupoDto>> GetGruposAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _context.Grupos.CountAsync();
                
                var grupos = await _context.Grupos
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(g => new GrupoDto
                    {
                        Id = g.Id,
                        Nombre = g.Nombre,
                        Descripcion = string.Empty,
                        FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync();

                return new PaginatedResult<GrupoDto>
                {
                    Items = grupos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener grupos");
                throw;
            }
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null) return false;

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto {ProductoId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteFormatoAsync(int id)
        {
            try
            {
                var formato = await _context.Formatos.FindAsync(id);
                if (formato == null) return false;

                _context.Formatos.Remove(formato);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar formato {FormatoId}", id);
                throw;
            }
        }

        public async Task<bool> DeletePresentacionAsync(int id)
        {
            try
            {
                var presentacion = await _context.Presentaciones.FindAsync(id);
                if (presentacion == null) return false;

                _context.Presentaciones.Remove(presentacion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar presentación {PresentacionId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteGrupoAsync(int id)
        {
            try
            {
                var grupo = await _context.Grupos.FindAsync(id);
                if (grupo == null) return false;

                _context.Grupos.Remove(grupo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar grupo {GrupoId}", id);
                throw;
            }
        }

    public async Task<List<FormatoDto>> GetFormatosListAsync()
    {
        try
        {
            return await _context.Formatos
                .Select(f => new FormatoDto
                {
                    Id = f.Id,
                    Nombre = f.Nombre,
                    Descripcion = string.Empty,
                    FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lista de formatos");
            throw;
        }
    }

    public async Task<List<PresentacionDto>> GetPresentacionesListAsync()
    {
        try
        {
            return await _context.Presentaciones
                .Select(p => new PresentacionDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = string.Empty,
                    FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lista de presentaciones");
            throw;
        }
    }

    public async Task<List<GrupoDto>> GetGruposListAsync()
    {
        try
        {
            return await _context.Grupos
                .Select(g => new GrupoDto
                {
                    Id = g.Id,
                    Nombre = g.Nombre,
                    Descripcion = string.Empty,
                    FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lista de grupos");
            throw;
        }
    }

    public async Task<bool> CreateProductoAsync(string nombre, int formatoId, int presentacionId, int grupoId)
        {
            try
            {
                var producto = new Producto
                {
                    Nombre = nombre,
                    FormatoId = formatoId,
                    PresentacionId = presentacionId,
                    GrupoId = grupoId
                };

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                throw;
            }
        }

        public async Task<bool> CreateFormatoAsync(string nombre, int unidades)
        {
            try
            {
                var formato = new Formato
                {
                    Nombre = nombre,
                    Unidades = unidades
                };

                _context.Formatos.Add(formato);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear formato");
                throw;
            }
        }

        public async Task<bool> CreatePresentacionAsync(string nombre)
        {
            try
            {
                var presentacion = new Presentacion
                {
                    Nombre = nombre
                };

                _context.Presentaciones.Add(presentacion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear presentación");
                throw;
            }
        }

        public async Task<bool> CreateGrupoAsync(string nombre, string? urlGrupo = null)
        {
            try
            {
                var grupo = new Grupo
                {
                    Nombre = nombre,
                    UrlGrupo = urlGrupo
                };

                _context.Grupos.Add(grupo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear grupo");
                throw;
            }
        }
    }

    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string CodigoProducto { get; set; } = string.Empty;
        public string FechaCreacion { get; set; } = string.Empty;
    }

    public class FormatoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FechaCreacion { get; set; } = string.Empty;
    }

    public class PresentacionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FechaCreacion { get; set; } = string.Empty;
    }

    public class GrupoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FechaCreacion { get; set; } = string.Empty;
    }
}

