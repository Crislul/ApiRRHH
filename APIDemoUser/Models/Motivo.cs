using System.ComponentModel.DataAnnotations;

namespace APIDemoUser.Models
{
    public class Motivo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
    }
}
