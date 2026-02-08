using PackingApp.Data;

namespace PackingApp.Core
{
    public sealed class Pedido
    {
        public Guid Id { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public bool Vigente { get; set; }
        public int EstadoPedidoId { get; set; }
        public EstadoPedido EstadoPedido { get; set; }
        public int Cantidad { get; set; }
        public int UnidadesDelFormato { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        public ApplicationUser Usuario { get; set; }
        public required string UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
