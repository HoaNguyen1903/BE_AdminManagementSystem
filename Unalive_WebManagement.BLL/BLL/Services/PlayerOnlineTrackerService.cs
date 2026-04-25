using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services;

public class PlayerOnlineTrackerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PlayerOnlineTrackerService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

    public PlayerOnlineTrackerService(IServiceProvider serviceProvider, ILogger<PlayerOnlineTrackerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PlayerOnlineTrackerService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await TrackPlayerCountsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while tracking player counts.");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("PlayerOnlineTrackerService is stopping.");
    }

    private async Task TrackPlayerCountsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var historyRepository = scope.ServiceProvider.GetRequiredService<IPlayerOnlineHistoryRepository>();

        var now = DateTime.UtcNow;
        var today = now.Date;

        // Automatically set users offline if they haven't sent a heartbeat in the last 2 minutes
        var offlineThreshold = now.AddMinutes(-2);
        await userRepository.SetInactiveUsersOfflineAsync(offlineThreshold);

        // Current Online: Users active in the last 5 minutes (now using the IsOnline flag)
        var onlineThreshold = now.AddMinutes(-5);
        var currentOnlineCount = await userRepository.CountOnlineUsersAsync(onlineThreshold);
        
        // DAU: Users active today
        var dauCount = await userRepository.CountDailyActiveUsersAsync(today);

        var existingHistory = await historyRepository.GetByDateAsync(today);

        if (existingHistory == null)
        {
            var newHistory = new PlayerOnlineHistory
            {
                Date = today,
                OnlineCount = currentOnlineCount, // Initial peak for today
                DailyActiveUsers = dauCount
            };
            await historyRepository.AddAsync(newHistory);
            _logger.LogInformation("Created new player online history for {Date}: Online={Online}, DAU={DAU}", today, currentOnlineCount, dauCount);
        }
        else
        {
            // Update peak online if current is higher
            if (currentOnlineCount > existingHistory.OnlineCount)
            {
                existingHistory.OnlineCount = currentOnlineCount;
            }
            
            // Update DAU (always update as it can only increase during the day)
            existingHistory.DailyActiveUsers = dauCount;
            
            await historyRepository.UpdateAsync(existingHistory);
            _logger.LogInformation("Updated player online history for {Date}: PeakOnline={PeakOnline}, DAU={DAU}", today, existingHistory.OnlineCount, dauCount);
        }
    }
}
