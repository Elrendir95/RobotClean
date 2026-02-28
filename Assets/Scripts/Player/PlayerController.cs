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
        [Header("Lane Switch Settings")]
        [SerializeField] private float laneTransitionSpeed = 0.12f;
        [SerializeField] private Transform[] lanes;
        [SerializeField] private bool canSwitchLanesInJump = true;
        [Header("Jump Settings")]
        [SerializeField] private float jumpCooldown = 0.2f;
        [SerializeField] private float jumpDuration = 0.9f;
        [SerializeField][Tooltip("Height in meters")] private float jumpHeight = 1.8f;
        [SerializeField] private AnimationCurve jumpCurve;

        // Jumping States
        private bool _isJumping = false;
        private bool _canJump = true;
        private float _groudY; // Saved ground positions

        private int currentLane = 1;
        private bool _isSwithingLane = false;

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
            StartCoroutine(JumpCoroutine());
        }

        /// <summary>
        /// Calculate the position in jumps using a Sinusoid
        /// Start the jumpCouldownCoroutine when finished
        /// </summary>
        /// <returns></returns>
        IEnumerator JumpCoroutine()
        {
            float jumpingTime = 0f;
            
            _canJump = false;
            _isJumping = true;
            
            while (jumpingTime < jumpDuration)
            {
                jumpingTime += Time.deltaTime;
                float p = jumpCurve.Evaluate(jumpingTime / jumpDuration);
                transform.position = new Vector3(transform.position.x, p *  jumpHeight, transform.position.z);
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
            yield return new WaitForSeconds(jumpCooldown);
            _canJump = true;
        }

        /// <summary>
        /// Check condition if player can change lane
        /// </summary>
        /// <returns>true if it can</returns>
        private bool CanSwitchLanes() => (canSwitchLanesInJump || !_isJumping) && !_isSwithingLane;

        /// <summary>
        /// Handle Right Direction pressed
        /// </summary>
        /// <param name="obj"></param>
        private void GoRight(InputAction.CallbackContext obj)
        {
            if (!CanSwitchLanes()) return;
            if (currentLane < lanes.Length - 1)
            {
                StartCoroutine(SmoothLaneTransitionCoroutine(currentLane + 1));
            }
        }

        /// <summary>
        /// Handle left direction pressed
        /// </summary>
        /// <param name="obj"></param>
        private void GoLeft(InputAction.CallbackContext obj)
        {
            if (!CanSwitchLanes()) return;
            if (currentLane > 0)
            {
                StartCoroutine(SmoothLaneTransitionCoroutine(currentLane - 1));
            }
        }

        /// <summary>
        /// Handle the transition beetween the lanes
        /// </summary>
        private IEnumerator SmoothLaneTransitionCoroutine(int destinationIndex)
        {
            _isSwithingLane = true;
            float transitionTime = 0f;
            while (transitionTime < laneTransitionSpeed)
            {
                Vector3 current = Vector3.Lerp(lanes[currentLane].position, lanes[destinationIndex].position, transitionTime / laneTransitionSpeed);
                transform.position = new Vector3(current.x, transform.position.y, current.z);
                transitionTime += Time.deltaTime;
                yield return null;
            }
            transform.position = new Vector3(lanes[destinationIndex].position.x,
                                             transform.position.y,
                                             lanes[destinationIndex].position.z);
            currentLane = destinationIndex;
            _isSwithingLane = false;
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
