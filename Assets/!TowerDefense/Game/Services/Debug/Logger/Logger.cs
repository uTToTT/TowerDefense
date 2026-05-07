using UnityEngine;

public class Logger : ILogger
{
    public void Log(string message)
    {
        Debug.Log(message);
    }
}
