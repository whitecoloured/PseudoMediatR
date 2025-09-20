using Microsoft.Extensions.DependencyInjection;

namespace PseudoMediatR.Implementations
{
    public sealed class Sender : ISender
    {
        private readonly IServiceProvider _servicePrvoider;
        public Sender(IServiceProvider serviceProvider)
        {
            _servicePrvoider = serviceProvider;
        }
        public TResponse Send<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            var handler = _servicePrvoider.GetService<IRequestHandler<TRequest, TResponse>>()
                ?? throw new ArgumentNullException("The request handler doesn't implement IRequestHandler<,> interface. Either you haven't registered the handler.");

            var response = handler.Handle(request);

            return response;

        }

        public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken ct=default)
            where TRequest : IRequest<TResponse>
        {

            var handler = _servicePrvoider.GetService<IAsyncRequestHandler<TRequest, TResponse>>()
                ?? throw new ArgumentNullException("The request handler doesn't implement IAsyncRequestHandler<,> interface. Either you haven't registered the handler.");

            var response = await handler.HandleAsync(request, ct);

            return response;
        }
    }
}
