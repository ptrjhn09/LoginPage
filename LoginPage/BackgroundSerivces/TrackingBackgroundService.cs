using EyeWearProduct.Data;
using EyeWearProduct.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class TrackingBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<OrderHub> _hubContext;

    public TrackingBackgroundService(
        IServiceScopeFactory scopeFactory,
        IHubContext<OrderHub> hubContext)
    {
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                var tracks = await context.TrackingHistory
                    .ToListAsync(stoppingToken);

                var changedOrders = new List<object>();

                foreach (var track in tracks)
                {
                    var minutes =
                        (DateTime.Now - track.CreatedDate).TotalMinutes;

                    string? newStatus = null;

                    if (minutes >= 1 && track.Status == "Pending")
                    {
                        newStatus = "In Progress";
                    }
                    else if (minutes >= 2 &&
                             track.Status == "In Progress")
                    {
                        newStatus = "Out for Delivery";
                    }
                    else if (minutes >= 3 &&
                             track.Status == "Out for Delivery")
                    {
                        newStatus = "Delivered";
                    }

                    if (newStatus is null)
                    {
                        continue;
                    }

                    track.Status = newStatus;
                    track.StatusDate = DateTime.Now;

                    changedOrders.Add(new
                    {
                        orderDetailId = track.OrderDetailId,
                        status = newStatus,
                        statusDate = track.StatusDate
                    });
                }

                if (changedOrders.Count > 0)
                {
                    await context.SaveChangesAsync(stoppingToken);

                    foreach (var orderUpdate in changedOrders)
                    {
                        await _hubContext.Clients.All.SendAsync(
                            "OrderStatusUpdated",
                            orderUpdate,
                            stoppingToken);
                    }
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            await Task.Delay(
                TimeSpan.FromMinutes(1),
                stoppingToken);
        }
    }
}