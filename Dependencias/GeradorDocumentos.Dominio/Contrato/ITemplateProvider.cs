namespace GeradorDocumentos.Dominio.Contrato;

public interface ITemplateProvider
{
    Task<string> ObterTemplateAsync(string nomeTemplate);
}
