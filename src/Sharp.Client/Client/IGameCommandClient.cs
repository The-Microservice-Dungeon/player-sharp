using Refit;
using Sharp.Client.Model;

namespace Sharp.Client.Client;

/// <summary>
///     Client which is responsible for commands
/// </summary>
public interface IGameCommandClient
{
    [Post("/commands")]
    Task<ComamndResponse> SendCommand([Body] CommandRequest request);
}