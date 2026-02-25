using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Input References")]
        [SerializeField] private InputActionReference left;
        [SerializeField] private InputActionReference right;
        [SerializeField] private InputActionReference jump;
        [Header("Corridor Switch Settings")]
        [SerializeField] private float corridorTransitionSpeed = 0.12f;
        [SerializeField][Tooltip("Size in meters")] private float corridorSize = 2.5f;
        [SerializeField] private bool canSwitchCorridorsInJump = true;
        [Header("Jump Settings")]
        [SerializeField] private float jumpCooldown = 0.2f;
        [SerializeField] private float jumpDuration = 0.9f;
        [SerializeField][Tooltip("Height in meters")] private float jumpHeight = 1.8f;
        [SerializeField] private AnimationCurve jumpCurve;

        // Jumping States
        private bool _isJumping = false;
        private bool _canJump = true;
        private float _groudY; // Saved ground positions
        private float _jumpingTime = 0f; // Track jump duration

        // Variable used in the transition
        private float _transitionTime = 0f; // Internal variable to keep track of current corridor transition
        private Vector3 _destination; // Corridor destination
        private Vector3 _startPosition; // Start corridor for the transtion
        private bool _isSwithingCorridor = false;

        private void Awake()
        {
            _groudY = transform.position.y;
        }

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
            if (_isJumping || !_canJump) return; // No double Jump, early return if already Jumping
            Debug.Log("Jump");
            StartCoroutine(JumpCoroutine());
        }

        /// <summary>
        /// Calculate the position in jumps using a Sinusoid
        /// Start the jumpCouldownCoroutine when finished
        /// </summary>
        /// <returns></returns>
        IEnumerator JumpCoroutine()
        {
            _canJump = false;
            _isJumping = true;
            _jumpingTime = 0f;
            while (_jumpingTime < jumpDuration)
            {
                _jumpingTime += Time.deltaTime;
                // Use Pi to get the Up part of the sin and a have curve from 0 to 0 passing by 1
                //float p = Mathf.Clamp01(Mathf.Sin(_jumpingTime / jumpDuration * Mathf.PI));
                //p = Mathf.Pow(p, jumpPower);
                float p = jumpCurve.Evaluate(_jumpingTime / jumpDuration);
                // Set Y position
                transform.position = new Vector3(transform.position.x, p *  jumpHeight, transform.position.z);
                // Return and resume at next frame
                yield return null;
            }
            transform.position = new Vector3(transform.position.x, _groudY, transform.position.z);
            _isJumping = false;
            StartCoroutine(JumpCooldownCoroutine());
        }

        /// <summary>
        /// Jump cooldown, set _canJump to true after jumpCooldown seconds
        /// </summary>
        /// <returns></returns>
        IEnumerator JumpCooldownCoroutine()
        {
            Debug.Log($"Landed Start CoolDown: Jump duration {_jumpingTime}");
            _jumpingTime = 0f;
            yield return new WaitForSeconds(jumpCooldown);
            _canJump = true;
        }

        /// <summary>
        /// Check condition if player can change corridor
        /// </summary>
        /// <returns>true if it can</returns>
        private bool CanSwitchCorridors() => (canSwitchCorridorsInJump || !_isJumping) && !_isSwithingCorridor;

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
            StartCoroutine(SmoothCorridorTransitionCoroutine());
        }

        /// <summary>
        /// Handle the transition beetween the corridors
        /// </summary>
        private IEnumerator SmoothCorridorTransitionCoroutine()
        {
            _isSwithingCorridor = true;
            _transitionTime = 0f;
            while (_transitionTime < corridorTransitionSpeed)
            {
                Vector3 current = Vector3.Lerp(_startPosition, _destination, _transitionTime / corridorTransitionSpeed);
                transform.position = new Vector3(current.x, transform.position.y, transform.position.z);
                _transitionTime += Time.deltaTime;
                yield return null;
            }
            transform.position = new Vector3(_destination.x, transform.position.y, transform.position.z);
            _isSwithingCorridor = false;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Initialize the curve
        /// </summary>
        private void Reset()
        {
            Keyframe[] keys = new Keyframe[3];
            keys[0] = new Keyframe(0f, 0f);
            keys[1] = new Keyframe(.5f, 1f);
            keys[2] = new Keyframe(1f, 0f);
            jumpCurve = new AnimationCurve(keys);
        }
#endif
    }
}
