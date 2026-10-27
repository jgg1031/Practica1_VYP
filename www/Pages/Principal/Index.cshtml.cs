using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nucleo.Modelos;
using www.Utilidades;

namespace www.Pages.Principal;

public class IndexModel : PageModel
{
    private readonly IDatos _datos;

    public IndexModel(IDatos datos)
    {
        _datos = datos;
    }

    public Usuario? UsuarioActual { get; set; }

    public IActionResult OnGet()
    {
        int? usuarioId = HttpContext.Session.GetInt32(ClavesSesion.UsuarioId);

        if (usuarioId is null)
        {
            return RedirectToPage("/Login/Index");
        }

        UsuarioActual = _datos.LeerUsuario(usuarioId.Value);

        if (UsuarioActual is null)
        {
            return RedirectToPage("/Login/Index");
        }

        // Especificar el espacio de nombres completo evita conflictos
        Nucleo.Modelos.Log entrada = new Nucleo.Modelos.Log
        {
            Id_Log = 1,
            Fecha = DateTime.Now,
            Mensaje = $"El usuario {UsuarioActual.Email} ha accedido a la pantalla principal."
        };

        _datos.AgregarLog(entrada);

        return Page();
    }
}