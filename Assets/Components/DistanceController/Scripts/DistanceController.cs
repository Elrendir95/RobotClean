using Library.References;
using UnityEngine;

namespace Components
{
    public class DistanceController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private FloatReference runtimeSpeed;
        [SerializeField] private FloatReference runDistance;

        private void FixedUpdate()
        {
            runDistance.Value += runtimeSpeed.Value * Time.fixedDeltaTime;
        }
    }
}
