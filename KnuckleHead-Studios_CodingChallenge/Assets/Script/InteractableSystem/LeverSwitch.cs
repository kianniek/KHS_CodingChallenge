using UnityEngine;
using UnityEngine.Events;

namespace Interactablesystem
{
    public class LeverSwitch : MonoBehaviour
    {
        [Header("Lever Settings")] public GameObject leverPivot;
        [SerializeField] private GameObject leverHandle;
        [SerializeField] private  float rotationAngle = 90f;
        [SerializeField] private  float rotationSpeed = 2f;

        private bool isLeverActivated = false;
        private Quaternion targetRotation;

        [SerializeField] private  UnityEvent<bool> onToggleLever;

        private void Awake()
        {
            if (leverPivot == null || leverHandle == null)
            {
                Debug.LogError("Lever Pivot and Handle must be assigned.");
                return;
            }

            targetRotation = leverPivot.transform.localRotation;

            if (onToggleLever == null)
            {
                onToggleLever = new UnityEvent<bool>();
            }
        }

        private void Update()
        {
            leverPivot.transform.localRotation = Quaternion.Slerp(
                leverPivot.transform.localRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }

        /// <summary>
        /// Method to toggle the lever (activate or deactivate the lever)
        /// </summary>
        /// <param name="activate"></param>
        public void ToggleLever(bool activate)
        {
            isLeverActivated = activate;

            if (isLeverActivated)
            {
                targetRotation = Quaternion.Euler(0f, 0f, rotationAngle);
            }
            else
            {
                targetRotation = Quaternion.Euler(0f, 0f, 45f);
            }

            onToggleLever.Invoke(activate);
        }
    }
}