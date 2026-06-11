using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa un rol o perfil de seguridad asignado a los usuarios del sistema (e.g., Administrador, Vendedor).
    /// </summary>
    public class Rol
    {
        /// <summary>
        /// Obtiene o establece el identificador único del rol.
        /// </summary>
        public int RolId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre descriptivo del rol de usuario.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;
    }
}

