using System;
using UnityEngine;

namespace TransformAttributes
{
    public class RotateToVelocity : MonoBehaviour
    {
        // Enum to choose the axis to point to the target
        public enum LookAtAxis
        {
            XAxis,
            YAxis,
            ZAxis
        }

        // Public variable to choose which axis to use
        [SerializeField] private LookAtAxis lookAtAxis = LookAtAxis.YAxis; // Default is Y axis

        private Rigidbody _rigidbody;

        private void OnEnable()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        public void LateUpdate()
        {
            // Rotate towards the velocity
            var targetPosition = transform.position + _rigidbody.linearVelocity;
            Rotate(targetPosition);
        }

        // Function to rotate towards the target with the chosen axis
        public void Rotate(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;

            if (direction != Vector3.zero)
            {
                // Get the rotation that points the specified axis toward the target
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

                // Determine the axis to use for looking at the target
                Vector3 axis = Vector3.up; // Default is Y axis

                switch (lookAtAxis)
                {
                    case LookAtAxis.XAxis:
                        axis = Vector3.right; // X axis
                        break;
                    case LookAtAxis.YAxis:
                        axis = Vector3.up; // Y axis (default)
                        break;
                    case LookAtAxis.ZAxis:
                        axis = Vector3.forward; // Z axis
                        break;
                }

                // Apply the rotation, adjusting for the desired look-at axis
                Quaternion adjustedRotation =
                    lookRotation * Quaternion.Inverse(Quaternion.LookRotation(axis, Vector3.up));
                transform.rotation = adjustedRotation;
            }
        }
    }
}