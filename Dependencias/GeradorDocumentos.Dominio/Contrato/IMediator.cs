using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorDocumentos.Dominio.Contrato;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
}
