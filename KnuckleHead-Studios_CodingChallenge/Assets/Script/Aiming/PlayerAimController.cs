using UnityEngine;

namespace Aiming
{
    public class PlayerAimController : MonoBehaviour
    {
        [SerializeField] private Transform aimTarget;

        [SerializeField] private Transform playerRotation;

        [SerializeField] private Transform playerHead;

        [SerializeField] private float aimDistance = 10f;

        // Update is called once per frame
        void Update()
        {
            MoveAimTarget();
        }

        private void MoveAimTarget()
        {
            aimTarget.position = playerHead.position + playerRotation.forward * aimDistance;
        }
    }
}