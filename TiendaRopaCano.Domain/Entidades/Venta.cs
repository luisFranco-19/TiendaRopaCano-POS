using System;
using System.Collections.Generic;
using System.Text;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa una transacción de venta realizada en la tienda, consolidando el total, la fecha y los detalles de los productos vendidos.
    /// </summary>
    public class Venta
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la venta.
        /// </summary>
        public int VentaId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del usuario que realizó la venta.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de la transacción de venta.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Obtiene o establece el importe total cobrado en la venta.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Propiedad de navegación para acceder a los datos del usuario/vendedor que registró la venta.
        /// </summary>
        public Usuario? Usuario { get; set; }

        /// <summary>
        /// Obtiene o establece el listado detallado de los productos incluidos en esta transacción de venta.
        /// </summary>
        public List<DetalleVenta> Detalles { get; set; } = new();
    }
}