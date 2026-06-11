using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Define los métodos de acceso a datos para la persistencia y consulta de categorías de productos.
    /// </summary>
    public interface ICategoriaRepository
    {
        /// <summary>
        /// Obtiene todas las categorías registradas en la base de datos de forma asíncrona.
        /// </summary>
        /// <returns>Colección de categorías.</returns>
        Task<IEnumerable<Categoria>> ObtenerTodasAsync();

        /// <summary>
        /// Inserta una nueva categoría en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="categoria">Objeto categoría con los datos a insertar.</param>
        /// <returns>Identificador único de la categoría recién creada.</returns>
        Task<int> InsertarAsync(Categoria categoria);

        /// <summary>
        /// Actualiza los datos de una categoría existente en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="categoria">Objeto categoría con los datos actualizados.</param>
        /// <returns><c>true</c> si se actualizó correctamente; de lo contrario, <c>false</c>.</returns>
        Task<bool> ActualizarAsync(Categoria categoria);

        /// <summary>
        /// Elimina físicamente una categoría de la base de datos por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="categoriaId">Identificador único de la categoría.</param>
        /// <returns><c>true</c> si se eliminó; de lo contrario, <c>false</c>.</returns>
        Task<bool> EliminarAsync(int categoriaId);
    }
}
