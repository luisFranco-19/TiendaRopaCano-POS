using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Datos.Contexto;
using TiendaRopaCano.Dominio.Entidades;
using Dapper;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Implementación de acceso a datos para la persistencia de categorías de productos utilizando Dapper y SQLite.
    /// </summary>
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ConfiguracionBaseDatos _db;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CategoriaRepository"/> con el configurador de base de datos.
        /// </summary>
        /// <param name="db">Configurador de la conexión a la base de datos.</param>
        public CategoriaRepository(ConfiguracionBaseDatos db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtiene todas las categorías registradas en la base de datos de forma asíncrona, ordenadas por nombre.
        /// </summary>
        /// <returns>Colección de objetos <see cref="Categoria"/>.</returns>
        public async Task<IEnumerable<Categoria>> ObtenerTodasAsync()
        {
            using var con = _db.GetConnection();
            return await con.QueryAsync<Categoria>(
                "SELECT * FROM Categorias ORDER BY Nombre"
            );
        }

        /// <summary>
        /// Inserta una nueva categoría en la base de datos de forma asíncrona y retorna su ID generado.
        /// </summary>
        /// <param name="categoria">Los datos de la nueva categoría.</param>
        /// <returns>El identificador único de la categoría registrada.</returns>
        public async Task<int> InsertarAsync(Categoria categoria)
        {
            using var con = _db.GetConnection();
            return await con.ExecuteScalarAsync<int>(
                @"INSERT INTO Categorias (Nombre, Descripcion)
                  VALUES (@Nombre, @Descripcion);
                  SELECT last_insert_rowid();",
                categoria
            );
        }

        /// <summary>
        /// Actualiza los datos de una categoría existente en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="categoria">El objeto categoría con los datos actualizados.</param>
        /// <returns><c>true</c> si se actualizó correctamente; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> ActualizarAsync(Categoria categoria)
        {
            using var con = _db.GetConnection();
            var filas = await con.ExecuteAsync(
                @"UPDATE Categorias SET
                  Nombre = @Nombre,
                  Descripcion = @Descripcion
                  WHERE CategoriaId = @CategoriaId",
                categoria
            );
            return filas > 0;
        }

        /// <summary>
        /// Elimina físicamente una categoría de la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="categoriaId">El identificador único de la categoría a eliminar.</param>
        /// <returns><c>true</c> si se eliminó; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> EliminarAsync(int categoriaId)
        {
            using var con = _db.GetConnection();
            var filas = await con.ExecuteAsync(
                "DELETE FROM Categorias WHERE CategoriaId = @CategoriaId",
                new { CategoriaId = categoriaId }
            );
            return filas > 0;
        }
    }
}
