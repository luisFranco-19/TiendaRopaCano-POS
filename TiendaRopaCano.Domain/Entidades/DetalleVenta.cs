using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa el detalle o línea individual de una venta, especificando el producto vendido, la cantidad y precios.
    /// Implementa <see cref="INotifyPropertyChanged"/> para soportar enlace de datos (data binding) en la UI.
    /// </summary>
    public class DetalleVenta : INotifyPropertyChanged
    {
        private int _detalleId;
        private int _ventaId;
        private int _productoId;
        private int _cantidad;
        private decimal _precioUnitario;
        private decimal _subtotal;
        private Producto? _producto;

        /// <summary>
        /// Obtiene o establece el identificador único del detalle de venta.
        /// </summary>
        public int DetalleId 
        { 
            get => _detalleId; 
            set { _detalleId = value; OnPropertyChanged(); } 
        }

        /// <summary>
        /// Obtiene o establece el identificador de la venta a la que pertenece este detalle.
        /// </summary>
        public int VentaId 
        { 
            get => _ventaId; 
            set { _ventaId = value; OnPropertyChanged(); } 
        }

        /// <summary>
        /// Obtiene o establece el identificador del producto vendido.
        /// </summary>
        public int ProductoId 
        { 
            get => _productoId; 
            set { _productoId = value; OnPropertyChanged(); } 
        }

        /// <summary>
        /// Obtiene o establece la cantidad de unidades vendidas. Al cambiar, recalcula automáticamente el subtotal.
        /// </summary>
        public int Cantidad 
        { 
            get => _cantidad; 
            set 
            { 
                _cantidad = value; 
                OnPropertyChanged(); 
                Subtotal = _cantidad * PrecioUnitario;
            } 
        }

        /// <summary>
        /// Obtiene o establece el precio unitario del producto al momento de la venta. Al cambiar, recalcula automáticamente el subtotal.
        /// </summary>
        public decimal PrecioUnitario 
        { 
            get => _precioUnitario; 
            set 
            { 
                _precioUnitario = value; 
                OnPropertyChanged(); 
                Subtotal = Cantidad * _precioUnitario;
            } 
        }

        /// <summary>
        /// Obtiene o establece el subtotal calculado (Cantidad * Precio Unitario) de esta línea de detalle.
        /// </summary>
        public decimal Subtotal 
        { 
            get => _subtotal; 
            set { _subtotal = value; OnPropertyChanged(); } 
        }

        /// <summary>
        /// Propiedad de navegación para acceder a la información detallada del producto.
        /// </summary>
        public Producto? Producto 
        { 
            get => _producto; 
            set { _producto = value; OnPropertyChanged(); } 
        }

        /// <summary>
        /// Evento que se desencadena cuando cambia una propiedad.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Invoca el evento <see cref="PropertyChanged"/> para notificar cambios en una propiedad.
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad modificada (se infiere automáticamente por CallerMemberName).</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}