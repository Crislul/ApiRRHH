namespace APIDemoUser.DTOs.Expendiente
{
    public class ExpedienteCreateDto
    {
        public int UsuarioId { get; set; }
        public string? Documento { get; set; } 
        public IFormFile? Archivo { get; set; } 
    }
}
