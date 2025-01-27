using FluentValidation;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.Web.Validators;

public sealed class CreatePalletRequestValidator : AbstractValidator<CreatePalletRequest>
{
    public CreatePalletRequestValidator()
    {
        RuleFor(r => r.Width).NotNull();
        RuleFor(r => r.Height).NotNull();
        RuleFor(r => r.Length).NotNull();
    }
}
