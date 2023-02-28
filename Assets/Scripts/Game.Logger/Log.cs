using System;

namespace Game.Logger
{
    public static class Log
    {
        public static void Info(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        public static void Warning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public static void Error(string message)
        {
            UnityEngine.Debug.LogError(message);
        }

        public static void Exception(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }
    }
}