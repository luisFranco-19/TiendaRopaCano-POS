using System;
using System.Collections.Generic;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Define los métodos para la generación de reportes y documentos en formato PDF (e.g. reportes de inventario, facturas, etc.).
    /// </summary>
    public interface IPdfService
    {
        /// <summary>
        /// Genera un documento PDF con el resumen financiero y reporte consolidado de ventas diarias en un rango de fechas.
        /// </summary>
        /// <param name="ventas">La lista de ventas agrupadas por día.</param>
        /// <param name="desde">Fecha inicial del rango del reporte.</param>
        /// <param name="hasta">Fecha final del rango del reporte.</param>
        /// <param name="totalVentas">Suma de las ventas totales del período.</param>
        /// <param name="totalUtilidad">Suma de la utilidad neta acumulada en el período.</param>
        /// <param name="totalTransacciones">Número total de transacciones realizadas.</param>
        /// <returns>Un arreglo de bytes que representa el archivo PDF generado.</returns>
        byte[] GenerarReporteVentas(IEnumerable<ReporteVentaDiaria> ventas, DateTime desde, DateTime hasta, decimal totalVentas, decimal totalUtilidad, int totalTransacciones);

        /// <summary>
        /// Genera un reporte PDF con la lista de productos cuyo stock actual es menor o igual al stock mínimo.
        /// </summary>
        /// <param name="productos">Colección de productos con bajo nivel de existencias.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        byte[] GenerarReporteInventarioBajoStock(IEnumerable<Producto> productos);

        /// <summary>
        /// Genera un reporte PDF con la lista de usuarios y empleados del sistema, indicando su estado y rol asignado.
        /// </summary>
        /// <param name="usuarios">Colección de usuarios a reportar.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        byte[] GenerarReporteUsuarios(IEnumerable<Usuario> usuarios);

        /// <summary>
        /// Genera un reporte PDF con el inventario completo, indicando stock, precios y costo del catálogo de productos.
        /// </summary>
        /// <param name="productos">Colección de todos los productos en catálogo.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        byte[] GenerarReporteInventarioCompleto(IEnumerable<Producto> productos);

        /// <summary>
        /// Genera un reporte PDF con el historial detallado de transacciones de ventas individuales en un período.
        /// </summary>
        /// <param name="ventas">Lista de ventas del período con sus respectivos usuarios y montos.</param>
        /// <param name="desde">Fecha inicial del período de búsqueda.</param>
        /// <param name="hasta">Fecha final del período de búsqueda.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        byte[] GenerarReporteHistorialVentas(IEnumerable<Venta> ventas, DateTime desde, DateTime hasta);

        /// <summary>
        /// Genera un comprobante fiscal o factura detallada de una venta específica en formato PDF.
        /// </summary>
        /// <param name="venta">El objeto venta que contiene el total, la fecha, el usuario y los detalles del producto.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        byte[] GenerarFacturaVenta(Venta venta);
    }
}
