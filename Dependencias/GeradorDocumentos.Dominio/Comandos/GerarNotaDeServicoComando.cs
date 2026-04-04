
using GeradorDocumentos.Dominio.Contrato;

namespace GeradorDocumentos.Dominio.Comandos;

public record GerarNotaDeServicoComando(
    // Prestador
    string NomePrestador,
    string SiglaPrestador,
    string CNPJPrestador,
    string EnderecoPrestador,
    string TelefonePrestador,
    string EmailPrestador,
    string InscricaoEstadual,
    string CargoResponsavel,

    // Tomador
    string NomeTomador,
    string DocumentoTomador,
    string EnderecoTomador,
    string TelefoneTomador,

    // Nota
    string NumeroNota,
    DateOnly DataEmissao,
    StatusNota Status,

    // Itens
    IReadOnlyList<ItemServicoDto> Itens,

    // Financeiro
    decimal Subtotal,
    decimal AliquotaISS,
    decimal ValorISS,
    decimal Desconto,
    decimal Total,

    // Extras
    string CondicoesPagamento,
    string Observacoes
) : IRequest<byte[]>;

public record ItemServicoDto(
    string Descricao,
    decimal Quantidade,
    string Unidade,
    decimal ValorUnitario,
    decimal ValorTotal
);

public enum StatusNota
{
    Pendente,
    Pago,
    Cancelado
}
