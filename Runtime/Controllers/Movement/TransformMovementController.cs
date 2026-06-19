using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Movement
{
    public sealed class TransformMovementController : MovementController
    {
        #region Fields
        private Vector3 _currentVelocity;
        #endregion

        #region Constructor
        public TransformMovementController(TopDownCharacterModel model, TopDownCharacterView view) : base(model, view) { }
        #endregion

        #region Executes
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