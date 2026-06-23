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
-- DATOS SEMILLA ADICIONALES: (Eliminados para desarrollo/producción limpia)
-- ============================================================




