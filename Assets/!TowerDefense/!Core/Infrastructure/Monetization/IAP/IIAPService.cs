public interface IIAPService
{
    bool IsNoAdsPurchased { get; }
    void BuyNoAds();
    void BuyGoldPack();
}