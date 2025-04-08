namespace APIDemoUser.DTOs.Autorizacion
{
    public class CreateAutorizacionDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellidoP { get; set; }
        public string UsuarioApellidoM { get; set; }
        public int AreaId { get; set; }
        public string AreaNombre { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }
        public string HoraSalida { get; set; }
        public string HoraEntrada { get; set; }
        public string HorarioTrabajo { get; set; }
        public string Asunto { get; set; }
        public string Fecha { get; set; }
        public int Estatus { get; set; }
    }
}
