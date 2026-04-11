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
    public async Task<IActionResult> Generate([FromBody] GerarDocumentoComando command , CancellationToken cancellationToken)
    {
        var pdf = await _mediator.Send(command, cancellationToken);

        return File(pdf , "application/pdf" , "recibo.pdf");
    }


    /// <summary>
    /// Gera um recibo em PDF.
    /// </summary>
    [HttpPost("recibo")]
    [ProducesResponseType(typeof(FileContentResult) , StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GerarRecibo(
        [FromBody] GerarReciboComando command ,
        CancellationToken cancellationToken)
    {
        var pdf = await _mediator.Send(command , cancellationToken);

        var fileName = $"recibo-{command.NumeroRecibo}-{command.Data:yyyy-MM-dd}.pdf";

        return File(pdf , "application/pdf" , fileName);
    }

    /// <summary>
    /// Gera uma nota de serviço em PDF.
    /// </summary>
    [HttpPost("nota-servico")]
    [ProducesResponseType(typeof(FileContentResult) , StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GerarNotaDeServico(
        [FromBody] GerarNotaDeServicoComando command ,
        CancellationToken cancellationToken)
    {
        var pdf = await _mediator.Send(command , cancellationToken);

        var fileName = $"nota-servico-{command.NumeroNota}-{command.DataEmissao:yyyy-MM-dd}.pdf";

        return File(pdf , "application/pdf" , fileName);
    }

    /// <summary>
    /// Gera um contracheque em PDF.
    /// </summary>
    [HttpPost("contracheque")]
    [ProducesResponseType(typeof(FileContentResult) , StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GerarContracheque(
        [FromBody] GerarContrachequeComando command ,
        CancellationToken cancellationToken)
    {
        var pdf = await _mediator.Send(command , cancellationToken);

        var fileName = $"contracheque-{command.Matricula}-{command.Competencia}.pdf";

        return File(pdf , "application/pdf" , fileName);
    }

    /// <summary>
    /// Gera um contrato de compra e venda em PDF.
    /// </summary>
    [HttpPost("contrato-compra-venda")]
    [ProducesResponseType(typeof(FileContentResult) , StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GerarContratoCompraVenda(
        [FromBody] GerarContratoCompraVendaComando command ,
        CancellationToken cancellationToken)
    {
        var pdf = await _mediator.Send(command , cancellationToken);

        var fileName = $"contrato-{command.NumeroContrato}-{command.DataAssinatura:yyyy-MM-dd}.pdf";

        return File(pdf , "application/pdf" , fileName);
    }
    [HttpPost("extract")]
    public async Task<IActionResult> Extract(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Arquivo inválido");

        using var reader = new StreamReader(file.OpenReadStream());
        var text = await reader.ReadToEndAsync(cancellationToken);

        var result = await _mediator.Send(new ExtractDataCommand(text),cancellationToken);

        return Ok(result);
    }

} 

