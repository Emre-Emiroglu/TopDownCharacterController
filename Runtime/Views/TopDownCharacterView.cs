using JetBrains.Annotations;
using ModelViewMediatorController.Runtime;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Views
{
    public class TopDownCharacterView : View
    {
        #region Fields
        [Header("Top Down Character View Fields")]
        [CanBeNull] [SerializeField] private Rigidbody rb;
        [CanBeNull] [SerializeField] private CharacterController characterController;
        #endregion

        #region Getters
        [CanBeNull] public Rigidbody Rb => rb;
        [CanBeNull] public CharacterController CharacterController => characterController;
        #endregion

    }
}