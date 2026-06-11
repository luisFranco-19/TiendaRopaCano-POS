using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Define los métodos de acceso a datos para la gestión de alertas de stock mínimo.
    /// </summary>
    public interface IAlertaRepository
    {
        /// <summary>
        /// Registra una nueva alerta de stock mínimo en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="alerta">El objeto alerta con la información técnica del stock mínimo y producto.</param>
        /// <returns>El identificador único asignado a la alerta recién creada.</returns>
        Task<int> InsertAsync(AlertaStock alerta);
    }
}
