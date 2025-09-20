
namespace PseudoMediatR.Interfaces
{
    /// <summary>
    /// Base interface for asynchronous request handlers
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles asynchronous request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TResponse> HandleAsync(TRequest request, CancellationToken ct=default);
    }
}
