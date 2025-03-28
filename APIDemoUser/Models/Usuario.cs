using System.ComponentModel.DataAnnotations;
using System.Net;

namespace APIDemoUser.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(60, ErrorMessage = "El nombre no puede superar los 60 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido Paterno es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
        public string ApellidoP { get; set; }

        [Required(ErrorMessage = "El apellido Materno es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
        public string ApellidoM { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [DataType(DataType.Password)]
        public string ContrasenaHash { get; set; } // Contraseña encriptada

        [Required(ErrorMessage = "El tipo de usuario es requerida.")]
        public int TipoUsuario { get; set; } // 2 = Admin, 1 = Usuario


        public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
        public ICollection<Autorizacion> Autorizaciones { get; set; } = new List<Autorizacion>();
    }
}

