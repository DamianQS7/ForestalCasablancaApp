using ForestalCasablancaApp.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public void GenerateTrozoAserrablePDF(TrozoAserrableViewModel model)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf(Preferences.Get("CurrentWorkingDirectory", "") + PdfGeneratorService.FormatFileName("TrozoAserrable"));
        }

        public void GenerateLeñaPDF(LeñaViewModel model)
        {
            throw new NotImplementedException();
        }

        public void GenerateMetroRumaPDF(MetroRumaViewModel model)
        {
            throw new NotImplementedException();
        }

        public static string FormatFileName(string despacho)
        {
            string dateAsString = DateTime.Now.ToString();

            dateAsString = dateAsString.Replace(":", "-");

            dateAsString = dateAsString.Replace(" ", "_");

            return $"/{despacho}_{dateAsString}.pdf";
        }
    }
}
