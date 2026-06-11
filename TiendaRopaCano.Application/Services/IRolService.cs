using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Define los métodos de servicio para la consulta de roles y perfiles de usuario.
    /// </summary>
    public interface IRolService
    {
        /// <summary>
        /// Obtiene de forma asíncrona la lista completa de roles de usuario configurados en el sistema.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="Rol"/>.</returns>
        Task<IEnumerable<Rol>> ObtenerTodosAsync();
    }
}
