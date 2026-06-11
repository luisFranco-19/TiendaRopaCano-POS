-- ============================================================
--   TIENDA DE ROPA - BASE DE DATOS SQLITE
--   Sistema de Ventas e Inventario
--   Tablas: 8
-- ============================================================

PRAGMA foreign_keys = ON;

-- ============================================================
-- TABLA 1: Roles
-- Datos fijos: Administrador y Vendedor
-- ============================================================
CREATE TABLE IF NOT EXISTS Roles (
    RolId   INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre  TEXT    NOT NULL UNIQUE
);

-- Datos semilla (se insertan una sola vez)
INSERT OR IGNORE INTO Roles (RolId, Nombre) VALUES (1, 'Administrador');
INSERT OR IGNORE INTO Roles (RolId, Nombre) VALUES (2, 'Vendedor');


-- ============================================================
-- TABLA 2: Usuarios
-- ============================================================
CREATE TABLE IF NOT EXISTS Usuarios (
    UsuarioId       INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreCompleto  TEXT    NOT NULL,
    NombreUsuario   TEXT    NOT NULL UNIQUE,
    Contrasena      TEXT    NOT NULL,
    RolId           INTEGER NOT NULL,
    Activo          INTEGER NOT NULL DEFAULT 1,

    FOREIGN KEY (RolId) REFERENCES Roles(RolId)
);

-- Nota: Se eliminó el usuario administrador por defecto para permitir que el usuario se registre desde la interfaz.



-- ============================================================
-- TABLA 3: Categorias
-- ============================================================
CREATE TABLE IF NOT EXISTS Categorias (
    CategoriaId INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre      TEXT    NOT NULL UNIQUE,
    Descripcion TEXT    NULL
);


-- ============================================================
-- TABLA 4: Productos
-- ============================================================
CREATE TABLE IF NOT EXISTS Productos (
    ProductoId   INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoriaId  INTEGER NOT NULL,
    Nombre       TEXT    NOT NULL,
    Descripcion  TEXT    NULL,
    Precio       REAL    NOT NULL,
    PrecioCompra REAL    NOT NULL,
    Stock        INTEGER NOT NULL DEFAULT 0,
    StockMinimo  INTEGER NOT NULL DEFAULT 5,
    Activo       INTEGER NOT NULL DEFAULT 1,
    FechaInactivacion TEXT NULL,

    FOREIGN KEY (CategoriaId) REFERENCES Categorias(CategoriaId)
);


-- ============================================================
-- TABLA 5: Ventas
-- ============================================================
CREATE TABLE IF NOT EXISTS Ventas (
    VentaId    INTEGER PRIMARY KEY AUTOINCREMENT,
    UsuarioId  INTEGER NOT NULL,
    Fecha      TEXT    NOT NULL,
    Total      REAL    NOT NULL,

    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);


-- ============================================================
-- TABLA 6: DetalleVentas
-- ============================================================
CREATE TABLE IF NOT EXISTS DetalleVentas (
    DetalleId      INTEGER PRIMARY KEY AUTOINCREMENT,
    VentaId        INTEGER NOT NULL,
    ProductoId     INTEGER NOT NULL,
    Cantidad       INTEGER NOT NULL,
    PrecioUnitario REAL    NOT NULL,
    Subtotal       REAL    NOT NULL,

    FOREIGN KEY (VentaId)    REFERENCES Ventas(VentaId),
    FOREIGN KEY (ProductoId) REFERENCES Productos(ProductoId)
);


-- ============================================================
-- TABLA 7: AlertasStock
-- Se genera automaticamente cuando Stock < StockMinimo
-- ============================================================
CREATE TABLE IF NOT EXISTS AlertasStock (
    AlertaId       INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductoId     INTEGER NOT NULL,
    StockActual    INTEGER NOT NULL,
    StockMinimo    INTEGER NOT NULL,
    Fecha          TEXT    NOT NULL,
    Revisada       INTEGER NOT NULL DEFAULT 0,
    RevisadaPor    INTEGER NULL,
    FechaRevision  TEXT    NULL,

    FOREIGN KEY (ProductoId)  REFERENCES Productos(ProductoId),
    FOREIGN KEY (RevisadaPor) REFERENCES Usuarios(UsuarioId)
);


-- ============================================================
-- TABLA 8: LogErrores
-- Niveles: INFO | WARNING | ERROR | CRITICAL
-- Modulos: Ventas | Inventario | Usuarios | Reportes | Sistema
-- ============================================================
CREATE TABLE IF NOT EXISTS LogErrores (
    LogId         INTEGER PRIMARY KEY AUTOINCREMENT,
    Fecha         TEXT    NOT NULL,
    UsuarioId     INTEGER NULL,
    Modulo        TEXT    NOT NULL,
    Accion        TEXT    NOT NULL,
    MensajeError  TEXT    NOT NULL,
    DetalleError  TEXT    NULL,
    Nivel         TEXT    NOT NULL DEFAULT 'ERROR',

    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);

-- ============================================================
-- DATOS SEMILLA ADICIONALES: Categorías de prueba
-- ============================================================
INSERT OR IGNORE INTO Categorias (CategoriaId, Nombre, Descripcion) VALUES (1, 'Camisetas y Tops', 'Camisetas, tops, blusas y playeras para uso diario');
INSERT OR IGNORE INTO Categorias (CategoriaId, Nombre, Descripcion) VALUES (2, 'Pantalones y Jeans', 'Pantalones casuales, formales, jeans y shorts');
INSERT OR IGNORE INTO Categorias (CategoriaId, Nombre, Descripcion) VALUES (3, 'Vestidos y Faldas', 'Vestidos de fiesta, casuales y faldas de todo tipo');
INSERT OR IGNORE INTO Categorias (CategoriaId, Nombre, Descripcion) VALUES (4, 'Abrigos y Chaquetas', 'Chaquetas, suéteres, abrigos y ropa de invierno');
INSERT OR IGNORE INTO Categorias (CategoriaId, Nombre, Descripcion) VALUES (5, 'Accesorios', 'Cinturones, gorras, bufandas y complementos de moda');

-- ============================================================
-- DATOS SEMILLA ADICIONALES: Productos de prueba
-- ============================================================
INSERT OR IGNORE INTO Productos (ProductoId, CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo) 
VALUES (1, 1, 'Camiseta Básica Algodón', 'Camiseta de algodón 100% color blanco unisex', 150.00, 75.00, 50, 10, 1);

INSERT OR IGNORE INTO Productos (ProductoId, CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo) 
VALUES (2, 1, 'Blusa de Seda Casual', 'Blusa elegante de seda para dama, variedad de colores', 350.00, 180.00, 25, 5, 1);

INSERT OR IGNORE INTO Productos (ProductoId, CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo) 
VALUES (3, 2, 'Jeans Slim Fit Azul', 'Jeans de mezclilla ajustados para caballero', 450.00, 220.00, 30, 8, 1);

INSERT OR IGNORE INTO Productos (ProductoId, CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo) 
VALUES (4, 2, 'Pantalón Chino Beige', 'Pantalón clásico casual de gabardina caballero', 400.00, 200.00, 20, 5, 1);

INSERT OR IGNORE INTO Productos (ProductoId, CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo) 
VALUES (5, 3, 'Vestido Corto de Verano', 'Vestido ligero con estampado floral para dama', 600.00, 300.00, 15, 3, 1);

INSERT OR IGNORE INTO Productos (ProductoId, CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo) 
VALUES (6, 4, 'Chaqueta de Mezclilla', 'Chaqueta clásica de mezclilla con botones metálicos', 750.00, 380.00, 12, 4, 1);

INSERT OR IGNORE INTO Productos (ProductoId, CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo) 
VALUES (7, 4, 'Suéter de Lana Gris', 'Suéter abrigador de cuello redondo', 500.00, 250.00, 18, 5, 1);

INSERT OR IGNORE INTO Productos (ProductoId, CategoriaId, Nombre, Descripcion, Precio, PrecioCompra, Stock, StockMinimo, Activo) 
VALUES (8, 5, 'Cinturón de Cuero Negro', 'Cinturón de cuero genuino ajustable', 180.00, 90.00, 40, 10, 1);


