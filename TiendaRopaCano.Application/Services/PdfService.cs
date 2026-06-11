using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Aplicacion.Servicios
{
    /// <summary>
    /// Servicio encargado de la generación de documentos en formato PDF utilizando la biblioteca QuestPDF.
    /// Configura la licencia comunitaria e implementa los reportes definidos en <see cref="IPdfService"/>.
    /// </summary>
    public class PdfService : IPdfService
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PdfService"/> y configura la licencia comunitaria de QuestPDF.
        /// </summary>
        public PdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        /// <summary>
        /// Genera un documento PDF con el resumen financiero y reporte consolidado de ventas diarias en un rango de fechas.
        /// </summary>
        /// <param name="ventas">La lista de ventas agrupadas por día.</param>
        /// <param name="desde">Fecha inicial del rango del reporte.</param>
        /// <param name="hasta">Fecha final del rango del reporte.</param>
        /// <param name="totalVentas">Suma de las ventas totales del período.</param>
        /// <param name="totalUtilidad">Suma de la utilidad neta acumulada en el período.</param>
        /// <param name="totalTransacciones">Número total de transacciones realizadas.</param>
        /// <returns>Un arreglo de bytes que representa el archivo PDF generado.</returns>
        public byte[] GenerarReporteVentas(IEnumerable<ReporteVentaDiaria> ventas, DateTime desde, DateTime hasta, decimal totalVentas, decimal totalUtilidad, int totalTransacciones)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("TIENDA DE ROPA CANO").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text("Reporte de Ventas e Ingresos").FontSize(14).FontColor(Colors.Grey.Medium);
                            col.Item().Text($"Periodo: {desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}").FontSize(10);
                        });

                        row.ConstantItem(100).Height(50).Placeholder(); // Espacio para logo
                    });

                    page.Content().PaddingVertical(10).Column(x =>
                    {
                        x.Spacing(15);

                        // Resumen de KPIs
                        x.Item().Row(row =>
                        {
                            row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                            {
                                c.Item().Text("TOTAL VENTAS").FontSize(10).FontColor(Colors.Grey.Medium);
                                c.Item().Text($"{totalVentas:C}").FontSize(16).Bold();
                            });
                            row.ConstantItem(10);
                            row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                            {
                                c.Item().Text("UTILIDAD ESTIMADA").FontSize(10).FontColor(Colors.Grey.Medium);
                                c.Item().Text($"{totalUtilidad:C}").FontSize(16).Bold().FontColor(Colors.Green.Medium);
                            });
                            row.ConstantItem(10);
                            row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                            {
                                c.Item().Text("TRANSACCIONES").FontSize(10).FontColor(Colors.Grey.Medium);
                                c.Item().Text($"{totalTransacciones}").FontSize(16).Bold();
                            });
                        });

                        // Tabla de Detalle
                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Fecha");
                                header.Cell().Element(CellStyle).Text("Cant. Ventas");
                                header.Cell().Element(CellStyle).Text("Total Ventas");
                                header.Cell().Element(CellStyle).Text("Utilidad");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            foreach (var v in ventas)
                            {
                                table.Cell().Element(ItemCellStyle).Text($"{v.Fecha:dd/MM/yyyy}");
                                table.Cell().Element(ItemCellStyle).Text($"{v.CantidadVentas}");
                                table.Cell().Element(ItemCellStyle).Text($"{v.TotalVentas:C}");
                                table.Cell().Element(ItemCellStyle).Text($"{v.Utilidad:C}");

                                static IContainer ItemCellStyle(IContainer container)
                                {
                                    return container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten3);
                                }
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Genera un reporte PDF con la lista de productos cuyo stock actual es menor o igual al stock mínimo.
        /// </summary>
        /// <param name="productos">Colección de productos con bajo nivel de existencias.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        public byte[] GenerarReporteInventarioBajoStock(IEnumerable<Producto> productos)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.Header().Text("Reporte de Stock Bajo").FontSize(20).SemiBold().FontColor(Colors.Red.Medium);

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Producto");
                            header.Cell().Element(CellStyle).Text("Categoría");
                            header.Cell().Element(CellStyle).Text("Stock");
                            header.Cell().Element(CellStyle).Text("Mínimo");

                            static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).BorderBottom(1).PaddingVertical(5);
                        });

                        foreach (var p in productos)
                        {
                            table.Cell().PaddingVertical(5).Text(p.Nombre);
                            table.Cell().PaddingVertical(5).Text(p.Categoria?.Nombre ?? "N/A");
                            table.Cell().PaddingVertical(5).Text(p.Stock.ToString()).FontColor(Colors.Red.Medium).Bold();
                            table.Cell().PaddingVertical(5).Text(p.StockMinimo.ToString());
                        }
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Genera un reporte PDF con la lista de usuarios y empleados del sistema, indicando su estado y rol asignado.
        /// </summary>
        /// <param name="usuarios">Colección de usuarios a reportar.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        public byte[] GenerarReporteUsuarios(IEnumerable<Usuario> usuarios)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.Header().Column(col =>
                    {
                        col.Item().Text("TIENDA DE ROPA CANO").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                        col.Item().Text("Reporte de Usuarios Registrados").FontSize(14).FontColor(Colors.Grey.Medium);
                        col.Item().Text($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(10);
                    });

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID");
                            header.Cell().Element(CellStyle).Text("Nombre Completo");
                            header.Cell().Element(CellStyle).Text("Usuario");
                            header.Cell().Element(CellStyle).Text("Rol");
                            header.Cell().Element(CellStyle).Text("Estado");

                            static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).BorderBottom(1).PaddingVertical(5);
                        });

                        foreach (var u in usuarios)
                        {
                            table.Cell().PaddingVertical(5).Text(u.UsuarioId.ToString());
                            table.Cell().PaddingVertical(5).Text(u.NombreCompleto);
                            table.Cell().PaddingVertical(5).Text(u.NombreUsuario);
                            table.Cell().PaddingVertical(5).Text(u.Rol?.Nombre ?? "N/A");
                            table.Cell().PaddingVertical(5).Text(u.Activo ? "Activo" : "Inactivo").FontColor(u.Activo ? Colors.Green.Medium : Colors.Red.Medium);
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Genera un reporte PDF con el inventario completo, indicando stock, precios y costo del catálogo de productos.
        /// </summary>
        /// <param name="productos">Colección de todos los productos en catálogo.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        public byte[] GenerarReporteInventarioCompleto(IEnumerable<Producto> productos)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.Header().Column(col =>
                    {
                        col.Item().Text("TIENDA DE ROPA CANO").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                        col.Item().Text("Reporte General de Inventario").FontSize(14).FontColor(Colors.Grey.Medium);
                        col.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(10);
                    });

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Producto");
                            header.Cell().Element(CellStyle).Text("Categoría");
                            header.Cell().Element(CellStyle).Text("Precio");
                            header.Cell().Element(CellStyle).Text("Stock");
                            header.Cell().Element(CellStyle).Text("Valor");

                            static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).BorderBottom(1).PaddingVertical(5);
                        });

                        foreach (var p in productos)
                        {
                            table.Cell().PaddingVertical(5).Text(p.Nombre);
                            table.Cell().PaddingVertical(5).Text(p.Categoria?.Nombre ?? "N/A");
                            table.Cell().PaddingVertical(5).Text($"{p.Precio:C}");
                            table.Cell().PaddingVertical(5).Text(p.Stock.ToString());
                            table.Cell().PaddingVertical(5).Text($"{(p.Precio * p.Stock):C}");
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Genera un reporte PDF con el historial detallado de transacciones de ventas individuales en un período.
        /// </summary>
        /// <param name="ventas">Lista de ventas del período con sus respectivos usuarios y montos.</param>
        /// <param name="desde">Fecha inicial del período de búsqueda.</param>
        /// <param name="hasta">Fecha final del período de búsqueda.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        public byte[] GenerarReporteHistorialVentas(IEnumerable<Venta> ventas, DateTime desde, DateTime hasta)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.Header().Column(col =>
                    {
                        col.Item().Text("TIENDA DE ROPA CANO").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                        col.Item().Text("Historial de Ventas").FontSize(14).FontColor(Colors.Grey.Medium);
                        col.Item().Text($"Periodo: {desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}").FontSize(10);
                    });

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID");
                            header.Cell().Element(CellStyle).Text("Fecha");
                            header.Cell().Element(CellStyle).Text("Usuario");
                            header.Cell().Element(CellStyle).Text("Items");
                            header.Cell().Element(CellStyle).Text("Total");

                            static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).BorderBottom(1).PaddingVertical(5);
                        });

                        foreach (var v in ventas)
                        {
                            table.Cell().PaddingVertical(5).Text(v.VentaId.ToString());
                            table.Cell().PaddingVertical(5).Text($"{v.Fecha:dd/MM/yyyy HH:mm}");
                            table.Cell().PaddingVertical(5).Text(v.Usuario?.NombreUsuario ?? "N/A");
                            table.Cell().PaddingVertical(5).Text(v.Detalles.Count.ToString());
                            table.Cell().PaddingVertical(5).Text($"{v.Total:C}").Bold();
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Genera un comprobante fiscal o factura detallada de una venta específica en formato PDF.
        /// </summary>
        /// <param name="venta">El objeto venta que contiene el total, la fecha, el usuario y los detalles del producto.</param>
        /// <returns>Un arreglo de bytes del archivo PDF generado.</returns>
        public byte[] GenerarFacturaVenta(Venta venta)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5); // Formato de ticket/factura pequeño y elegante
                    page.Margin(0.8f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.Verdana));

                    page.Header().Column(col =>
                    {
                        col.Spacing(2);
                        col.Item().Text("TIENDA DE ROPA CANO").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                        col.Item().Text("FACTURA DE VENTA / TICKET").FontSize(10).SemiBold().FontColor(Colors.Grey.Darken2);
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text($"Factura No: #{venta.VentaId}").Bold();
                                c.Item().Text($"Fecha: {venta.Fecha:dd/MM/yyyy HH:mm}");
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text($"Vendedor: {venta.Usuario?.NombreCompleto ?? "N/A"}").AlignRight();
                            });
                        });
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                    });

                    page.Content().PaddingVertical(8).Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Producto
                                columns.RelativeColumn(1); // Cant
                                columns.RelativeColumn(1); // Precio
                                columns.RelativeColumn(1); // Total
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Prenda");
                                header.Cell().Element(CellStyle).Text("Cant");
                                header.Cell().Element(CellStyle).Text("Precio");
                                header.Cell().Element(CellStyle).Text("Total");

                                static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).BorderBottom(1).PaddingVertical(3);
                            });

                            foreach (var d in venta.Detalles)
                            {
                                table.Cell().Element(ItemStyle).Text(d.Producto?.Nombre ?? "Producto");
                                table.Cell().Element(ItemStyle).Text(d.Cantidad.ToString());
                                table.Cell().Element(ItemStyle).Text($"{d.PrecioUnitario:C}");
                                table.Cell().Element(ItemStyle).Text($"{d.Subtotal:C}");

                                static IContainer ItemStyle(IContainer container) => container.PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2);
                            }
                        });

                        col.Item().AlignRight().Text(x =>
                        {
                            x.Span("TOTAL COMPRA: ").Bold().FontSize(11);
                            x.Span($"{venta.Total:C}").Bold().FontSize(12).FontColor(Colors.Green.Medium);
                        });
                    });

                    page.Footer().AlignCenter().Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        col.Item().PaddingTop(4).Text("¡Gracias por su preferencia!").FontSize(8).Italic().FontColor(Colors.Grey.Medium).AlignCenter();
                    });
                });
            }).GeneratePdf();
        }
    }
}
