using GeradorDocumentos.Dominio.Comandos;
using GeradorDocumentos.Dominio.Contrato;
using Microsoft.AspNetCore.Mvc;

namespace GeradorDocumentos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GerarDocumentoComando command)
    {
        var pdf = await _mediator.Send(command);

        return File(pdf , "application/pdf" , "recibo.pdf");
    }

    [HttpPost("extract")]
    public async Task<IActionResult> Extract(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Arquivo inválido");

        using var reader = new StreamReader(file.OpenReadStream());
        var text = await reader.ReadToEndAsync();

        var result = await _mediator.Send(new ExtractDataCommand(text));

        return Ok(result);
    }
} 

