using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Datos.Contexto;
using TiendaRopaCano.Dominio.Entidades;
using Dapper;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Implementación de acceso a datos para la generación de reportes consolidados utilizando Dapper y SQLite.
    /// </summary>
    public class ReporteRepository : IReporteRepository
    {
        private readonly ConfiguracionBaseDatos _db;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReporteRepository"/> con el configurador de base de datos.
        /// </summary>
        /// <param name="db">Configurador de la conexión a la base de datos.</param>
        public ReporteRepository(ConfiguracionBaseDatos db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtiene de la base de datos las ventas diarias consolidadas en un período determinado de forma asíncrona.
        /// </summary>
        /// <param name="desde">Fecha inicial.</param>
        /// <param name="hasta">Fecha final.</param>
        /// <returns>Colección de objetos <see cref="ReporteVentaDiaria"/>.</returns>
        public async Task<IEnumerable<ReporteVentaDiaria>> ObtenerVentasDiariasAsync(DateTime desde, DateTime hasta)
        {
            using var con = _db.GetConnection();
            var query = @"
                SELECT 
                    date(v.Fecha) as Fecha,
                    SUM(dv.Subtotal) as TotalVentas,
                    SUM(dv.Cantidad * p.PrecioCompra) as TotalCosto,
                    SUM(dv.Subtotal) - SUM(dv.Cantidad * p.PrecioCompra) as Utilidad,
                    COUNT(DISTINCT v.VentaId) as CantidadVentas
                FROM Ventas v
                JOIN DetalleVentas dv ON v.VentaId = dv.VentaId
                JOIN Productos p ON dv.ProductoId = p.ProductoId
                WHERE date(v.Fecha) BETWEEN date(@Desde) AND date(@Hasta)
                GROUP BY date(v.Fecha)
                ORDER BY date(v.Fecha)";

            return await con.QueryAsync<ReporteVentaDiaria>(query, new { Desde = desde, Hasta = hasta });
        }

        /// <summary>
        /// Obtiene de la base de datos la lista de productos que están en bajo stock de forma asíncrona.
        /// </summary>
        /// <returns>Colección de productos con existencias por debajo o igual del límite mínimo.</returns>
        public async Task<IEnumerable<Producto>> ObtenerProductosBajoStockAsync()
        {
            using var con = _db.GetConnection();
            var query = @"
                SELECT p.*, c.CategoriaId, c.Nombre, c.Descripcion
                FROM Productos p
                LEFT JOIN Categorias c ON p.CategoriaId = c.CategoriaId
                WHERE p.Stock <= p.StockMinimo 
                  AND (p.Activo = 1 OR (p.Stock <= 0 AND (p.FechaInactivacion IS NULL OR datetime(p.FechaInactivacion) >= datetime('now', 'localtime', '-30 days'))))
                ORDER BY p.Stock ASC";

            return await con.QueryAsync<Producto, Categoria, Producto>(
                query,
                (prod, cat) =>
                {
                    prod.Categoria = cat;
                    return prod;
                },
                splitOn: "CategoriaId"
            );
        }
    }
}
