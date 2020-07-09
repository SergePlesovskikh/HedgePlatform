using DinkToPdf;
using DinkToPdf.Contracts;
using HedgePlatform.BLL.Infr;
using HedgePlatform.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace HedgePlatform.BLL.Services
{
    public class PDFService : IPDFService
    {
        private IConverter _converter;
        private readonly ILogger _logger = Log.CreateLogger<PDFService>();
        
        public PDFService (IConverter converter)
        {
            _converter = converter;
        }

        public byte[] PdfConvert(string html)
        {
            try
            {
                var doc = GetDoc(html);
                byte[] pdf = _converter.Convert(doc);
                return pdf;
            }
            catch (Exception ex)
            {
                _logger.LogError("PDF convertation error");
                throw new ValidationException("PDF_ERROR", ex.Message);
            }
        }

        private static HtmlToPdfDocument GetDoc(string html)
        {
            return new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4Plus
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };
        }
    }
}
