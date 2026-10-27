using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nucleo.Modelos;
using www.Utilidades;

namespace www.Pages.Log;

public class IndexModel : PageModel
{
    private readonly IDatos _datos;

    public IndexModel(IDatos datos)
    {
        _datos = datos;
    }

    public string NombreUsuario { get; private set; } = "";

    // Usamos la clase auxiliar definida al final del archivo
    public List<EntradaLog> Eventos { get; private set; } = [];

    public List<string> Niveles { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public string TextoBusqueda { get; set; } = "";

    [BindProperty(SupportsGet = true)]
    public string NivelSeleccionado { get; set; } = "";

    public IActionResult OnGet()
    {
        int? idUsuario = HttpContext.Session.GetInt32(ClavesSesion.UsuarioId);

        if (idUsuario is null)
        {
            return RedirectToPage("/Login/Index");
        }

        var usuario = _datos.LeerUsuario(idUsuario.Value);

        if (usuario is null)
        {
            HttpContext.Session.Remove(ClavesSesion.UsuarioId);
            return RedirectToPage("/Login/Index");
        }

        // CORRECCIÓN 1: 'Email' con 'm' minúscula
        NombreUsuario = usuario.Email;

        // CORRECCIÓN 2: Adaptamos tu modelo Log a lo que espera la vista
        var eventos = _datos.Logs
            .Select(log => new EntradaLog(
                log.Fecha,      // En tu modelo Log se llama Fecha
                "Info",         // Valor por defecto para Nivel
                log.Mensaje,
                usuario.Nombre  // Usamos el nombre del usuario actual
            ))
            .ToList();

        Niveles = eventos
            .Select(evento => evento.Nivel)
            .Where(nivel => !string.IsNullOrWhiteSpace(nivel))
            .Distinct()
            .OrderBy(nivel => nivel)
            .ToList();

        if (!string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            eventos = eventos
                .Where(evento =>
                    evento.Mensaje.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    evento.NombreUsuario.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(NivelSeleccionado))
        {
            eventos = eventos
                .Where(evento => string.Equals(evento.Nivel, NivelSeleccionado, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        Eventos = eventos.OrderByDescending(evento => evento.FechaHora).ToList();

        return Page();
    }
}

/// <summary>
/// Clase auxiliar (ViewModel) para adaptar los datos a la vista web de Log.
/// </summary>
public class EntradaLog
{
    public DateTime FechaHora { get; set; }
    public string Nivel { get; set; }
    public string Mensaje { get; set; }
    public string NombreUsuario { get; set; }

    public EntradaLog(DateTime fechaHora, string nivel, string mensaje, string nombreUsuario)
    {
        FechaHora = fechaHora;
        Nivel = nivel;
        Mensaje = mensaje;
        NombreUsuario = nombreUsuario;
    }
}