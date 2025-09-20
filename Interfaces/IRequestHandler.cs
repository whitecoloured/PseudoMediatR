namespace PseudoMediatR.Interfaces
{
    /// <summary>
    /// Base interface for synchronous request handlers
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles synchronous request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TResponse Handle(TRequest request);
    }
}
