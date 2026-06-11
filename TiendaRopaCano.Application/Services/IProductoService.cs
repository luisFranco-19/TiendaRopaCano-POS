using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Define los métodos de servicio para la gestión y control de inventario de productos.
    /// </summary>
    public interface IProductoService
    {
        /// <summary>
        /// Obtiene de forma asíncrona todos los productos registrados en el inventario.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="Producto"/>.</returns>
        Task<IEnumerable<Producto>> ObtenerTodosAsync();

        /// <summary>
        /// Busca de forma asíncrona un producto específico por su identificador único.
        /// </summary>
        /// <param name="productoId">El identificador único del producto.</param>
        /// <returns>El objeto <see cref="Producto"/> correspondiente, o <c>null</c> si no se encuentra.</returns>
        Task<Producto?> ObtenerPorIdAsync(int productoId);

        /// <summary>
        /// Obtiene de forma asíncrona los productos pertenecientes a una categoría específica.
        /// </summary>
        /// <param name="categoriaId">Identificador de la categoría.</param>
        /// <returns>Una colección de productos filtrados por categoría.</returns>
        Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId);

        /// <summary>
        /// Obtiene de forma asíncrona todos los productos cuyo stock actual sea inferior o igual a su stock mínimo configurado.
        /// </summary>
        /// <returns>Una colección de productos que requieren reabastecimiento.</returns>
        Task<IEnumerable<Producto>> ObtenerStockBajoAsync();

        /// <summary>
        /// Registra un nuevo producto en el catálogo de inventario de forma asíncrona.
        /// </summary>
        /// <param name="producto">El objeto producto con los datos a insertar.</param>
        /// <returns>El identificador único del nuevo producto registrado.</returns>
        Task<int> InsertarAsync(Producto producto);

        /// <summary>
        /// Actualiza los datos de un producto existente en el sistema de forma asíncrona.
        /// </summary>
        /// <param name="producto">El objeto producto con la información modificada.</param>
        /// <returns><c>true</c> si se actualizó correctamente; de lo contrario, <c>false</c>.</returns>
        Task<bool> ActualizarAsync(Producto producto);

        /// <summary>
        /// Elimina un producto del catálogo del sistema por su identificador de forma asíncrona.
        /// </summary>
        /// <param name="productoId">Identificador único del producto a eliminar.</param>
        /// <returns><c>true</c> si se eliminó correctamente; de lo contrario, <c>false</c>.</returns>
        Task<bool> EliminarAsync(int productoId);

        /// <summary>
        /// Modifica o actualiza el stock actual de un producto de forma asíncrona (generalmente usado al registrar una compra o venta).
        /// </summary>
        /// <param name="productoId">Identificador único del producto.</param>
        /// <param name="cantidad">La cantidad a sumar (positiva) o restar (negativa) del inventario.</param>
        /// <returns><c>true</c> si el stock se actualizó de forma satisfactoria; de lo contrario, <c>false</c>.</returns>
        Task<bool> ActualizarStockAsync(int productoId, int cantidad);
    }
}
