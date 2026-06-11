using Microsoft.Data.Sqlite;
using System.Reflection;

namespace TiendaRopaCano.Datos.Contexto
{
    /// <summary>
    /// Administra la configuración y la inicialización de la base de datos SQLite para la aplicación.
    /// </summary>
    public class ConfiguracionBaseDatos
    {
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConfiguracionBaseDatos"/> con la cadena de conexión especificada.
        /// </summary>
        /// <param name="connectionString">La cadena de conexión de SQLite.</param>
        public ConfiguracionBaseDatos(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Crea y retorna una nueva conexión abierta o lista para abrirse a la base de datos de SQLite.
        /// </summary>
        /// <returns>Una instancia de <see cref="SqliteConnection"/>.</returns>
        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        /// <summary>
        /// Abre la conexión e inicializa la estructura de tablas ejecutando el script SQL embebido de inicialización de base de datos.
        /// </summary>
        public void InicializarBaseDeDatos()
        {
            using var con = new SqliteConnection(_connectionString);
            con.Open();

            var script = ObtenerScript();

            using var comando = con.CreateCommand();
            comando.CommandText = script;
            comando.ExecuteNonQuery();
        }

        /// <summary>
        /// Recupera y lee el contenido del archivo de script SQL de creación de base de datos embebido en los recursos del ensamblado.
        /// </summary>
        /// <returns>Una cadena con el script SQL de creación de la base de datos.</returns>
        /// <exception cref="Exception">Lanza una excepción si no se encuentra el recurso embebido del script SQL.</exception>
        private string ObtenerScript()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var recurso = assembly.GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith("TiendaRopaCano_Database.sql"));

            if (recurso == null)
                throw new Exception("No se encontró el script SQL.");

            using var stream = assembly.GetManifestResourceStream(recurso);
            using var reader = new StreamReader(stream!);
            return reader.ReadToEnd();
        }
    }
}