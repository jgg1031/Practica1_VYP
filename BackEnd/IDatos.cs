using Nucleo.Modelos;

namespace BackEnd;

public interface IDatos
{
    public Dictionary<int, Usuario> Usuarios { get; }
    public Dictionary<int, Rol> Roles { get; }
    public Dictionary<int, Proyecto> Proyectos { get; }
    public List<Log> Logs { get; }

    // Usuarios
    Usuario? GuardarUsuario(Usuario usuario);
    Usuario? LeerUsuario(int idUsuario);
    Usuario? LeerUsuario(string eMail);
    Usuario? EliminarUsuario(int idUsuario);

    // Roles
    Rol? GuardarRol(Rol rol);
    Rol? LeerRol(int idRol);
    Rol? EliminarRol(int idRol);

    // Proyectos
    Proyecto? GuardarProyecto(Proyecto proyecto);
    Proyecto? LeerProyecto(int idProyecto);
    Proyecto? EliminarProyecto(int idProyecto);

    // Log
    void AgregarLog(Log log);
    int LimpiarLog();

    void Reiniciar();
}