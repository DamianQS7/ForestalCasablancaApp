using ForestalCasablancaApp.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;
using IContainer = QuestPDF.Infrastructure.IContainer;
using System.Collections.ObjectModel;
using ForestalCasablancaApp.Models;
using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;

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
            string folder = Preferences.Get("CurrentWorkingDirectory", "");
            string fileName = PdfGeneratorService.GenerateFileName("Trozo");
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
                             .Component(new CamionInfo("Datos Camión", SubtitleSize, FirstColumnSize, model.DatosCamion.Patente,
                             model.DatosCamion.Chofer, model.DatosCamion.RutChofer, model.DatosCamion.EmpresaTransportista));

                            // Detalles de Carga

                            // Lista 1
                            if (model.MedidasEspecieUno.Count > 0)
                            {
                                x.Item()
                             .Component(new EspecieInfo("Producto 1", SubtitleSize, FirstColumnSize, model.EspecieUno, model.LargoEspecieUno));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.MedidasEspecieUno, model.TotalSumLista1, model.FinalTotalSumLista1));
                            }

                            // Lista 2
                            if (model.MedidasEspecieDos.Count > 0)
                            {
                                // If there is content in the first list, add a page break
                                if (model.MedidasEspecieUno.Count > 0)
                                    x.Item().Height(0).PageBreak();


                                x.Item()
                                .Component(new EspecieInfo("Producto 2", SubtitleSize, FirstColumnSize, model.EspecieDos, model.LargoEspecieDos));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.MedidasEspecieDos, model.TotalSumLista2, model.FinalTotalSumLista2));
                            }

                            // Lista 3
                            if (model.MedidasEspecieTres.Count > 0)
                            {
                                if (model.MedidasEspecieDos.Count > 0 || model.MedidasEspecieUno.Count > 0)
                                    x.Item().Height(0).PageBreak();

                                x.Item()
                             .Component(new EspecieInfo("Producto 3", SubtitleSize, FirstColumnSize, model.EspecieTres, model.LargoEspecieTres));

                                // Detalle de Carga (Titulo)
                                x.Item().Column(col =>
                                {
                                    col.Item()
                                       .Component(new SectionTitle("Detalle de Carga", SubtitleSize));
                                });

                                // Detalle de Carga (Tabla)
                                x.Item().Component(new TrozoAserrableDetails(model.MedidasEspecieTres, model.TotalSumLista3, model.FinalTotalSumLista3));
                            }

                            // Summary
                            x.Item().Column(col =>
                            {
                                // Title
                                col.Item()
                                   .Component(new SectionTitle("Resumen", SubtitleSize));

                                //Table
                                col.Item().Component(new TrozoAserrableSummary(model));
                            });

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
            string folder = Preferences.Get("CurrentWorkingDirectory", "");
            string fileName = PdfGeneratorService.GenerateFileName("Leña");
            string finalPath = Path.Combine(folder, fileName);

            // Format the information to be displayed
            List<double> alturas = new List<double>();

            foreach(var item in model.Despacho.Alturas)
            {
                if(item is null)
                    alturas.Add(0);
                else
                    alturas.Add((double)item);
            }

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
                        .Component(new DocumentHeader("Venta Leña", ImagePath, TitleSize, model.Folio));

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
                             .Component(new CamionInfo("Datos Camión", SubtitleSize, FirstColumnSize, model.DatosCamion.Patente,
                             model.DatosCamion.Chofer, model.DatosCamion.RutChofer, model.DatosCamion.EmpresaTransportista));

                            // Detalles de Carga
                            x.Item().Column(col =>
                            {
                                // Title
                                col.Item()
                                   .Component(new SectionTitle("Detalle Despacho", SubtitleSize));

                                // Medidas
                                col.Item()
                                   .Table(table =>
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
                                           header.Cell().ColumnSpan(4).Element(HeaderCellStyle).Text("Medidas");
                                       });

                                       // Table Content
                                       // Table Content
                                       table.Cell().Element(SummaryCellStyle).Text("Largo Camión: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(model.Despacho.LargoCamion.ToString());
                                       table.Cell().Element(SummaryCellStyle).Text("N° de Bancos : ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(model.Despacho.Bancos.ToString());

                                       table.Cell().Element(SummaryCellStyle).Text("Altura 1: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(alturas[0].ToString());
                                       table.Cell().Element(SummaryCellStyle).Text("Altura 2: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(alturas[1].ToString());

                                       table.Cell().Element(SummaryCellStyle).Text("Altura 3: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(alturas[2].ToString());
                                       table.Cell().Element(SummaryCellStyle).Text("Altura 4: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(alturas[3].ToString());

                                       table.Cell().Element(SummaryCellStyle).Text("Altura 5: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(alturas[4].ToString());
                                       table.Cell().Element(SummaryCellStyle).Text("Altura 6: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(alturas[5].ToString());

                                       table.Cell().Element(SummaryCellStyle).Text("Altura 7: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(alturas[6].ToString());
                                       table.Cell().Element(SummaryCellStyle).Text("Altura 8: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(alturas[7].ToString());
                                   });
                                
                                // Palomera
                                col.Item()
                                   .Table(table =>
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
                                           header.Cell().ColumnSpan(4).Element(HeaderCellStyle).Text("Palomera");
                                       });

                                       // Table Content
                                       table.Cell().Element(SummaryCellStyle).Text("Ancho Palomera: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(model.Despacho.AnchoPalomera.ToString());
                                       table.Cell().Element(SummaryCellStyle).Text("Alto Palomera : ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(model.Despacho.AltoPalomera.ToString());
                                   });

                                // Resumen
                                col.Item()
                                   .Table(table =>
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
                                           header.Cell().ColumnSpan(4).Element(HeaderCellStyle).Text("Resumen Final");
                                       });

                                       // Table Content
                                       table.Cell().Element(SummaryCellStyle).Text("Altura Media: ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(model.Despacho.AlturaMedia.ToString());
                                       table.Cell().Element(SummaryCellStyle).Text("Total : ").SemiBold();
                                       table.Cell().Element(SummaryCellStyle).Text(model.Despacho.TotalMetrosLeña.ToString());
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

        public void GenerateMetroRumaPDF(MetroRumaViewModel model)
        {
            // Set the file name
            string folder = Preferences.Get("CurrentWorkingDirectory", "");
            string fileName = PdfGeneratorService.GenerateFileName("MetroRuma");
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
                        .Component(new DocumentHeader("Venta Leña", ImagePath, TitleSize, model.Folio));

                    page.Footer()
                        .Element(ComposeFooter);
                });
            }).GeneratePdf(finalPath);

            // Open the generated PDF
            //await Launcher.Default.OpenAsync(new OpenFileRequest(fileName, new ReadOnlyFile(finalPath)));
        }

        #region Helper Methods

        /// <summary>
        /// Static method that formats the name of the file to be generated.
        /// </summary>
        /// <param name="despacho">String that will help to identify which module generated the PDF</param>
        /// <returns></returns>
        public static string GenerateFileName(string despacho)
        {            
            // Get the current date and time
            string dateAsString = DateTime.Now.ToString("dd/MM/yy HH/mm/ss");

            // Remove the characters that we don't want in the file name
            string formattedDate = dateAsString.Replace(":", "").Replace(" ", "_").Replace("/", "-");

            // Return the formatted string
            return $"{despacho}_{formattedDate}.pdf";
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

        static IContainer SummaryHeaderCellStyle(IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold())
                            .PaddingVertical(5)
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
                          .PaddingRight(300)
                          .Row(row =>
                          {
                              row.RelativeItem()
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
                          .PaddingRight(300)
                          .Row(row =>
                          {
                              row.RelativeItem()
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

            public EspecieInfo(string title, int subtitle, int columnSize, string especie, string largo)
            {
                SubtitleSize = subtitle;
                FirstColumnSize = columnSize;
                Especie = especie;
                Largo = largo;
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
                        header.Cell().Element(SummaryHeaderCellStyle).Text("Volumen");
                    });

                    // Table Content based on the number of products

                    // Product 1
                    if(ViewModel.MedidasEspecieUno.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.EspecieUno} {ViewModel.LargoEspecieUno} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.TotalSumLista1.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.FinalTotalSumLista1.ToString("F2"));
                    }

                    // Product 2
                    if (ViewModel.MedidasEspecieDos.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.EspecieDos} {ViewModel.LargoEspecieDos} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.TotalSumLista2.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.FinalTotalSumLista2.ToString("F2"));
                    }

                    // Product 3
                    if (ViewModel.MedidasEspecieTres.Count > 0)
                    {
                        string producto = $"Trozo Aserrable {ViewModel.EspecieTres} {ViewModel.LargoEspecieTres} m.";
                        table.Cell().Element(SummaryCellStyle).Text(producto);
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.TotalSumLista3.ToString());
                        table.Cell().Element(SummaryCellStyle).Text(ViewModel.FinalTotalSumLista3.ToString("F2"));
                    }
                });
            }
        }
        #endregion
    }
}
