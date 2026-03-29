using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorDocumentos.Dominio.Entidades;

public class Documento
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string DocumentoNumero { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
