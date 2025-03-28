using System.ComponentModel.DataAnnotations;
using System.Net;

namespace APIDemoUser.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
        public ICollection<Autorizacion> Autorizaciones { get; set; } = new List<Autorizacion>();
    }
}
