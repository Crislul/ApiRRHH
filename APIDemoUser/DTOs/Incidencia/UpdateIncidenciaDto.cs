namespace APIDemoUser.DTOs.Incidencia
{
    public class UpdateIncidenciaDto
    {
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int UsuarioId { get; set; }
        public int AreaId { get; set; }
        public int CategoriaId { get; set; }
        public int MotivoId { get; set; }
        public int Estatus { get; set; }
    }
}
