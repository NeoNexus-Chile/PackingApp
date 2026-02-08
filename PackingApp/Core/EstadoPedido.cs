using System.ComponentModel.DataAnnotations;

namespace PackingApp.Core
{
    public sealed class EstadoPedido
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Nombre { get; set; }
    }
}
