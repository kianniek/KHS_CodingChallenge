using UnityEngine;

namespace InteractableSystem
{
    [CreateAssetMenu(fileName = "InteractionGlobalSettings", menuName = "Interaction System/Interaction Global Settings")]
    public class InteractionGlobalSettings : ScriptableObject
    {
        [Header("Interaction Settings")]
        [Tooltip("The maximum distance from which the player can interact with objects.")]
        public float interactionDistance = 2f;
        [Tooltip("The layer mask used to filter out objects that the player can interact with.")]
        public LayerMask interactionLayerMask;
    }
}