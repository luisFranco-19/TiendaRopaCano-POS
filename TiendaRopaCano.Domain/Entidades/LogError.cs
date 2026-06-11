using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa un registro de error o excepción que ocurre en el sistema para facilitar la depuración y auditoría.
    /// </summary>
    public class LogError
    {
        /// <summary>
        /// Obtiene o establece el identificador único del registro de error.
        /// </summary>
        public int LogId { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que ocurrió el error.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del usuario que estaba usando el sistema al momento de ocurrir el error (opcional).
        /// </summary>
        public int? UsuarioId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del módulo o clase donde ocurrió la excepción (e.g., "UsuarioService").
        /// </summary>
        public string Modulo { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la acción, método o función donde se produjo la falla.
        /// </summary>
        public string Accion { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el mensaje descriptivo del error (ex.Message).
        /// </summary>
        public string MensajeError { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el detalle técnico de la excepción (e.g., stack trace).
        /// </summary>
        public string? DetalleError { get; set; }

        /// <summary>
        /// Obtiene o establece el nivel de severidad del registro (e.g., "ERROR", "WARNING", "INFO"). Por defecto es "ERROR".
        /// </summary>
        public string Nivel { get; set; } = "ERROR";
    }
}