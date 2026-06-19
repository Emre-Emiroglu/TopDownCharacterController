using TopDownCharacterController.Runtime.Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterControllerMovementMethod = TopDownCharacterController.Runtime.Enums.CharacterControllerMovementMethod;

namespace TopDownCharacterController.Runtime.Data
{
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
        public InputAction MoveAction => moveAction;
        public InputAction LookAction => lookAction;
        public InputType InputType => inputType;
        public MovementType MovementType => movementType;
        public TransformMovementMethod TransformMovementMethod => transformMovementMethod;
        public float MinimumMovementMagnitude => minimumMovementMagnitude;
        public float MovementSpeed => movementSpeed;
        public float MovementSmoothTime => movementSmoothTime;
        public bool UseUnscaledDeltaTimeOnMovement => useUnscaledDeltaTimeOnMovement;
        public PhysicMovementMethod PhysicMovementMethod => physicMovementMethod;
        public ForceMode PhysicMovementForceMode => physicMovementForceMode;
        public CharacterControllerMovementMethod CharacterControllerMovementMethod => characterControllerMovementMethod;
        public RotationType RotationType => rotationType;
        public RotationMethod RotationMethod => rotationMethod;
        public float MinimumRotationMagnitude => minimumRotationMagnitude;
        public float RotationSpeed => rotationSpeed;
        public bool UseUnscaledDeltaTimeOnRotation => useUnscaledDeltaTimeOnRotation;
        #endregion
    }
}