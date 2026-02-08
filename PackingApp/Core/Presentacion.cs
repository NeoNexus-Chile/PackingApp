using System.ComponentModel.DataAnnotations;

namespace PackingApp.Core
{
    public sealed class Presentacion
    {
        public int Id { get; set; }
        [MaxLength(125)]
        public required string Nombre { get; set; }
    }
}
