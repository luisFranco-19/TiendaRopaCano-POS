using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Dominio.Entidades;
using TiendaRopaCano.Aplicacion.Auxiliares;
using Dapper;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Implementación de los servicios para la administración y lógica de negocio de usuarios.
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly ILogErrorRepository _logRepo;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UsuarioService"/> con los repositorios requeridos.
        /// </summary>
        /// <param name="usuarioRepo">El repositorio para acceder a datos de usuarios.</param>
        /// <param name="logRepo">El repositorio para registrar logs de error.</param>
        public UsuarioService(IUsuarioRepository usuarioRepo, ILogErrorRepository logRepo)
        {
            _usuarioRepo = usuarioRepo;
            _logRepo = logRepo;
        }

        /// <summary>
        /// Valida las credenciales de un usuario de forma asíncrona.
        /// Si son válidas y el usuario está activo, retorna su información.
        /// </summary>
        /// <param name="nombreUsuario">El nombre de usuario.</param>
        /// <param name="contrasena">La contraseña en texto plano.</param>
        /// <returns>El objeto <see cref="Usuario"/> autenticado, o <c>null</c> si no coincide o está inactivo.</returns>
        public async Task<Usuario?> ObtenerPorCredencialesAsync(string nombreUsuario, string contrasena)
        {
            try
            {
                var usuario = await _usuarioRepo.ObtenerPorNombreUsuarioAsync(nombreUsuario);
                
                if (usuario != null && usuario.Activo && EncriptadorContrasena.Verify(usuario.Contrasena, contrasena))
                {
                    return usuario;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("UsuarioService", "ObtenerPorCredencialesAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema de forma asíncrona.
        /// </summary>
        /// <returns>Colección de usuarios.</returns>
        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            try
            {
                return await _usuarioRepo.ObtenerTodosAsync();
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("UsuarioService", "ObtenerTodosAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Inserta un nuevo usuario en la base de datos de forma asíncrona, aplicando hash a su contraseña.
        /// </summary>
        /// <param name="usuario">Los datos del nuevo usuario.</param>
        /// <returns>El ID del usuario insertado.</returns>
        public async Task<int> InsertarAsync(Usuario usuario)
        {
            try
            {
                usuario.Contrasena = EncriptadorContrasena.Hash(usuario.Contrasena);
                return await _usuarioRepo.InsertarAsync(usuario);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("UsuarioService", "InsertarAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente de forma asíncrona, aplicando hash si la contraseña ha cambiado.
        /// </summary>
        /// <param name="usuario">Los datos del usuario.</param>
        /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            try
            {
                // Solo hashear si es una contraseña nueva en texto plano
                // Las contraseñas hasheadas con PBKDF2 tienen formato largo con "." como separador
                if (!string.IsNullOrEmpty(usuario.Contrasena) && 
                    usuario.Contrasena.Length < 50 && 
                    !usuario.Contrasena.Contains("."))
                {
                    usuario.Contrasena = EncriptadorContrasena.Hash(usuario.Contrasena);
                }
                
                return await _usuarioRepo.ActualizarAsync(usuario);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("UsuarioService", "ActualizarAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Cambia el estado de activación de un usuario en el sistema de forma asíncrona.
        /// </summary>
        /// <param name="usuarioId">ID del usuario.</param>
        /// <param name="activo">Nuevo estado de activación.</param>
        /// <returns><c>true</c> si el estado cambió; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> CambiarEstadoAsync(int usuarioId, bool activo)
        {
            try
            {
                return await _usuarioRepo.CambiarEstadoAsync(usuarioId, activo);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("UsuarioService", "CambiarEstadoAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina físicamente un usuario del sistema de forma asíncrona.
        /// </summary>
        /// <param name="usuarioId">ID del usuario a eliminar.</param>
        /// <returns><c>true</c> si se eliminó; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> EliminarAsync(int usuarioId)
        {
            try
            {
                return await _usuarioRepo.EliminarAsync(usuarioId);
            }
            catch (Exception ex)
            {
                await RegistrarLogAsync("UsuarioService", "EliminarAsync", ex);
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
