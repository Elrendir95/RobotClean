using UnityEngine;
using UnityEditor;
using Components.AudioSystem; // Assure-toi que le namespace correspond

[CustomEditor(typeof(AudioSO))]
public class AudioSOEditor : Editor
{
    private Editor _cachedConfigEditor;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AudioSO audio = (AudioSO)target;

        if (audio.config != null)
        {
            GUILayout.Label("Audio Settings", EditorStyles.boldLabel);

            Editor.CreateCachedEditor(audio.config, null, ref _cachedConfigEditor);

            EditorGUI.BeginChangeCheck();

            _cachedConfigEditor.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                audio.UpdatePreview();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Select an AudioConfigSO.", MessageType.Info);
        }
        DrawDebugActions(audio);
    }

    private static void DrawDebugActions(AudioSO audio)
    {
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("▶ Test Sound", GUILayout.Height(30)))
        {
            audio.TestSound();
        }

        if (GUILayout.Button("■ Stop", GUILayout.Height(30)))
        {
            audio.StopTest();
        }

        EditorGUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
    }
}
