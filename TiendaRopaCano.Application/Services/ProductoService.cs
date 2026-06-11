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
    /// Implementación de los servicios para la lógica de negocio del inventario de productos y generación de alertas de bajo stock.
    /// </summary>
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepo;
        private readonly ILogErrorRepository _logRepo;
        private readonly IAlertaRepository _alertaRepo;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ProductoService"/> con los repositorios requeridos.
        /// </summary>
        /// <param name="productoRepo">El repositorio de acceso a datos de productos.</param>
        /// <param name="logRepo">El repositorio para registrar logs de error.</param>
        /// <param name="alertaRepo">El repositorio para registrar alertas de stock.</param>
        public ProductoService(IProductoRepository productoRepo, ILogErrorRepository logRepo, IAlertaRepository alertaRepo)
        {
            _productoRepo = productoRepo;
            _logRepo = logRepo;
            _alertaRepo = alertaRepo;
        }

        /// <summary>
        /// Obtiene todos los productos registrados en el inventario de forma asíncrona.
        /// </summary>
        /// <returns>Colección de productos.</returns>
        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            try
            {
                return await _productoRepo.ObtenerTodosAsync();
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "ObtenerTodosAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un producto específico por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="productoId">El identificador único del producto.</param>
        /// <returns>El producto correspondiente, o <c>null</c> si no se encuentra.</returns>
        public async Task<Producto?> ObtenerPorIdAsync(int productoId)
        {
            try
            {
                return await _productoRepo.ObtenerPorIdAsync(productoId);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "ObtenerPorIdAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los productos de una categoría específica de forma asíncrona.
        /// </summary>
        /// <param name="categoriaId">Identificador de la categoría.</param>
        /// <returns>Colección de productos filtrados por categoría.</returns>
        public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            try
            {
                return await _productoRepo.ObtenerPorCategoriaAsync(categoriaId);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "ObtenerPorCategoriaAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de productos que están con existencias bajas (stock menor o igual al stock mínimo) de forma asíncrona.
        /// </summary>
        /// <returns>Colección de productos con bajo stock.</returns>
        public async Task<IEnumerable<Producto>> ObtenerStockBajoAsync()
        {
            try
            {
                return await _productoRepo.ObtenerStockBajoAsync();
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "ObtenerStockBajoAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Inserta un nuevo producto en el catálogo de inventario de forma asíncrona.
        /// </summary>
        /// <param name="producto">El objeto producto con los datos a registrar.</param>
        /// <returns>El ID del producto insertado.</returns>
        public async Task<int> InsertarAsync(Producto producto)
        {
            try
            {
                return await _productoRepo.InsertarAsync(producto);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "InsertarAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un producto de forma asíncrona. 
        /// Si el stock resultante del producto actualizado está por debajo del mínimo, se genera una alerta.
        /// </summary>
        /// <param name="producto">El objeto producto con los datos modificados.</param>
        /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> ActualizarAsync(Producto producto)
        {
            try
            {
                var res = await _productoRepo.ActualizarAsync(producto);
                // verificar alerta
                if (res)
                {
                    var p = await _productoRepo.ObtenerPorIdAsync(producto.ProductoId);
                    if (p != null && p.Stock <= p.StockMinimo)
                    {
                        await CrearAlertaStockAsync(p);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "ActualizarAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina físicamente un producto del catálogo por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="productoId">Identificador del producto a eliminar.</param>
        /// <returns><c>true</c> si se eliminó; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> EliminarAsync(int productoId)
        {
            try
            {
                return await _productoRepo.EliminarAsync(productoId);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "EliminarAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Modifica el stock de un producto específico de forma asíncrona, incrementándolo o decrementándolo.
        /// Genera una alerta de stock bajo si las existencias restantes quedan por debajo del límite mínimo.
        /// </summary>
        /// <param name="productoId">Identificador único del producto.</param>
        /// <param name="cantidad">Cantidad a sumar (positiva) o restar (negativa).</param>
        /// <returns><c>true</c> si se actualizó correctamente el stock; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> ActualizarStockAsync(int productoId, int cantidad)
        {
            try
            {
                var success = await _productoRepo.ActualizarStockAsync(productoId, cantidad);
                if (success)
                {
                    var p = await _productoRepo.ObtenerPorIdAsync(productoId);
                    if (p != null && p.Stock <= p.StockMinimo)
                    {
                        await CrearAlertaStockAsync(p);
                    }
                }
                return success;
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "ActualizarStockAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Crea una alerta de stock bajo de forma asíncrona para un producto en particular.
        /// </summary>
        private async Task CrearAlertaStockAsync(Producto p)
        {
            try
            {
                var alerta = new AlertaStock
                {
                    ProductoId = p.ProductoId,
                    StockActual = p.Stock,
                    StockMinimo = p.StockMinimo,
                    Fecha = DateTime.UtcNow,
                    Revisada = false
                };

                await _alertaRepo.InsertAsync(alerta);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("ProductoService", "CrearAlertaStockAsync", ex);
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
                // no más
            }
        }
    }
}
