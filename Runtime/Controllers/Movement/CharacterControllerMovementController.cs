using System;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Movement
{
    /// <summary>
    /// Moves the character using Unity's <see cref="CharacterController"/> component with optional manual gravity simulation.
    /// Requires a CharacterController assigned on <see cref="TopDownCharacterView"/>; exits silently if it is null.
    /// </summary>
    public sealed class CharacterControllerMovementController : MovementController
    {
        #region Fields
        private float _verticalVelocity;
        private Vector3 _currentHorizontal;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the controller with the shared model and view.
        /// </summary>
        /// <param name="model">Shared runtime state providing move input and settings.</param>
        /// <param name="view">MonoBehaviour whose CharacterController is driven each frame.</param>
        public CharacterControllerMovementController(TopDownCharacterModel model, TopDownCharacterView view) : base(
            model, view) { }
        #endregion

        #region Executes
        /// <summary>
        /// Applies smoothed horizontal movement and, when using <c>Move</c>, accumulates vertical velocity for gravity.
        /// No-ops silently if <see cref="TopDownCharacterView.CharacterController"/> is null.
        /// </summary>
        /// <param name="parameters">Not used; reserved for base-class contract compatibility.</param>
        public override void Execute(params object[] parameters)
        {
            CharacterController characterController = View.CharacterController;

            if (!characterController)
                return;

            TopDownCharacterSettings settings = Model.Settings;
            
            float timeFactor = GetTimeFactor();
            
            Vector2 moveInput = Model.MoveInput;
            
            Vector3 targetHorizontal = new Vector3(moveInput.x, 0f, moveInput.y) * settings.MovementSpeed;

            _currentHorizontal = ResolveVector3(_currentHorizontal, targetHorizontal, timeFactor);

            switch (settings.CharacterControllerMovementMethod)
            {
                case CharacterControllerMovementMethod.Move:
                    if (characterController.isGrounded && _verticalVelocity < 0f)
                        _verticalVelocity = -2f;

                    _verticalVelocity += Physics.gravity.y * timeFactor;

                    characterController.Move(
                        new Vector3(_currentHorizontal.x, _verticalVelocity, _currentHorizontal.z) * timeFactor);
                    break;
                case CharacterControllerMovementMethod.SimpleMove:
                    characterController.SimpleMove(new Vector3(_currentHorizontal.x, 0f, _currentHorizontal.z));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(settings.CharacterControllerMovementMethod),
                        settings.CharacterControllerMovementMethod,
                        "Unsupported CharacterControllerMovementMethod. Expected Move or SimpleMove.");
            }
        }
        #endregion
    }
}