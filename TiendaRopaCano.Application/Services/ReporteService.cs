using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Implementación de los servicios para la obtención de estadísticas financieras, inventarios y exportación de reportes a CSV.
    /// </summary>
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepo;
        private readonly ILogErrorRepository _logRepo;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReporteService"/> con los repositorios requeridos.
        /// </summary>
        /// <param name="reporteRepo">El repositorio para acceder a estadísticas y reportes.</param>
        /// <param name="logRepo">El repositorio para registrar logs de error.</param>
        public ReporteService(IReporteRepository reporteRepo, ILogErrorRepository logRepo)
        {
            _reporteRepo = reporteRepo;
            _logRepo = logRepo;
        }

        /// <summary>
        /// Obtiene de forma asíncrona la lista de ventas diarias consolidadas en un período determinado.
        /// </summary>
        /// <param name="desde">Fecha inicial de búsqueda.</param>
        /// <param name="hasta">Fecha final de búsqueda.</param>
        /// <returns>Colección de objetos <see cref="ReporteVentaDiaria"/>.</returns>
        public async Task<IEnumerable<ReporteVentaDiaria>> ObtenerVentasDiariasAsync(DateTime desde, DateTime hasta)
        {
            try
            {
                return await _reporteRepo.ObtenerVentasDiariasAsync(desde, hasta);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ReporteService", "ObtenerVentasDiariasAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene de forma asíncrona los productos con bajo nivel de existencias (stock menor o igual al mínimo).
        /// </summary>
        /// <returns>Colección de productos con bajo stock.</returns>
        public async Task<IEnumerable<Producto>> ObtenerProductosBajoStockAsync()
        {
            try
            {
                return await _reporteRepo.ObtenerProductosBajoStockAsync();
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ReporteService", "ObtenerProductosBajoStockAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Genera el contenido de un archivo CSV conteniendo el reporte diario de ventas.
        /// </summary>
        /// <param name="ventas">La colección de datos diarios de ventas.</param>
        /// <returns>Arreglo de bytes en formato UTF-8 listo para escribirse en disco.</returns>
        public byte[] GenerarCsvVentas(IEnumerable<ReporteVentaDiaria> ventas)
        {
            var sb = new System.Text.StringBuilder();
            // Header
            sb.AppendLine("Fecha,Cantidad Ventas,Total Ventas,Utilidad");

            foreach (var venta in ventas)
            {
                sb.AppendLine($"{venta.Fecha:yyyy-MM-dd},{venta.CantidadVentas},{venta.TotalVentas.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)},{venta.Utilidad.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}");
            }

            return System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Genera el contenido de un archivo CSV conteniendo la lista de productos del inventario.
        /// </summary>
        /// <param name="productos">La colección de productos.</param>
        /// <returns>Arreglo de bytes en formato UTF-8 listo para escribirse en disco.</returns>
        public byte[] GenerarCsvInventario(IEnumerable<Producto> productos)
        {
            var sb = new System.Text.StringBuilder();
            // Header
            sb.AppendLine("ID,Nombre,Categoría,Precio,Stock,Stock Mínimo,Estado");

            foreach (var p in productos)
            {
                var estado = p.Activo ? "Activo" : "Inactivo";
                sb.AppendLine($"{p.ProductoId},{p.Nombre},{p.Categoria?.Nombre ?? "Sin Categoría"},{p.Precio.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)},{p.Stock},{p.StockMinimo},{estado}");
            }

            return System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Registra un log de error de forma asíncrona y segura (sin propagar excepciones del log).
        /// </summary>
        private async Task RegistrarLogAsync(string modulo, string accion, Exception ex)
        {
            try
            {
                var log = new LogError
                {
                    Fecha = DateTime.UtcNow,
                    Modulo = modulo,
                    Accion = accion,
                    MensajeError = ex.Message,
                    DetalleError = ex.ToString()
                };

                await _logRepo.InsertAsync(log);
            }
            catch
            {
                // Ignorar error de log para evitar ciclos
            }
        }
    }
}
