using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Seguridad;

namespace Nucleo.Tests.Seguridad;

[TestClass]
public class ServicioContrasenasTests
{
    [TestMethod]
    public void ValidarPassword_OchoCasosDePrueba_DevuelveResultadoEsperado()
    {
        // ID 1: Valor Nulo -> F (false)
        Assert.IsFalse(ServicioContrasenas.EsValida(null!));

        // ID 2: Todo Mayusculas -> F (false)
        Assert.IsFalse(ServicioContrasenas.EsValida("P@SSW0RD"));

        // ID 3: Todo Numeros -> F (false)
        Assert.IsFalse(ServicioContrasenas.EsValida("12345678"));

        // ID 4: Menos de 8 caracteres -> F (false)
        Assert.IsFalse(ServicioContrasenas.EsValida("pasword"));

        // ID 5: Todo Minusculas -> F (false)
        Assert.IsFalse(ServicioContrasenas.EsValida("password"));

        // ID 6: No caracteres especiales -> F (false)
        Assert.IsFalse(ServicioContrasenas.EsValida("Password"));

        // ID 7: No hay numeros -> F (false)
        Assert.IsFalse(ServicioContrasenas.EsValida("P@ssword"));

        // ID 8: Contraseña correcta -> T (true)
        Assert.IsTrue(ServicioContrasenas.EsValida("P@ssw0rd"));
    }
}