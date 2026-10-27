using Nucleo.Seguridad;
using System;

namespace Nucleo.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Propiedades para el manejo del hash y la sal guardadas por separado
        public string ContrasenaHash { get; set; } = string.Empty;
        public string ContrasenaSal { get; set; } = string.Empty;

        // Propiedades de control para el bloqueo de seguridad (3 intentos fallidos)
        public int IntentosFallidos { get; set; } = 0;
        public bool EstaBloqueada { get; set; } = false;

        public override bool Equals(object? obj)
        {
            if (obj is Usuario otro)
            {
                return this.IdUsuario == otro.IdUsuario;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return IdUsuario.GetHashCode();
        }

        public override string ToString()
        {
            return $"Usuario [ID: {IdUsuario}, Nombre: {Nombre}, Email: {Email}, Bloqueada: {EstaBloqueada}]";
        }

        /// <summary>
        /// Comprueba si la contraseña introducida coincide con la guardada utilizando el servicio de hash y gestiona el bloqueo de seguridad.
        /// </summary>
        public bool CompruebaPassword(string passwordIngresada)
        {
            // Si la cuenta está bloqueada o la contraseña está vacía, falla directamente
            if (string.IsNullOrEmpty(passwordIngresada) || EstaBloqueada)
            {
                GestionarFallos();
                return false;
            }

            // Comprobamos la contraseña usando el método Verificar de ServicioHash pasándole el hash y la sal almacenados
            bool esValido = ServicioHash.Verificar(passwordIngresada, this.ContrasenaHash, this.ContrasenaSal);

            if (esValido)
            {
                // Si acierta, reiniciamos el contador de intentos fallidos a 0
                this.IntentosFallidos = 0;
                return true;
            }
            else
            {
                // Si no coincide, incrementa fallos y bloquea si llega a 3
                GestionarFallos();
                return false;
            }
        }

        /// <summary>
        /// Incrementa el contador de fallos y bloquea la cuenta si llega o supera los 3 intentos.
        /// </summary>
        private void GestionarFallos()
        {
            this.IntentosFallidos++;
            if (this.IntentosFallidos >= 3)
            {
                this.EstaBloqueada = true;
            }
        }

        /// <summary>
        /// Cambia la contraseña comprobando primero que la antigua coincide y validando la complejidad de la nueva.
        /// </summary>
        public void CambiarContrasena(string passwordAntigua, string passwordNueva)
        {
            // 1. Validar que la contraseña nueva no sea nula y cumpla la política de seguridad
            if (string.IsNullOrEmpty(passwordNueva))
            {
                throw new ArgumentException("La nueva contraseña no puede ser nula o vacía.");
            }

            if (!ServicioContrasenas.EsValida(passwordNueva))
            {
                throw new ArgumentException("La nueva contraseña no cumple con los requisitos de complejidad de seguridad.");
            }

            // 2. Comprobar que la contraseña antigua coincide con la actual
            bool coincideAntigua = CompruebaPassword(passwordAntigua);
            if (!coincideAntigua)
            {
                throw new InvalidOperationException("La contraseña antigua no coincide.");
            }

            // 3. Si coincide, utilizamos ServicioHash.Generar() para obtener el nuevo hash y sal por separado
            ResultadoHash nuevoResultado = ServicioHash.Generar(passwordNueva);
            this.ContrasenaHash = nuevoResultado.Hash;
            this.ContrasenaSal = nuevoResultado.Sal;
        }
    }
}