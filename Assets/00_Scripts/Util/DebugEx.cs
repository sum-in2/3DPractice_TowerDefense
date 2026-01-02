using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class DebugEx
{
    public static void Log(object message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Debug.Log($"[{memberName}:{lineNumber}] {message}");
    }
}
