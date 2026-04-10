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
            // Displauy AudioSO.config when Set
            GUILayout.Label("Audio Settings", EditorStyles.boldLabel);

            // Create Editor for the audio.config
            CreateCachedEditor(audio.config, null, ref _cachedConfigEditor);

            EditorGUI.BeginChangeCheck();

            // Add the audio.config Editor on the Inspector
            _cachedConfigEditor.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                // If changes are detected update the AudioSO preview
                audio.UpdatePreview();
            }
        }
        else
        {
            // Add a text to remind to select au AudioConfigSO
            EditorGUILayout.HelpBox("Select an AudioConfigSO.", MessageType.Info);
        }

        // Display the buttons
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
