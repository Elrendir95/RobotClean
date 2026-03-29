using Components.AudioSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioConfigSO))]
public class AudioConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();

        if (EditorGUI.EndChangeCheck())
        {
            GameObject temp = GameObject.Find("AudioPreview_TEMP");
            if (temp != null && temp.TryGetComponent(out AudioSource source))
            {
                AudioConfigSO config = (AudioConfigSO)target;
                config.ApplyToSource(source);
                source.loop = true;
                source.spatialBlend = 0;
            }
        }
    }
}
