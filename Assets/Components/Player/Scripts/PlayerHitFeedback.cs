using System.Collections;
using Components.EventSystem;
using UnityEngine;

public class PlayerHitFeedback : MonoBehaviour
{
    private Renderer _meshRenderer;
    [SerializeField, Tooltip("Delay in seconds")] private float feedbackDelay;
    [SerializeField] private Material invincibleMaterial;
    private Material _baseMaterial;

    void Start()
    {
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
            StartCoroutine(HitFeedbackCoroutine());
        }
        else
        {
            _meshRenderer.sharedMaterial = _baseMaterial;
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
