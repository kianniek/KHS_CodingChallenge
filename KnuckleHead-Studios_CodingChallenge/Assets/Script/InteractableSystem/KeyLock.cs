using UnityEngine;
using UnityEngine.Events;

namespace Interactablesystem
{
    public class KeyLock : MonoBehaviour
    {
        // References to the key and lock GameObjects
        [SerializeField] private GameObject key; // The key GameObject
        [SerializeField] private  GameObject lockBody; // The lock body (the part where the key goes into)

        [SerializeField] private  float rotationAngle = 90f; // Angle the key rotates to when turned (e.g., 90 degrees)
        [SerializeField] private  float rotationSpeed = 2f; // Speed of the key rotation

        [SerializeField] private  bool isKeyTurned = false; // Current state of the key (whether it's turned or not)
        [SerializeField] private  Quaternion targetRotation; // The target rotation of the key

        // UnityEvent to trigger the key toggle action
        [SerializeField] private  UnityEvent<bool> onToggleKey;

        private void Awake()
        {
            if (key == null || lockBody == null)
            {
                Debug.LogError("Key and LockBody must be assigned.");
                return;
            }

            // Set the initial rotation of the key (not turned)
            targetRotation = key.transform.rotation;

            // Add listener to the UnityEvent
            if (onToggleKey == null)
                onToggleKey = new UnityEvent<bool>();
        }

        private void Update()
        {
            // Smoothly rotate the key to the target rotation
            key.transform.rotation = Quaternion.Slerp(
                key.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }

        /// <summary>
        /// Method to toggle the key (turn the key in the lock)
        /// </summary>
        /// <param name="turn"></param>
        public void ToggleKey(bool turn)
        {
            isKeyTurned = turn;

            // Set the target rotation based on the toggle state (on or off)
            if (isKeyTurned)
            {
                targetRotation = Quaternion.Euler(key.transform.rotation.eulerAngles.x,
                    key.transform.rotation.eulerAngles.y, rotationAngle); // Rotate key to "on" position
            }
            else
            {
                targetRotation = Quaternion.Euler(key.transform.rotation.eulerAngles.x,
                    key.transform.rotation.eulerAngles.y, 0f); // Rotate key to "off" position
            }

            // Fire the UnityEvent to notify other systems
            onToggleKey.Invoke(turn);
        }
    }
}