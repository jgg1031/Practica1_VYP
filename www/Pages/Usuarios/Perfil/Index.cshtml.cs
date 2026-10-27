using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nucleo.Modelos;
using www.Utilidades;
using UsuarioModelo = Nucleo.Modelos.Usuario;

namespace www.Pages.Usuarios.Perfil;

public class IndexModel : PageModel
{
    private readonly IDatos _datos;

    public IndexModel(IDatos datos)
    {
        _datos = datos;
    }

    public string NombreUsuarioCabecera { get; private set; } = string.Empty;

    public string NombreProyectoActual { get; private set; } = "Sin proyecto seleccionado";

    public int IdUsuario { get; private set; }

    public bool SinPassword { get; private set; }

    public bool CuentaActiva { get; private set; }

    public int Rol { get; private set; }

    public int IntentosErroneos { get; private set; }

    public int NumeroPasswordsAnteriores { get; private set; }

    public string FechaUltimoAccesoTexto { get; private set; } = "No consta";

    public string FechaAccesoActualTexto { get; private set; } = "No consta";

    public string UltimoCambioPasswordTexto { get; private set; } = "No consta";

    public string BloquearAccesoHastaTexto { get; private set; } = "No consta";

    public string CaducidadCuentaTexto { get; private set; } = "No consta";

    [BindProperty]
    [Required(ErrorMessage = "Debe indicar el correo electrónico.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    [Display(Name = "Correo electrónico")]
    public string EMail { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Debe indicar el nombre.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BindProperty]
    [StringLength(150, ErrorMessage = "Los apellidos no pueden superar 150 caracteres.")]
    [Display(Name = "Apellidos")]
    public string Apellidos { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Debe indicar la contraseña actual.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña actual")]
    public string PasswordActual { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Debe indicar la nueva contraseña.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [Display(Name = "Nueva contraseña")]
    public string PasswordNueva { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Debe repetir la nueva contraseña.")]
    [DataType(DataType.Password)]
    [Compare(nameof(PasswordNueva), ErrorMessage = "Las nuevas contraseñas no coinciden.")]
    [Display(Name = "Repetir nueva contraseña")]
    public string ConfirmacionPassword { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        UsuarioModelo? usuario = ObtenerUsuarioActual();

        if (usuario is null)
        {
            LimpiarSesion();
            return RedirectToPage("/Login/Index");
        }

        CargarCabecera(usuario);
        CargarDatosEditables(usuario);
        CargarDatosCuenta(usuario);

        return Page();
    }

    public IActionResult OnPostGuardarPerfil()
    {
        UsuarioModelo? usuario = ObtenerUsuarioActual();

        if (usuario is null)
        {
            LimpiarSesion();
            return RedirectToPage("/Login/Index");
        }

        CargarCabecera(usuario);
        CargarDatosCuenta(usuario);
        QuitarValidacionPassword();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        string emailNormalizado = EMail.Trim();

        UsuarioModelo? usuarioConMismoEmail = _datos.LeerUsuario(emailNormalizado);

        if (usuarioConMismoEmail is not null && usuarioConMismoEmail.IdUsuario != usuario.IdUsuario)
        {
            ModelState.AddModelError(
                nameof(EMail),
                "Ya existe otro usuario con ese correo electrónico.");

            return Page();
        }

        usuario.Email = emailNormalizado;
        usuario.Nombre = Nombre.Trim();

        _datos.GuardarUsuario(usuario);

        TempData["MensajePerfil"] = "Los datos del perfil se han actualizado.";

        return RedirectToPage();
    }

    public IActionResult OnPostCambiarPassword()
    {
        UsuarioModelo? usuario = ObtenerUsuarioActual();

        if (usuario is null)
        {
            LimpiarSesion();
            return RedirectToPage("/Login/Index");
        }

        CargarCabecera(usuario);
        CargarDatosEditables(usuario);
        CargarDatosCuenta(usuario);
        QuitarValidacionPerfil();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            usuario.CambiarContrasena(PasswordActual, PasswordNueva);
            _datos.GuardarUsuario(usuario);

            TempData["MensajePassword"] = "La contraseña se ha cambiado correctamente.";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(nameof(PasswordNueva), ex.Message);
            return Page();
        }
    }

    private UsuarioModelo? ObtenerUsuarioActual()
    {
        int? idUsuario = HttpContext.Session.GetInt32(ClavesSesion.UsuarioId);

        if (idUsuario is not int id)
        {
            return null;
        }

        return _datos.LeerUsuario(id);
    }

    private void CargarDatosEditables(UsuarioModelo usuario)
    {
        EMail = usuario.Email;
        Nombre = usuario.Nombre;
        Apellidos = string.Empty;
    }

    private void CargarDatosCuenta(UsuarioModelo usuario)
    {
        IdUsuario = usuario.IdUsuario;
        SinPassword = !string.IsNullOrEmpty(usuario.ContrasenaHash);
        CuentaActiva = !usuario.EstaBloqueada;
        Rol = 0;
        IntentosErroneos = usuario.IntentosFallidos;
        NumeroPasswordsAnteriores = 0;

        FechaUltimoAccesoTexto = "No consta";
        FechaAccesoActualTexto = "No consta";
        UltimoCambioPasswordTexto = "No consta";
        BloquearAccesoHastaTexto = "No consta";
        CaducidadCuentaTexto = "No consta";
    }

    private void CargarCabecera(UsuarioModelo usuario)
    {
        NombreUsuarioCabecera = usuario.Nombre.Trim();

        int? idProyecto = HttpContext.Session.GetInt32(ClavesSesion.ProyectoActualId);

        if (idProyecto is not int id)
        {
            NombreProyectoActual = "Sin proyecto seleccionado";
            return;
        }

        // Control de tipo anulable
        Proyecto? proyecto = _datos.LeerProyecto(id);

        if (proyecto is null)
        {
            HttpContext.Session.Remove(ClavesSesion.ProyectoActualId);
            NombreProyectoActual = "Sin proyecto seleccionado";
            return;
        }

        NombreProyectoActual = proyecto.Nombre;
    }

    // Método para obtener todos los proyectos desde el Diccionario de IDatos
    public List<Proyecto> ObtenerTodosLosProyectos()
    {
        return _datos.Proyectos.Values.ToList();
    }

    private void QuitarValidacionPassword()
    {
        ModelState.Remove(nameof(PasswordActual));
        ModelState.Remove(nameof(PasswordNueva));
        ModelState.Remove(nameof(ConfirmacionPassword));
    }

    private void QuitarValidacionPerfil()
    {
        ModelState.Remove(nameof(EMail));
        ModelState.Remove(nameof(Nombre));
        ModelState.Remove(nameof(Apellidos));
    }

    private void LimpiarSesion()
    {
        HttpContext.Session.Remove(ClavesSesion.EmailPendiente);
        HttpContext.Session.Remove(ClavesSesion.UsuarioId);
        HttpContext.Session.Remove(ClavesSesion.ProyectoActualId);
    }
}