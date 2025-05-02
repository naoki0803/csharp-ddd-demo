using Microsoft.AspNetCore.Mvc;

namespace TodoApi;

[Route("api/Controller")]
public class UserController : ControllerBase
{
    private readonly UserApplicationService _userApplicationService;

    public UserController(UserApplicationService userApplicationService)
    {
        _userApplicationService = userApplicationService;
    }

    [HttpPost]
    public void Post([FromBody] UserPostRequestModel request)
    {
        var command = new UserRegisterCommand(request.Name);
        _userApplicationService.Register(command);
    }

}
