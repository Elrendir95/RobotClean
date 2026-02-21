using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class SpeedDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 7f;

        private TextMeshProUGUI _text;
        void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
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
            moveSpeed += 0.1f;
            _text.text = moveSpeed.ToString("F2");
        }

        private void DecreaseSpeed(InputAction.CallbackContext obj)
        {
            moveSpeed -= 0.1f;
            _text.text = moveSpeed.ToString("F2");
        }
#endif
    }
}
