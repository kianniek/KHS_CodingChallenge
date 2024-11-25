using UnityEngine;
using UnityEngine.Events;

namespace Interactablesystem
{
    public class LeverSwitch : MonoBehaviour
    {
        [Header("Lever Settings")] public GameObject leverPivot;
        public GameObject leverHandle;
        public float rotationAngle = 90f;
        public float rotationSpeed = 2f;

        private bool isLeverActivated = false;
        private Quaternion targetRotation;

        public UnityEvent<bool> onToggleLever;

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