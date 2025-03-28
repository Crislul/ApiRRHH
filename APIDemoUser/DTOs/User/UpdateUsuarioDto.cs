namespace APIDemoUser.DTOs.User
{
    public class UpdateUsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public string Correo { get; set; }
        public string ContrasenaHash { get; set; }
        public int TipoUsuario { get; set; }
    }
}
