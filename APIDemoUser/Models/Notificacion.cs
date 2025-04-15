namespace APIDemoUser.Models
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string? Mensaje { get; set; }
        public string? Tipo { get; set; } // incidencia,salida,respuesta
        public string Estado { get; set; } = "pendiente"; // 
        public DateTime Fecha { get; set; } = DateTime.Now;

        public int? PermisoId { get; set; } // ID de la incidencia o salida 
        public int? UsuarioId { get; set; } // ID del usuario al que va la notificación

        public string? TipoPermiso { get; set; } //incidencia, salida para la redireccion de respuesta
    }

}
