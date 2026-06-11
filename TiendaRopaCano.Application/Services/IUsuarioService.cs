using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Define los métodos de servicio para la gestión de usuarios, autenticación y seguridad en el sistema.
    /// </summary>
    public interface IUsuarioService
    {
        /// <summary>
        /// Realiza la autenticación de un usuario validando su nombre de usuario y contraseña de forma asíncrona.
        /// </summary>
        /// <param name="nombreUsuario">El nombre de usuario o credencial de acceso.</param>
        /// <param name="contrasena">La contraseña en texto plano ingresada por el usuario.</param>
        /// <returns>El objeto <see cref="Usuario"/> autenticado con sus datos si las credenciales son válidas y está activo; de lo contrario, <c>null</c>.</returns>
        Task<Usuario?> ObtenerPorCredencialesAsync(string nombreUsuario, string contrasena);

        /// <summary>
        /// Obtiene de forma asíncrona todos los usuarios registrados en el sistema.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="Usuario"/>.</returns>
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();

        /// <summary>
        /// Registra un nuevo usuario en el sistema de forma asíncrona, encriptando su contraseña.
        /// </summary>
        /// <param name="usuario">El objeto usuario que contiene la información a registrar.</param>
        /// <returns>El identificador único asignado al nuevo usuario.</returns>
        Task<int> InsertarAsync(Usuario usuario);

        /// <summary>
        /// Actualiza los datos de un usuario existente de forma asíncrona.
        /// </summary>
        /// <param name="usuario">El objeto usuario con la información modificada.</param>
        /// <returns><c>true</c> si los cambios se guardaron de forma exitosa; de lo contrario, <c>false</c>.</returns>
        Task<bool> ActualizarAsync(Usuario usuario);

        /// <summary>
        /// Habilita o deshabilita la cuenta de un usuario por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario.</param>
        /// <param name="activo">Valor booleano que indica si el usuario estará activo (<c>true</c>) o inactivo (<c>false</c>).</param>
        /// <returns><c>true</c> si el estado se cambió correctamente; de lo contrario, <c>false</c>.</returns>
        Task<bool> CambiarEstadoAsync(int usuarioId, bool activo);

        /// <summary>
        /// Elimina físicamente un usuario del sistema de forma asíncrona.
        /// </summary>
        /// <param name="usuarioId">Identificador único del usuario a eliminar.</param>
        /// <returns><c>true</c> si se eliminó correctamente; de lo contrario, <c>false</c>.</returns>
        Task<bool> EliminarAsync(int usuarioId);
    }
}
