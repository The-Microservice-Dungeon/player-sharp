using Refit;
using Sharp.Infrastructure.Network.Model;

namespace Sharp.Infrastructure.Network.Client;

/// <summary>
///     Client which is responsible for commands
/// </summary>
public interface IGameCommandClient
{
    [Post("/commands")]
    Task<ComamndResponse> SendCommand([Body] CommandRequest request);
}