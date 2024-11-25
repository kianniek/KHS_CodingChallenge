using System.Collections;
using UnityEngine;

namespace Interactablesystem
{
    public class DoorHandler : MonoBehaviour
    {
        public float openAngle = 90f; // The angle the door will rotate when opened
        public float closeAngle = 0f; // The angle the door will be at when closed
        public float doorSpeed = 2f; // Speed at which the door rotates
        public Transform playerTransform; // Reference to the camera's transform

        private bool isOpening = false;
        private bool isClosing = false;

        public void DoorInteraction(bool isOpen)
        {
            if (isOpen)
            {
                StartCoroutine(OpenDoor());
            }
            else
            {
                StartCoroutine(CloseDoor());
            }
        }

        private IEnumerator OpenDoor()
        {
            isOpening = true;
            isClosing = false;
            Debug.Log("Door opened");

            // Determine which direction the door should swing based on the camera's position
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            float dotProduct = Vector3.Dot(transform.right, directionToPlayer);
            
            Debug.Log(dotProduct);

            // If the camera is in front of the door, swing it open to the right
            if (dotProduct > 0)
            {
                openAngle = 90f; // Open to the right
            }
            else
            {
                openAngle = -90f; // Open to the left
            }

            float startAngle = transform.eulerAngles.y;
            float targetAngle = openAngle;

            // Smoothly rotate the door to the target angle over time
            float timeElapsed = 0f;
            while (timeElapsed < 1f)
            {
                timeElapsed += Time.deltaTime * doorSpeed;
                float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, timeElapsed);
                transform.eulerAngles = new Vector3(0, currentAngle, 0);
                yield return null; // Wait for the next frame
            }

            transform.eulerAngles = new Vector3(0, targetAngle, 0); // Ensure it ends exactly at the target angle
        }

        private IEnumerator CloseDoor()
        {
            isClosing = true;
            isOpening = false;
            Debug.Log("Door closed");

            float startAngle = transform.eulerAngles.y;
            float targetAngle = closeAngle;

            // Smoothly rotate the door to the target angle over time
            float timeElapsed = 0f;
            while (timeElapsed < 1f)
            {
                timeElapsed += Time.deltaTime * doorSpeed;
                float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, timeElapsed);
                transform.eulerAngles = new Vector3(0, currentAngle, 0);
                yield return null; // Wait for the next frame
            }

            transform.eulerAngles = new Vector3(0, targetAngle, 0); // Ensure it ends exactly at the target angle
        }
    }
}