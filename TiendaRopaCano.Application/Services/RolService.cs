using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Implementación de los servicios de consulta para la gestión de roles.
    /// </summary>
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepo;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RolService"/> con su respectivo repositorio inyectado.
        /// </summary>
        /// <param name="rolRepo">El repositorio de acceso a datos de roles.</param>
        public RolService(IRolRepository rolRepo)
        {
            _rolRepo = rolRepo;
        }

        /// <summary>
        /// Obtiene de forma asíncrona todos los roles registrados en la base de datos.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="Rol"/>.</returns>
        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            return await _rolRepo.ObtenerTodosAsync();
        }
    }
}
