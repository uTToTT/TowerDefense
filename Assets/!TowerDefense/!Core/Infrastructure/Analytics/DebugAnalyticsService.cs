public class DebugAnalyticsService : IAnalyticsService
{
    private readonly ILogger _logger;

    public DebugAnalyticsService(ILogger logger)
    {
        _logger = logger;
    }

    public void TrackAdWatched(string placement) =>
        _logger.Log($"ad_watched | placement={placement}");

    public void TrackPurchase(string productId) =>
        _logger.Log($"purchase | product={productId}");

    public void TrackTowerPurchased(string type, int cost) =>
        _logger.Log($"tower_purchased | type={type} cost={cost}");

    public void TrackWaveCompleted(int wave, int enemiesKilled) =>
        _logger.Log($"wave_completed | wave={wave} enemies={enemiesKilled}");
}
