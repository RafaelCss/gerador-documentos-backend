using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorDocumentos.Dominio.Contrato;

public interface IAIService
{
    Task<string> ExtractAsync(string prompt);
}
