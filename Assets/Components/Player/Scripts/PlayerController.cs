using System.Collections;
using Library.References;
using Components.EventSystem;
using Components.StateMachine;
using Components.StateMachine.States;
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
        [SerializeField] private InputActionReference slideDown;
        [Header("Lane Switch Settings")]
        [SerializeField] private float laneTransitionSpeed = 0.12f;
        [SerializeField] private Transform[] lanes;
        [SerializeField] private bool canSwitchLanesInJump = true;
        [Header("Jump Settings")]
        [SerializeField] private FloatReference jumpCooldown;
        [SerializeField] private float jumpDuration = 0.9f;
        [SerializeField][Tooltip("Height in meters")] private float jumpHeight = 1.8f;
        [SerializeField] private AnimationCurve jumpCurve;
        [Header("Sliding Down Settings")]
        [SerializeField] private FloatReference slidingDownDuration;
        [Header("Speed")]
        [SerializeField] private FloatReference currentSpeed;
        [SerializeField] private FloatReference startSpeed;
        [Header("Components")]
        [SerializeField] private Animator animator;

        // Jumping States
        private bool _isJumping;
        private bool _canJump = true;
        private float _groudY; // Saved ground positions

        private bool _isSlidingDown;

        private int _currentLane = 1;
        private bool _isSwitchingLane;
        private bool _isDead;

        private void Awake()
        {
            _groudY = transform.position.y;
        }

        private void OnEnable()
        {
            Events.OnLifeCountChanged += OnLifeCountChanged;
            Events.OnStateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            Events.OnLifeCountChanged -= OnLifeCountChanged;
            Events.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(State newState)
        {
            if (newState is not GameState)
            {
                left.action.performed -= GoLeft;
                right.action.performed -= GoRight;
                jump.action.performed -= Jump;
                slideDown.action.performed -= SlideDown;
                if (newState is PauseState)
                {
                    animator.speed = 0;
                }
                return;
            }
            animator.speed = 1;
            left.action.performed += GoLeft;
            right.action.performed += GoRight;
            jump.action.performed += Jump;
            slideDown.action.performed += SlideDown;
            animator.SetTrigger("IsRunning");
        }

        private void SlideDown(InputAction.CallbackContext obj)
        {
            if (_isJumping || _isDead || _isSlidingDown) return;
            StartCoroutine(SlideDownCoroutine());
        }

        private void OnLifeCountChanged(float currentLife)
        {
            if (currentLife <= 0 && !_isDead)
            {
                animator.SetTrigger("IsDead");
                _isDead = true;
            }
        }

        /// <summary>
        /// Handle Jump pressed
        /// </summary>
        /// <param name="obj"></param>
        private void Jump(InputAction.CallbackContext obj)
        {
            if (_isJumping || !_canJump || _isDead || _isSlidingDown) return;
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
            float duration = jumpDuration * (startSpeed.Value / currentSpeed.Value);

            _canJump = false;
            _isJumping = true;
            animator.SetBool("IsJumping", true);

            while (jumpingTime < duration)
            {
                // Mutiply by animator speed to pause the jump during pause
                jumpingTime += Time.deltaTime * animator.speed;
                float p = jumpCurve.Evaluate(jumpingTime / duration);
                transform.position = new Vector3(transform.position.x, p *  jumpHeight, transform.position.z);
                yield return null;
            }
            transform.position = new Vector3(transform.position.x, _groudY, transform.position.z);
            _isJumping = false;
            animator.SetBool("IsJumping", false);
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
        private bool CanSwitchLanes() => (canSwitchLanesInJump || !_isJumping) && !_isSwitchingLane && !_isDead;

        /// <summary>
        /// Handle Right Direction pressed
        /// </summary>
        /// <param name="obj"></param>
        private void GoRight(InputAction.CallbackContext obj)
        {
            if (!CanSwitchLanes()) return;
            if (_currentLane < lanes.Length - 1)
            {
                StartCoroutine(SmoothLaneTransitionCoroutine(_currentLane + 1));
            }
        }

        /// <summary>
        /// Handle left direction pressed
        /// </summary>
        /// <param name="obj"></param>
        private void GoLeft(InputAction.CallbackContext obj)
        {
            if (!CanSwitchLanes()) return;
            if (_currentLane > 0)
            {
                StartCoroutine(SmoothLaneTransitionCoroutine(_currentLane - 1));
            }
        }

        /// <summary>
        /// Handle the transition beetween the lanes
        /// </summary>
        private IEnumerator SmoothLaneTransitionCoroutine(int destinationIndex)
        {
            _isSwitchingLane = true;
            float transitionTime = 0f;
            while (transitionTime < laneTransitionSpeed)
            {
                Vector3 current = Vector3.Lerp(lanes[_currentLane].position, lanes[destinationIndex].position, transitionTime / laneTransitionSpeed);
                transform.position = new Vector3(current.x, transform.position.y, current.z);
                // Mutiply by animator speed to pause the transition during pause
                transitionTime += Time.deltaTime * animator.speed;
                yield return null;
            }
            transform.position = new Vector3(lanes[destinationIndex].position.x,
                                             transform.position.y,
                                             lanes[destinationIndex].position.z);
            _currentLane = destinationIndex;
            _isSwitchingLane = false;
        }

        private IEnumerator SlideDownCoroutine()
        {
            _isSlidingDown = true;
            animator.SetBool("IsSlidingDown", true);
            Events.OnPlayerSlidingDown?.Invoke(true);

            var slideTimer = 0f;
            var duration = slidingDownDuration * (startSpeed.Value / currentSpeed.Value);

            while (slideTimer <= duration)
            {
                slideTimer += Time.deltaTime;
                yield return null;
            }

            _isSlidingDown = false;
            animator.SetBool("IsSlidingDown", false);
            Events.OnPlayerSlidingDown?.Invoke(false);
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
