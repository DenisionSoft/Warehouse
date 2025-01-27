using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Business.Boxes;
using Warehouse.Web.Contracts.Models;
using Warehouse.Web.Contracts.Models.Box;
using Warehouse.Web.Contracts.Routes;

namespace Warehouse.Web.Controllers;

[ApiController]
public sealed class BoxController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IValidator<CreateBoxRequest> createBoxRequestValidator;
    private readonly IValidator<UpdateBoxRequest> updateBoxRequestValidator;
    private readonly IValidator<PaginationParams> paginationParamsValidator;
    private readonly IBoxService boxService;

    public BoxController(
        IMapper mapper,
        IBoxService boxService,
        IValidator<CreateBoxRequest> createBoxRequestValidator,
        IValidator<UpdateBoxRequest> updateBoxRequestValidator,
        IValidator<PaginationParams> paginationParamsValidator
        )
    {
        this.mapper = mapper;
        this.createBoxRequestValidator = createBoxRequestValidator;
        this.updateBoxRequestValidator = updateBoxRequestValidator;
        this.paginationParamsValidator = paginationParamsValidator;
        this.boxService = boxService;
    }

    [HttpGet(ApiRoute.Warehouse.Boxes)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllResponse<BoxResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var validationResult = await paginationParamsValidator.ValidateAsync(paginationParams, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var boxes = await boxService.GetAllAsync(paginationParams.Offset ?? 0, paginationParams.Limit ?? 10, cancellationToken);
        return Ok(new GetAllResponse<BoxResponse>(mapper.Map<List<BoxResponse>>(boxes), boxes.Count));
    }

    [HttpGet(ApiRoute.Warehouse.PalletBoxes)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllResponse<BoxResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllByPalletId([FromRoute] long palletId, CancellationToken cancellationToken)
    {
        var boxes = await boxService.GetAllByPalletIdAsync(palletId, cancellationToken);
        return Ok(new GetAllResponse<BoxResponse>(mapper.Map<List<BoxResponse>>(boxes), boxes.Count));
    }

    [HttpGet(ApiRoute.Warehouse.Box)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoxResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetById([FromRoute] long boxId, CancellationToken cancellationToken)
    {
        var box = await boxService.GetByIdAsync(boxId, cancellationToken);
        return Ok(mapper.Map<BoxResponse>(box));
    }

    [HttpPost(ApiRoute.Warehouse.PalletBoxes)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BoxResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Add(
        [FromRoute] long palletId,
        [FromBody] CreateBoxRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await createBoxRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var createBoxCommand = mapper.Map<CreateBoxCommand>(request);
        var box = await boxService.AddAsync(palletId, createBoxCommand, cancellationToken);
        return Created($"{Request.Path}/{box.Id}", mapper.Map<BoxResponse>(box));
    }

    [HttpPatch(ApiRoute.Warehouse.PalletBox)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoxResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update(
        [FromRoute] long palletId,
        [FromRoute] long boxId,
        [FromBody] UpdateBoxRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await updateBoxRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var updateBoxCommand = mapper.Map<UpdateBoxCommand>(request);
        var box = await boxService.UpdateAsync(palletId, boxId, updateBoxCommand, cancellationToken);
        return Ok(mapper.Map<BoxResponse>(box));
    }

    [HttpDelete(ApiRoute.Warehouse.Box)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] long boxId, CancellationToken cancellationToken)
    {
        await boxService.DeleteAsync(boxId, cancellationToken);
        return NoContent();
    }
}
