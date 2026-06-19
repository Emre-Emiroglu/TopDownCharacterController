using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Movement
{
    /// <summary>
    /// Moves the character by directly offsetting the Transform position each frame.
    /// No physics or collision detection is performed.
    /// </summary>
    public sealed class TransformMovementController : MovementController
    {
        #region Fields
        private Vector3 _currentVelocity;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the controller with the shared model and view.
        /// </summary>
        /// <param name="model">Shared runtime state providing move input and settings.</param>
        /// <param name="view">MonoBehaviour whose Transform is translated each frame.</param>
        public TransformMovementController(TopDownCharacterModel model, TopDownCharacterView view) : base(model, view) { }
        #endregion

        #region Executes
        /// <summary>
        /// Computes the smoothed velocity from move input and translates the Transform position.
        /// Exits early if the resolved velocity magnitude is below epsilon.
        /// </summary>
        /// <param name="parameters">Not used; reserved for base-class contract compatibility.</param>
        public override void Execute(params object[] parameters)
        {
            TopDownCharacterSettings settings = Model.Settings;
            
            Vector2 moveInput = Model.MoveInput;

            Vector3 target = moveInput.sqrMagnitude >= settings.MinimumMovementMagnitude
                ? new Vector3(moveInput.x, 0f, moveInput.y) * settings.MovementSpeed
                : Vector3.zero;

            float timeFactor = GetTimeFactor();

            _currentVelocity = ResolveVector3(_currentVelocity, target, timeFactor);

            if (_currentVelocity.sqrMagnitude < Mathf.Epsilon)
                return;

            View.transform.position += _currentVelocity * timeFactor;
        }
        #endregion
    }
}