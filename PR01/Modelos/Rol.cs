namespace Nucleo.Modelos
{
    public class Rol
    {
        public int Id_Rol { get; set; }
        public string NombreRol { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is Rol otro)
            {
                return this.Id_Rol == otro.Id_Rol;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id_Rol.GetHashCode();
        }

        public override string ToString()
        {
            return $"Rol [ID: {Id_Rol}, Nombre: {NombreRol}]";
        }

        public void QuitarTodosLosPermisos()
        {
        }

        public void ConcederTodosLosPermisos()
        {
        }
    }
}