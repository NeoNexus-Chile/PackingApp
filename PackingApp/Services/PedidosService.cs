using Microsoft.EntityFrameworkCore;
using PackingApp.Core;
using PackingApp.Data;

namespace PackingApp.Services
{
    public class PedidosService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PedidosService> _logger;

        public PedidosService(ApplicationDbContext context, ILogger<PedidosService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaginatedResult<PedidoDto>> GetPedidosAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = await _context.Pedidos.CountAsync();
                
                var pedidos = await _context.Pedidos
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new PedidoDto
                    {
                        Id = p.Id.ToString(),
                        NumeroPedido = p.Id.ToString().Substring(0, 8),
                        EmpresaNombre = "Empresa",
                        EstadoNombre = p.EstadoPedido != null ? p.EstadoPedido.Nombre : "Sin estado",
                        FechaPedido = p.FechaIngreso.ToString("dd/MM/yyyy"),
                        FechaEntrega = p.FechaActualizacion.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync();

                return new PaginatedResult<PedidoDto>
                {
                    Items = pedidos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pedidos");
                throw;
            }
        }

        public async Task<PedidoDto?> GetPedidoByIdAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var guidId)) return null;

                var pedido = await _context.Pedidos
                    .Include(p => p.EstadoPedido)
                    .FirstOrDefaultAsync(p => p.Id == guidId);

                if (pedido == null) return null;

                return new PedidoDto
                {
                    Id = pedido.Id.ToString(),
                    NumeroPedido = pedido.Id.ToString().Substring(0, 8),
                    EmpresaNombre = "Empresa",
                    EstadoNombre = pedido.EstadoPedido != null ? pedido.EstadoPedido.Nombre : "Sin estado",
                    FechaPedido = pedido.FechaIngreso.ToString("dd/MM/yyyy"),
                    FechaEntrega = pedido.FechaActualizacion.ToString("dd/MM/yyyy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pedido {PedidoId}", id);
                throw;
            }
        }

        public async Task<bool> DeletePedidoAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var guidId)) return false;

                var pedido = await _context.Pedidos.FindAsync(guidId);
                if (pedido == null) return false;

                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar pedido {PedidoId}", id);
                throw;
            }
        }

        public async Task<bool> CreatePedidoAsync(string usuarioId, int estadoPedidoId, int cantidad, int unidadesDelFormato, int productoId)
        {
            try
            {
                var pedido = new Pedido
                {
                    Id = Guid.NewGuid(),
                    FechaIngreso = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    Vigente = true,
                    UsuarioId = usuarioId,
                    EstadoPedidoId = estadoPedidoId,
                    Cantidad = cantidad,
                    UnidadesDelFormato = unidadesDelFormato,
                    ProductoId = productoId
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear pedido");
                throw;
            }
        }
    }

    public class PedidoDto
    {
        public string Id { get; set; } = string.Empty;
        public string NumeroPedido { get; set; } = string.Empty;
        public string EmpresaNombre { get; set; } = string.Empty;
        public string EstadoNombre { get; set; } = string.Empty;
        public string FechaPedido { get; set; } = string.Empty;
        public string FechaEntrega { get; set; } = string.Empty;
    }
}

