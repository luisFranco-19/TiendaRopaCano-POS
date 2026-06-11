using System;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Presentacion.Auxiliares
{
    /// <summary>
    /// Maneja la sesión del usuario autenticado en la aplicación.
    /// </summary>
    public static class GestorSesion
    {
        /// <summary>
        /// Obtiene o establece el usuario actualmente autenticado en la sesión de la aplicación.
        /// </summary>
        public static Usuario? UsuarioActual { get; set; }

        /// <summary>
        /// Obtiene un valor booleano que indica si el usuario actual posee permisos de Administrador.
        /// </summary>
        public static bool EsAdministrador =>
            UsuarioActual?.Rol?.Nombre?.Equals("Administrador", StringComparison.OrdinalIgnoreCase) == true
            || UsuarioActual?.RolId == 1;

        /// <summary>
        /// Borra la información de sesión actualizando el usuario a nulo para cerrar la sesión.
        /// </summary>
        public static void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
