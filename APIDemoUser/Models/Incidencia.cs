using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIDemoUser.Models
{
    public class Incidencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public string Fecha { get; set; }

        [Required]
        public string FechaInicio { get; set; }

        [Required]
        public string FechaFin { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public Area Area { get; set; }

        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        [ForeignKey("Motivo")]
        public int MotivoId { get; set; }
        public Motivo Motivo { get; set; }

        [Required]
        public int Estatus { get; set; }

        
        public byte[]? Archivo { get; set; }

        public string? NombreArchivo { get; set; }

       
    }

}
