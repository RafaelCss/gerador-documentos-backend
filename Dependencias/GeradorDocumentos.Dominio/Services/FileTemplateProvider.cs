using GeradorDocumentos.Dominio.Contrato;
using Microsoft.Extensions.Hosting;

namespace GeradorDocumentos.Dominio.Services;

public class FileTemplateProvider(IHostEnvironment env) : ITemplateProvider
{
    private readonly string _templatesPath = Path.Combine(env.ContentRootPath , "Templates");

    public async Task<string> ObterTemplateAsync(string nomeTemplate)
    {
        var caminho = Path.Combine(_templatesPath , nomeTemplate);
        return await File.ReadAllTextAsync(caminho);
    }
}
