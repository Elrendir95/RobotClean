using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sections
{
    public class MoveForward : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 7f;
#if UNITY_DEVELOPMENT_BUILD || UNITY_EDITOR
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
            moveSpeed += 0.1f;
        }

        private void DecreaseSpeed(InputAction.CallbackContext obj)
        {
            moveSpeed -= 0.1f;
        }
#endif
        private void Update()
        {
            transform.Translate(-Vector3.forward * (moveSpeed * Time.deltaTime));
        }
    }
}
