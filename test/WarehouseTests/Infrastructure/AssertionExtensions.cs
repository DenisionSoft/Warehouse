using FluentAssertions;

namespace Warehouse.WarehouseTests.Infrastructure;

public static class AssertionExtensions
{
    public static T ShouldNotBeNull<T>(this T? actualValue)
        where T : class
    {
        actualValue.Should().NotBeNull();

        if (actualValue == null)
        {
            const string message = "Will never be thrown, needed only to trick the compiler";
            throw new ArgumentNullException(nameof(actualValue), message);
        }

        return actualValue;
    }
}
