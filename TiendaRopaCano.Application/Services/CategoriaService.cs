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
    /// Implementación de los servicios de lógica de negocio y gestión para las categorías de productos.
    /// </summary>
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly ILogErrorRepository _logRepo;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CategoriaService"/> con los repositorios requeridos.
        /// </summary>
        /// <param name="categoriaRepo">El repositorio de acceso a datos de categorías.</param>
        /// <param name="logRepo">El repositorio para registrar logs de error.</param>
        public CategoriaService(ICategoriaRepository categoriaRepo, ILogErrorRepository logRepo)
        {
            _categoriaRepo = categoriaRepo;
            _logRepo = logRepo;
        }

        /// <summary>
        /// Obtiene todas las categorías de la base de datos de forma asíncrona.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="Categoria"/>.</returns>
        public async Task<IEnumerable<Categoria>> ObtenerTodasAsync()
        {
            try
            {
                return await _categoriaRepo.ObtenerTodasAsync();
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("CategoriaService", "ObtenerTodasAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Inserta una nueva categoría en el sistema de forma asíncrona.
        /// </summary>
        /// <param name="categoria">Los datos de la nueva categoría.</param>
        /// <returns>El identificador único asignado a la categoría insertada.</returns>
        public async Task<int> InsertarAsync(Categoria categoria)
        {
            try
            {
                return await _categoriaRepo.InsertarAsync(categoria);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("CategoriaService", "InsertarAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de una categoría existente de forma asíncrona.
        /// </summary>
        /// <param name="categoria">Los datos actualizados de la categoría.</param>
        /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> ActualizarAsync(Categoria categoria)
        {
            try
            {
                return await _categoriaRepo.ActualizarAsync(categoria);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("CategoriaService", "ActualizarAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina físicamente una categoría del sistema por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="categoriaId">Identificador único de la categoría a eliminar.</param>
        /// <returns><c>true</c> si se eliminó; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> EliminarAsync(int categoriaId)
        {
            try
            {
                return await _categoriaRepo.EliminarAsync(categoriaId);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("CategoriaService", "EliminarAsync", ex);
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
            catch { }
        }
    }
}
