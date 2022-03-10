using Microsoft.AspNetCore.Mvc;

namespace Player.Sharp.Controllers;

[Route("health")]
[ApiController]
public class ServiceHealthController : ControllerBase
{
    [HttpGet("status")]
    public ActionResult<StatusResponse> GetStatus()
    {
        return Ok(new StatusResponse("UP"));
    }
}

public class StatusResponse
{
    public StatusResponse(string status)
    {
        Status = status;
    }

    public string Status { get; set; }
}