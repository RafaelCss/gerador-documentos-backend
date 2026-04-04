
using GeradorDocumentos.Dominio.Contrato;

namespace GeradorDocumentos.Dominio.Comandos;

public record GerarReciboComando(
    // Empresa emissora
    string NomeEmpresa,
    string CNPJ,
    string EnderecoEmpresa,

    // Recibo
    string NumeroRecibo,
    DateOnly Data,

    // Pagador
    string NomePagador,
    string DocumentoPagador,
    string EnderecoPagador,
    string TelefonePagador,

    // Valor
    decimal Valor,
    string ValorExtenso,
    string Descricao,

    // Recebedor / assinatura
    string NomeRecebedor,
    string CargoRecebedor
) : IRequest<byte[]>;
