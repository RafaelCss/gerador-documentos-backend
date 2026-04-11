using GeradorDocumentos.Dominio.Contrato;

namespace GeradorDocumentos.Dominio.Comandos;

public record GerarContratoCompraVendaComando(
// Identificação do contrato
string NumeroContrato ,

// Vendedor
string NomeVendedor ,
string NacionalidadeVendedor ,
string EstadoCivilVendedor ,
string ProfissaoVendedor ,
string DocumentoVendedor ,
string RGVendedor ,
string EnderecoVendedor ,
string CEPVendedor ,

// Comprador
string NomeComprador ,
string NacionalidadeComprador ,
string EstadoCivilComprador ,
string ProfissaoComprador ,
string DocumentoComprador ,
string RGComprador ,
string EnderecoComprador ,
string CEPComprador ,

// Objeto / bem
string TipoBem ,
string DescricaoBem ,
string MarcaModelo ,
string IdentificacaoBem ,
string EstadoConservacao ,
string ObservacoesBem ,

// Valor
decimal Valor ,
string ValorExtenso ,

// Pagamento
decimal ValorEntrada ,
string EntradaExtenso ,
string FormaPagamentoEntrada ,
decimal ValorSaldo ,
string SaldoExtenso ,
int ParcelasRestantes ,
decimal ValorParcela ,
DateOnly DataPrimeiraParcela ,
string FormaPagamentoSaldo ,

// Entrega
DateOnly DataEntrega ,
string LocalEntrega ,
string ResponsavelFrete ,  

// Garantia
string PrazoGarantia ,
string DescricaoGarantia ,

// Rescisão
int PrazoNotificacaoRescisao ,
int PercentualMultaRescisao ,

// Foro
string ComarcaForo ,
string EstadoForo ,

// Assinatura
string CidadeAssinatura ,
DateOnly DataAssinatura ,

// Testemunhas
string NomeTestemunha1 ,
string CPFTestemunha1 ,
string NomeTestemunha2 ,
string CPFTestemunha2
) : IRequest<byte[]>;
