using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public static class EditorAudio
{
    public static void PlayClip(AudioClip clip, bool stopPrevious = false, int startSample = 0, bool loop = false)
    {
        if (clip == null) return;

        if (stopPrevious) StopAllClips();

        var unityEditorAssembly = typeof(AudioImporter).Assembly;
        var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        var method = audioUtilClass.GetMethod(
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null);

        method.Invoke(null, new object[] { clip, startSample, loop });
    }

    public static void StopAllClips()
    {
        var unityEditorAssembly = typeof(AudioImporter).Assembly;
        var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        var method = audioUtilClass.GetMethod(
            "StopAllPreviewClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { },
            null);

        method.Invoke(null, new object[] { });
    }
}