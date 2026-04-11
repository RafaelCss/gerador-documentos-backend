using GeradorDocumentos.Dominio.Comandos;
using GeradorDocumentos.Dominio.Contrato;
using System.Text;

/// <summary>
/// Carrega um template HTML da pasta Templates/ e substitui as variáveis {{Variavel}}.
/// Compatível com qualquer IPdfService que aceite HTML como string.
/// </summary>
public class DocumentoTemplateHandle
{
    private readonly ITemplateProvider _templateProvider;

    public DocumentoTemplateHandle(ITemplateProvider templateProvider)
    {
        _templateProvider = templateProvider;
    }

    public async Task<string> RenderAsync(
        string templateName ,
        Dictionary<string , string> variables)
    {
        var html = await _templateProvider.ObterTemplateAsync($"{templateName}.html");

        foreach (var (key , value) in variables)
            html = html.Replace($"{{{{{key}}}}}" , value ?? string.Empty);

        return html;
    }
}
// RECIBO
public class GerarReciboHandler(IPdfService pdfService , DocumentoTemplateHandle documentTemplate) : IRequestHandler<GerarReciboComando, byte[]>
{
    public async ValueTask<byte[]> Handle(GerarReciboComando req, CancellationToken ct)
    {
        var html = await documentTemplate.RenderAsync("Recibo", new()
        {
            ["NomeEmpresa"]      = req.NomeEmpresa,
            ["CNPJ"]             = req.CNPJ,
            ["EnderecoEmpresa"]  = req.EnderecoEmpresa,
            ["NumeroRecibo"]     = req.NumeroRecibo,
            ["NomePagador"]      = req.NomePagador,
            ["DocumentoPagador"] = req.DocumentoPagador,
            ["EnderecoPagador"]  = req.EnderecoPagador,
            ["TelefonePagador"]  = req.TelefonePagador,
            ["Valor"]            = req.Valor.ToString("N2"),
            ["ValorExtenso"]     = req.ValorExtenso,
            ["Descricao"]        = req.Descricao,
            ["Data"]             = req.Data.ToString("dd/MM/yyyy"),
            ["DataEmissao"]      = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
            ["NomeRecebedor"]    = req.NomeRecebedor,
            ["CargoRecebedor"]   = req.CargoRecebedor,
        });

        return await pdfService.GenerateAsync(html);
    }
}

// NOTA DE SERVIÇO
public class GerarNotaHandler(IPdfService pdfService , DocumentoTemplateHandle documentTemplate) : IRequestHandler<GerarNotaDeServicoComando , byte[]>
{
    public async ValueTask<byte[]> Handle(GerarNotaDeServicoComando req , CancellationToken ct)
    {
        // Gera as linhas da tabela de itens dinamicamente
        var itensHtml = new StringBuilder();
        foreach (var item in req.Itens)
        {
            itensHtml.AppendLine($"""
                <tr>
                  <td>{item.Descricao}</td>
                  <td>{item.Quantidade}</td>
                  <td>{item.Unidade}</td>
                  <td>{item.ValorUnitario:N2}</td>
                  <td>{item.ValorTotal:N2}</td>
                </tr>
            """);
        }

        var html =  await documentTemplate.RenderAsync("NotaDeServico", new()
        {
            ["SiglaPrestador"]           = req.SiglaPrestador,
            ["NomePrestador"]            = req.NomePrestador,
            ["CNPJPrestador"]            = req.CNPJPrestador,
            ["EnderecoPrestador"]        = req.EnderecoPrestador,
            ["TelefonePrestador"]        = req.TelefonePrestador,
            ["EmailPrestador"]           = req.EmailPrestador,
            ["InscricaoEstadualPrestador"] = req.InscricaoEstadual,
            ["CargoResponsavel"]         = req.CargoResponsavel,
            ["NomeTomador"]              = req.NomeTomador,
            ["DocumentoTomador"]         = req.DocumentoTomador,
            ["EnderecoTomador"]          = req.EnderecoTomador,
            ["TelefoneTomador"]          = req.TelefoneTomador,
            ["NumeroNota"]               = req.NumeroNota,
            ["DataEmissao"]              = req.DataEmissao.ToString("dd/MM/yyyy"),
            ["StatusClass"]              = req.Status.ToString().ToLower(),   
            ["StatusTexto"]              = req.Status.ToString(),
            ["{{#each Itens}}{{/each}}"] = itensHtml.ToString(), 
            ["Subtotal"]                 = req.Subtotal.ToString("N2"),
            ["AliquotaISS"]              = req.AliquotaISS.ToString("F1"),
            ["ValorISS"]                 = req.ValorISS.ToString("N2"),
            ["Desconto"]                 = req.Desconto.ToString("N2"),
            ["Total"]                    = req.Total.ToString("N2"),
            ["CondicoesPagamento"]       = req.CondicoesPagamento,
            ["Observacoes"]              = req.Observacoes,
        });

        return await pdfService.GenerateAsync(html);
    }
}

// CONTRACHEQUE
public class GerarContrachequeHandler(IPdfService pdfService, DocumentoTemplateHandle documentTemplate) : IRequestHandler<GerarContrachequeComando, byte[]>
{
    public async ValueTask<byte[]> Handle(GerarContrachequeComando req, CancellationToken ct)
    {
        var proventosHtml  = BuildRubricaRows(req.Proventos);
        var descontosHtml  = BuildRubricaRows(req.Descontos);

        var html = await documentTemplate.RenderAsync("Contracheque", new()
        {
            ["NomeEmpresa"]      = req.NomeEmpresa,
            ["CNPJEmpresa"]      = req.CNPJEmpresa,
            ["EnderecoEmpresa"]  = req.EnderecoEmpresa,
            ["Competencia"]      = req.Competencia,               
            ["NomeFuncionario"]  = req.NomeFuncionario,
            ["Matricula"]        = req.Matricula,
            ["Cargo"]            = req.Cargo,
            ["Departamento"]     = req.Departamento,
            ["CPFFuncionario"]   = req.CPF,
            ["PIS"]              = req.PIS,
            ["DataAdmissao"]     = req.DataAdmissao.ToString("dd/MM/yyyy"),
            ["CBO"]              = req.CBO,
            ["TotalProventos"]   = req.TotalProventos.ToString("N2"),
            ["TotalDescontos"]   = req.TotalDescontos.ToString("N2"),
            ["SalarioLiquido"]   = req.SalarioLiquido.ToString("N2"),
            ["BaseINSS"]         = req.BaseINSS.ToString("N2"),
            ["BaseFGTS"]         = req.BaseFGTS.ToString("N2"),
            ["BaseIRRF"]         = req.BaseIRRF.ToString("N2"),
            ["FGTSMes"]          = req.FGTSMes.ToString("N2"),
            ["SalarioBase"]      = req.SalarioBase.ToString("N2"),
            ["BancoHoras"]       = req.BancoHoras.ToString("N1"),

            ["{{#each Proventos}}{{/each}}"] = proventosHtml,
            ["{{#each Descontos}}{{/each}}"] = descontosHtml,
        });

        return await pdfService.GenerateAsync(html);
    }

    private static string BuildRubricaRows(IEnumerable<RubricaDto> items)
    {
        var sb = new StringBuilder();
        var i  = 0;
        foreach (var item in items)
        {
            var zebraClass = i % 2 == 0 ? "zebra" : "";
            sb.AppendLine($"""
                <tr class="{zebraClass}">
                  <td>{item.Descricao}</td>
                  <td>{item.Referencia}</td>
                  <td>{item.Valor:N2}</td>
                </tr>
            """);
            i++;
        }
        return sb.ToString();
    }
}



public class GerarContratoCompraVendaHandler(IPdfService pdfService , DocumentoTemplateHandle documentTemplate)
    : IRequestHandler<GerarContratoCompraVendaComando , byte[]>
{

    public async ValueTask<byte[]> Handle(
        GerarContratoCompraVendaComando req ,
        CancellationToken cancellationToken)
    {
        var html = await documentTemplate.RenderAsync("ContratoCompraVenda" , new()
        {
            // Identificação
            ["NumeroContrato"] = req.NumeroContrato ,
            ["DataEmissao"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm") ,

            // Vendedor
            ["NomeVendedor"] = req.NomeVendedor ,
            ["NacionalidadeVendedor"] = req.NacionalidadeVendedor ,
            ["EstadoCivilVendedor"] = req.EstadoCivilVendedor ,
            ["ProfissaoVendedor"] = req.ProfissaoVendedor ,
            ["DocumentoVendedor"] = req.DocumentoVendedor ,
            ["RGVendedor"] = req.RGVendedor ,
            ["EnderecoVendedor"] = req.EnderecoVendedor ,
            ["CEPVendedor"] = req.CEPVendedor ,

            // Comprador
            ["NomeComprador"] = req.NomeComprador ,
            ["NacionalidadeComprador"] = req.NacionalidadeComprador ,
            ["EstadoCivilComprador"] = req.EstadoCivilComprador ,
            ["ProfissaoComprador"] = req.ProfissaoComprador ,
            ["DocumentoComprador"] = req.DocumentoComprador ,
            ["RGComprador"] = req.RGComprador ,
            ["EnderecoComprador"] = req.EnderecoComprador ,
            ["CEPComprador"] = req.CEPComprador ,

            // Bem
            ["TipoBem"] = req.TipoBem ,
            ["DescricaoBem"] = req.DescricaoBem ,
            ["MarcaModelo"] = req.MarcaModelo ,
            ["IdentificacaoBem"] = req.IdentificacaoBem ,
            ["EstadoConservacao"] = req.EstadoConservacao ,
            ["ObservacoesBem"] = req.ObservacoesBem ,

            // Valor
            ["Valor"] = req.Valor.ToString("N2") ,
            ["ValorExtenso"] = req.ValorExtenso ,

            // Pagamento
            ["ValorEntrada"] = req.ValorEntrada.ToString("N2") ,
            ["EntradaExtenso"] = req.EntradaExtenso ,
            ["FormaPagamentoEntrada"] = req.FormaPagamentoEntrada ,
            ["ValorSaldo"] = req.ValorSaldo.ToString("N2") ,
            ["SaldoExtenso"] = req.SaldoExtenso ,
            ["ParcelasRestantes"] = req.ParcelasRestantes.ToString() ,
            ["ValorParcela"] = req.ValorParcela.ToString("N2") ,
            ["DataPrimeiraParcela"] = req.DataPrimeiraParcela.ToString("dd/MM/yyyy") ,
            ["FormaPagamentoSaldo"] = req.FormaPagamentoSaldo ,

            // Entrega
            ["DataEntrega"] = req.DataEntrega.ToString("dd/MM/yyyy") ,
            ["LocalEntrega"] = req.LocalEntrega ,
            ["ResponsavelFrete"] = req.ResponsavelFrete ,

            // Garantia
            ["PrazoGarantia"] = req.PrazoGarantia ,
            ["DescricaoGarantia"] = req.DescricaoGarantia ,

            // Rescisão
            ["PrazoNotificacaoRescisao"] = req.PrazoNotificacaoRescisao.ToString() ,
            ["PercentualMultaRescisao"] = req.PercentualMultaRescisao.ToString() ,

            // Foro
            ["ComarcaForo"] = req.ComarcaForo ,
            ["EstadoForo"] = req.EstadoForo ,

            // Assinatura
            ["CidadeAssinatura"] = req.CidadeAssinatura ,
            ["DataAssinatura"] = req.DataAssinatura.ToString("dd 'de' MMMM 'de' yyyy" ,
                                            new System.Globalization.CultureInfo("pt-BR")) ,

            // Testemunhas
            ["NomeTestemunha1"] = req.NomeTestemunha1 ,
            ["CPFTestemunha1"] = req.CPFTestemunha1 ,
            ["NomeTestemunha2"] = req.NomeTestemunha2 ,
            ["CPFTestemunha2"] = req.CPFTestemunha2 ,
        });

        return await pdfService.GenerateAsync(html);
    }
}