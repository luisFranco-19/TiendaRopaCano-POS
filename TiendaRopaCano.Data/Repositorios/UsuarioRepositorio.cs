using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Datos.Contexto;
using TiendaRopaCano.Dominio.Entidades;
using Dapper;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Implementación de acceso a datos para la persistencia de usuarios en la base de datos SQLite utilizando Dapper.
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ConfiguracionBaseDatos _db;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UsuarioRepository"/> con el configurador de base de datos.
        /// </summary>
        /// <param name="db">Configurador de la conexión a la base de datos.</param>
        public UsuarioRepository(ConfiguracionBaseDatos db)
        {
            _db = db;
        }

        /// <summary>
        /// Busca de forma asíncrona a un usuario con base en sus credenciales en la base de datos.
        /// </summary>
        /// <param name="nombreUsuario">El nombre de usuario.</param>
        /// <param name="contrasena">La contraseña ya hasheada.</param>
        /// <returns>El usuario si las credenciales coinciden y está activo; de lo contrario, <c>null</c>.</returns>
        public async Task<Usuario?> ObtenerPorCredencialesAsync(string nombreUsuario, string contrasena)
        {
            using var con = _db.GetConnection();
            return await con.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Contrasena = @Contrasena AND Activo = 1",
                new { NombreUsuario = nombreUsuario, Contrasena = contrasena }
            );
        }

        /// <summary>
        /// Busca de forma asíncrona un usuario utilizando únicamente su nombre de usuario, incluyendo su rol.
        /// </summary>
        /// <param name="nombreUsuario">El nombre de usuario.</param>
        /// <returns>El usuario con su rol asociado, o <c>null</c>.</returns>
        public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
        {
            using var con = _db.GetConnection();
            var usuarios = await con.QueryAsync<Usuario, Rol, Usuario>(
                @"SELECT u.*, r.RolId, r.Nombre 
                  FROM Usuarios u 
                  LEFT JOIN Roles r ON u.RolId = r.RolId 
                  WHERE u.NombreUsuario = @NombreUsuario",
                (usuario, rol) =>
                {
                    usuario.Rol = rol;
                    return usuario;
                },
                new { NombreUsuario = nombreUsuario },
                splitOn: "RolId"
            );
            return usuarios.FirstOrDefault();
        }

        /// <summary>
        /// Obtiene de forma asíncrona todos los usuarios registrados en el sistema, excluyendo al administrador principal.
        /// </summary>
        /// <returns>Colección de usuarios con su rol asociado, ordenados por nombre completo.</returns>
        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            using var con = _db.GetConnection();
            var usuarios = await con.QueryAsync<Usuario, Rol, Usuario>(
                @"SELECT u.*, r.RolId, r.Nombre 
                  FROM Usuarios u 
                  LEFT JOIN Roles r ON u.RolId = r.RolId 
                  ORDER BY u.NombreCompleto",
                (usuario, rol) =>
                {
                    usuario.Rol = rol;
                    return usuario;
                },
                splitOn: "RolId"
            );
            return usuarios;
        }

        /// <summary>
        /// Habilita o deshabilita la cuenta de un usuario por su identificador de forma asíncrona.
        /// </summary>
        /// <param name="usuarioId">ID del usuario.</param>
        /// <param name="activo">Valor booleano que indica el nuevo estado.</param>
        /// <returns><c>true</c> si el estado se cambió correctamente; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> CambiarEstadoAsync(int usuarioId, bool activo)
        {
            using var con = _db.GetConnection();
            var filas = await con.ExecuteAsync(
                "UPDATE Usuarios SET Activo = @Activo WHERE UsuarioId = @UsuarioId",
                new { Activo = activo, UsuarioId = usuarioId }
            );
            return filas > 0;
        }

        /// <summary>
        /// Inserta un nuevo registro de usuario en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="usuario">Objeto usuario a insertar.</param>
        /// <returns>Identificador único asignado al usuario recién insertado.</returns>
        public async Task<int> InsertarAsync(Usuario usuario)
        {
            using var con = _db.GetConnection();
            return await con.ExecuteScalarAsync<int>(
                @"INSERT INTO Usuarios (NombreCompleto, NombreUsuario, Contrasena, RolId, Activo)
                  VALUES (@NombreCompleto, @NombreUsuario, @Contrasena, @RolId, @Activo);
                  SELECT last_insert_rowid();",
                usuario
            );
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente de forma asíncrona.
        /// </summary>
        /// <param name="usuario">Objeto usuario con los datos modificados.</param>
        /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            using var con = _db.GetConnection();
            var filas = await con.ExecuteAsync(
                @"UPDATE Usuarios SET
                  NombreCompleto = @NombreCompleto,
                  NombreUsuario = @NombreUsuario,
                  Contrasena = @Contrasena,
                  RolId = @RolId,
                  Activo = @Activo
                  WHERE UsuarioId = @UsuarioId",
                usuario
            );
            return filas > 0;
        }

        /// <summary>
        /// Elimina físicamente un registro de usuario de la base de datos de forma asíncrona.
        /// Si existen restricciones por llaves foráneas (por ejemplo, si tiene ventas asociadas), se cae de compatibilidad y realiza una desactivación lógica.
        /// </summary>
        /// <param name="usuarioId">ID del usuario a eliminar.</param>
        /// <returns><c>true</c> si se eliminó o desactivó correctamente; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> EliminarAsync(int usuarioId)
        {
            using var con = _db.GetConnection();
            try
            {
                // Limpiar referencias en LogErrores
                await con.ExecuteAsync(
                    "UPDATE LogErrores SET UsuarioId = NULL WHERE UsuarioId = @UsuarioId",
                    new { UsuarioId = usuarioId }
                );

                // Limpiar referencias en AlertasStock
                await con.ExecuteAsync(
                    "UPDATE AlertasStock SET RevisadaPor = NULL WHERE RevisadaPor = @UsuarioId",
                    new { UsuarioId = usuarioId }
                );

                var filas = await con.ExecuteAsync(
                    "DELETE FROM Usuarios WHERE UsuarioId = @UsuarioId",
                    new { UsuarioId = usuarioId }
                );
                return filas > 0;
            }
            catch
            {
                // Si tiene relaciones (ventas asociadas), hacemos desactivación lógica
                var filas = await con.ExecuteAsync(
                    "UPDATE Usuarios SET Activo = 0 WHERE UsuarioId = @UsuarioId",
                    new { UsuarioId = usuarioId }
                );
                return filas > 0;
            }
        }
    }
}
