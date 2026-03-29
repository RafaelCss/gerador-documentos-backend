using GeradorDocumentos.Dominio.Contrato;

namespace GeradorDocumentos.Infra.Servicos.FakeAI;
public class FakeAIService : IAIService
{
    public Task<string> ExtractAsync(string prompt)
    {
        // MOCK inicial (trocar por OpenAI depois)
        return Task.FromResult("{ \"nome\": \"Teste\", \"valor\": 100 }");
    }
}
