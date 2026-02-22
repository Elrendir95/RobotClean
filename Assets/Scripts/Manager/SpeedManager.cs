using System.Collections;
using Library.References;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class SpeedManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private FloatReference startSpeed;
        [SerializeField] private FloatReference maxSpeed;
        [SerializeField] private FloatReference runtimeSpeed;
        [SerializeField] private FloatReference increaseSpeedTime;
        [SerializeField] private FloatReference increaseSpeedAmount;
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        [Header("Debug Action References")]
        [SerializeField] private InputActionReference increaseSpeed;
        [SerializeField] private InputActionReference decreaseSpeed;

        private void OnEnable()
        {
            increaseSpeed.action.performed += IncreaseSpeed;
            decreaseSpeed.action.performed += DecreaseSpeed;
        }

        private void OnDisable()
        {
            increaseSpeed.action.performed -= IncreaseSpeed;
            decreaseSpeed.action.performed -= DecreaseSpeed;
        }

        private void IncreaseSpeed(InputAction.CallbackContext obj)
        {
            runtimeSpeed.Value += 0.1f;
        }

        private void DecreaseSpeed(InputAction.CallbackContext obj)
        {
            runtimeSpeed.Value -= 0.1f;
        }
#endif

        private void Start()
        {
            runtimeSpeed.Value = startSpeed.Value;
            StartCoroutine(SpeedCoroutine());
        }

        IEnumerator SpeedCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(increaseSpeedTime.Value);
                runtimeSpeed.Value += increaseSpeedAmount.Value;
                runtimeSpeed.Value = Mathf.Clamp(runtimeSpeed.Value, startSpeed, maxSpeed.Value);
            }
        }
    }
}
