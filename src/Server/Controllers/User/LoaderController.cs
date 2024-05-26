using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Svk.Shared.Misc;
using Svk.Shared.Users;
using Swashbuckle.AspNetCore.Annotations;

namespace Svk.Server.Controllers.User;

[ApiController]
[Route("Api/[controller]")]
[Authorize]
public class LoaderController : ControllerBase
{
    private readonly ILoaderService loaderService;

    public LoaderController(ILoaderService loaderService)
    {
        this.loaderService = loaderService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get a list of loaders",
        Description = "Get a list of loaders with pagination and filtering")
    ]
    public async Task<Result.GetItemsPaginated<LoaderDto.Index>> GetIndex([FromQuery] UserRequest.Index request)
    {
        return await loaderService.GetIndexAsync(request);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [SwaggerOperation(
        Summary = "Create a loader",
        Description = "Create a loader"
        )
    ]
    public async Task<int> Create(LoaderDto.Mutate model)
    {
        return await loaderService.CreateAsync(model);
    }

    [HttpGet("{loaderId}")]
    [SwaggerOperation(
        Summary = "Get a loader",
        Description = "Get a loader by id"
        )
    ]
    public async Task<ActionResult<LoaderDto.Detail>> GetDetail(int loaderId)
    {
        return await loaderService.GetDetailAsync(loaderId);
    }

    [HttpGet("auth/{auth0Id}")]
    [SwaggerOperation(
        Summary = "Get a loader",
        Description = "Get a loader by auth0Id"
        )
    ]
    public async Task<ActionResult<LoaderDto.Detail>> GetDetailAuth(string auth0Id)
    {
        return await loaderService.GetDetailAsync(auth0Id);
    }


    [HttpPut("{loaderId}")]
    [Authorize(Roles = "Manager")]
    [SwaggerOperation(
        Summary = "Edit a loader",
        Description = "Edit a loader"
        )
    ]
    public async Task<IActionResult> Edit(int loaderId, LoaderDto.Mutate model)
    {
        await loaderService.EditAsync(loaderId, model);
        return NoContent();
    }

    [HttpDelete("{loaderId}")]
    [Authorize(Roles = "Manager")]
    [SwaggerOperation(
        Summary = "Delete a loader",
        Description = "Delete a loader"
        )
    ]
    public async Task<IActionResult> Delete(int loaderId)
    {
        await loaderService.DeleteAsync(loaderId);
        return NoContent();
    }
}