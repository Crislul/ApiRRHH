namespace APIDemoUser.DTOs.Incidencia
{
    public class CreateIncidenciaDto
    {
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int? UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }  
        public int? AreaId { get; set; }
        public string AreaNombre { get; set; }  
        public int? CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }  
        public int? MotivoId { get; set; }
        public string MotivoNombre { get; set; }
        public int EstatusAdmin { get; set; }
        public int EstatusDir { get; set; }

        public string? NombreArchivo { get; set; }
    }
}
