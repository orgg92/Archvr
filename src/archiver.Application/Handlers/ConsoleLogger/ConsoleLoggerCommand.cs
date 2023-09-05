namespace archiver.Application.Handlers.ConsoleLogger
{
    using MediatR;
    using System;
    using System.Threading;

    public class ConsoleLoggerCommand : IRequest<Unit>
    {
    }

    //public class ConsoleLoggerHandler : IRequestHandler<ConsoleLoggerCommand, Unit>
    //{
    //    Unit Handle(ConsoleLoggerCommand request, CancellationToken cancellationToken)
    //    {
    //        Console.WriteLine();

    //        //return Task.FromResult<Unit>;
            
    //    }
    //}
}
