using System;
using Library.Variables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sections
{
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
