using UnityEngine;

public class Logger : ILogger
{
    public void Log(string message)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log(message);
#endif
    }
}
