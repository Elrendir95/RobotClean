using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Audio;
using System.Reflection;
#endif
namespace Components.AudioSystem
{
    [CreateAssetMenu(fileName = "newAudioSO", menuName = "SFX")]
    public class AudioSO : ScriptableObject
    {
        public AudioClip clip;
        public AudioConfigSO config;

#if UNITY_EDITOR
        private AudioSource _previewSource;
        // Methode used from the Inspector to Test the sound configuration

        public void TestSound()
        {
            if (clip == null || config == null) return;
            StopTest();

            // Create a temporary GameObject
            GameObject go = new GameObject("AudioPreview_TEMP");
            // Add an AudioSource
            _previewSource = go.AddComponent<AudioSource>();
            // Update it
            UpdatePreview();
            // Play the Sound
            _previewSource.Play();
        }

        public void StopTest()
        {
            if (_previewSource != null)
            {
                DestroyImmediate(_previewSource.gameObject);
            }
        }

        public void UpdatePreview()
        {
            if (_previewSource != null)
            {
                config.ApplyToSource(_previewSource);
                _previewSource.clip = clip;
                // Force the Spatial Blend to 0 in preview to be sure we can hear it
                _previewSource.spatialBlend = 0;
                _previewSource.loop = true;
            }
        }
#endif
    }
}
