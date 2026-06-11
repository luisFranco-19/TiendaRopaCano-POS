using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Datos.Contexto;
using TiendaRopaCano.Dominio.Entidades;
using Dapper;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Implementación de acceso a datos para la obtención de roles de seguridad utilizando Dapper y SQLite.
    /// </summary>
    public class RolRepository : IRolRepository
    {
        private readonly ConfiguracionBaseDatos _db;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RolRepository"/> con el configurador de base de datos.
        /// </summary>
        /// <param name="db">Configurador de la conexión a la base de datos.</param>
        public RolRepository(ConfiguracionBaseDatos db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtiene todos los roles de usuario registrados en la base de datos de forma asíncrona, ordenados por nombre.
        /// </summary>
        /// <returns>Colección de objetos <see cref="Rol"/>.</returns>
        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            using var con = _db.GetConnection();
            return await con.QueryAsync<Rol>("SELECT * FROM Roles ORDER BY Nombre");
        }
    }
}
