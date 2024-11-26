using System;
using EquipmentSystem;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InteractableSystem
{
    public enum ColliderShape
    {
        Sphere,
        Cube
    }

    public enum InteractionType
    {
        Interact,
        Equip
    }

#if UNITY_EDITOR
    [CanEditMultipleObjects]
#endif
    public class Interactable : MonoBehaviour
    {
        [Header("Interactable Prompt Settings")]
        [SerializeField, Tooltip("The canvas displayed when the player is near the object")]
        internal GameObject promptCanvas;

        [SerializeField, Tooltip("The offset of the prompt canvas")]
        internal Vector3 promptCanvasOffset = Vector3.up;

        [SerializeField, Tooltip("Whether the prompt canvas uses world space or not")]
        internal bool useWorldSpaceDirections = false;

        [SerializeField, Tooltip("The tag of the object that triggers the prompt canvas")]
        internal string promptTriggerTag = "Player";

        [SerializeField, Tooltip("Choose the shape of the _collider")]
        private ColliderShape colliderShape = ColliderShape.Sphere;

        [SerializeField] private InteractionGlobalSettings interactionGlobalSettings;

        [SerializeField, Tooltip("Set the _collider as a trigger or not")]
        private bool triggerOnly = true;

        //[Header("Interaction Settings")]
        [SerializeField] internal InteractionType interactionType; // Whether the object can be equipped or not

        [SerializeField, Tooltip("The slot where the item will be equipped")]
        private EquipmentSlot slot;

        private PlayerInputHandler playerInputHandler;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private bool isEquipped; // Only used for internal logic

        private void OnEnable()
        {
            if (promptCanvas != null)
                promptCanvas.SetActive(false);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            AddPromptCanvas();
            AddCollider();
        }

        public virtual void Interact(PlayerInputHandler playerInputHandler)
        {
            switch (interactionType)
            {
                case InteractionType.Interact:
                    Debug.Log("Interacting with object...");
                    break;
                case InteractionType.Equip:
                    Equip(playerInputHandler);
                    break;
            }
        }

        public virtual void Equip(PlayerInputHandler playerInputHandler)
        {
            playerInputHandler.EquipInteractableItem(slot, this.gameObject);
        }

        private void AddCollider()
        {
            // Check if there's already a _collider
            Collider existingCollider = GetComponent<Collider>();
            if (existingCollider != null)
            {
                if (existingCollider.isTrigger)
                {
                    _collider = existingCollider;
                }
            }

            switch (colliderShape)
            {
                case ColliderShape.Sphere:
                    SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
                    float worldSpaceRadius = interactionGlobalSettings.interactionDistance /
                                             Mathf.Max(transform.lossyScale.x, transform.lossyScale.y,
                                                 transform.lossyScale.z);
                    sphereCollider.radius = worldSpaceRadius;
                    sphereCollider.isTrigger = triggerOnly;
                    _collider = sphereCollider;
                    break;

                case ColliderShape.Cube:
                    BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                    var intDist = interactionGlobalSettings.interactionDistance;
                    boxCollider.size = new Vector3(intDist / transform.lossyScale.x, intDist / transform.lossyScale.y,
                        intDist / transform.lossyScale.z);
                    boxCollider.isTrigger = triggerOnly;
                    _collider = boxCollider;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Set the _collider as active or inactive.
        /// </summary>
        /// <param name="_isActive"></param>
        public void ColliderSetActive(bool _isActive)
        {
            if (!_collider)
            {
                return;
            }

            _collider.enabled = _isActive;
        }

        /// <summary>
        /// Set the _rigidbody as kinematic or not.
        /// </summary>
        /// <param name="_isKinematic"></param>
        public void RigidbodySetDiabled(bool _isKinematic)
        {
            if (!_rigidbody)
            {
                return;
            }

            _rigidbody.isKinematic = _isKinematic;
            isEquipped = _isKinematic;
        }

        private void AddPromptCanvas()
        {
            // Instantiate the prompt canvas as a world space canvas
            promptCanvas = Instantiate(promptCanvas, transform.position, Quaternion.identity);
            promptCanvas.name = $"{name}_PromptCanvasInstance";

            // Set the promptCanvas to a world space canvas
            Canvas canvas = promptCanvas.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.WorldSpace; // Make sure the canvas is in world space
            }

            // Now parent the prompt canvas to the object, but maintain world space positioning
            promptCanvas.transform.SetParent(null); // Detach from the object so it's not affected by rotation

            // Apply the desired offset (position above the object)
            promptCanvas.transform.position = transform.position + promptCanvasOffset;

            // Deactivate the prompt canvas initially
            promptCanvas.SetActive(false);
        }


        private void Update()
        {
            if (promptCanvas != null && promptCanvas.activeSelf)
            {
                // Ensure the prompt canvas stays above the object
                Vector3 worldPosition = transform.position + promptCanvasOffset;
                promptCanvas.transform.position = worldPosition;

                // Optionally, adjust the canvas position based on camera or world space logic here
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player")) // Check if the _collider belongs to the player
            {
                //check with dot product if the player is facing the object
                Vector3 directionToPlayer = other.transform.position - transform.position;
                directionToPlayer.Normalize();

                Vector3 playerForward = other.transform.forward;
                playerForward.Normalize();

                float dotProduct = Vector3.Dot(directionToPlayer, playerForward);

                if (dotProduct > 0.7f)
                {
                    DisablePromptCanvas();
                    return;
                }

                EnablePromptCanvas();
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) // Check if the _collider belongs to the player
            {
                DisablePromptCanvas();
            }
        }

        /// <summary>
        /// Enable the prompt canvas.
        /// </summary>
        public void EnablePromptCanvas()
        {
            if (promptCanvas != null && !isEquipped)
                promptCanvas.SetActive(true);
        }

        /// <summary>
        /// Disable the prompt canvas.
        /// </summary>
        public void DisablePromptCanvas()
        {
            if (promptCanvas != null)
                promptCanvas.SetActive(false);
        }

        private void OnDestroy()
        {
            if (promptCanvas != null)
                Destroy(promptCanvas);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            switch (colliderShape)
            {
                case ColliderShape.Sphere:
                    Gizmos.DrawWireSphere(transform.position, interactionGlobalSettings.interactionDistance);
                    break;
                case ColliderShape.Cube:
                    var intDist = interactionGlobalSettings.interactionDistance;
                    Gizmos.DrawWireCube(transform.position, new Vector3(intDist, intDist, intDist));
                    break;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Interactable))]
    [CanEditMultipleObjects]
    public class InteractableEditor : Editor
    {
        SerializedProperty interactionGlobalSettingsProp;
        SerializedProperty promptCanvasProp;
        SerializedProperty promptTriggerTagProp;
        SerializedProperty interactionTypeProp;
        SerializedProperty slotProp;
        SerializedProperty colliderShapeProp;
        SerializedProperty triggerOnlyProp;

        // New variables added for the custom editor
        SerializedProperty promptCanvasOffsetProp;
        SerializedProperty useWorldSpaceDirectionsProp;

        private void OnEnable()
        {
            // Link serialized properties
            interactionGlobalSettingsProp = serializedObject.FindProperty("interactionGlobalSettings");
            promptCanvasProp = serializedObject.FindProperty("promptCanvas");
            promptTriggerTagProp = serializedObject.FindProperty("promptTriggerTag");
            interactionTypeProp = serializedObject.FindProperty("interactionType");
            slotProp = serializedObject.FindProperty("slot");
            colliderShapeProp = serializedObject.FindProperty("colliderShape");
            triggerOnlyProp = serializedObject.FindProperty("triggerOnly");

            // Bind the new properties
            promptCanvasOffsetProp = serializedObject.FindProperty("promptCanvasOffset");
            useWorldSpaceDirectionsProp = serializedObject.FindProperty("useWorldSpaceDirections");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Start editing the serialized object

            // Draw the script reference as non-editable
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((Interactable)target),
                typeof(Interactable), false);
            GUI.enabled = true;

            EditorGUILayout.Space();

            // Draw Interaction Global Settings
            EditorGUILayout.LabelField("Interaction Global Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(interactionGlobalSettingsProp, new GUIContent("Interaction Global Settings"));

            EditorGUILayout.Space();

            // Draw Prompt Settings
            EditorGUILayout.LabelField("Prompt Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(promptCanvasProp, new GUIContent("Prompt Canvas"));
            EditorGUILayout.PropertyField(promptTriggerTagProp, new GUIContent("Prompt Trigger Tag"));

            // Draw new properties
            EditorGUILayout.PropertyField(promptCanvasOffsetProp, new GUIContent("Prompt Canvas Offset"));
            EditorGUILayout.PropertyField(useWorldSpaceDirectionsProp, new GUIContent("Use World Space Directions"));

            EditorGUILayout.Space();

            // Draw Interaction Settings
            EditorGUILayout.LabelField("Interaction Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(interactionTypeProp, new GUIContent("Interaction Type"));

            if ((InteractionType)interactionTypeProp.enumValueIndex == InteractionType.Equip)
            {
                EditorGUILayout.PropertyField(slotProp, new GUIContent("Slot"));
            }

            EditorGUILayout.Space();

            // Draw Collider Settings
            EditorGUILayout.LabelField("Collider Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(colliderShapeProp, new GUIContent("Collider Shape"));

            EditorGUILayout.PropertyField(triggerOnlyProp, new GUIContent("Trigger Only"));

            serializedObject.ApplyModifiedProperties(); // Apply changes to all selected objects
        }
    }
#endif
}