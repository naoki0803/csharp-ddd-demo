using Microsoft.AspNetCore.Mvc;

namespace TodoApi;

[Route("api/Controller")]
public class UserController : ControllerBase
{
    private readonly UserRegisterService _registerService;
    private readonly UserGetService _getService;
    private readonly UserGetAllService _getAllService;
    private readonly UserUpdateService _updateService;
    private readonly UserDeleteService _deleteService;

    public UserController(
        UserRegisterService registerService,
        UserGetService getService,
        UserGetAllService getAllService,
        UserUpdateService updateService,
        UserDeleteService deleteService)
    {
        _registerService = registerService;
        _getService = getService;
        _getAllService = getAllService;
        _updateService = updateService;
        _deleteService = deleteService;
    }

    [HttpPost]
    public void Post([FromBody] UserPostRequestModel request)
    {
        var command = new UserRegisterCommand(request.Name);
        _registerService.Handle(command);
    }

    [HttpGet]
    public async Task<List<UserIndexResponseModel>> Index()
    {
        var result = await _getAllService.Handle();
        var users = result.Select(user => new UserIndexResponseModel(user.Id, user.Name)).ToList();
        return users;
    }
}
