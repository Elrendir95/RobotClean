using Library.References;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SpeedDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private FloatReference speed;

        private TextMeshProUGUI _text;
        void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _text.text = speed.Value.ToString("F2");
        }

        void OnEnable()
        {
            speed.variable.onValueChanged.AddListener(UpdateSpeed);
        }

        private void UpdateSpeed(float speedValue)
        {
            _text.text = speedValue.ToString("F2");
        }
    }
}
