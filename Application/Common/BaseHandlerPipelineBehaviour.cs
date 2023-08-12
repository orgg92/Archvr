﻿namespace Application.Common
{
    using Application.Handlers;
    using Application.Interfaces;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseHandlerPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse> 
    {
        private readonly IConsoleService _consoleService;

        public BaseHandlerPipelineBehaviour(IConsoleService consoleService)
        {
            _consoleService = consoleService;
                    }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // find the logging announcement and write to console

            var handlerName = typeof(TRequest).Name;

            try
            {
                
                _consoleService.WriteToConsole($"[{DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm")}] {SharedContent.ReturnMessageForHandler(handlerName)}");

                var response = await next();

                return response;
            }
            // in all handlers throw a ProgramException so that all exceptions produce an error code and a corresponding console message
            catch (ProgramException e)
            {
                _consoleService.WriteToConsole($"[{DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm")}] {SharedContent.ReturnErrorMessageForErrorCode(e.ErrorCode)}");
                throw e;
            }
        }
    }

}
