using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Components.EventSystem;

namespace Components.AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private int minAudioSources = 10;
        [SerializeField] private int maxAudioSources = 50;

        private int _audioSourcesCount;
        private readonly List<AudioSource> _activeAudioSource = new();
        private readonly List<AudioSource> _disabledAudioSource = new();
        private readonly List<AudioSource> _loopingAudioSource = new();

        private void Awake()
        {
            for (int i = 0; i < minAudioSources; i++)
            {
                _disabledAudioSource.Add(NewAudioSource());
            }

            Events.PlayAudio += PlayAudio;
            Events.PlayAudioAt += PlayAudioAt;
            Events.StopAllLoops += StopAllLoops;
        }

        private void OnDestroy()
        {
            Events.PlayAudio -= PlayAudio;
            Events.PlayAudioAt -= PlayAudioAt;
            Events.StopAllLoops -= StopAllLoops;
        }

        private AudioSource NewAudioSource(bool active = false)
        {
            if (_audioSourcesCount >= maxAudioSources) return null;

            // Create the GameObject
            GameObject newGameObject = new GameObject($"AudioSource_{_audioSourcesCount}");
            newGameObject.transform.SetParent(transform);
            newGameObject.SetActive(active);

            // Attach an AudioSource
            var audioSource = newGameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            _audioSourcesCount++;

            // Return the AudioSource
            return audioSource;
        }

        private AudioSource GetAvailableAudioSource()
        {
            AudioSource audioSource;
            if (_disabledAudioSource.Count > 0)
            {
                // For performance reason get the last audioSource from the list
                audioSource = _disabledAudioSource[^1];
                _disabledAudioSource.RemoveAt(_disabledAudioSource.Count - 1);

                audioSource.gameObject.SetActive(true);
                _activeAudioSource.Add(audioSource);
                return audioSource;
            }

            // No AudioSource available create a new AudioSource
            audioSource = NewAudioSource(true);
            if (audioSource != null)
            {
                _activeAudioSource.Add(audioSource);
            }
            return audioSource;
        }

        private IEnumerator DisableAudioSourceCoroutine(AudioSource audioSource, float duration = -1f)
        {
            if (duration <= 0f)
            {
                // Wait until AudioSource reached the End of the Audio clip
                yield return new WaitUntil(() => audioSource.time >= audioSource.clip.length);
            }
            else
            {
                yield return new WaitForSeconds(duration);
            }
            _activeAudioSource.Remove(audioSource);
            audioSource.gameObject.SetActive(false);
            audioSource.clip = null;
            _disabledAudioSource.Add(audioSource);
        }

        /// <summary>
        /// Play the AudioSO on the given audioSource
        /// </summary>
        /// <param name="sfx"></param>
        /// <param name="audioSource"></param>
        /// <param name="duration"></param>
        private void Play(AudioSO sfx, AudioSource audioSource, float duration = -1f)
        {
            sfx.config.ApplyToSource(audioSource);
            if (sfx.config.loop)
            {
                _loopingAudioSource.Add(audioSource);
            }
            audioSource.clip = sfx.clip;
            audioSource.Play();
            StartCoroutine(DisableAudioSourceCoroutine(audioSource));
        }

        private void PlayAudio(AudioSO sfx)
        {
            var audioSource = GetAvailableAudioSource();
            if (audioSource != null)
            {
                Play(sfx, audioSource);
            }
        }

        private void PlayAudioFor(AudioSO sfx, float duration = -1f)
        {
            var audioSource = GetAvailableAudioSource();
            if (audioSource != null)
            {
                Play(sfx, audioSource, duration);
            }
        }

        private void PlayAudioAt(AudioSO sfx, Vector3 location)
        {
            var audioSource = GetAvailableAudioSource();
            if (audioSource != null)
            {
                audioSource.transform.position = location;
                Play(sfx, audioSource);
            }
        }

        private void PlayAudioAtFor(AudioSO sfx, Vector3 location, float duration)
        {
            var audioSource = GetAvailableAudioSource();
            if (audioSource != null)
            {
                audioSource.transform.position = location;
                Play(sfx, audioSource, duration);
            }
        }

        private void PauseAllLoops()
        {
            foreach (var audioSource in _loopingAudioSource)
            {
                audioSource.Pause();
            }
        }

        private void ResumeAllLoops()
        {
            foreach (var audioSource in _loopingAudioSource)
            {
                audioSource.UnPause();
            }
        }

        private void StopAllLoops()
        {
            for (int i = _loopingAudioSource.Count - 1; i >= 0; i--)
            {
                _loopingAudioSource[i].Stop();
                _loopingAudioSource.RemoveAt(i);
            }
        }
    }
}
