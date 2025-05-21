namespace APIDemoUser.DTOs.Autorizacion
{
    public class UpdateAutorizacionDto
    {
        public int UsuarioId { get; set; }
        public int AreaId { get; set; }
        public int CategoriaId { get; set; }
        public string HoraSalida { get; set; }
        public string HoraEntrada { get; set; }
        public string HorarioTrabajo { get; set; }
        public string Asunto { get; set; }
        public string Fecha { get; set; }
        public int EstatusAdmin { get; set; }
        public int EstatusDir { get; set; }
    }
}
