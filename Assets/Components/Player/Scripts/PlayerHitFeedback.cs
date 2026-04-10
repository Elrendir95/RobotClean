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

    /// <summary>
    /// Handle OnPlayerInvincible Events, This is triggered
    /// when the player become invincible after a hit
    /// </summary>
    /// <param name="isInvincible"></param>
    private void HandleOnPlayerInvincible(bool isInvincible)
    {
        // Stop all Coroutines to avoid multiple coroutine at the same time
        StopAllCoroutines();

        if (isInvincible) // is now invincible, so just got hit
        {
            // Play HitSound if one is set
            if (_playHitSound) Events.PlayAudio?.Invoke(hitSound);
            _isInvincible = true;
            // Start the blinking effect
            StartCoroutine(HitFeedbackCoroutine());
        }
        else if (_isInvincible) // just get out of invincible state
        {
            _isInvincible = false;
            // restore base material
            _meshRenderer.sharedMaterial = _baseMaterial;
            // play invincibility end sound if is set
            if (_playInvincibilityEndSound) Events.PlayAudio?.Invoke(invincibilityEndSound);
        }
    }

    /// <summary>
    /// Coroutine that update the material of the player to make it blinking
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitFeedbackCoroutine()
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
