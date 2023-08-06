namespace Application.Common
{
    using Application.Handlers;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProgramNotificationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse> 
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // any request that throws an error gets reported in the same format here

            var handlerName = typeof(TRequest).Name;

            try
            {
                Console.WriteLine(SharedContent.ResponsiveSpacer);
                Console.WriteLine(SharedContent.HandlerLoggingMessages.Where(y => y.Name == handlerName).Select(y => y.Message).First());

                var response = await next();

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

}
