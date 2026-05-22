using System;
using System.Collections.Generic;
using System.Diagnostics;
using TToTT.Core.Purchasing;
using UnityEngine;

public class CustomLogHandler : ILogHandler
{
    private readonly ILogHandler _default;

    public CustomLogHandler()
    {
        _default = UnityEngine.Debug.unityLogger.logHandler;
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        _default.LogException(exception, context);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        var caller = GetCaller();
        var color = GetColor(caller);
        _default.LogFormat(logType, context, $"<color=#{color}><b>[{caller}]</b></color> {format}", args);
    }

    private static string GetColor(string name)
    {
        var hue = (uint)name.GetHashCode() / (float)uint.MaxValue;
        var color = Color.HSVToRGB(hue, 0.6f, 1f);
        return ColorUtility.ToHtmlStringRGB(color);
    }

    private static string GetCaller()
    {
        var stack = new StackTrace(2, false);

        var skipTypes = new HashSet<string>
        {
            nameof(Logger),
            nameof(IAPLogger),
            nameof(CustomLogHandler),
        };

        for (int i = 0; i < stack.FrameCount; i++)
        {
            var type = stack.GetFrame(i)?.GetMethod()?.DeclaringType;
            if (type is null) continue;
            if (type.Namespace?.StartsWith("UnityEngine") == true) continue;
            if (type.Namespace?.StartsWith("System") == true) continue;
            if (skipTypes.Contains(type.Name)) continue; 

            return type.Name;
        }

        return "Unknown";
    }
}