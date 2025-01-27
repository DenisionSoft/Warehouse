using FluentValidation;
using Warehouse.Web.Contracts.Models;

namespace Warehouse.Web.Validators;

public sealed class PaginationParamsValidator : AbstractValidator<PaginationParams>
{
    public PaginationParamsValidator()
    {
        RuleFor(p => p.Offset).GreaterThanOrEqualTo(0).When(p => p.Offset.HasValue);
        RuleFor(p => p.Limit).GreaterThan(0).When(p => p.Limit.HasValue);
    }
}
