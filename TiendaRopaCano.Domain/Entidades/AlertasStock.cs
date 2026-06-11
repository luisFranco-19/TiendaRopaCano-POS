using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa una alerta de stock mínimo para un producto en el inventario.
    /// Se genera automáticamente cuando la cantidad de un producto es igual o menor a su límite mínimo.
    /// </summary>
    public class AlertaStock
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la alerta.
        /// </summary>
        public int AlertaId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del producto asociado a la alerta.
        /// </summary>
        public int ProductoId { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad actual de stock disponible del producto al momento de generarse la alerta.
        /// </summary>
        public int StockActual { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad mínima de stock configurada para el producto.
        /// </summary>
        public int StockMinimo { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se generó la alerta.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la alerta ya ha sido revisada o atendida por un usuario.
        /// </summary>
        public bool Revisada { get; set; } = false;

        /// <summary>
        /// Obtiene o establece el identificador del usuario que revisó la alerta.
        /// </summary>
        public int? RevisadaPor { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que la alerta fue revisada.
        /// </summary>
        public DateTime? FechaRevision { get; set; }

        /// <summary>
        /// Propiedad de navegación para acceder a los detalles del producto asociado.
        /// </summary>
        public Producto? Producto { get; set; }
    }
}