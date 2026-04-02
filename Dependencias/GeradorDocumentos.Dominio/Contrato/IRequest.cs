namespace GeradorDocumentos.Dominio.Contrato;

public interface IRequest<TResponse> { }
public interface IRequestHandler<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    ValueTask<TResponse> Handle(TRequest request , CancellationToken cancellationToken);
}
