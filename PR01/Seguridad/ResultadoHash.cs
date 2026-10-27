using System;

namespace Nucleo.Seguridad;

public class ResultadoHash
{
    public string Hash { get; init; } = string.Empty;
    public string Sal { get; init; } = string.Empty;
    public DateTime Creacion { get; init; } = DateTime.Now;
}