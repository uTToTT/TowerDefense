using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace TToTT.Core.Purchasing
{
    static class IAPService
    {
        const string ENVIRONMENT = "production";

        public static async void Initialize(Action onSuccess, Action<string> onError)
        {
            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName(ENVIRONMENT);

                await UnityServices.InitializeAsync(options);

                onSuccess();
            }
            catch (Exception exception)
            {
                onError(exception.Message);
            }
        }
    }
}
