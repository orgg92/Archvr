namespace archiver.Application.Common
{
    using archiver.Application.Handlers;
    using archiver.Application.Handlers.FileArchiver;
    using archiver.Application.Interfaces;
    using archiver.Core;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// announces messages to console and log file and handles exceptions 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    
    public class BaseHandlerPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> 
    {
        private readonly IConsoleService _consoleService;

        public BaseHandlerPipelineBehaviour(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var handlerName = typeof(TRequest).Name;

            try
            {
                // if request is to archive a file do not announce message in pipeline
                if (handlerName != HandlerNames.FileArchiverCommand.ToString())
                {
                    // find the logging announcement and write to console
                    var handlerMessage = SharedContent.ReturnMessageForHandler(handlerName);
                    await _consoleService.WriteToConsole($"{SharedContent.ReturnFormattedDateTimeToString()} {handlerMessage}");
                }

                var response = await next();

                return response;
            }

            // catch all ProgramExceptions to retrieve the corresponding error code message for user
            catch (ProgramException e)
            {
                await _consoleService.WriteToConsole
                    ($"{SharedContent.ReturnFormattedDateTimeToString()} {SharedContent.ReturnErrorMessageForErrorCode(e.ErrorCode.ToString())}");

                throw e;
            }
        }
    }

}
