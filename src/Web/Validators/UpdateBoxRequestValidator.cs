using FluentValidation;
using Warehouse.Web.Contracts.Models.Box;

namespace Warehouse.Web.Validators;

public sealed class UpdateBoxRequestValidator : AbstractValidator<UpdateBoxRequest>
{
    public UpdateBoxRequestValidator()
    {
        RuleFor(r => r.ExpirationDate).GreaterThanOrEqualTo(r => r.ProductionDate).When(r => r.ExpirationDate.HasValue && r.ProductionDate.HasValue);
    }
}
