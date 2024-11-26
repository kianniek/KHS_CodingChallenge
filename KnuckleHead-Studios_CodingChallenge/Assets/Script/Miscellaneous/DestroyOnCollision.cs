using UnityEngine;
using UnityEngine.Events;

namespace Miscellaneous
{
    public class DestroyOnCollision : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayerMask;

        [SerializeField] private UnityEvent onDestroy;

        private void OnCollisionEnter(Collision other)
        {
            if (collisionLayerMask != other.gameObject.layer)
                return;

            Destroy(gameObject);
            onDestroy.Invoke();
        }
    }
}