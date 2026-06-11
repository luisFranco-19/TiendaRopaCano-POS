using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa un usuario o empleado registrado en el sistema que posee credenciales de acceso y un rol específico.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de usuario utilizado para iniciar sesión.
        /// </summary>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la contraseña del usuario (generalmente almacenada como un hash seguro).
        /// </summary>
        public string Contrasena { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el identificador del rol asociado al usuario.
        /// </summary>
        public int RolId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cuenta del usuario está activa o deshabilitada.
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Propiedad de navegación para acceder a los detalles del rol asignado.
        /// </summary>
        public Rol? Rol { get; set; }
    }
}
