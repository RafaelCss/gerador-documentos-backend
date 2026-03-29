namespace GeradorDocumentos.Dominio.Contrato;

public interface IAIService
{
    Task<string> ExtractAsync(string prompt);
}
