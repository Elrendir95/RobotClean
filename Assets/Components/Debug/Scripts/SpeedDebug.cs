#if UNITY_EDITOR || DEVELOPMENT_BUILD
using Library.References;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpeedDebug : MonoBehaviour
{
    [SerializeField] private FloatReference speed;

    void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            speed.Value += 0.1f;
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            speed.Value -= 0.1f;
        }
    }
}
#endif
