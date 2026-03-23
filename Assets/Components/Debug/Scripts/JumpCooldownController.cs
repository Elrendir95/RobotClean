#if UNITY_EDITOR || DEVELOPMENT_BUILD
using Library.References;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpCooldownController : MonoBehaviour
{
    [SerializeField] private FloatReference jumpCooldown;

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            jumpCooldown.Value += 0.01f;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            jumpCooldown.Value -= 0.01f;
        }
    }
}
#endif
