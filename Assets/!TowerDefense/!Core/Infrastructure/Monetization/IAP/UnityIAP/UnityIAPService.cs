using UnityEngine.Purchasing;

public class UnityIAPService : IIAPService, IStoreListener
{
    private IStoreController _store;
    private readonly IAnalyticsService _analytics;

    public bool IsNoAdsPurchased { get; private set; }

    public UnityIAPService(IAnalyticsService analytics)
    {
        _analytics = analytics;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(ProductIds.NoAds, ProductType.NonConsumable);
        builder.AddProduct(ProductIds.GoldPack, ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyNoAds() => _store?.InitiatePurchase(ProductIds.NoAds);
    public void BuyGoldPack() => _store?.InitiatePurchase(ProductIds.GoldPack);

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _store = controller;
        var noAds = controller.products.WithID(ProductIds.NoAds);
        IsNoAdsPurchased = noAds != null && noAds.hasReceipt;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var id = args.purchasedProduct.definition.id;
        _analytics.TrackPurchase(id);

        if (id == ProductIds.NoAds)
            IsNoAdsPurchased = true;

        if (id == ProductIds.GoldPack)
        {
            // IEconomy.GiveGold();
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message) { }
    public void OnInitializeFailed(InitializationFailureReason error) { }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason) { }
}