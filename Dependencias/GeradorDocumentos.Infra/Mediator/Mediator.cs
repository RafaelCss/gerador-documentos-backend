using GeradorDocumentos.Dominio.Contrato;


namespace GeradorDocumentos.Infra.Mediator;

public class Mediator(IServiceProvider provider) : IMediator
{
    private readonly IServiceProvider _provider = provider;

    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request ,
        CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(request.GetType() , typeof(TResponse));

        dynamic handler = _provider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler não encontrado para {handlerType}");

        return await handler.Handle((dynamic)request , cancellationToken);
    }
}