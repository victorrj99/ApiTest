using Microsoft.AspNetCore.Mvc;

namespace ApiOwn.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get() 
        => Ok();
}