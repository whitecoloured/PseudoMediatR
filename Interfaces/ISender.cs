

namespace PseudoMediatR.Interfaces
{
    public interface ISender
    {
        /// <summary>
        /// Handles synchronous requests. Request handlers must implement IRequestHandler interface.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        TResponse Send<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>;
        /// <summary>
        /// Handles asynchronous requests. Request handlers must implement IAsyncRequestHandler interface.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken ct=default)
            where TRequest : IRequest<TResponse>;
    }
}
