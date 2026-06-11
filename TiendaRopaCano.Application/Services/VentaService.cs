using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Dominio.Entidades;
using Dapper;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Implementación de los servicios para el control, consulta y registro de transacciones de ventas.
    /// </summary>
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly ILogErrorRepository _logRepo;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="VentaService"/> con los repositorios requeridos.
        /// </summary>
        /// <param name="ventaRepo">El repositorio para acceder a datos de ventas.</param>
        /// <param name="productoRepo">El repositorio para acceder a datos de productos.</param>
        /// <param name="logRepo">El repositorio para registrar logs de error.</param>
        public VentaService(IVentaRepository ventaRepo, IProductoRepository productoRepo, ILogErrorRepository logRepo)
        {
            _ventaRepo = ventaRepo;
            _productoRepo = productoRepo;
            _logRepo = logRepo;
        }

        /// <summary>
        /// Obtiene todas las ventas registradas en el sistema de forma asíncrona.
        /// </summary>
        /// <returns>Colección de ventas.</returns>
        public async Task<IEnumerable<Venta>> ObtenerTodasAsync()
        {
            try
            {
                return await _ventaRepo.ObtenerTodasAsync();
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("VentaService", "ObtenerTodasAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Busca una venta específica por su identificador único de forma asíncrona, incluyendo sus líneas de detalle.
        /// </summary>
        /// <param name="ventaId">ID de la venta a consultar.</param>
        /// <returns>La venta encontrada, o <c>null</c> si no existe.</returns>
        public async Task<Venta?> ObtenerPorIdAsync(int ventaId)
        {
            try
            {
                return await _ventaRepo.ObtenerPorIdAsync(ventaId);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("VentaService", "ObtenerPorIdAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de ventas realizadas dentro de un período de fechas de forma asíncrona.
        /// </summary>
        /// <param name="desde">Fecha inicial del rango.</param>
        /// <param name="hasta">Fecha final del rango.</param>
        /// <returns>Colección de ventas en el rango establecido.</returns>
        public async Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta)
        {
            try
            {
                return await _ventaRepo.ObtenerPorFechaAsync(desde, hasta);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("VentaService", "ObtenerPorFechaAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Registra una nueva venta de forma asíncrona. 
        /// Verifica que haya suficiente stock disponible de cada producto antes de confirmar la venta.
        /// </summary>
        /// <param name="venta">El objeto venta que contiene el total, la fecha, el usuario y los detalles.</param>
        /// <returns>El identificador único de la venta insertada.</returns>
        /// <exception cref="Exception">Arroja una excepción si algún producto no existe o el stock es insuficiente.</exception>
        public async Task<int> RegistrarVentaAsync(Venta venta)
        {
            try
            {
                // verificar stock suficiente
                foreach (var detalle in venta.Detalles)
                {
                    var producto = await _productoRepo.ObtenerPorIdAsync(detalle.ProductoId);
                    if (producto == null) throw new Exception($"Producto {detalle.ProductoId} no existe");
                    if (producto.Stock < detalle.Cantidad) throw new Exception($"Stock insuficiente para producto {producto.Nombre}");
                }

                // si todo ok, insertar venta
                var id = await _ventaRepo.InsertarAsync(venta);
                return id;
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("VentaService", "RegistrarVentaAsync", ex);
                throw;
            }
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
            }
        }
    }
}
