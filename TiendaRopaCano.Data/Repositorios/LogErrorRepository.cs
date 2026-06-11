using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;
using TiendaRopaCano.Datos.Contexto;
using Dapper;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Implementación de acceso a datos para registrar los errores del sistema utilizando Dapper y SQLite.
    /// </summary>
    public class LogErrorRepository : ILogErrorRepository
    {
        private readonly ConfiguracionBaseDatos _db;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LogErrorRepository"/> con el configurador de base de datos.
        /// </summary>
        /// <param name="db">Configurador de la conexión a la base de datos.</param>
        public LogErrorRepository(ConfiguracionBaseDatos db)
        {
            _db = db;
        }

        /// <summary>
        /// Registra un nuevo log de error en la base de datos de forma asíncrona y retorna el ID del log generado.
        /// </summary>
        /// <param name="log">Los datos técnicos del log de error.</param>
        /// <returns>El identificador único asignado al registro de error.</returns>
        public async Task<int> InsertAsync(LogError log)
        {
            using var con = _db.GetConnection();
            return await con.ExecuteScalarAsync<int>(
                @"INSERT INTO LogErrors (Fecha, UsuarioId, Modulo, Accion, MensajeError, DetalleError, Nivel)
                  VALUES (@Fecha, @UsuarioId, @Modulo, @Accion, @MensajeError, @DetalleError, @Nivel);
                  SELECT last_insert_rowid();",
                log
            );
        }
    }
}
