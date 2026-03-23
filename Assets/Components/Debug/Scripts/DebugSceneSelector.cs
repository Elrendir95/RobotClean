#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugSceneSelector : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame ||  Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            SceneManager.LoadScene("JumpCooldownTestScene");
        }
    }
}
#endif
