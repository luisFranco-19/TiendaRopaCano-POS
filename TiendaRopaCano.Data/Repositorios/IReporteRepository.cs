using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Define los métodos de acceso a datos para la generación de estadísticas y reportes agregados.
    /// </summary>
    public interface IReporteRepository
    {
        /// <summary>
        /// Obtiene de la base de datos las ventas diarias consolidadas en un período determinado de forma asíncrona.
        /// </summary>
        /// <param name="desde">Fecha inicial.</param>
        /// <param name="hasta">Fecha final.</param>
        /// <returns>Colección de objetos <see cref="ReporteVentaDiaria"/>.</returns>
        Task<IEnumerable<ReporteVentaDiaria>> ObtenerVentasDiariasAsync(DateTime desde, DateTime hasta);

        /// <summary>
        /// Obtiene de la base de datos la lista de productos que están en bajo stock de forma asíncrona.
        /// </summary>
        /// <returns>Colección de productos con existencias por debajo o igual del límite mínimo.</returns>
        Task<IEnumerable<Producto>> ObtenerProductosBajoStockAsync();
    }
}
