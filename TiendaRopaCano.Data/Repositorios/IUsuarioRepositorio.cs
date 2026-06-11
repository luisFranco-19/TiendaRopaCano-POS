using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Define los métodos de acceso a datos para la persistencia, consulta y gestión de cuentas de usuario.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Busca de forma asíncrona a un usuario con base en sus credenciales.
        /// </summary>
        /// <param name="nombreUsuario">El nombre de usuario.</param>
        /// <param name="contrasena">Contraseña hasheada.</param>
        /// <returns>El usuario si las credenciales coinciden; de lo contrario, <c>null</c>.</returns>
        Task<Usuario?> ObtenerPorCredencialesAsync(string nombreUsuario, string contrasena);

        /// <summary>
        /// Busca de forma asíncrona un usuario utilizando únicamente su nombre de usuario.
        /// </summary>
        /// <param name="nombreUsuario">El nombre de usuario.</param>
        /// <returns>El usuario si se encuentra; de lo contrario, <c>null</c>.</returns>
        Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);

        /// <summary>
        /// Obtiene de forma asíncrona todos los usuarios registrados en el sistema, cargando la información de su Rol asociado.
        /// </summary>
        /// <returns>Colección de usuarios.</returns>
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();

        /// <summary>
        /// Inserta un nuevo registro de usuario en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="usuario">Objeto usuario a insertar.</param>
        /// <returns>Identificador único asignado al usuario recién insertado.</returns>
        Task<int> InsertarAsync(Usuario usuario);

        /// <summary>
        /// Actualiza los datos de un usuario existente de forma asíncrona.
        /// </summary>
        /// <param name="usuario">Objeto usuario con los datos modificados.</param>
        /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
        Task<bool> ActualizarAsync(Usuario usuario);

        /// <summary>
        /// Habilita o deshabilita la cuenta de un usuario por su identificador de forma asíncrona.
        /// </summary>
        /// <param name="usuarioId">ID del usuario.</param>
        /// <param name="activo">Valor booleano que indica el nuevo estado.</param>
        /// <returns><c>true</c> si el estado se cambió correctamente; de lo contrario, <c>false</c>.</returns>
        Task<bool> CambiarEstadoAsync(int usuarioId, bool activo);

        /// <summary>
        /// Elimina físicamente un registro de usuario de la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="usuarioId">ID del usuario a eliminar.</param>
        /// <returns><c>true</c> si se eliminó; de lo contrario, <c>false</c>.</returns>
        Task<bool> EliminarAsync(int usuarioId);
    }
}
