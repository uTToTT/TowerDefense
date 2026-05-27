public interface IIAPService
{
    bool IsNoAdsPurchased { get; }
    void GetNoAds();
    void GetGoldPack();
}