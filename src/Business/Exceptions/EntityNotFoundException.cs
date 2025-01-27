namespace Warehouse.Business.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message, string entityId) : base(message)
    {
    }
}
