using ForestalCasablancaApp.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;
using IContainer = QuestPDF.Infrastructure.IContainer;
using System.Collections.ObjectModel;
using ForestalCasablancaApp.Models;

namespace ForestalCasablancaApp.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        #region Properties

        public int TitleSize { get; set; } = 18;
        public int SubtitleSize { get; set; } = 14;
        public int NormalSize { get; set; } = 12;
        public int FooterSize { get; set; } = 10;
        public int FirstColumnSize { get; set; } = 140;
        public string ImagePath { get; set; }

        #endregion

        public PdfGeneratorService() 
        { 
            ImagePath = Path.Combine(FileSystem.Current.AppDataDirectory, "pdf_image.png");
        }

        public async void GenerateTrozoAserrablePDF(TrozoAserrableViewModel model)
        {
            // Set the file name
            string folder = Preferences.Get("CurrentWorkingDirectory", FileSystem.Current.CacheDirectory);
            string fileName = PdfGeneratorService.GenerateFileName(model.Cliente.Nombre);
            string finalPath = Path.Combine(folder, fileName);

            // Design the PDF
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    #region Page Settings

                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(NormalSize));

                    #endregion

                    page.Header()
                        .Component(new DocumentHeader("Venta en Trozos", ImagePath, TitleSize, model.Folio));

                    page.Content()
                        .PaddingVertical(0, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Height(0);

                            //Datos Cliente
                            x.Item()
                             .Component(new ClientInfo("Datos Cliente", SubtitleSize, FirstColumnSize, model.Cliente.Nombre, model.Cliente.RUT));

                            // Datos Camión
                            x.Item()
                             .Component(new CamionInfoTrozo("Datos Camión", SubtitleSize, FirstColumnSize, model.DatosCamion.Patente,
                             model.DatosCamion.Chofer, model.DatosCamion.RutChofer, model.DatosCamion.EmpresaTransportista));

                            // Observaciones
                            x.Item().Component(new ObservacionesSection("Observaciones", SubtitleSize, 
                                string.IsNullOrEmpty(model.Comments) ? "Sin observaciones" : model.Comments));

                            // Summary
                            x.Item().Column(col =>
                            {
                                // Title
                                col.Item()
                                   .Component(new SectionTitle("Resumen", SubtitleSize));

                                //Table
                                col.Item().Component(new TrozoAserrableSummary(model));
                                col.Item().PageBreak();
                            });

                            // Detalles de Carga

                            // Lista 1
                            if (model.Especie1.ListaMedidas.Count > 0)
                            {
                                x.Item().Component(new EspecieInfo("Producto 1", SubtitleSize, FirstColumnSize, 
                                    model.Especie1.Especie, model.Especie1.LargoEspecie, model.Especie1.UnidadOrigen));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.Especie1.ListaMedidas, 
                                    model.Especie1.CantidadTotalSum, model.Especie1.TotalSumFinal));
                            }

                            // Lista 2
                            if (model.Especie2.ListaMedidas.Count > 0)
                            {
                                if (model.Especie1.ListaMedidas.Count > 0)
                                    x.Item().Height(0).PageBreak();

                                x.Item().Component(new EspecieInfo("Producto 2", SubtitleSize, FirstColumnSize,
                                    model.Especie2.Especie, model.Especie2.LargoEspecie, model.Especie2.UnidadOrigen));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.Especie1.ListaMedidas,
                                    model.Especie2.CantidadTotalSum, model.Especie2.TotalSumFinal));
                            }

                            // Lista 3
                            if (model.Especie3.ListaMedidas.Count > 0)
                            {
                                if (model.Especie1.ListaMedidas.Count > 0 || model.Especie2.ListaMedidas.Count > 0)
                                    x.Item().Height(0).PageBreak();

                                x.Item().Component(new EspecieInfo("Producto 3", SubtitleSize, FirstColumnSize,
                                    model.Especie3.Especie, model.Especie3.LargoEspecie, model.Especie3.UnidadOrigen));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.Especie3.ListaMedidas,
                                    model.Especie3.CantidadTotalSum, model.Especie3.TotalSumFinal));
                            }
                            // Lista 4
                            if (model.Especie4.ListaMedidas.Count > 0)
                            {
                                if (model.Especie1.ListaMedidas.Count > 0 || model.Especie2.ListaMedidas.Count > 0
                                    || model.Especie3.ListaMedidas.Count > 0)
                                    x.Item().Height(0).PageBreak();

                                x.Item().Component(new EspecieInfo("Producto 4", SubtitleSize, FirstColumnSize,
                                    model.Especie4.Especie, model.Especie4.LargoEspecie, model.Especie4.UnidadOrigen));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.Especie4.ListaMedidas,
                                    model.Especie4.CantidadTotalSum, model.Especie4.TotalSumFinal));
                            }
                            // Lista 5
                            if (model.Especie5.ListaMedidas.Count > 0)
                            {
                                if (model.Especie1.ListaMedidas.Count > 0 || model.Especie2.ListaMedidas.Count > 0
                                    || model.Especie3.ListaMedidas.Count > 0 || model.Especie4.ListaMedidas.Count > 0)
                                    x.Item().Height(0).PageBreak();

                                x.Item().Component(new EspecieInfo("Producto 5", SubtitleSize, FirstColumnSize,
                                    model.Especie5.Especie, model.Especie5.LargoEspecie, model.Especie5.UnidadOrigen));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.Especie5.ListaMedidas,
                                    model.Especie5.CantidadTotalSum, model.Especie5.TotalSumFinal));
                            }
                            // Lista 6
                            if (model.Especie6.ListaMedidas.Count > 0)
                            {
                                if (model.Especie1.ListaMedidas.Count > 0 || model.Especie2.ListaMedidas.Count > 0
                                || model.Especie3.ListaMedidas.Count > 0 || model.Especie4.ListaMedidas.Count > 0 || model.Especie5.ListaMedidas.Count > 0)
                                    x.Item().Height(0).PageBreak();

                                x.Item().Component(new EspecieInfo("Producto 6", SubtitleSize, FirstColumnSize,
                                    model.Especie6.Especie, model.Especie6.LargoEspecie, model.Especie6.UnidadOrigen));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.Especie6.ListaMedidas,
                                    model.Especie6.CantidadTotalSum, model.Especie6.TotalSumFinal));
                            }
                        });

                    page.Footer()
                        .Element(ComposeFooter);
                });
            }).GeneratePdf(finalPath);

            // Open the generated PDF
            await Launcher.Default.OpenAsync(new OpenFileRequest(fileName, new ReadOnlyFile(finalPath)));
        }
        
        public async void GenerateLeñaPDF(LeñaViewModel model)
        {
            // Set the file name
            string folder = Preferences.Get("CurrentWorkingDirectory", FileSystem.Current.CacheDirectory);
            string fileName = PdfGeneratorService.GenerateFileName(model.Cliente.Nombre);
            string finalPath = Path.Combine(folder, fileName);

            // Design the PDF
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Page Settings
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(NormalSize));

                    // Document Sections
                    page.Header()
                        .Component(new DocumentHeader("Leña", ImagePath, TitleSize, model.Folio));

                    page.Content()
                        .PaddingVertical(0, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Height(0);

                            //Datos Cliente
                            x.Item()
                             .Component(new ClientInfo("Datos Cliente", SubtitleSize, FirstColumnSize, model.Cliente.Nombre, 
                             model.Cliente.RUT));

                            // Datos Camión
                            x.Item()
                             .Component(new CamionInfo("Datos Camión", SubtitleSize, FirstColumnSize, model.DatosCamion.Patente, 
                             model.DatosCamion.Chofer, model.DatosCamion.RutChofer, model.DatosCamion.EmpresaTransportista));

                            // Observaciones
                            x.Item().Component(new ObservacionesSection("Observaciones", SubtitleSize,
                                string.IsNullOrEmpty(model.Comments) ? "Sin observaciones" : model.Comments));

                            // Despacho Leña
                            x.Item().Component(new DespachoInfo("Despacho Leña", SubtitleSize, FirstColumnSize, 
                                model.Despacho.Especie, model.Despacho.UnidadOrigen));

                            // Detalles de Carga
                            x.Item().Column(col =>
                            {
                                col.Spacing(3);

                                col.Item()
                                   .Component(new SectionTitle("Detalles de Carga", SubtitleSize));

                                col.Item()
                                   .Component(new DetalleCarga("Camión", "Largo Camión", model.Despacho.AlturaMedia, model.Despacho.LargoCamion,
                                   model.Despacho.Bancos, model.Despacho.AlturaMediaPalomera, model.Despacho.AnchoPalomera));


                                col.Item().Height(12);
                                col.Item().Table(table =>
                                {
                                    // Table columns definition
                                    table.ColumnsDefinition(col =>
                                    {
                                        col.RelativeColumn();
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(SummaryHeaderCellStyle).Text("Resultado Cantidad de Leña");
                                    });

                                    table.Cell().Element(SummaryFinalCellStyle).Text($"{model.Despacho.TotalMetros:F2} ML");
                                });
                            });
                            
                        });

                    page.Footer()
                        .Element(ComposeFooter);
                });
            }).GeneratePdf(finalPath);

            // Open the generated PDF
            await Launcher.Default.OpenAsync(new OpenFileRequest(fileName, new ReadOnlyFile(finalPath)));
        }

        public async void GenerateMetroRumaPDF(MetroRumaViewModel model)
        {
            // Set the file name
            string folder = Preferences.Get("CurrentWorkingDirectory", FileSystem.Current.CacheDirectory);
            string fileName = PdfGeneratorService.GenerateFileName(model.Cliente.Nombre);
            string finalPath = Path.Combine(folder, fileName);

            // Design the PDF
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Page Settings
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(NormalSize));

                    // Document Sections
                    page.Header()
                        .Component(new DocumentHeader("Metro Ruma", ImagePath, TitleSize, model.Folio));

                    page.Content()
                        .PaddingVertical(0, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Height(0);

                            //Datos Cliente
                            x.Item()
                             .Component(new ClientInfo("Datos Cliente", SubtitleSize, FirstColumnSize, model.Cliente.Nombre,
                             model.Cliente.RUT));

                            // Datos Camión
                            x.Item()
                             .Component(new CamionInfo("Datos Camión", SubtitleSize, FirstColumnSize, model.DatosCamion.Patente,
                             model.DatosCamion.Chofer, model.DatosCamion.RutChofer, model.DatosCamion.EmpresaTransportista));

                            // Observaciones
                            x.Item().Component(new ObservacionesSection("Observaciones", SubtitleSize,
                                string.IsNullOrEmpty(model.Comments) ? "Sin observaciones" : model.Comments));

                            // Despacho Leña
                            x.Item().Component(new DespachoInfo("Despacho Metro Ruma", SubtitleSize, FirstColumnSize,
                                model.Despacho.Especie, model.Despacho.UnidadOrigen));

                            // Detalles de Carga
                            x.Item().Column(col =>
                            {
                                col.Spacing(3);

                                col.Item()
                                   .Component(new SectionTitle("Detalles de Carga", SubtitleSize));

                                col.Item()
                                   .Component(new DetalleCarga("Camión y Carro", "Ancho Camión", model.Despacho.AlturaMedia, model.Despacho.LargoCamion,
                                   model.Despacho.Bancos, model.Despacho.AlturaMediaPalomera, model.Despacho.AnchoPalomera));


                                col.Item().Height(12);
                                col.Item().Table(table =>
                                {
                                    // Table columns definition
                                    table.ColumnsDefinition(col =>
                                    {
                                        col.RelativeColumn();
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(SummaryHeaderCellStyle).Text("Resultado Cantidad de Metro Ruma");
                                    });

                                    table.Cell().Element(SummaryFinalCellStyle).Text($"{model.Despacho.TotalMetros:F2} MR");
                                });
                            });

                        });

                    page.Footer()
                        .Element(ComposeFooter);
                });
            }).GeneratePdf(finalPath);

            // Open the generated PDF
            await Launcher.Default.OpenAsync(new OpenFileRequest(fileName, new ReadOnlyFile(finalPath)));
        }

        #region Helper Methods

        /// <summary>
        /// Static method that formats the name of the file to be generated.
        /// </summary>
        /// <param name="nombreCliente">String that represents the Nombre property in the Cliente object of the viewModel</param>
        /// <returns></returns>
        public static string GenerateFileName(string nombreCliente)
        {
            // Get the next file number
            int fileNumber = GetNextFileNumber();

            // Get user initials
            string userInitials = Preferences.Get("CurrentUserInitials", "XX");

            // Get the current date
            string dateAsString = DateTime.Now.ToString("dd/MM/yy");
            string formattedDate = dateAsString.Replace("/", "-");

            // Get cliente's name
            string formattedCliente;
            if (string.IsNullOrEmpty(nombreCliente))
                formattedCliente = "Sin_Nombre";
            else
                formattedCliente = nombreCliente.Trim().Split(' ')[0];

            // Return the formatted string
            return $"{fileNumber}{userInitials}{formattedDate}_{formattedCliente}.pdf";
        }

        /// <summary>
        /// Generates a new file number and saves it in the app's preferences.
        /// </summary>
        /// <returns></returns>
        public static int GetNextFileNumber()
        {
            int currentNumber = Preferences.Get("CurrentFileNumber", 1);

            Preferences.Set("CurrentFileNumber", currentNumber + 1);

            return currentNumber;
        }

        /// <summary>
        /// Static method that defines the style for the header cells of a table.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        static IContainer HeaderCellStyle(IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold())
                            .PaddingVertical(5)
                            .BorderBottom(1)
                            .AlignCenter()
                            .BorderColor(Colors.Black);
        }

        /// <summary>
        /// Static method that defines the style for the cells of a table.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        static IContainer CellStyle(IContainer container)
        {
            return container.BorderBottom(1)
                            .AlignCenter()
                            .BorderColor(Colors.Grey.Lighten2)
                            .PaddingVertical(5);
        }

        static IContainer SummaryCellStyle(IContainer container)
        {
            return container.Border(1)
                            .AlignCenter()
                            .BorderColor(Colors.Grey.Lighten2)
                            .PaddingVertical(5);
        }

        static IContainer SummaryFinalCellStyle(IContainer container)
        {
            return container.Border(1)
                            .AlignCenter()
                            .DefaultTextStyle(x => x.FontSize(18))
                            .BorderColor(Colors.Grey.Lighten2)
                            .PaddingVertical(5);
        }

        static IContainer SummaryHeaderCellStyle(IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold())
                            .PaddingTop(5)
                            .Border(1)
                            .AlignCenter()
                            .BorderColor(Colors.Black);
        }

        /// <summary>
        /// Method that defines the layout for the footer of the PDF.
        /// </summary>
        /// <param name="container"></param>
        private void ComposeFooter(IContainer container)
        {
            container
                    .AlignCenter()
                    .DefaultTextStyle(x => x.FontSize(FooterSize))
                    .Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                    });
        }

        #endregion

        #region Components

        public class DocumentHeader : IComponent
        {
            public string Title { get; set; }
            public string ImagePath { get; set; }
            public int TitleSize { get; set; }
            public string Folio { get; set; }

            public DocumentHeader(string title, string imagePath, int titleSize, string folio)
            {
                Title = title;
                ImagePath = imagePath;
                TitleSize = titleSize;
                Folio = folio;
            }

            public void Compose(IContainer container)
            {
                container.Column(column =>
                {
                    column.Item()
                          .PaddingBottom(5)
                          .Row(row =>
                          {
                              row.ConstantItem(80)
                                 .Image(ImagePath);

                              row.RelativeItem()
                                  .AlignRight()
                                  .PaddingTop(5)
                                  .Column(column =>
                                  {
                                      column.Spacing(5);

                                      column.Item()
                                            .Text("Detalle Despacho")
                                            .Bold()
                                            .FontSize(TitleSize);

                                      column.Item()
                                            .Text(Title);
                                  });
                          });

                    // N° & Fecha ColumnHeaders
                    column.Item()
                          .PaddingRight(270)
                          .Row(row =>
                          {
                              row.ConstantItem(150)
                                 .Border(1)
                                 .AlignCenter()
                                 .Text(x =>
                                 {
                                     x.Span("N°")
                                      .Bold();
                                 });

                              row.RelativeItem()
                                 .Border(1)
                                 .AlignCenter()
                                 .Text(x =>
                                 {
                                     x.Span("Fecha")
                                      .Bold();
                                 });
                          });

                    // N° y Fecha actual values
                    column.Item()
                          .PaddingRight(270)
                          .Row(row =>
                          {
                              row.ConstantItem(150)
                                 .Border(1)
                                 .AlignCenter()
                                 .Text(Folio);

                              row.RelativeItem()
                                 .Border(1)
                                 .AlignCenter()
                                 .Text(DateTime.Now.ToString());
                          });
                });
            }
        }

        public class SectionTitle : IComponent
        {
            public string Title { get; set; }
            public int SubtitleSize { get; set; }

            public SectionTitle(string title, int size)
            {
                Title = title;
                SubtitleSize = size;
            }

            public void Compose(IContainer container)
            {
                container.Text(Title)
                         .FontSize(SubtitleSize)
                         .Bold()
                         .Underline();
            }
            
        }

        public class ObservacionesSection : IComponent
        {
            public string SectionTitle { get; set; }
            public int SubtitleSize { get; set; }
            public string Observaciones { get; set; }


            public ObservacionesSection(string title, int subtitleSize, string observaciones)
            {
                SectionTitle = title;
                SubtitleSize = subtitleSize;
                Observaciones = observaciones;
            }

            public void Compose(IContainer container)
            {
                container.Column(col =>
                {
                    col.Spacing(3);

                    col.Item()
                       .Component(new SectionTitle(SectionTitle, SubtitleSize));

                    col.Item()
                       .Border(1)
                       .AlignCenter()
                       .Text(Observaciones);
                });
            }
        }
        public class SingleRow : IComponent
        {
            public int ColumnSize { get; set; }
            public string ColumnLabel { get; set; }
            public string ColumnValue { get; set; }

            public SingleRow(int size, string label, string value)
            {
                ColumnSize = size;
                ColumnLabel = label;
                ColumnValue = value;
            }

            public void Compose(IContainer container)
            {
                container.Row(row =>
                {
                    row.ConstantItem(ColumnSize)
                       .Border(1)
                       .Text(ColumnLabel);

                    row.RelativeItem()
                       .Border(1)
                       .AlignCenter()
                       .Text(ColumnValue);
                });
            }
        }

        public class EspecieInfo : IComponent
        {
            public string SectionTitle { get; set; }
            public int SubtitleSize { get; set; }
            public int FirstColumnSize { get; set; }
            public string Especie { get; set; }
            public string Largo { get; set; }
            public string UnidadOrigen { get; set; }

            public EspecieInfo(string title, int subtitle, int columnSize, string especie, string largo, string origen)
            {
                SubtitleSize = subtitle;
                FirstColumnSize = columnSize;
                Especie = especie;
                Largo = largo;
                SectionTitle = title;
                UnidadOrigen = origen;

            }
            public void Compose(IContainer container)
            {
                container.Column(col =>
                {
                    col.Spacing(3);

                    col.Item()
                       .Component(new SectionTitle(SectionTitle, SubtitleSize));
                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Unidad de origen:", UnidadOrigen));
                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Producto:", Especie));
                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Largo Cubicación:", Largo));
                });
            }
        }

        public class ClientInfo : IComponent
        {
            public string SectionTitle { get; set; }
            public int SubtitleSize { get; set; }
            public int FirstColumnSize { get; set; }
            public string Cliente { get; set; }
            public string RUT { get; set; }

            public ClientInfo(string title, int subtitle, int columnSize, string cliente, string rut)
            {
                SubtitleSize = subtitle;
                FirstColumnSize = columnSize;
                Cliente = cliente;
                RUT = rut;
                SectionTitle = title;
            }
            public void Compose(IContainer container)
            {
                container.Column(col =>
                {
                    col.Spacing(3);

                    col.Item()
                       .Component(new SectionTitle(SectionTitle, SubtitleSize));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Cliente:", Cliente));
                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " RUT:", RUT));
                });
            }
        }

        public class CamionInfo : IComponent
        {
            public string SectionTitle { get; set; }
            public int SubtitleSize { get; set; }
            public int FirstColumnSize { get; set; }
            public string Patente { get; set; }
            public string Chofer { get; set; }
            public string RutChofer { get; set; }
            public string EmpresaTransportista { get; set; }

            public CamionInfo(string title, int subtitle, int columnSize, string patente, string chofer, string rut, string empresa)
            {
                SubtitleSize = subtitle;
                FirstColumnSize = columnSize;
                SectionTitle = title;
                Patente = patente;
                Chofer = chofer;
                RutChofer = rut;
                EmpresaTransportista = empresa;
            }

            public void Compose(IContainer container)
            {
                container.Column(col =>
                {
                    col.Spacing(3);

                    col.Item()
                       .Component(new SectionTitle(SectionTitle, SubtitleSize));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Empresa de Transporte:",
                           EmpresaTransportista));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Chofer:", Chofer));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " RUT:", RutChofer));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Patentes", Patente));
                });
            }
        }

        public class CamionInfoTrozo : IComponent
        {
            public string SectionTitle { get; set; }
            public int SubtitleSize { get; set; }
            public int FirstColumnSize { get; set; }
            public string Patente { get; set; }
            public string Chofer { get; set; }
            public string RutChofer { get; set; }
            public string EmpresaTransportista { get; set; }

            public CamionInfoTrozo(string title, int subtitle, int columnSize, string patente, string chofer, string rut, string empresa)
            {
                SubtitleSize = subtitle;
                FirstColumnSize = columnSize;
                SectionTitle = title;
                Patente = patente;
                Chofer = chofer;
                RutChofer = rut;
                EmpresaTransportista = empresa;
            }

            public void Compose(IContainer container)
            {
                container.Column(col =>
                {
                    col.Spacing(3);

                    col.Item()
                       .Component(new SectionTitle(SectionTitle, SubtitleSize));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Empresa de Transporte:",
                           EmpresaTransportista));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Chofer:", Chofer));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " RUT:", RutChofer));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Patentes", Patente));
                });
            }
        }

        public class DespachoInfo : IComponent
        {
            public string SectionTitle { get; set; }
            public int SubtitleSize { get; set; }
            public int FirstColumnSize { get; set; }
            public string TipoLena { get; set; }
            public string Origen { get; set; }

            public DespachoInfo(string title, int subtitle, int columnSize, string tipoLena, string origen)
            {
                SubtitleSize = subtitle;
                FirstColumnSize = columnSize;
                SectionTitle = title;
                TipoLena = tipoLena;
                Origen = origen;
            }

            public void Compose(IContainer container)
            {
                container.Column(col =>
                {
                    col.Spacing(3);

                    col.Item()
                       .Component(new SectionTitle(SectionTitle, SubtitleSize));

                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Tipo Leña:",
                                                 TipoLena));
                    col.Item()
                       .Component(new SingleRow(FirstColumnSize, " Unidad de Origen",
                                                 Origen));
                });
            }
        }

        public class DetalleCarga : IComponent
        {
            public string SectionTitle { get; set; }
            public string RowName { get; set; }
            public double AlturaMedia { get; set; }
            public string MedidaCamion { get; set; }
            public string Bancos { get; set; }
            public string AnchoPalomera { get; set; }
            public double AlturaMediaPalomera { get; set; }
            public string AlturaMediaPalomeraStr
            {
                get
                {
                    if (AlturaMediaPalomera == 0)
                        return string.Empty;
                    else
                        return AlturaMediaPalomera.ToString("F2");
                }
            }
            
            public DetalleCarga(string title, string rowName, double alturaMedia, string medidaCamion, string bancos,
                double altoPalomera, string anchoPalomera)
            {
                SectionTitle = title;
                RowName = rowName;
                AlturaMedia = alturaMedia;
                MedidaCamion = medidaCamion;
                Bancos = bancos;
                AlturaMediaPalomera = altoPalomera;
                AnchoPalomera = anchoPalomera;
            }

            public void Compose(IContainer container)
            {
                container.Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.Spacing(70);
                        // Camion Table
                        row.AutoItem().Table(table =>
                        {
                            // Table columns definition
                            table.ColumnsDefinition(col =>
                            {
                                col.ConstantColumn(140);
                                col.ConstantColumn(80);
                            });

                            // Table Header definition
                            table.Header(header =>
                            {
                                header.Cell().ColumnSpan(2).Element(SummaryHeaderCellStyle).Text(SectionTitle);
                            });

                            // Table Content
                            table.Cell().Element(SummaryCellStyle).Text("H Media (m)");
                            table.Cell().Element(SummaryCellStyle).Text(AlturaMedia.ToString("F2"));

                            table.Cell().Element(SummaryCellStyle).Text($"{RowName} (m)");
                            table.Cell().Element(SummaryCellStyle).Text(MedidaCamion);

                            table.Cell().Element(SummaryCellStyle).Text("N° de Bancos");
                            table.Cell().Element(SummaryCellStyle).Text(Bancos);
                        });

                        // Palomera Table
                        row.AutoItem().Table(table =>
                        {
                            // Table columns definition
                            table.ColumnsDefinition(col =>
                            {
                                col.ConstantColumn(140);
                                col.ConstantColumn(80);
                            });

                            // Table Header definition
                            table.Header(header =>
                            {
                                header.Cell().ColumnSpan(2).Element(SummaryHeaderCellStyle).Text("Palomera");
                            });

                            // Table Content
                            table.Cell().Element(SummaryCellStyle).Text("H Media (m)");
                            table.Cell().Element(SummaryCellStyle).Text(AlturaMediaPalomeraStr);

                            table.Cell().Element(SummaryCellStyle).Text("Ancho (m)");
                            table.Cell().Element(SummaryCellStyle).Text(AnchoPalomera);
                        });
                    });
                });
            }
        }

        public class TrozoAserrableDetails : IComponent
        {
            public ObservableCollection<MedidaTrozoAserrable> Medidas { get; set; }
            public int TotalSum { get; set; }
            public double FinalTotalSum { get; set; }

            public TrozoAserrableDetails(ObservableCollection<MedidaTrozoAserrable> medidas, int totalSum, double finalTotalSum)
            {
                Medidas = medidas;
                TotalSum = totalSum;
                FinalTotalSum = finalTotalSum;
            }

            public void Compose(IContainer container)
            {
                container.Table(table =>
                {
                    // Table columns definition
                    table.ColumnsDefinition(col =>
                    {
                        col.RelativeColumn();
                        col.RelativeColumn();
                        col.RelativeColumn();
                        col.RelativeColumn();
                    });

                    // Table Header definition
                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCellStyle).Text("Diámetro");
                        header.Cell().Element(HeaderCellStyle).Text("Total (Q)");
                        header.Cell().Element(HeaderCellStyle).Text("Volumen (V)");
                        header.Cell().Element(HeaderCellStyle).Text("Total (Q x V)");
                    });

                    // Aqui agregar un foreach que recorra la lista de productos
                    foreach (var item in Medidas)
                    {
                        table.Cell().Element(CellStyle).Text(item.Diametro.ToString());
                        table.Cell().Element(CellStyle).Text(item.Cantidad.ToString());
                        table.Cell().Element(CellStyle).Text(item.Volumen.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.Total.ToString());
                    }

                    // Last row to display totals
                    table.Cell().Element(CellStyle).Text("Total:");
                    table.Cell().Element(CellStyle).Text(TotalSum.ToString());
                    table.Cell().Element(CellStyle).Text("");
                    table.Cell().Element(CellStyle).Text(FinalTotalSum.ToString("F2"));
                });
            }
        }

        public class TrozoAserrableSummary : IComponent
        {
            public TrozoAserrableViewModel ViewModel { get; set; }

            public TrozoAserrableSummary(TrozoAserrableViewModel model)
            {
                ViewModel = model;
            }

            public void Compose(IContainer container)
            {
                container.Table(table =>
                {
                    // Table columns definition
                    table.ColumnsDefinition(col =>
                    {
                        col.ConstantColumn(240);
                        col.RelativeColumn();
                        col.RelativeColumn();
                    });

                    // Table Header definition
                    table.Header(header =>
                    {
                        header.Cell().Element(SummaryHeaderCellStyle).Text("Producto");
                        header.Cell().Element(SummaryHeaderCellStyle).Text("Cantidad de Trozos");
                        header.Cell().Element(SummaryHeaderCellStyle).Text("Volumen (m3)");
                    });

                    // Table Content based on the number of products

                    // Product 1
                    if(ViewModel.Especie1.ListaMedidas.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.Especie1.Especie} {ViewModel.Especie1.LargoEspecie} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie1.CantidadTotalSum.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie1.TotalSumFinal.ToString("F2"));
                    }

                    // Product 2
                    if (ViewModel.Especie2.ListaMedidas.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.Especie2.Especie} {ViewModel.Especie2.LargoEspecie} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie2.CantidadTotalSum.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie2.TotalSumFinal.ToString("F2"));
                    }

                    // Product 3
                    if (ViewModel.Especie3.ListaMedidas.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.Especie3.Especie} {ViewModel.Especie3.LargoEspecie} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie3.CantidadTotalSum.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie3.TotalSumFinal.ToString("F2"));
                    }

                    if (ViewModel.Especie4.ListaMedidas.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.Especie4.Especie} {ViewModel.Especie4.LargoEspecie} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie4.CantidadTotalSum.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie4.TotalSumFinal.ToString("F2"));
                    }

                    if (ViewModel.Especie5.ListaMedidas.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.Especie5.Especie} {ViewModel.Especie5.LargoEspecie} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie5.CantidadTotalSum.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie5.TotalSumFinal.ToString("F2"));
                    }

                    if (ViewModel.Especie6.ListaMedidas.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.Especie6.Especie} {ViewModel.Especie6.LargoEspecie} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie6.CantidadTotalSum.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.Especie6.TotalSumFinal.ToString("F2"));
                    }

                    table.Cell().Element(SummaryCellStyle).Text("Total:");
                    table.Cell().Element(SummaryCellStyle).Text(ViewModel.CantidadFinalDespacho.ToString());
                    table.Cell().Element(SummaryCellStyle).Text(ViewModel.VolumenFinalDespacho.ToString("F2"));
                });
            }
        }
        #endregion
    }
}
