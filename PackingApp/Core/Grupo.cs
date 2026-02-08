using System.ComponentModel.DataAnnotations;

namespace PackingApp.Core
{
    public sealed class Grupo
    {
        public int Id { get; set; }
        [MaxLength(125)]
        public required string Nombre { get; set; }
        [MaxLength(750)]
        public string? UrlGrupo { get; set; }
    }
}
