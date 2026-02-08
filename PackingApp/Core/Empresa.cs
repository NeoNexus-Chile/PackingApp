using System.ComponentModel.DataAnnotations;

namespace PackingApp.Core
{
    public sealed class Empresa
    {
        public int Id { get; set; }
        [MaxLength(125)]
        public required string Nombre { get; set; }
        public int Rut { get; set; }
        [MaxLength(1)]
        public required string Dv { get; set; }
    }
}
