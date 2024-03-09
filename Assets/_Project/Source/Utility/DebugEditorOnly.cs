using UnityEngine;

namespace Utility
{
    public static class DebugEditorOnly
    {
        public static void LogEditorOnly(this MonoBehaviour monoBehaviour, string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        public static void LogWarningEditorOnly(this MonoBehaviour monoBehaviour, string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }

        public static void LogErrorEditorOnly(this MonoBehaviour monoBehaviour, string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }
    }
}
