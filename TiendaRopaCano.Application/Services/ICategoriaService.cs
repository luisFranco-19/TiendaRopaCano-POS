using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Define los métodos de servicio para la gestión y lógica de negocio relacionada con las categorías de productos.
    /// </summary>
    public interface ICategoriaService
    {
        /// <summary>
        /// Obtiene de forma asíncrona todas las categorías registradas en el sistema.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="Categoria"/>.</returns>
        Task<IEnumerable<Categoria>> ObtenerTodasAsync();

        /// <summary>
        /// Registra una nueva categoría en el sistema de forma asíncrona.
        /// </summary>
        /// <param name="categoria">El objeto categoría que se desea insertar.</param>
        /// <returns>El identificador único asignado a la nueva categoría.</returns>
        Task<int> InsertarAsync(Categoria categoria);

        /// <summary>
        /// Actualiza los datos de una categoría existente en el sistema de forma asíncrona.
        /// </summary>
        /// <param name="categoria">El objeto categoría con los datos actualizados.</param>
        /// <returns><c>true</c> si se actualizó correctamente; de lo contrario, <c>false</c>.</returns>
        Task<bool> ActualizarAsync(Categoria categoria);

        /// <summary>
        /// Elimina una categoría del sistema por su identificador de forma asíncrona.
        /// </summary>
        /// <param name="categoriaId">El identificador único de la categoría a eliminar.</param>
        /// <returns><c>true</c> si se eliminó correctamente; de lo contrario, <c>false</c>.</returns>
        Task<bool> EliminarAsync(int categoriaId);
    }
}
