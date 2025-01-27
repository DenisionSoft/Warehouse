using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Business.Pallets;
using Warehouse.Web.Contracts.Models;
using Warehouse.Web.Contracts.Models.Pallet;
using Warehouse.Web.Contracts.Routes;

namespace Warehouse.Web.Controllers;

[ApiController]
public sealed class PalletController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IValidator<CreatePalletRequest> createPalletRequestValidator;
    private readonly IValidator<PaginationParams> paginationParamsValidator;
    private readonly IPalletService palletService;

    public PalletController(
        IMapper mapper,
        IPalletService palletService,
        IValidator<CreatePalletRequest> createPalletRequestValidator,
        IValidator<PaginationParams> paginationParamsValidator
        )
    {
        this.mapper = mapper;
        this.createPalletRequestValidator = createPalletRequestValidator;
        this.paginationParamsValidator = paginationParamsValidator;
        this.palletService = palletService;
    }

    [HttpGet(ApiRoute.Warehouse.Pallets)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllResponse<PalletResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var validationResult = await paginationParamsValidator.ValidateAsync(paginationParams, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var pallets = await palletService.GetAllAsync(paginationParams.Offset ?? 0, paginationParams.Limit ?? 10, cancellationToken);
        return Ok(new GetAllResponse<PalletResponse>(mapper.Map<List<PalletResponse>>(pallets), pallets.Count));
    }

    [HttpGet(ApiRoute.Warehouse.Pallet)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PalletResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetById([FromRoute] long palletId, CancellationToken cancellationToken)
    {
        var pallet = await palletService.GetByIdAsync(palletId, cancellationToken);
        return Ok(mapper.Map<PalletResponse>(pallet));
    }

    [HttpPost(ApiRoute.Warehouse.Pallets)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PalletResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] CreatePalletRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await createPalletRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var createPalletCommand = mapper.Map<CreatePalletCommand>(request);
        var pallet = await palletService.AddAsync(createPalletCommand, cancellationToken);
        return Created($"{Request.Path}/{pallet.Id}", mapper.Map<PalletResponse>(pallet));
    }

    [HttpPatch(ApiRoute.Warehouse.Pallet)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PalletResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(
        [FromRoute] long palletId,
        [FromBody] UpdatePalletRequest request,
        CancellationToken cancellationToken)
    {
        var updatePalletCommand = mapper.Map<UpdatePalletCommand>(request);
        var pallet = await palletService.UpdateAsync(palletId, updatePalletCommand, cancellationToken);
        return Ok(mapper.Map<PalletResponse>(pallet));
    }

    [HttpDelete(ApiRoute.Warehouse.Pallet)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] long palletId, CancellationToken cancellationToken)
    {
        await palletService.DeleteAsync(palletId, cancellationToken);
        return NoContent();
    }
}
