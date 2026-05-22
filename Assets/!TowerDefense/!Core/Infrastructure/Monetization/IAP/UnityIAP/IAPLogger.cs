using System.Collections.Generic;
using System.Text;
using UnityEngine.Purchasing;

namespace TToTT.Core.Purchasing
{
    public class IAPLogger
    {
        private readonly ILogger _logger;

        #region Init

        public IAPLogger(ILogger logger)
        {
            _logger = logger;
        }

        #endregion

        public void Log(string msg) => _logger.Log(msg);

        public void LogFetchedProducts(List<Product> products)
        {
            var sb = new StringBuilder();

            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    sb.AppendLine($"Fetched {product.definition.id}");
                }
            }
            else
            {
                Log("No Products Fetched.");
                return;
            }

            Log(sb.ToString());
        }

        public void LogConfirmedOrder(Product product, IOrderInfo orderInfo)
        {
            Log($"Confirmed Product | product={product.definition.id}\n" +
                $"Product transaction id: {orderInfo.TransactionID}.\n" +
                $"Product receipt length: {orderInfo.Receipt?.Length}.\n" +
                $"Product Type: {product.definition.type}");
        }

        public void LogCompletedPurchase(Product product, IOrderInfo orderInfo)
        {
            Log($"Purchased Product | product={product.definition.id}\n" +
                $"Product transaction id: {orderInfo.TransactionID}.\n" +
                $"Product receipt length: {orderInfo.Receipt?.Length}.\n" +
                $"Product Type: '{product.definition.type}'");
        }

        public void LogFailedConfirmation(Product product, PurchaseFailureReason reason)
        {
            Log("Purchase Confirmation Failed\n" +
                $"Product: '{product.definition.storeSpecificId}'\n" +
                $"FailureReason: {reason.ToString()}.");
        }

        public void LogFailedPurchase(Product product, PurchaseFailureReason reason)
        {
            Log("PurchaseFailed\n" +
                $"Product: '{product.definition.storeSpecificId}'\n" +
                $"FailureReason: {reason.ToString()}.");
        }

        public void LogDeferredPurchase(Product product)
        {
            Log("PurchaseDeferred\n" +
                $"Product: '{product.definition.storeSpecificId}'");
        }
    }
}
