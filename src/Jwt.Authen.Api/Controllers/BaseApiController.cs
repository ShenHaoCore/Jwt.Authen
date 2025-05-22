using Microsoft.AspNetCore.Mvc;

namespace Jwt.Authen.Api.Controllers;


/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class BaseApiController : ControllerBase
{

}
