using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProjectManagementPlatform.Nucleo.Seguridad;

namespace Nucleo.Tests.Seguridad;

[TestClass]
public class ServicioContrasenasTests
{
    [TestMethod]
    public void EsValida_DeberiaRetornarResultadoEsperado()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida(""));
        Assert.IsFalse(ServicioContrasenas.EsValida("12345678"));
        Assert.IsFalse(ServicioContrasenas.EsValida("Contr4"));
        Assert.IsFalse(ServicioContrasenas.EsValida("contr4seña"));
        Assert.IsFalse(ServicioContrasenas.EsValida("CONTRASEÑ@"));
        Assert.IsFalse(ServicioContrasenas.EsValida("contraseñ@"));
        Assert.IsFalse(ServicioContrasenas.EsValida("Contraseñ@"));
        Assert.IsTrue(ServicioContrasenas.EsValida("Contr4señ@"));
    }
}
