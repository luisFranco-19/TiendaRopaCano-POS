using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Define los métodos de servicio para la recopilación de datos estadísticos y exportación de reportes en diferentes formatos (CSV, etc.).
    /// </summary>
    public interface IReporteService
    {
        /// <summary>
        /// Obtiene de forma asíncrona la lista consolidada de ventas diarias realizadas entre dos fechas especificadas.
        /// </summary>
        /// <param name="desde">Fecha de inicio del rango.</param>
        /// <param name="hasta">Fecha de fin del rango.</param>
        /// <returns>Una colección de objetos <see cref="ReporteVentaDiaria"/>.</returns>
        Task<IEnumerable<ReporteVentaDiaria>> ObtenerVentasDiariasAsync(DateTime desde, DateTime hasta);

        /// <summary>
        /// Obtiene de forma asíncrona la lista de productos que están en stock bajo (existencias por debajo o igual del límite mínimo).
        /// </summary>
        /// <returns>Una colección de productos con bajo stock.</returns>
        Task<IEnumerable<Producto>> ObtenerProductosBajoStockAsync();

        /// <summary>
        /// Exporta la lista de ventas consolidada en un arreglo de bytes con formato CSV.
        /// </summary>
        /// <param name="ventas">Colección de datos de ventas a exportar.</param>
        /// <returns>Arreglo de bytes que representa el archivo CSV generado.</returns>
        byte[] GenerarCsvVentas(IEnumerable<ReporteVentaDiaria> ventas);

        /// <summary>
        /// Exporta la lista de productos en inventario en un arreglo de bytes con formato CSV.
        /// </summary>
        /// <param name="productos">Colección de productos a exportar.</param>
        /// <returns>Arreglo de bytes que representa el archivo CSV generado.</returns>
        byte[] GenerarCsvInventario(IEnumerable<Producto> productos);
    }
}
