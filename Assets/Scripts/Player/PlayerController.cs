using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference left;
    [SerializeField] private InputActionReference right;
    [SerializeField] private InputActionReference jump;
    [Header("Corridor Switch Settings")]
    [SerializeField] private float corridorTransitionSpeed = 0.12f;
    [SerializeField][Tooltip("Size in meters")] private float corridorSize = 2.5f;
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 500f;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private float jumpDuration = 0.9f;
    [SerializeField][Tooltip("Height in meters")] private float jumpHeight = 1.8f;
    [SerializeField] private bool canSwitchCorridorsInJump = true;

    private bool _isJumping = false;
    private bool _canJump = true;

    private void OnEnable()
    {
        left.action.performed += GoLeft;
        right.action.performed += GoRight;
        jump.action.performed += Jump;
    }

    private void OnDisable()
    {
        left.action.performed -= GoLeft;
        right.action.performed -= GoRight;
        jump.action.performed -= Jump;
    }

    /// <summary>
    /// Handle Jump pressed
    /// </summary>
    /// <param name="obj"></param>
    private void Jump(InputAction.CallbackContext obj)
    {
        if (_isJumping || !_canJump) return; // No doble Jump, early return if already Jumping
        StartCoroutine(JumpCoroutine());
    }

    /// <summary>
    /// Coroutine that simulates the Jump behaviour
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpCoroutine()
    {
        _isJumping = true;
        transform.Translate(new Vector3(0,jumpHeight,0));
        yield return new WaitForSeconds(jumpDuration);
        transform.Translate(new Vector3(0,-jumpHeight,0));
        _isJumping = false;
    }

    /// <summary>
    /// Check condition if player can change corridor
    /// </summary>
    /// <returns>true if it can</returns>
    private bool CanSwitchCorridors() => canSwitchCorridorsInJump || !_isJumping;

    /// <summary>
    /// Handle Right Direction pressed
    /// </summary>
    /// <param name="obj"></param>
    private void GoRight(InputAction.CallbackContext obj)
    {
        if (!CanSwitchCorridors()) return;
        if (transform.position.x < corridorSize)
        {
            transform.Translate(new Vector3(corridorSize, 0, 0));
        }
    }
    /// <summary>
    /// Handle left direction pressed
    /// </summary>
    /// <param name="obj"></param>
    private void GoLeft(InputAction.CallbackContext obj)
    {
        if (!CanSwitchCorridors()) return;
        if (transform.position.x > -corridorSize)
        {
            transform.Translate(new Vector3(-corridorSize, 0, 0));
        }
    }
}
