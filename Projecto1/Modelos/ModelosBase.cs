namespace Nucleo.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string EMail { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public bool CompruebaPassword(string password)
        {
            return !string.IsNullOrEmpty(password) && PasswordHash == password;
        }

        public override string ToString() => EMail;
    }

    public record EntradaLog(DateTime Fecha, string Categoria, string Accion, string Usuario);
}

namespace BackEnd
{
    using Nucleo.Modelos;

    public interface IDatos
    {
        Usuario? LeerUsuario(string email);
        Usuario? LeerUsuario(int idUsuario);
        void AgregarLog(EntradaLog entrada);
    }

    public class DatosMemoria : IDatos
    {
        private readonly List<Usuario> _usuarios =
        [
            new Usuario { IdUsuario = 1, EMail = "admin@ubu.es", PasswordHash = "Contr4señ@" }
        ];

        private readonly List<EntradaLog> _logs = [];

        public Usuario? LeerUsuario(string email) =>
            _usuarios.FirstOrDefault(u => u.EMail.Equals(email, StringComparison.OrdinalIgnoreCase));

        public Usuario? LeerUsuario(int idUsuario) =>
            _usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

        public void AgregarLog(EntradaLog entrada) => _logs.Add(entrada);
    }
}
