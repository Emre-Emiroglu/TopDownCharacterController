using TopDownCharacterController.Runtime.Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterControllerMovementMethod = TopDownCharacterController.Runtime.Enums.CharacterControllerMovementMethod;

namespace TopDownCharacterController.Runtime.Data
{
    /// <summary>
    /// ScriptableObject holding all runtime configuration for the TopDownCharacterController package.
    /// Create via <c>TopDownCharacterController/TopDownCharacterSettings</c> in the Asset menu.
    /// </summary>
    [CreateAssetMenu(fileName = "TopDownCharacterSettings",
        menuName = "TopDownCharacterController/TopDownCharacterSettings")]
    public class TopDownCharacterSettings : ScriptableObject
    {
        #region Fields
        [Header("Input Settings")]
        [SerializeField] private InputAction moveAction;
        [SerializeField] private InputAction lookAction;
        [SerializeField] private InputType inputType;
        [Header("Movement Settings")]
        [SerializeField] private MovementType movementType;
        [Range(0f, 1f)] [SerializeField] private float minimumMovementMagnitude;
        [Range(0f, 128f)] [SerializeField] private float movementSpeed;
        [Range(0f, 20f)] [SerializeField] private float movementSmoothTime;
        [SerializeField] private bool useUnscaledDeltaTimeOnMovement;
        [Header("Transform Movement Settings")]
        [SerializeField] private TransformMovementMethod transformMovementMethod;
        [Header("Physic Movement Settings")]
        [SerializeField] private PhysicMovementMethod physicMovementMethod;
        [SerializeField] private ForceMode physicMovementForceMode;
        [Header("Character Controller Movement Settings")]
        [SerializeField] private CharacterControllerMovementMethod characterControllerMovementMethod;
        [Header("Rotation Settings")]
        [SerializeField] private RotationType rotationType;
        [SerializeField] private RotationMethod rotationMethod;
        [Range(0f, 1f)] [SerializeField] private float minimumRotationMagnitude;
        [Range(0f, 1080)] [SerializeField] private float rotationSpeed;
        [SerializeField] private bool useUnscaledDeltaTimeOnRotation;
        #endregion

        #region Getters
        /// <summary>Input action that supplies the two-axis movement vector.</summary>
        public InputAction MoveAction => moveAction;
        /// <summary>Input action that supplies the two-axis look/aim vector.</summary>
        public InputAction LookAction => lookAction;
        /// <summary>Active input device scheme used to select the correct <c>InputController</c> subclass.</summary>
        public InputType InputType => inputType;
        /// <summary>Locomotion backend that drives the character's position each frame.</summary>
        public MovementType MovementType => movementType;
        /// <summary>Smoothing algorithm used by <c>TransformMovementController</c>.</summary>
        public TransformMovementMethod TransformMovementMethod => transformMovementMethod;
        /// <summary>Minimum sqrMagnitude of move input required to begin applying movement.</summary>
        public float MinimumMovementMagnitude => minimumMovementMagnitude;
        /// <summary>Units per second (or force magnitude) applied to the character when moving.</summary>
        public float MovementSpeed => movementSpeed;
        /// <summary>Smoothing time used by Lerp, SmoothDamp, and MoveTowards movement algorithms.</summary>
        public float MovementSmoothTime => movementSmoothTime;
        /// <summary>When true, movement uses <c>Time.unscaledDeltaTime</c>; otherwise <c>Time.deltaTime</c>.</summary>
        public bool UseUnscaledDeltaTimeOnMovement => useUnscaledDeltaTimeOnMovement;
        /// <summary>API used to apply locomotion to the Rigidbody component.</summary>
        public PhysicMovementMethod PhysicMovementMethod => physicMovementMethod;
        /// <summary>ForceMode passed to <c>AddForce</c> and <c>AddRelativeForce</c> calls.</summary>
        public ForceMode PhysicMovementForceMode => physicMovementForceMode;
        /// <summary>API used to drive the CharacterController component.</summary>
        public CharacterControllerMovementMethod CharacterControllerMovementMethod => characterControllerMovementMethod;
        /// <summary>Input source that determines the character's look direction each frame.</summary>
        public RotationType RotationType => rotationType;
        /// <summary>Quaternion interpolation algorithm used by all rotation controllers.</summary>
        public RotationMethod RotationMethod => rotationMethod;
        /// <summary>Minimum sqrMagnitude of look input required to begin applying rotation.</summary>
        public float MinimumRotationMagnitude => minimumRotationMagnitude;
        /// <summary>Angular speed (degrees per second for RotateTowards; blend factor for Lerp/Slerp) applied each frame.</summary>
        public float RotationSpeed => rotationSpeed;
        /// <summary>When true, rotation uses <c>Time.unscaledDeltaTime</c>; otherwise <c>Time.deltaTime</c>.</summary>
        public bool UseUnscaledDeltaTimeOnRotation => useUnscaledDeltaTimeOnRotation;
        #endregion
    }
}