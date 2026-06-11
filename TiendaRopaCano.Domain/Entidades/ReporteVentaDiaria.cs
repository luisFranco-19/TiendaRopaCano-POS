using System;

namespace TiendaRopaCano.Dominio.Entidades
{
    /// <summary>
    /// Representa un modelo consolidado o agregación estadística de las ventas del día, útil para generar informes financieros.
    /// </summary>
    public class ReporteVentaDiaria
    {
        /// <summary>
        /// Obtiene o establece la fecha del reporte en formato de cadena (generalmente YYYY-MM-DD).
        /// </summary>
        public string Fecha { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el monto total de ingresos percibidos por ventas en el día.
        /// </summary>
        public decimal TotalVentas { get; set; }

        /// <summary>
        /// Obtiene o establece el costo total de los productos vendidos (suma de los precios de compra).
        /// </summary>
        public decimal TotalCosto { get; set; }

        /// <summary>
        /// Obtiene o establece la utilidad neta o ganancia obtenida en el día (Total Ventas - Total Costo).
        /// </summary>
        public decimal Utilidad { get; set; }

        /// <summary>
        /// Obtiene o establece el número o cantidad total de transacciones de ventas realizadas en el día.
        /// </summary>
        public int CantidadVentas { get; set; }
    }
}
