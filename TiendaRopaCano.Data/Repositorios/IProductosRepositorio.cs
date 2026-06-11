using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Define los métodos de acceso a datos para la persistencia, consulta y control de inventario de productos.
    /// </summary>
    public interface IProductoRepository
    {
        /// <summary>
        /// Obtiene todos los productos registrados en la base de datos de forma asíncrona.
        /// </summary>
        /// <returns>Colección de productos.</returns>
        Task<IEnumerable<Producto>> ObtenerTodosAsync();

        /// <summary>
        /// Busca un producto específico por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="productoId">El identificador único del producto.</param>
        /// <returns>El objeto <see cref="Producto"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
        Task<Producto?> ObtenerPorIdAsync(int productoId);

        /// <summary>
        /// Obtiene todos los productos pertenecientes a una categoría específica de forma asíncrona.
        /// </summary>
        /// <param name="categoriaId">Identificador de la categoría.</param>
        /// <returns>Colección de productos filtrados.</returns>
        Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId);

        /// <summary>
        /// Obtiene de forma asíncrona la lista de productos cuyo stock está por debajo o igual al stock mínimo.
        /// </summary>
        /// <returns>Colección de productos en stock bajo.</returns>
        Task<IEnumerable<Producto>> ObtenerStockBajoAsync();

        /// <summary>
        /// Registra un nuevo producto en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="producto">El objeto producto con los datos a insertar.</param>
        /// <returns>El identificador único del producto insertado.</returns>
        Task<int> InsertarAsync(Producto producto);

        /// <summary>
        /// Actualiza los datos de un producto existente en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="producto">El objeto producto con los datos modificados.</param>
        /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
        Task<bool> ActualizarAsync(Producto producto);

        /// <summary>
        /// Elimina de forma lógica o física un producto de la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="productoId">Identificador único del producto a eliminar.</param>
        /// <returns><c>true</c> si se eliminó; de lo contrario, <c>false</c>.</returns>
        Task<bool> EliminarAsync(int productoId);

        /// <summary>
        /// Incrementa o decrementa de forma asíncrona las existencias (stock) de un producto en la base de datos.
        /// </summary>
        /// <param name="productoId">Identificador único del producto.</param>
        /// <param name="cantidad">Cantidad a sumar (positiva) o restar (negativa).</param>
        /// <returns><c>true</c> si el stock se actualizó de forma correcta; de lo contrario, <c>false</c>.</returns>
        Task<bool> ActualizarStockAsync(int productoId, int cantidad);
    }
}
