using LuxTravel.app.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LuxTravel.app.Services;

public class InvoiceService
{
    public void GenerateAgencyInvoice(Agency agency)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Segoe UI"));

                page.Header()
                    .Background(Colors.Blue.Darken3)
                    .Padding(20)
                    .Column(column =>
                    {
                        column.Item().AlignCenter().Text("LuxTravel")
                            .FontSize(28)
                            .Bold()
                            .FontColor(Colors.White);

                        column.Item().AlignCenter().Text("LUXURY TRAVEL EXPERIENCES")
                            .FontSize(9)
                            .FontColor(Colors.Blue.Lighten2);
                    });

                page.Content()
                    .PaddingVertical(20)
                    .Column(column =>
                    {
                        // Invoice Title
                        column.Item().PaddingBottom(10).Column(col =>
                        {
                            col.Item().Text("AGENCY INVOICE")
                                .FontSize(20)
                                .Bold()
                                .FontColor(Colors.Blue.Darken3);

                            col.Item().PaddingTop(3).Text($"Generated: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);
                        });

                        // Divider
                        column.Item().PaddingVertical(10).LineHorizontal(2).LineColor(Colors.Blue.Medium);

                        // Agency Information
                        column.Item().PaddingVertical(15).Background(Colors.Blue.Lighten4)
                            .Padding(15).Column(col =>
                            {
                                col.Item().Text("Agency Information")
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken3);

                                col.Item().PaddingTop(10).Row(row =>
                                {
                                    row.RelativeItem().Column(leftCol =>
                                    {
                                        leftCol.Item().Text($"Agency Name: {agency.Name}").FontSize(10);
                                        leftCol.Item().PaddingTop(3).Text($"Owner: {agency.Owner.UserName}").FontSize(10);
                                        leftCol.Item().PaddingTop(3).Text($"Location: {agency.City}, {agency.Country}").FontSize(10);
                                    });

                                    row.RelativeItem().Column(rightCol =>
                                    {
                                        rightCol.Item().Text($"Address: {agency.Address}").FontSize(10);
                                        rightCol.Item().PaddingTop(3).Text($"Established: {agency.CreatedAt:dd/MM/yyyy}").FontSize(10);
                                        rightCol.Item().PaddingTop(3).Text($"Total Tours: {agency.TotalToursCreated}").FontSize(10);
                                    });
                                });
                            });

                        // Financial Summary
                        column.Item().PaddingTop(20).Column(col =>
                        {
                            col.Item().Text("Financial Summary")
                                .FontSize(14)
                                .Bold()
                                .FontColor(Colors.Blue.Darken3);

                            // Summary Table
                            col.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                });

                                // Header
                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Blue.Darken2)
                                        .Padding(8)
                                        .Text("Description")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell().Background(Colors.Blue.Darken2)
                                        .Padding(8)
                                        .AlignRight()
                                        .Text("Amount (GEL)")
                                        .FontColor(Colors.White)
                                        .Bold();
                                });

                                // Rows
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).Text("Total Bookings");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).AlignRight().Text(agency.TotalBookings.ToString());

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).Text("Total Earnings");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).AlignRight().Text($"{agency.TotalEarnings:N2}");

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).Text("Current Balance");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).AlignRight().Text($"{agency.Balance:N2}");

                                table.Cell().Background(Colors.Blue.Lighten3)
                                    .Padding(8).Text("Average per Booking")
                                    .Bold();
                                table.Cell().Background(Colors.Blue.Lighten3)
                                    .Padding(8).AlignRight()
                                    .Text(agency.TotalBookings > 0
                                        ? $"{(agency.TotalEarnings / agency.TotalBookings):N2}"
                                        : "0.00")
                                    .Bold();
                            });
                        });

                        // Performance Metrics
                        column.Item().PaddingTop(20).Column(col =>
                        {
                            col.Item().Text("Performance Metrics")
                                .FontSize(14)
                                .Bold()
                                .FontColor(Colors.Blue.Darken3);

                            col.Item().PaddingTop(10).Row(row =>
                            {
                                row.RelativeItem().Padding(5).Background(Colors.Blue.Lighten4)
                                    .Padding(10).Column(metricCol =>
                                    {
                                        metricCol.Item().AlignCenter().Text("Tours Created")
                                            .FontSize(10).FontColor(Colors.Grey.Darken2);
                                        metricCol.Item().AlignCenter().PaddingTop(5).Text(agency.TotalToursCreated.ToString())
                                            .FontSize(20).Bold().FontColor(Colors.Blue.Darken3);
                                    });

                                row.RelativeItem().Padding(5).Background(Colors.Blue.Lighten4)
                                    .Padding(10).Column(metricCol =>
                                    {
                                        metricCol.Item().AlignCenter().Text("Total Bookings")
                                            .FontSize(10).FontColor(Colors.Grey.Darken2);
                                        metricCol.Item().AlignCenter().PaddingTop(5).Text(agency.TotalBookings.ToString())
                                            .FontSize(20).Bold().FontColor(Colors.Blue.Darken3);
                                    });

                                row.RelativeItem().Padding(5).Background(Colors.Blue.Lighten4)
                                    .Padding(10).Column(metricCol =>
                                    {
                                        metricCol.Item().AlignCenter().Text("Avg Occupancy")
                                            .FontSize(10).FontColor(Colors.Grey.Darken2);
                                        metricCol.Item().AlignCenter().PaddingTop(5)
                                            .Text(agency.TotalToursCreated > 0
                                                ? $"{(agency.TotalBookings * 100.0 / agency.TotalToursCreated):N0}%"
                                                : "N/A")
                                            .FontSize(20).Bold().FontColor(Colors.Blue.Darken3);
                                    });
                            });
                        });

                        // Notes Section
                        column.Item().PaddingTop(20).Column(col =>
                        {
                            col.Item().BorderTop(1).BorderColor(Colors.Grey.Lighten1)
                                .PaddingTop(10)
                                .Text("Notes")
                                .FontSize(10)
                                .Italic()
                                .FontColor(Colors.Grey.Darken2);

                            col.Item().PaddingTop(5).Text("This invoice reflects all financial activities since agency establishment. " +
                                "For detailed transaction history, please contact LuxTravel support.")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken1);
                        });
                    });

                page.Footer()
                    .Background(Colors.Grey.Lighten3)
                    .Padding(10)
                    .Column(column =>
                    {
                        column.Item().AlignCenter().Text("LuxTravel")
                            .FontSize(9)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        column.Item().AlignCenter().Text("123 Travel Lane, Suite 100, New York, NY 10001")
                            .FontSize(7)
                            .FontColor(Colors.Grey.Darken2);

                        column.Item().AlignCenter().Text("support@luxtravel.com | +1 (555) 123-4567")
                            .FontSize(7)
                            .FontColor(Colors.Grey.Darken2);
                    });
            });
        });

        // Generate PDF
        string fileName = $"Invoice_{agency.Name.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        string folderPath = @"C:\Users\Giorgi\Desktop\TravelAgencyLogging";

        // Create directory if it doesn't exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fullPath = Path.Combine(folderPath, fileName);
        document.GeneratePdf(fullPath);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Invoice generated successfully!");
        Console.WriteLine($"📄 Saved to: {folderPath}");
        Console.ResetColor();

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
        Console.Clear();
        return;
    }
}
