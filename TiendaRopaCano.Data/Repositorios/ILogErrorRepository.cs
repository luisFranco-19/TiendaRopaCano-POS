using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Define los métodos de acceso a datos para almacenar los registros de logs y excepciones del sistema.
    /// </summary>
    public interface ILogErrorRepository
    {
        /// <summary>
        /// Registra un nuevo error o excepción en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="log">El objeto conteniendo el módulo, mensaje de error y pila de llamadas.</param>
        /// <returns>El identificador único asignado al registro de error.</returns>
        Task<int> InsertAsync(LogError log);
    }
}
