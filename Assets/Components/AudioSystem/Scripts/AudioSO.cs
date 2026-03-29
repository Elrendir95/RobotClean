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
        public void TestSound()
        {
            if (clip == null || config == null) return;
            StopTest();

            GameObject go = new GameObject("AudioPreview_TEMP");

            _previewSource = go.AddComponent<AudioSource>();

            UpdatePreview();

            _previewSource.Play();
        }

        public void StopTest()
        {
            GameObject oldPreview = GameObject.Find("AudioPreview_TEMP");
            if (oldPreview != null)
            {
                DestroyImmediate(oldPreview);
            }
        }

        public void UpdatePreview()
        {
            if (_previewSource != null)
            {
                config.ApplyToSource(_previewSource);
                _previewSource.clip = clip;
                _previewSource.spatialBlend = 0;
                _previewSource.loop = true;
            }
        }
#endif
    }
}
