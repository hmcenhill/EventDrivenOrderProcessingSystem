using Microsoft.EntityFrameworkCore;

using OrderProcessing.Domain.Models;
using OrderProcessing.Domain.Models.Materials;
using OrderProcessing.Infrastructure.Persistence;

using Testcontainers.PostgreSql;

namespace OrderProcessing.IntegrationTests;

public class WorkOrderTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public WorkOrderTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task WorkOrder_CreateSaveRetrieve()
    {
        // Arrange
        var defaultProduct = new Product("Item-001", "Default Product");
        var workOrder = new WorkOrder("Macho Man", defaultProduct, 3, "The cream rises to the top!");
        var sku = new StockKeepingUnit(defaultProduct);
        workOrder.AdvanceToNextStep("Elizabeth", "oh yeah!");
        workOrder.AssignSku(sku);
        workOrder.SetHold("Macho Man", "Oh no!");
        workOrder.ReleaseHold("Macho Man", "Oh yeah!");

        _fixture.Context.WorkOrders.Add(workOrder);
        await _fixture.Context.SaveChangesAsync();

        var newContext = await _fixture.GetNewWorkOrderProcessingDbContext();
        var workOrderRetreived = newContext.WorkOrders
            .Include(w => w.AssignedStock)
            .Include(w => w.StateHistory)
            .FirstOrDefault(wo => wo.Id == workOrder.Id);

        // Assert
        Assert.Equal(workOrder.Id, workOrderRetreived?.Id);
        Assert.Equal(workOrder.CurrentStatus, workOrderRetreived?.CurrentStatus);
        Assert.Equal(workOrder.StateHistory.Count, workOrderRetreived?.StateHistory.Count);
        Assert.Equal(workOrder.StateHistory.First().Status, workOrderRetreived?.StateHistory.First().Status);
        Assert.Equal(workOrder.AssignedStock.Count, workOrderRetreived?.AssignedStock.Count);
        Assert.Equal(workOrder.AssignedStock.First().SerialNumber, workOrderRetreived?.AssignedStock.First().SerialNumber);
    }
}

public class DatabaseFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; private set; } = null!;
    public WorkOrderProcessingDbContext Context { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        Container = new PostgreSqlBuilder("postgres:15.1").Build();
        await Container.StartAsync();

        Context = await GetNewWorkOrderProcessingDbContext();
        await Context.Database.MigrateAsync();
    }

    public async Task<WorkOrderProcessingDbContext> GetNewWorkOrderProcessingDbContext()
    {
        var options = new DbContextOptionsBuilder<WorkOrderProcessingDbContext>()
            .UseNpgsql(Container.GetConnectionString())
            .Options;

        return new WorkOrderProcessingDbContext(options);
    }

    public async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await Container.DisposeAsync();
    }
}