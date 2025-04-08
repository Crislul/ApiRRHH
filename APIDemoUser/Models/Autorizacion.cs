using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIDemoUser.Models
{
    public class Autorizacion
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public Area Area { get; set; }

        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        [Required]
        public string HoraSalida { get; set; }

        [Required]
        public string HoraEntrada { get; set; }

        [Required]
        [StringLength(100)]
        public string HorarioTrabajo { get; set; }

        [Required]
        [StringLength(255)]
        public string Asunto { get; set; }

        [Required]
        public string Fecha { get; set; }

        [Required]
        public int Estatus { get; set; }
    }
}
