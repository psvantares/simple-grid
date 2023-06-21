using UnityEngine;

namespace Game.Utilities
{
    public static class RichUnity
    {
        public static bool IsEditor() => Application.platform is RuntimePlatform.WindowsEditor or RuntimePlatform.OSXEditor;
    }
}