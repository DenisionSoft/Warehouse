namespace Warehouse.WarehouseTests.Infrastructure;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<TestApplication>
{
    public const string Name = "Warehouse integration tests collection";

    public const string Category = "IntegrationTests";
}
