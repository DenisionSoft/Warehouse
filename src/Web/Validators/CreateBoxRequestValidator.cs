using FluentValidation;
using Warehouse.Web.Contracts.Models.Box;

namespace Warehouse.Web.Validators;

public sealed class CreateBoxRequestValidator : AbstractValidator<CreateBoxRequest>
{
    public CreateBoxRequestValidator()
    {
        RuleFor(r => r.Width).NotNull();
        RuleFor(r => r.Height).NotNull();
        RuleFor(r => r.Length).NotNull();
        RuleFor(r => r.Weight).NotNull();
        RuleFor(r => r.GivenDate).NotNull();
        RuleFor(r => r.IsExpirationDate).NotNull();
    }
}
