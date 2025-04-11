namespace APIDemoUser.Models
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string? Mensaje { get; set; }
        public string? Tipo { get; set; } // "incidencia" o "salida"
        public string Estado { get; set; } = "pendiente"; // o "leido"
        public DateTime Fecha { get; set; } = DateTime.Now;

        public int? PermisoId { get; set; } // ID de la incidencia o salida 
    }

}
