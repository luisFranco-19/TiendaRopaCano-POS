using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa un artículo o prenda de vestir disponible para la venta en la tienda.
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Obtiene o establece el identificador único del producto.
        /// </summary>
        public int ProductoId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la categoría a la que pertenece el producto.
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre o título del producto.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la descripción detallada o características del producto.
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Obtiene o establece el precio de venta al público general.
        /// </summary>
        public decimal Precio { get; set; }

        /// <summary>
        /// Obtiene o establece el precio de costo o de adquisición del producto.
        /// </summary>
        public decimal PrecioCompra { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad disponible de este producto en el almacén/inventario.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Obtiene o establece el stock mínimo permitido antes de generar una alerta de reabastecimiento. Por defecto es 5.
        /// </summary>
        public int StockMinimo { get; set; } = 5;

        /// <summary>
        /// Obtiene o establece un valor que indica si el producto está habilitado para la venta.
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Propiedad de navegación para acceder a los datos de la categoría asociada al producto.
        /// </summary>
        public Categoria? Categoria { get; set; }
    }
}