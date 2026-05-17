public interface IAnalyticsService
{
    void TrackWaveCompleted(int wave, int enemiesKilled);
    void TrackAdWatched(string placement);
    void TrackPurchase(string productId);
    void TrackTowerPurchased(string type, int cost);
}