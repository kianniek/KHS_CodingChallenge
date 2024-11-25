using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Interactablesystem
{
    public class ButtonScript : MonoBehaviour
    {
        [SerializeField] private GameObject buttonMovingPart;
        private MeshFilter buttonMesh;
        private float lowerAmount;

        [SerializeField] private bool isToggleable;
        private bool isPressed;
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

        public void ResetButtonPosition()
        {
            Vector3 currentPosition = buttonMovingPart.transform.position;
            buttonMovingPart.transform.position =
                new Vector3(currentPosition.x, currentPosition.y + lowerAmount, currentPosition.z);
        }
    }
}