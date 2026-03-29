using System;
using System.Collections;
using System.Collections.Generic;
using Components.AudioSystem;
using Library.References;
using Components.EventSystem;
using Components.InputBuffer;
using Components.StateMachine;
using Components.StateMachine.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private List<InputMapping> inputMappings;
        [Header("Lane Switch Settings")]
        [SerializeField] private float laneTransitionSpeed = 0.12f;
        [SerializeField] private Transform[] lanes;
        [SerializeField] private bool canSwitchLanesInJump = true;
        [Header("Jump Settings")]
        [SerializeField] private FloatReference jumpCooldown;
        [SerializeField] private float jumpDuration = 0.9f;
        [SerializeField][Tooltip("Height in meters")] private float jumpHeight = 1.8f;
        [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private AudioSO jumpSound;
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

        private readonly InputBufferHandler _inputBuffer= new();
        private readonly Dictionary<Guid, InputMapping> _inputMappings = new();

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

        private void Update()
        {
            if (_isDead) return;
            // Handle translations
            ProcessBufferedInput(ActionType.Left, () => CanSwitchLanes() && _currentLane > 0, () => SmoothLaneTransitionCoroutine(_currentLane - 1));
            ProcessBufferedInput(ActionType.Right, () => CanSwitchLanes() && _currentLane < lanes.Length - 1, () => SmoothLaneTransitionCoroutine(_currentLane + 1));

            // Handle Jumps
            ProcessBufferedInput(ActionType.Jump, () => !_isJumping && _canJump && !_isSlidingDown, JumpCoroutine);
            // Handle SlideDown
            ProcessBufferedInput(ActionType.SlideDown, () => !_isJumping && !_isSlidingDown, SlideDownCoroutine);
        }

        private void HandleBufferedInput(InputAction.CallbackContext obj)
        {
            if (_inputMappings.TryGetValue(obj.action.id, out InputMapping input))
            {
                _inputBuffer.AddInput(input.type, input.bufferingTime);
            }
        }

        private void ProcessBufferedInput(ActionType type, Func<bool> condition, Func<IEnumerator> actionCoroutine)
        {
            if (_inputBuffer.IsBuffered(type) && condition())
            {
                _inputBuffer.Consume(type);
                StartCoroutine(actionCoroutine());
            }
        }

        private void OnStateChanged(State newState)
        {
            bool isGameState = newState is GameState;
            animator.speed = newState is PauseState ? 0 : 1;

            foreach (var input in inputMappings)
            {
                if (isGameState)
                {
                    input.inputActionReference.action.performed += HandleBufferedInput;
                    _inputMappings[input.inputActionReference.action.id] = input;
                }
                else  input.inputActionReference.action.performed -= HandleBufferedInput;
            }

            if (isGameState) animator.SetTrigger("IsRunning");
            else
            {
                _inputBuffer.ClearInput();
                _inputMappings.Clear();
            }
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
            Events.PlayAudio?.Invoke(jumpSound);
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
            if (jumpCooldown.Value > 0f) StartCoroutine(JumpCooldownCoroutine());
            else _canJump = true;
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
