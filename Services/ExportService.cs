using ClosedXML.Excel;
using okumatakibisanalkutuphane.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace okumatakibisanalkutuphane.Services
{
    public class ExportService
    {
        public byte[] GenerateExcel(List<Book> books)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Kitap Listesi");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Başlık";
                worksheet.Cell(1, 3).Value = "Yazar";
                worksheet.Cell(1, 4).Value = "Tür";
                worksheet.Cell(1, 5).Value = "Toplam Sayfa";
                worksheet.Cell(1, 6).Value = "Okunan Sayfa";
                worksheet.Cell(1, 7).Value = "Durum";

                var range = worksheet.Range(1, 1, 1, 7);
                range.Style.Font.Bold = true;
                range.Style.Fill.BackgroundColor = XLColor.LightBlue;

                for (int i = 0; i < books.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = books[i].Id;
                    worksheet.Cell(i + 2, 2).Value = books[i].Title;
                    worksheet.Cell(i + 2, 3).Value = books[i].Author;
                    worksheet.Cell(i + 2, 4).Value = books[i].Genre;
                    worksheet.Cell(i + 2, 5).Value = books[i].TotalPages;
                    worksheet.Cell(i + 2, 6).Value = books[i].CurrentPage;
                    worksheet.Cell(i + 2, 7).Value = books[i].Status;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] GeneratePdf(List<Book> books)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Text("LibrisTrack Pro - Kitap Listesi")
                        .SemiBold().FontSize(20).FontColor(Colors.Indigo.Medium);

                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(60);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID");
                            header.Cell().Element(CellStyle).Text("Başlık");
                            header.Cell().Element(CellStyle).Text("Yazar");
                            header.Cell().Element(CellStyle).Text("Tür");
                            header.Cell().Element(CellStyle).Text("Sayfa");
                            header.Cell().Element(CellStyle).Text("Durum");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                            }
                        });

                        foreach (var book in books)
                        {
                            table.Cell().Element(CellStyle).Text(book.Id.ToString());
                            table.Cell().Element(CellStyle).Text(book.Title);
                            table.Cell().Element(CellStyle).Text(book.Author);
                            table.Cell().Element(CellStyle).Text(book.Genre);
                            table.Cell().Element(CellStyle).Text($"{book.CurrentPage}/{book.TotalPages}");
                            table.Cell().Element(CellStyle).Text(book.Status);

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
