using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Svk.Server.Util;
using Svk.Shared.Controles;
using Svk.Shared.Misc;
using Swashbuckle.AspNetCore.Annotations;

namespace Svk.Server.Controllers.Controle;

[ApiController]
[Route("Api/[controller]")]
[Authorize]
public class ControleController : ControllerBase
{
    private readonly IControleService controleService;

    private ILogger<ControleController> _logger;

    public ControleController(IControleService controleService, ILogger<ControleController> logger)
    {
        this.controleService = controleService;
        _logger = logger;
    }


    

    [HttpGet]
    [ProducesResponseType(typeof(Result.GetItemsPaginated<ControleDto.Index>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get a list of controles", Description = "Get a list of controles") ]
    public async Task<Result.GetItemsPaginated<ControleDto.Index>> GetIndex([FromQuery] ControleRequest.Index req)
    {
        return await controleService.GetIndexAsync(req);
    }

    [HttpGet("{routenummer}")]
    [SwaggerOperation(Summary = "Get a controle", Description = "Get a controle by routenummer")]
    public async Task<ControleDto.Detail> GetIndex([FromRoute] int routenummer)
    {
        return await controleService.GetDetailAsync(routenummer);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a controle",
        Description = "Create a controle")
    ]
    public async Task<IActionResult> Create([FromBody] ControleDto.Create model)
    {
        if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var bearerToken = authHeader.ToString().Substring("Bearer ".Length).Trim();
            var auth0Id = Utils.GetAuth0IdFromToken(bearerToken);
            model.auth0id = auth0Id;
        }

        var controleId = await controleService.CreateAsync(model);
        return CreatedAtAction(nameof(Create), controleId);
    }

    [HttpPut("{controleId}")]
    [SwaggerOperation(Summary = "Edit a controle", Description = "Edit a controle") ]
    public async Task<ControleResult.Edit> Edit([FromRoute] int controleId, [FromBody] ControleDto.Edit model)
    {
        return await controleService.EditAsync(controleId, model);
    }
}