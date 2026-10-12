#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class PlayModeUtility
{
    [MenuItem("Tools/Start Play Mode")]
    public static void StartPlayMode()
    {
        if (!EditorApplication.isPlaying)
            EditorApplication.isPlaying = true;
    }

    [MenuItem("Tools/Stop Play Mode")]
    public static void StopPlayMode()
    {
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
    }
}
#endif
