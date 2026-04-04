using GeradorDocumentos.Dominio.Contrato;

namespace GeradorDocumentos.Dominio.Comandos;

public record GerarContrachequeComando(
    // Empresa
    string NomeEmpresa,
    string CNPJEmpresa,
    string EnderecoEmpresa,

    // Competência
    string Competencia,   // ex: "Março/2025"

    // Funcionário
    string NomeFuncionario,
    string Matricula,
    string Cargo,
    string Departamento,
    string CPF,
    string PIS,
    DateOnly DataAdmissao,
    string CBO,

    // Rubricas
    IReadOnlyList<RubricaDto> Proventos,
    IReadOnlyList<RubricaDto> Descontos,

    // Totais
    decimal TotalProventos,
    decimal TotalDescontos,
    decimal SalarioLiquido,

    // Bases de cálculo
    decimal SalarioBase,
    decimal BaseINSS,
    decimal BaseFGTS,
    decimal BaseIRRF,
    decimal FGTSMes,
    decimal BancoHoras
) : IRequest<byte[]>;

public record RubricaDto(
    string Descricao,
    string Referencia,   // ex: "30 dias", "8%", "220 h"
    decimal Valor
);
