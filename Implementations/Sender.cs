using System.Reflection;

namespace PseudoMediatR.Implementations
{
    public sealed class Sender : ISender
    {
        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();

            var responseType = typeof(TResponse);

            var handlerType = Assembly.GetEntryAssembly()?.GetTypes()
                                .Where(p => p.GetInterfaces()
                                            .Contains(typeof(IRequestHandler<,>)
                                            .MakeGenericType(requestType, responseType)))
                                .FirstOrDefault();

            if (handlerType is null)
            {
                throw new ArgumentNullException("The request handler doesn't implement IRequestHandler interface.");
            }

            var handler = Activator.CreateInstance(handlerType);

            var methods = handlerType.GetMethods()
                            .Where(p => p.Name == "Handle")
                            .ToArray();


            if (methods.Length == 0)
            {
                throw new ArgumentNullException("The Handle() method wasn't implemented.");
            }

            if (methods.Length > 1)
            {
                throw new AmbiguousMatchException("The request handler can't implement Handle() method twice.");
            }

            var response = methods.First().Invoke(handler, [request]);

            if (response is null)
            {
                return default;
            }

            return (TResponse)response;

        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct=default)
        {
            var requestType = request.GetType();

            var responseType = typeof(TResponse);

            var handlerType = Assembly.GetEntryAssembly()?.GetTypes()
                                .Where(p => p.GetInterfaces()
                                            .Contains(typeof(IAsyncRequestHandler<,>)
                                            .MakeGenericType(requestType, responseType)))
                                .FirstOrDefault();

            if (handlerType is null)
            {
                throw new ArgumentNullException("The request handler doesn't implement IAsyncRequestHandler interface.");
            }

            var handler = Activator.CreateInstance(handlerType);

            var methods = handlerType.GetMethods()
                            .Where(p => p.Name == "HandleAsync")
                            .ToArray();

            if (methods.Length == 0)
            {
                throw new ArgumentNullException("The HandleAsync() method wasn't implemented.");
            }

            if (methods.Length > 1)
            {
                throw new AmbiguousMatchException("The request handler can't implement HandleAsync() method twice.");
            }

            var taskResult = methods.First().Invoke(handler, [request, ct]) as Task;

            if (taskResult is null)
            {
                throw new ArgumentNullException("Can't fetch the Task variable from the MethodInfo variable.");
            }

            await taskResult;

            var propertyInfo = taskResult?.GetType()?.GetProperty("Result");

            if (propertyInfo is null)
            {
                throw new ArgumentNullException("Can't fetch the Result attribute from the Task variable");
            }

            var response = propertyInfo?.GetValue(taskResult);

            if (response is null)
            {
                return default;
            }

            return (TResponse)response;
        }
    }
}
