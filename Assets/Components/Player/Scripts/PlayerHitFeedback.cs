using System.Collections;
using Components.AudioSystem;
using Components.EventSystem;
using UnityEngine;

public class PlayerHitFeedback : MonoBehaviour
{
    private Renderer _meshRenderer;
    [SerializeField, Tooltip("Delay in seconds")] private float feedbackDelay;
    [SerializeField] private Material invincibleMaterial;
    [SerializeField] private AudioSO hitSound;
    [SerializeField] private AudioSO invincibilityEndSound;
    private Material _baseMaterial;

    private bool _isInvincible;
    private bool _playHitSound;
    private bool _playInvincibilityEndSound;
    void Start()
    {
        _playHitSound = hitSound != null;
        _playInvincibilityEndSound = invincibilityEndSound != null;
        _meshRenderer = GetComponent<Renderer>();
        _baseMaterial = _meshRenderer.sharedMaterial;
        Events.OnPlayerInvincible += HandleOnPlayerInvincible;
    }

    private void OnDestroy()
    {
        Events.OnPlayerInvincible -= HandleOnPlayerInvincible;
    }

    private void HandleOnPlayerInvincible(bool isInvincible)
    {
        StopAllCoroutines();
        if (isInvincible)
        {
            if (_playHitSound) Events.PlayAudio?.Invoke(hitSound);
            _isInvincible = true;
            StartCoroutine(HitFeedbackCoroutine());
        }
        else if (_isInvincible)
        {
            _isInvincible = false;
            _meshRenderer.sharedMaterial = _baseMaterial;
            if (_playInvincibilityEndSound) Events.PlayAudio?.Invoke(invincibilityEndSound);
        }
    }

    IEnumerator HitFeedbackCoroutine()
    {
        var waitDelay = new WaitForSeconds(feedbackDelay);
        while (true)
        {
            _meshRenderer.sharedMaterial = invincibleMaterial;
            yield return waitDelay;
            _meshRenderer.sharedMaterial = _baseMaterial;
            yield return waitDelay;
        }
    }
}
