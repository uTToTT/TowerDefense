using UnityEngine;
using UnityEngine.Purchasing;

public class Donation : MonoBehaviour
{
    [SerializeField] private GameObject _frameDonation;

    void Start()
    {
        _frameDonation.SetActive(false);
    }

    public void OnPurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "com.uttottgames.towerguns.donate50":
                break;
            case "com.uttottgames.towerguns.donate150":
                break;
            case "com.uttottgames.towerguns.donate200":
                break;
            default:
                break;
        }
    }
}
