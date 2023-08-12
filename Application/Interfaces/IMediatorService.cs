namespace Application.Interfaces
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMediatorService
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}