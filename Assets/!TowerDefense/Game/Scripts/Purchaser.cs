using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour
{
    public void OnPurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "com.uttottgames.towerguns.removeads":
                RemoveAds();
                break;
            default:
                break;
        }
    }

    private void RemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAds", 1);
        Debug.Log("Remove ads");
        EventBus.onRemoveAds?.Invoke();
    }
}

