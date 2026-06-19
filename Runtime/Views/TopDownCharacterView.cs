using JetBrains.Annotations;
using ModelViewMediatorController.Runtime;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Views
{
    /// <summary>
    /// MonoBehaviour view for the top-down character. Exposes optional physics components to the MVMC controller layer.
    /// </summary>
    public class TopDownCharacterView : View
    {
        #region Fields
        [Header("Top Down Character View Fields")]
        [CanBeNull] [SerializeField] private Rigidbody rb;
        [CanBeNull] [SerializeField] private CharacterController characterController;
        #endregion

        #region Getters
        /// <summary>Optional Rigidbody used by <c>PhysicMovementController</c>. Null when <c>MovementType</c> is not <c>Physic</c>.</summary>
        [CanBeNull] public Rigidbody Rb => rb;
        /// <summary>Optional CharacterController used by <c>CharacterControllerMovementController</c>. Null when <c>MovementType</c> is not <c>CharacterController</c>.</summary>
        [CanBeNull] public CharacterController CharacterController => characterController;
        #endregion

    }
}