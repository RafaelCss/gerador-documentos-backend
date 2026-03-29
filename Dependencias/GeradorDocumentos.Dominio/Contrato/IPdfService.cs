using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorDocumentos.Dominio.Contrato;

public interface IPdfService
{
    Task<byte[]> GenerateAsync(string html);
}