using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Define los métodos de acceso a datos para la persistencia, consulta y registro de transacciones de ventas y sus líneas de detalle.
    /// </summary>
    public interface IVentaRepository
    {
        /// <summary>
        /// Obtiene todas las ventas registradas de forma asíncrona.
        /// </summary>
        /// <returns>Colección de ventas.</returns>
        Task<IEnumerable<Venta>> ObtenerTodasAsync();

        /// <summary>
        /// Busca de forma asíncrona una venta por su identificador único, cargando el usuario/vendedor y los detalles de venta asociados.
        /// </summary>
        /// <param name="ventaId">Identificador único de la venta.</param>
        /// <returns>El objeto <see cref="Venta"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
        Task<Venta?> ObtenerPorIdAsync(int ventaId);

        /// <summary>
        /// Obtiene de forma asíncrona la lista de ventas ocurridas en un rango de fechas.
        /// </summary>
        /// <param name="desde">Fecha inicial del rango.</param>
        /// <param name="hasta">Fecha final del rango.</param>
        /// <returns>Colección de ventas.</returns>
        Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta);

        /// <summary>
        /// Inserta una nueva venta y sus líneas de detalle en la base de datos de forma asíncrona, de forma transaccional.
        /// </summary>
        /// <param name="venta">El objeto venta conteniendo la fecha, total, vendedor y los detalles de producto.</param>
        /// <returns>El identificador único asignado a la venta registrada.</returns>
        Task<int> InsertarAsync(Venta venta);
    }
}
