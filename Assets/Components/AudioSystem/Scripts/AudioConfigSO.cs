using UnityEngine;
using UnityEngine.Audio;

namespace Components.AudioSystem
{
    [CreateAssetMenu(fileName = "newAudioConfigSO", menuName = "Configs/Audio")]
    public class AudioConfigSO : ScriptableObject
    {
        public AudioMixerGroup output;
        public bool loop;
        [Range(0, 256)] public int priority = 128;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(-5f, 5f)] public float pitch = 1f;
        public bool randomizePitch;
        [Range(0f, 3f)] public float pitchRange;

        [Range(0f, 1f)] public float spatialBlend = 1f;
        public float minDistance;
        public float maxDistance = 500f;

        public void ApplyToSource(AudioSource source)
        {
            source.loop = loop;
            source.priority = priority;
            source.outputAudioMixerGroup = output;
            source.spatialBlend = spatialBlend;
            source.volume = volume;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
            if (randomizePitch)
            {
                float randomPitch = Random.Range(pitch - pitchRange, pitch + pitchRange);
                source.pitch = randomPitch;
            }
            else
            {
                source.pitch = pitch;
            }
        }
    }
}
