using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Svk.Shared.Misc;
using Svk.Shared.Users;
using Swashbuckle.AspNetCore.Annotations;

namespace Svk.Server.Controllers.User;

[ApiController]
[Route("Api/[controller]")]
[Authorize]
public class ManagerController : ControllerBase
{
    private readonly IManagerService managerService;


    public string? GetAuth0IdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var auth0IdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");


        var id = auth0IdClaim?.Value.Split('|').Last();
        return id;
    }

    public ManagerController(IManagerService managerService)
    {
        this.managerService = managerService;
    }

    [HttpGet]
    [SwaggerOperation(
            Summary = "Get a list of managers",
            Description = "Get a list of managers with pagination and filtering"
        )
    ]
    public async Task<Result.GetItemsPaginated<ManagerDto.Index>> GetIndex([FromQuery] UserRequest.Index request)
    {
        return await managerService.GetIndexAsync(request);
    }

    [HttpGet("{managerId}")]
    [SwaggerOperation(
            Summary = "Get a manager",
            Description = "Get a manager by id"
        )
    ]
    public async Task<ManagerDto.Detail> GetDetail(int managerId)
    {
        return await managerService.GetDetailAsync(managerId);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [SwaggerOperation(
            Summary = "Create a manager",
            Description = "Create a manager"
        )
    ]
    public async Task<int> Create(ManagerDto.Mutate model)
    {
        return await managerService.CreateAsync(model);
    }

    [HttpPut("{managerId}")]
    [Authorize(Roles = "Manager")]
    [SwaggerOperation(
            Summary = "Edit a manager",
            Description = "Edit a manager"
        )
    ]
    public async Task<IActionResult> Edit(int managerId, ManagerDto.Mutate model)
    {
        await managerService.EditAsync(managerId, model);
        return NoContent();
    }

    [HttpDelete("{managerId}")]
    [Authorize(Roles = "Manager")]
    [SwaggerOperation(
            Summary = "Delete a manager",
            Description = "Delete a manager"
        )
    ]
    public async Task<IActionResult> Delete(int managerId)
    {
        var manager = await managerService.GetDetailAsync(managerId);
        if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var bearerToken = authHeader.ToString().Substring("Bearer ".Length).Trim();
            var auth0Id = GetAuth0IdFromToken(bearerToken);
            if (manager.Auth0Id == auth0Id)
            {
                return Unauthorized();
            }
        }

        await managerService.DeleteAsync(managerId);

        return NoContent();
    }
}