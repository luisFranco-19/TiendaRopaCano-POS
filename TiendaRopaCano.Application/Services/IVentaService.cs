using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Define los métodos de servicio para la orquestación y el registro de ventas, control de inventario asociado y facturación.
    /// </summary>
    public interface IVentaService
    {
        /// <summary>
        /// Obtiene de forma asíncrona la lista de todas las transacciones de ventas registradas en el sistema.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="Venta"/>.</returns>
        Task<IEnumerable<Venta>> ObtenerTodasAsync();

        /// <summary>
        /// Busca de forma asíncrona una venta específica por su identificador único, incluyendo sus detalles de venta asociados.
        /// </summary>
        /// <param name="ventaId">Identificador único de la venta.</param>
        /// <returns>El objeto <see cref="Venta"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
        Task<Venta?> ObtenerPorIdAsync(int ventaId);

        /// <summary>
        /// Obtiene de forma asíncrona el listado de ventas realizadas en un rango de fechas.
        /// </summary>
        /// <param name="desde">Fecha inicial de búsqueda.</param>
        /// <param name="hasta">Fecha final de búsqueda.</param>
        /// <returns>Una colección de ventas que coinciden con el rango de fechas.</returns>
        Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta);

        /// <summary>
        /// Registra una nueva venta de forma asíncrona, actualizando simultáneamente el stock de los productos involucrados en una transacción atómica.
        /// </summary>
        /// <param name="venta">El objeto venta que contiene el total, la fecha, el usuario y el detalle de productos.</param>
        /// <returns>El identificador único asignado a la nueva venta.</returns>
        Task<int> RegistrarVentaAsync(Venta venta);
    }
}
