using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaRopaCano.Datos.Contexto;
using TiendaRopaCano.Dominio.Entidades;
using Dapper;


namespace TiendaRopaCano.Datos.Repositorios
{
    /// <summary>
    /// Implementación de acceso a datos para la gestión física de productos en la base de datos SQLite usando Dapper.
    /// </summary>
    public class ProductoRepository : IProductoRepository
    {
        private readonly ConfiguracionBaseDatos _db;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ProductoRepository"/> con el configurador de base de datos.
        /// </summary>
        /// <param name="db">Configurador de la conexión a la base de datos.</param>
        public ProductoRepository(ConfiguracionBaseDatos db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtiene todos los productos registrados, realizando un LEFT JOIN con categorías.
        /// </summary>
        /// <returns>Colección de productos con su respectiva categoría mapeada.</returns>
        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            using var con = _db.GetConnection();
            var query = @"
                SELECT p.*, c.CategoriaId, c.Nombre, c.Descripcion
                FROM Productos p
                LEFT JOIN Categorias c ON p.CategoriaId = c.CategoriaId
                WHERE p.Activo = 1 
                   OR p.FechaInactivacion IS NULL 
                   OR datetime(p.FechaInactivacion) >= datetime('now', 'localtime', '-30 days')";

            return await con.QueryAsync<Producto, Categoria, Producto>(
                query,
                (producto, categoria) =>
                {
                    producto.Categoria = categoria;
                    return producto;
                },
                splitOn: "CategoriaId"
            );
        }

        /// <summary>
        /// Busca un producto por su identificador único, incluyendo los datos de su categoría.
        /// </summary>
        /// <param name="productoId">Identificador único del producto.</param>
        /// <returns>El producto mapeado, o <c>null</c> si no se encuentra.</returns>
        public async Task<Producto?> ObtenerPorIdAsync(int productoId)
        {
            using var con = _db.GetConnection();
            var query = @"
                SELECT p.*, c.CategoriaId, c.Nombre, c.Descripcion
                FROM Productos p
                LEFT JOIN Categorias c ON p.CategoriaId = c.CategoriaId
                WHERE p.ProductoId = @ProductoId";

            var productos = await con.QueryAsync<Producto, Categoria, Producto>(
                query,
                (producto, categoria) =>
                {
                    producto.Categoria = categoria;
                    return producto;
                },
                new { ProductoId = productoId },
                splitOn: "CategoriaId"
            );
            return productos.FirstOrDefault();
        }

        /// <summary>
        /// Obtiene la lista de productos activos filtrada por categoría.
        /// </summary>
        /// <param name="categoriaId">Identificador de la categoría.</param>
        /// <returns>Colección de productos de dicha categoría.</returns>
        public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            using var con = _db.GetConnection();
            var query = @"
                SELECT p.*, c.CategoriaId, c.Nombre, c.Descripcion
                FROM Productos p
                LEFT JOIN Categorias c ON p.CategoriaId = c.CategoriaId
                WHERE p.CategoriaId = @CategoriaId AND p.Activo = 1";

            return await con.QueryAsync<Producto, Categoria, Producto>(
                query,
                (producto, categoria) =>
                {
                    producto.Categoria = categoria;
                    return producto;
                },
                new { CategoriaId = categoriaId },
                splitOn: "CategoriaId"
            );
        }

        /// <summary>
        /// Obtiene de forma asíncrona todos los productos activos cuyo stock sea menor o igual al mínimo.
        /// </summary>
        /// <returns>Colección de productos con bajo stock.</returns>
        public async Task<IEnumerable<Producto>> ObtenerStockBajoAsync()
        {
            using var con = _db.GetConnection();
            return await con.QueryAsync<Producto>(
                @"SELECT * FROM Productos 
                  WHERE Stock <= StockMinimo 
                    AND (Activo = 1 OR (Stock <= 0 AND (FechaInactivacion IS NULL OR datetime(FechaInactivacion) >= datetime('now', 'localtime', '-30 days'))))"
            );
        }

        /// <summary>
        /// Inserta un nuevo producto en la base de datos y retorna su ID generado.
        /// Desactiva automáticamente el producto si se registra con stock cero.
        /// </summary>
        /// <param name="producto">Los datos del producto a insertar.</param>
        /// <returns>ID del producto insertado.</returns>
        public async Task<int> InsertarAsync(Producto producto)
        {
            using var con = _db.GetConnection();
            return await con.ExecuteScalarAsync<int>(
                @"INSERT INTO Productos 
                (CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo, FechaInactivacion)
                VALUES 
                (@CategoriaId, @Nombre, @Descripcion, @Precio, @PrecioCompra, @Stock, @StockMinimo, 
                 CASE WHEN @Stock <= 0 THEN 0 ELSE 1 END,
                 CASE WHEN @Stock <= 0 THEN datetime('now', 'localtime') ELSE NULL END);
                SELECT last_insert_rowid();",
                producto
            );
        }

        /// <summary>
        /// Actualiza los datos de un producto y recalcula su estado de activo con base en su stock.
        /// </summary>
        /// <param name="producto">Los datos modificados del producto.</param>
        /// <returns><c>true</c> si se actualizó correctamente; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> ActualizarAsync(Producto producto)
        {
            using var con = _db.GetConnection();
            var filas = await con.ExecuteAsync(
                @"UPDATE Productos SET
                CategoriaId = @CategoriaId,
                Nombre = @Nombre,
                Descripcion = @Descripcion,
                Precio = @Precio,
                PrecioCompra = @PrecioCompra,
                StockMinimo = @StockMinimo,
                Activo = CASE WHEN Stock <= 0 THEN 0 ELSE 1 END,
                FechaInactivacion = CASE WHEN Stock <= 0 THEN COALESCE(FechaInactivacion, datetime('now', 'localtime')) ELSE NULL END
                WHERE ProductoId = @ProductoId",
                producto
            );
            return filas > 0;
        }

        /// <summary>
        /// Elimina físicamente el producto si no tiene ventas asociadas; de lo contrario, lanza una excepción.
        /// </summary>
        /// <param name="productoId">Identificador del producto a eliminar.</param>
        /// <returns><c>true</c> si el producto fue eliminado; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> EliminarAsync(int productoId)
        {
            using var con = _db.GetConnection();
            
            // Verificar si el producto tiene al menos una venta asociada en DetalleVentas
            var cantVentas = await con.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM DetalleVentas WHERE ProductoId = @ProductoId",
                new { ProductoId = productoId }
            );

            if (cantVentas > 0)
            {
                throw new InvalidOperationException("No se puede eliminar el producto porque ya cuenta con ventas registradas en el sistema.");
            }

            // Eliminar primero las alertas de stock asociadas para no violar restricciones de clave foránea
            await con.ExecuteAsync(
                "DELETE FROM AlertasStock WHERE ProductoId = @ProductoId",
                new { ProductoId = productoId }
            );

            // Eliminar físicamente el producto de la base de datos
            var filas = await con.ExecuteAsync(
                "DELETE FROM Productos WHERE ProductoId = @ProductoId",
                new { ProductoId = productoId }
            );

            return filas > 0;
        }

        /// <summary>
        /// Resta la cantidad especificada de existencias al producto y desactiva el producto si el stock restante es menor o igual a cero.
        /// </summary>
        /// <param name="productoId">Identificador del producto.</param>
        /// <param name="cantidad">Cantidad a restar del stock actual.</param>
        /// <returns><c>true</c> si se actualizó el stock; de lo contrario, <c>false</c>.</returns>
        public async Task<bool> ActualizarStockAsync(int productoId, int cantidad)
        {
            using var con = _db.GetConnection();
            var filas = await con.ExecuteAsync(
                @"UPDATE Productos 
                  SET Stock = Stock - @Cantidad,
                      Activo = CASE WHEN (Stock - @Cantidad) <= 0 THEN 0 ELSE 1 END,
                      FechaInactivacion = CASE WHEN (Stock - @Cantidad) <= 0 THEN datetime('now', 'localtime') ELSE NULL END
                  WHERE ProductoId = @ProductoId",
                new { ProductoId = productoId, Cantidad = cantidad }
            );
            return filas > 0;
        }
    }
}