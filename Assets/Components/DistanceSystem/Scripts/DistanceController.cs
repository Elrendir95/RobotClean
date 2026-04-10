using Library.References;
using UnityEngine;

namespace Components
{
    public class DistanceController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private FloatReference runtimeSpeed;
        [SerializeField] private FloatReference runDistance;

        private void Start()
        {
            // initialize the references, in case of previous ru
            runDistance.Value = 0;
        }
        private void FixedUpdate()
        {
            // At each fixed update increment the distance
            runDistance.Value += runtimeSpeed.Value * Time.fixedDeltaTime;
        }
    }
}
