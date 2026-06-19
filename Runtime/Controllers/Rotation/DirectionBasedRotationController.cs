using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Rotation
{
    /// <summary>
    /// Rotates the character to face its current movement direction.
    /// When <c>RotationType</c> is <c>DirectionBased</c>, the mediator feeds move input into <c>LookInput</c> before calling this controller.
    /// </summary>
    public class DirectionBasedRotationController : RotationController
    {
        #region Constructor
        /// <summary>
        /// Initializes the controller with the shared model and view.
        /// </summary>
        /// <param name="model">Shared runtime state providing look input and settings.</param>
        /// <param name="view">MonoBehaviour whose Transform rotation is updated each frame.</param>
        public DirectionBasedRotationController(TopDownCharacterModel model, TopDownCharacterView view) : base(model,
            view) { }
        #endregion

        #region Executes
        /// <summary>
        /// Rotates the character Transform toward the look-input direction using the configured <see cref="RotationMethod"/>.
        /// Exits early if the look-input magnitude is below <c>MinimumRotationMagnitude</c>.
        /// </summary>
        /// <param name="parameters">Not used; reserved for base-class contract compatibility.</param>
        public override void Execute(params object[] parameters)
        {
            TopDownCharacterSettings settings = Model.Settings;
            Vector2 lookInput = Model.LookInput;

            if (lookInput.sqrMagnitude < settings.MinimumRotationMagnitude)
                return;

            Vector3 dir = new Vector3(lookInput.x, 0f, lookInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            Transform viewTransform = View.transform;

            viewTransform.rotation = ApplyRotationAlgorithm(viewTransform.rotation, targetRotation,
                settings.RotationMethod, settings.RotationSpeed, GetTimeFactor());
        }
        #endregion
    }
}