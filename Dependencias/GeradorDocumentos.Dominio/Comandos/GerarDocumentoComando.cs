using GeradorDocumentos.Dominio.Contrato;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorDocumentos.Dominio.Comandos;


public record GerarDocumentoComando(
    string Nome ,
    string Documento ,
    decimal Valor
) : IRequest<byte[]>;

public record ExtractDataCommand(string Text) : IRequest<string>;

public class GenerateDocumentHandler(IPdfService pdfService) : IRequestHandler<GerarDocumentoComando , byte[]>
{
    private readonly IPdfService _pdfService = pdfService;

    public async ValueTask<byte[]> Handle(GerarDocumentoComando request , CancellationToken cancellationToken)
    {
        var html = $@"
        <html>
        <body>
            <h1>Recibo</h1>
            <p>Nome: {request.Nome}</p>
            <p>Documento: {request.Documento}</p>
            <p>Valor: R$ {request.Valor}</p>
        </body>
        </html>";

        return await _pdfService.GenerateAsync(html);
    }
}
public class ExtractDataHandler : IRequestHandler<ExtractDataCommand , string>
{
    private readonly IAIService _ai;

    public ExtractDataHandler(IAIService ai)
    {
        _ai = ai;
    }

    public async ValueTask<string> Handle(ExtractDataCommand request , CancellationToken cancellationToken)
    {
        var prompt = $@"
        Extraia Nome, CPF/CNPJ, Valor e Data.
        Retorne JSON válido.

        Texto:
        {request.Text}";

        return await _ai.ExtractAsync(prompt);
    }
}