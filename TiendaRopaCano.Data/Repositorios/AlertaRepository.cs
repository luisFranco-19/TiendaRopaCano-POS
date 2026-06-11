using System.Threading.Tasks;
using TiendaRopaCano.Dominio.Entidades;
using TiendaRopaCano.Datos.Contexto;
using Dapper;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Implementación de acceso a datos para la persistencia de alertas de stock mínimo utilizando Dapper y SQLite.
    /// </summary>
    public class AlertaRepository : IAlertaRepository
    {
        private readonly ConfiguracionBaseDatos _db;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AlertaRepository"/> con el configurador de base de datos.
        /// </summary>
        /// <param name="db">Configurador de la conexión a la base de datos.</param>
        public AlertaRepository(ConfiguracionBaseDatos db)
        {
            _db = db;
        }

        /// <summary>
        /// Registra una nueva alerta de stock mínimo en la base de datos y retorna el ID generado de forma asíncrona.
        /// </summary>
        /// <param name="alerta">Los datos de la alerta.</param>
        /// <returns>El identificador único de la alerta registrada.</returns>
        public async Task<int> InsertAsync(AlertaStock alerta)
        {
            using var con = _db.GetConnection();
            return await con.ExecuteScalarAsync<int>(
                @"INSERT INTO AlertasStock (ProductoId, StockActual, StockMinimo, Fecha, Revisada)
                  VALUES (@ProductoId, @StockActual, @StockMinimo, @Fecha, @Revisada);
                  SELECT last_insert_rowid();",
                alerta
            );
        }
    }
}
