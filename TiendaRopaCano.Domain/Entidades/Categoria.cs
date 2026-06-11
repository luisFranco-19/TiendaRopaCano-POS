using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa una categoría a la que pertenecen los productos de la tienda (e.g., Camisas, Pantalones, Calzado).
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la categoría.
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la descripción opcional de la categoría.
        /// </summary>
        public string? Descripcion { get; set; }
    }
}
