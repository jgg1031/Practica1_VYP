using System;

namespace Nucleo.Modelos
{
    public class Log
    {
        public int Id_Log { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Mensaje { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is Log otro)
            {
                return this.Id_Log == otro.Id_Log;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id_Log.GetHashCode();
        }

        public override string ToString()
        {
            return $"Log [ID: {Id_Log}, Fecha: {Fecha}, Mensaje: {Mensaje}]";
        }
    }
}