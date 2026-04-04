using GeradorDocumentos.Dominio.Comandos;
using GeradorDocumentos.Dominio.Contrato;
using System.Text;

/// <summary>
/// Carrega um template HTML da pasta Templates/ e substitui as variáveis {{Variavel}}.
/// Compatível com qualquer IPdfService que aceite HTML como string.
/// </summary>
public static class DocumentTemplate
{
    private static readonly string TemplatesPath =
        Path.Combine(AppContext.BaseDirectory, "C:\\Projetos\\GeradorDocumentos\\Dependencias\\GeradorDocumentos.Dominio\\Templates\\");

    /// <summary>
    /// Carrega o arquivo de template e faz substituição simples de variáveis.
    /// </summary>
    /// <param name="templateName">Nome do arquivo sem extensão, ex: "Recibo"</param>
    /// <param name="variables">Dicionário com chave = nome da variável (sem {{}}), valor = conteúdo</param>
    public static string Render(string templateName, Dictionary<string, string> variables)
    {
        var path = Path.Combine(TemplatesPath, $"{templateName}.html");

        if (!File.Exists(path))
            throw new FileNotFoundException($"Template '{templateName}.html' não encontrado em {TemplatesPath}");

        var html = File.ReadAllText(path, Encoding.UTF8);

        foreach (var (key, value) in variables)
            html = html.Replace($"{{{{{key}}}}}", value ?? string.Empty);

        return html;
    }
}
// RECIBO
public class GerarReciboHandler(IPdfService pdfService) : IRequestHandler<GerarReciboComando, byte[]>
{
    public async ValueTask<byte[]> Handle(GerarReciboComando req, CancellationToken ct)
    {
        var html = DocumentTemplate.Render("Recibo", new()
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
public class GerarNotaHandler(IPdfService pdfService) : IRequestHandler<GerarNotaDeServicoComando , byte[]>
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

        var html = DocumentTemplate.Render("NotaDeServico", new()
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
            ["StatusClass"]              = req.Status.ToString().ToLower(),   // "pendente" | "pago" | "cancelado"
            ["StatusTexto"]              = req.Status.ToString(),
            ["{{#each Itens}}{{/each}}"] = itensHtml.ToString(), // substituição direta do bloco
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
public class GerarContrachequeHandler(IPdfService pdfService) : IRequestHandler<GerarContrachequeComando, byte[]>
{
    public async ValueTask<byte[]> Handle(GerarContrachequeComando req, CancellationToken ct)
    {
        var proventosHtml  = BuildRubricaRows(req.Proventos);
        var descontosHtml  = BuildRubricaRows(req.Descontos);

        var html = DocumentTemplate.Render("Contracheque", new()
        {
            ["NomeEmpresa"]      = req.NomeEmpresa,
            ["CNPJEmpresa"]      = req.CNPJEmpresa,
            ["EnderecoEmpresa"]  = req.EnderecoEmpresa,
            ["Competencia"]      = req.Competencia,               // ex: "Março/2025"
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
            // Substitua os blocos {{#each}} com HTML gerado
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
