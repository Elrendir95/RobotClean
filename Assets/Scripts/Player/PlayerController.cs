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


    // Variable used in the transition
    private float _transitionTime = 0f; // Internal variable to keep track of current corridor transition
    private Vector3 _destination; // Corridor destination
    private Vector3 _startPosition; // Start corridor for the transtion

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
    private bool CanSwitchCorridors() => (canSwitchCorridorsInJump || !_isJumping) && _transitionTime == .0f;

    /// <summary>
    /// Handle Right Direction pressed
    /// </summary>
    /// <param name="obj"></param>
    private void GoRight(InputAction.CallbackContext obj)
    {
        if (!CanSwitchCorridors()) return;
        if (transform.position.x < corridorSize)
        {
            SetDestination(new Vector3(corridorSize, 0, 0));
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
            SetDestination(new Vector3(-corridorSize, 0, 0));
        }
    }

    /// <summary>
    /// The de destination corridor, to handle a smooth transition
    /// </summary>
    /// <param name="destination"></param>
    private void SetDestination(Vector3 destination)
    {
        _destination = transform.position + destination;
        _destination.y = 0f;
        _destination.z = 0f;
        _startPosition = new Vector3(transform.position.x, 0f, 0f);
    }

    private void Update()
    {
        SmoothCorridorTransition();
    }

    /// <summary>
    /// Handle the transition beetween the corridors
    /// </summary>
    private void SmoothCorridorTransition()
    {
        if (transform.position.x == _destination.x) return;

        if (_transitionTime > corridorTransitionSpeed)
        {
            transform.position = new Vector3(_destination.x, transform.position.y, transform.position.z);
            _transitionTime = 0f;
        }
        else
        {
            Vector3 current = Vector3.Lerp(_startPosition, _destination, _transitionTime / corridorTransitionSpeed);
            transform.position = new Vector3(current.x, transform.position.y, transform.position.z);
            _transitionTime += Time.deltaTime;
        }
    }
}
