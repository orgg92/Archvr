namespace Application.Common
{
    using Application.Handlers;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseHandlerPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse> 
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // find the logging announcement and write to console

            var handlerName = typeof(TRequest).Name;

            try
            {
                
                SharedContent.LogToConsole(SharedContent.ReturnMessageForHandler(handlerName));

                var response = await next();

                return response;
            }
            // in all handlers throw a ProgramException so that all exceptions produce an error code and a corresponding console message
            catch (ProgramException e)
            {
                SharedContent.LogToConsole(SharedContent.ReturnErrorMessageForErrorCode(e.ErrorCode), true);
                throw e;
            }
        }
    }

}
