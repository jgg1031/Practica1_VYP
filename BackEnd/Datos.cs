using Nucleo.Modelos;
using Nucleo.Seguridad;

namespace BackEnd;

public class Datos : IDatos
{
    public Dictionary<int, Usuario> Usuarios { get; private set; } = new();
    public Dictionary<int, Rol> Roles { get; private set; } = new();
    public Dictionary<int, Proyecto> Proyectos { get; private set; } = new();
    public List<Log> Logs { get; private set; } = new();

    public Datos()
    {
        Reiniciar();
    }

    public void Reiniciar()
    {
        Usuarios.Clear();
        Roles.Clear();
        Proyectos.Clear();
        Logs.Clear();

        // Generamos un hash real con "1234" usando tu ServicioHash
        ResultadoHash hash = ServicioHash.Generar("1234");

        Usuario admin = new Usuario
        {
            IdUsuario = 1,
            Nombre = "Administrador",
            Email = "admin@ubu.es",
            ContrasenaHash = hash.Hash,
            ContrasenaSal = hash.Sal
        };

        Usuarios.Add(admin.IdUsuario, admin);
    }

    #region Usuarios
    public Usuario? GuardarUsuario(Usuario usuario)
    {
        if (usuario.IdUsuario <= 0)
        {
            usuario.IdUsuario = Usuarios.Keys.DefaultIfEmpty(0).Max() + 1;
        }
        Usuarios[usuario.IdUsuario] = usuario;
        return usuario;
    }

    public Usuario? LeerUsuario(int idUsuario)
    {
        Usuarios.TryGetValue(idUsuario, out var usuario);
        return usuario;
    }

    public Usuario? LeerUsuario(string eMail)
    {
        return Usuarios.Values.FirstOrDefault(u => u.Email.Equals(eMail, StringComparison.OrdinalIgnoreCase));
    }

    public Usuario? EliminarUsuario(int idUsuario)
    {
        if (Usuarios.TryGetValue(idUsuario, out var usuario))
        {
            Usuarios.Remove(idUsuario);
            return usuario;
        }
        return null;
    }
    #endregion

    #region Roles
    public Rol? GuardarRol(Rol rol)
    {
        if (rol.Id_Rol <= 0)
        {
            rol.Id_Rol = Roles.Keys.DefaultIfEmpty(0).Max() + 1;
        }
        Roles[rol.Id_Rol] = rol;
        return rol;
    }

    public Rol? LeerRol(int idRol)
    {
        Roles.TryGetValue(idRol, out var rol);
        return rol;
    }

    public Rol? EliminarRol(int idRol)
    {
        if (Roles.TryGetValue(idRol, out var rol))
        {
            Roles.Remove(idRol);
            return rol;
        }
        return null;
    }
    #endregion

    #region Proyectos
    public Proyecto? GuardarProyecto(Proyecto proyecto)
    {
        if (proyecto.Id_Proyecto <= 0)
        {
            proyecto.Id_Proyecto = Proyectos.Keys.DefaultIfEmpty(0).Max() + 1;
        }
        Proyectos[proyecto.Id_Proyecto] = proyecto;
        return proyecto;
    }

    public Proyecto? LeerProyecto(int idProyecto)
    {
        Proyectos.TryGetValue(idProyecto, out var proyecto);
        return proyecto;
    }

    public Proyecto? EliminarProyecto(int idProyecto)
    {
        if (Proyectos.TryGetValue(idProyecto, out var proyecto))
        {
            Proyectos.Remove(idProyecto);
            return proyecto;
        }
        return null;
    }
    #endregion

    #region Log
    public void AgregarLog(Log log)
    {
        Logs.Add(log);
    }

    public int LimpiarLog()
    {
        int total = Logs.Count;
        Logs.Clear();
        return total;
    }
    #endregion
}