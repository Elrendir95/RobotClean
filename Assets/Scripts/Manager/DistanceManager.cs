using System;
using Library.References;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Deprecated, this was used in early builds to demonstrate to the GD
    /// </summary>
    public class DistanceManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private FloatReference runtimeSpeed;
        [SerializeField] private FloatReference runDistance;

        private void Update()
        {
            runDistance.Value += runtimeSpeed.Value * Time.deltaTime;
        }
    }
}
