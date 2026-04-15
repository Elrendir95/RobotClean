using Library.Variables;
using UnityEngine;

namespace Sections
{
    /// <summary>
    /// Deprecated, this was used in early builds to demonstrate to the GD
    /// </summary>
    public class MoveForward : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private FloatVariable moveSpeed;
        private void Update()
        {
            transform.Translate(-Vector3.forward * (moveSpeed.Value * Time.deltaTime));
        }
    }
}
