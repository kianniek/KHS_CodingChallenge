using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Interactablesystem
{
    public class ButtonScript : MonoBehaviour
    {
        private MeshFilter buttonMesh;
        private float lowerAmount;
        private bool isPressed;

        [SerializeField] private GameObject buttonMovingPart;
        [SerializeField] private bool isToggleable;
        [SerializeField] private float popUpDelay = 1f;

        public UnityEvent onButtonPressed;

        private void Start()
        {
            buttonMesh = buttonMovingPart.GetComponent<MeshFilter>();

            if (buttonMesh == null)
            {
                Debug.LogError("Button does not have a BoxCollider component attached.");
                return;
            }

            lowerAmount = buttonMesh.mesh.bounds.size.y * 0.2f;
        }

        /// <summary>
        /// Presses the button.
        /// </summary>
        public void PressButton()
        {
            if (isToggleable)
            {
                if (!isPressed)
                {
                    LowerButton();
                    onButtonPressed.Invoke();
                    isPressed = true;
                }
                else
                {
                    ResetButtonPosition();
                    isPressed = false;
                }
            }
            else
            {
                LowerButton();
                onButtonPressed.Invoke();
                StartCoroutine(PopUpButton());
            }
        }

        private void LowerButton()
        {
            Vector3 currentPosition = buttonMovingPart.transform.position;
            buttonMovingPart.transform.position =
                new Vector3(currentPosition.x, currentPosition.y - lowerAmount, currentPosition.z);
        }

        IEnumerator PopUpButton()
        {
            yield return new WaitForSeconds(popUpDelay);
            ResetButtonPosition();
        }

        /// <summary>
        /// Resets the button to its original position.
        /// </summary>
        public void ResetButtonPosition()
        {
            Vector3 currentPosition = buttonMovingPart.transform.position;
            buttonMovingPart.transform.position =
                new Vector3(currentPosition.x, currentPosition.y + lowerAmount, currentPosition.z);
        }
    }
}