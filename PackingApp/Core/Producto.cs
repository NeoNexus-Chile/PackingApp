using System.ComponentModel.DataAnnotations;

namespace PackingApp.Core
{
    public sealed class Producto
    {
        public int Id { get; set; }
        [MaxLength(450)]
        public required string Nombre { get; set; }
        public int FormatoId { get; set; }
        public Formato Formato { get; set; }
        public int PresentacionId { get; set; }
        public Presentacion Presentacion { get; set; }
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
    }
}
