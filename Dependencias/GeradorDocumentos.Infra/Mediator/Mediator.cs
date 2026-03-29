using GeradorDocumentos.Dominio.Contrato;


namespace GeradorDocumentos.Infra.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(request.GetType() , typeof(TResponse));

        dynamic handler = _provider.GetService(handlerType)!;

        return await handler.Handle((dynamic)request , CancellationToken.None);
    }
}
