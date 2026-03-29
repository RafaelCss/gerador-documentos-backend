using GeradorDocumentos.Dominio.Contrato;
using PuppeteerSharp;

namespace GeradorDocumentos.Infra.Servicos.Pdf;

public class PdfService : IPdfService
{
    public async Task<byte[]> GenerateAsync(string html)
    {
        await new BrowserFetcher().DownloadAsync();

        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });

        using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);

        return await page.PdfDataAsync();
    }
}
