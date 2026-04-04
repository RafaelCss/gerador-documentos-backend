using GeradorDocumentos.Dominio.Contrato;
using GeradorDocumentos.Dominio.Services;
using GeradorDocumentos.Infra.Servicos.FakeAI;
using GeradorDocumentos.Infra.Servicos.Pdf;
using Microsoft.Extensions.DependencyInjection;

namespace GeradorDocumento.Ioc.Dominio;

public static class ConfiguraDominioExtensao
{
    public static IServiceCollection  AddConfiguraDominio(this IServiceCollection services)
    {
        services.AddScoped<IPdfService , PdfService>();
        services.AddScoped<IAIService , FakeAIService>();
        services.AddScoped<ITemplateProvider , FileTemplateProvider>();
        services.AddScoped<DocumentoTemplateHandle>();
        return services;

    }
}
