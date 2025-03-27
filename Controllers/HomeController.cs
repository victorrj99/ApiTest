using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOwn.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    [Authorize]
    public IActionResult Get() 
        => Ok();
}