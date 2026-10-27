using System;

namespace Nucleo.Modelos;

public class Proyecto
{
    // Propiedad principal utilizada en las vistas y modelos
    public int IdProyecto { get; set; }

    // Alias de compatibilidad con Id_Proyecto
    public int Id_Proyecto
    {
        get => IdProyecto;
        set => IdProyecto = value;
    }

    // Propiedad principal utilizada en las vistas y modelos
    public string Nombre { get; set; } = string.Empty;

    // Alias de compatibilidad con NombreProyecto
    public string NombreProyecto
    {
        get => Nombre;
        set => Nombre = value;
    }

    public string Descripcion { get; set; } = string.Empty;

    // Propiedades requeridas por Index.cshtml e Index.cshtml.cs
    public string Siglas { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public bool Activo { get; set; } = true;

    public override bool Equals(object? obj)
    {
        if (obj is Proyecto otro)
        {
            return this.IdProyecto == otro.IdProyecto;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return IdProyecto.GetHashCode();
    }

    public override string ToString()
    {
        return $"Proyecto [ID: {IdProyecto}, Nombre: {Nombre}]";
    }
}