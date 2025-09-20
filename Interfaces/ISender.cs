

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
        TResponse Send<TResponse>(IRequest<TResponse> request);
        /// <summary>
        /// Handles asynchronous requests. Request handlers must implement IAsyncRequestHandler interface.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct=default);
    }
}
