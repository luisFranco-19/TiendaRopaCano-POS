using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Define los métodos de acceso a datos para la consulta y obtención de roles de seguridad de usuarios.
    /// </summary>
    public interface IRolRepository
    {
        /// <summary>
        /// Obtiene de forma asíncrona la lista de todos los roles configurados en la base de datos.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="Rol"/>.</returns>
        Task<IEnumerable<Rol>> ObtenerTodosAsync();
    }
}
