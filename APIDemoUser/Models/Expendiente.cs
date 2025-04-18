namespace APIDemoUser.Models
{
    public class Expediente
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string Archivo { get; set; } = string.Empty;
        public DateTime FechaSubida { get; set; } = DateTime.Now;

    }
}
