using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Datos.Contexto;
using TiendaRopaCano.Dominio.Entidades;
using Dapper;

namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Implementación de acceso a datos para la gestión y persistencia transaccional de ventas utilizando Dapper y SQLite.
    /// </summary>
    public class VentaRepository : IVentaRepository
    {
        private readonly ConfiguracionBaseDatos _db;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="VentaRepository"/> con el configurador de base de datos.
        /// </summary>
        /// <param name="db">Configurador de la conexión a la base de datos.</param>
        public VentaRepository(ConfiguracionBaseDatos db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtiene todas las ventas registradas, incluyendo su usuario/vendedor y cargando todas las líneas de detalle de forma optimizada.
        /// </summary>
        /// <returns>Colección de ventas ordenadas por fecha descendente.</returns>
        public async Task<IEnumerable<Venta>> ObtenerTodasAsync()
        {
            using var con = _db.GetConnection();
            var sql = @"
                SELECT v.*, u.UsuarioId, u.NombreCompleto, u.NombreUsuario, u.RolId, u.Activo
                FROM Ventas v
                LEFT JOIN Usuarios u ON v.UsuarioId = u.UsuarioId
                ORDER BY v.Fecha DESC";

            var ventas = (await con.QueryAsync<Venta, Usuario, Venta>(
                sql,
                (venta, usuario) =>
                {
                    venta.Usuario = usuario;
                    return venta;
                },
                splitOn: "UsuarioId"
            )).ToList();

            if (ventas.Any())
            {
                var ventaIds = ventas.Select(v => v.VentaId).ToList();
                var sqlDetalles = @"
                    SELECT d.*, p.ProductoId, p.Nombre, p.Precio, p.PrecioCompra, p.Stock, p.StockMinimo, p.Activo, p.Descripcion
                    FROM DetalleVentas d
                    INNER JOIN Productos p ON d.ProductoId = p.ProductoId
                    WHERE d.VentaId IN @VentaIds";

                var detalles = await con.QueryAsync<DetalleVenta, Producto, DetalleVenta>(
                    sqlDetalles,
                    (detalle, producto) =>
                    {
                        detalle.Producto = producto;
                        return detalle;
                    },
                    new { VentaIds = ventaIds },
                    splitOn: "ProductoId"
                );

                var detallesPorVenta = detalles.GroupBy(d => d.VentaId)
                                               .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var venta in ventas)
                {
                    venta.Detalles = detallesPorVenta.TryGetValue(venta.VentaId, out var listaDetalles) 
                        ? listaDetalles 
                        : new List<DetalleVenta>();
                }
            }

            return ventas;
        }

        /// <summary>
        /// Obtiene una venta específica por su identificador, incluyendo sus líneas de detalle correspondientes.
        /// </summary>
        /// <param name="ventaId">Identificador único de la venta.</param>
        /// <returns>El objeto venta, o <c>null</c> si no se encuentra.</returns>
        public async Task<Venta?> ObtenerPorIdAsync(int ventaId)
        {
            using var con = _db.GetConnection();
            var venta = await con.QueryFirstOrDefaultAsync<Venta>(
                "SELECT * FROM Ventas WHERE VentaId = @VentaId",
                new { VentaId = ventaId }
            );

            if (venta != null)
            {
                var sqlDetalles = @"
                    SELECT d.*, p.ProductoId, p.Nombre, p.Precio, p.PrecioCompra, p.Stock, p.StockMinimo, p.Activo, p.Descripcion
                    FROM DetalleVentas d
                    INNER JOIN Productos p ON d.ProductoId = p.ProductoId
                    WHERE d.VentaId = @VentaId";

                var detalles = await con.QueryAsync<DetalleVenta, Producto, DetalleVenta>(
                    sqlDetalles,
                    (detalle, producto) =>
                    {
                        detalle.Producto = producto;
                        return detalle;
                    },
                    new { VentaId = ventaId },
                    splitOn: "ProductoId"
                );
                venta.Detalles = detalles.AsList();
            }

            return venta;
        }

        /// <summary>
        /// Obtiene la lista de ventas ocurridas dentro de un rango de fechas de forma asíncrona, cargando sus vendedores y detalles.
        /// </summary>
        /// <param name="desde">Fecha inicial.</param>
        /// <param name="hasta">Fecha final.</param>
        /// <returns>Colección de ventas registradas en el período.</returns>
        public async Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta)
        {
            using var con = _db.GetConnection();
            var sql = @"
                SELECT v.*, u.UsuarioId, u.NombreCompleto, u.NombreUsuario, u.RolId, u.Activo
                FROM Ventas v
                LEFT JOIN Usuarios u ON v.UsuarioId = u.UsuarioId
                WHERE date(v.Fecha) BETWEEN date(@Desde) AND date(@Hasta)
                ORDER BY v.Fecha DESC";

            var ventas = (await con.QueryAsync<Venta, Usuario, Venta>(
                sql,
                (venta, usuario) =>
                {
                    venta.Usuario = usuario;
                    return venta;
                },
                new { Desde = desde, Hasta = hasta },
                splitOn: "UsuarioId"
            )).ToList();

            if (ventas.Any())
            {
                var ventaIds = ventas.Select(v => v.VentaId).ToList();
                var sqlDetalles = @"
                    SELECT d.*, p.ProductoId, p.Nombre, p.Precio, p.PrecioCompra, p.Stock, p.StockMinimo, p.Activo, p.Descripcion
                    FROM DetalleVentas d
                    INNER JOIN Productos p ON d.ProductoId = p.ProductoId
                    WHERE d.VentaId IN @VentaIds";

                var detalles = await con.QueryAsync<DetalleVenta, Producto, DetalleVenta>(
                    sqlDetalles,
                    (detalle, producto) =>
                    {
                        detalle.Producto = producto;
                        return detalle;
                    },
                    new { VentaIds = ventaIds },
                    splitOn: "ProductoId"
                );

                var detallesPorVenta = detalles.GroupBy(d => d.VentaId)
                                               .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var venta in ventas)
                {
                    venta.Detalles = detallesPorVenta.TryGetValue(venta.VentaId, out var listaDetalles) 
                        ? listaDetalles 
                        : new List<DetalleVenta>();
                }
            }

            return ventas;
        }

        /// <summary>
        /// Inserta una nueva venta y sus líneas de detalle en la base de datos de forma asíncrona y transaccional.
        /// Descuenta automáticamente el stock de los productos involucrados en la transacción.
        /// </summary>
        /// <param name="venta">El objeto venta conteniendo el vendedor, la fecha, el total y sus detalles.</param>
        /// <returns>El ID asignado a la venta insertada.</returns>
        /// <exception cref="Exception">Arroja una excepción en caso de que ocurra algún fallo durante la inserción.</exception>
        public async Task<int> InsertarAsync(Venta venta)
        {
            using var con = _db.GetConnection();
            con.Open();
            using var transaccion = con.BeginTransaction();

            try
            {
                // Insertar la venta
                var ventaId = await con.ExecuteScalarAsync<int>(
                    @"INSERT INTO Ventas (UsuarioId, Fecha, Total)
                    VALUES (@UsuarioId, @Fecha, @Total);
                    SELECT last_insert_rowid();",
                    venta, transaccion
                );

                // Insertar cada detalle y descontar stock
                foreach (var detalle in venta.Detalles)
                {
                    detalle.VentaId = ventaId;

                    await con.ExecuteAsync(
                        @"INSERT INTO DetalleVentas 
                        (VentaId, ProductoId, Cantidad, PrecioUnitario, Subtotal)
                        VALUES 
                        (@VentaId, @ProductoId, @Cantidad, @PrecioUnitario, @Subtotal)",
                        detalle, transaccion
                    );

                    // Descontar stock automáticamente y desactivar si llega a 0
                    await con.ExecuteAsync(
                        @"UPDATE Productos 
                          SET Stock = Stock - @Cantidad,
                              Activo = CASE WHEN (Stock - @Cantidad) <= 0 THEN 0 ELSE 1 END
                          WHERE ProductoId = @ProductoId",
                        new { detalle.Cantidad, detalle.ProductoId }, transaccion
                    );
                }

                transaccion.Commit();
                return ventaId;
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                throw new Exception($"Error al registrar la venta: {ex.Message}");
            }
        }
    }
}
