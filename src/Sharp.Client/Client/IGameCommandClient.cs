using Refit;
using Sharp.Client.Model;

namespace Sharp.Client.Client;

public interface IGameCommandClient
{
    [Post("/commands")]
    Task<ComamndResponse> SendCommand([Body] CommandRequest request);
}