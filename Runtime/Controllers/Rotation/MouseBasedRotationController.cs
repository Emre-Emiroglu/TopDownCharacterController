using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Rotation
{
    /// <summary>
    /// Rotates the character to face the mouse cursor by projecting a screen-space ray onto the ground plane.
    /// Requires a camera reference to perform the <c>ScreenPointToRay</c> conversion.
    /// </summary>
    public sealed class MouseBasedRotationController : RotationController
    {
        #region ReadonlyFields
        private readonly Camera _gameCamera;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the controller and stores the camera used for screen-to-world raycasting.
        /// </summary>
        /// <param name="model">Shared runtime state providing look input (mouse screen position) and settings.</param>
        /// <param name="view">MonoBehaviour whose Transform rotation is updated each frame.</param>
        /// <param name="gameCamera">Camera used to convert screen coordinates to a world-space ray.</param>
        public MouseBasedRotationController(TopDownCharacterModel model, TopDownCharacterView view, Camera gameCamera) :
            base(model, view) => _gameCamera = gameCamera;
        #endregion

        #region Executes
        /// <summary>
        /// Raycasts from the mouse position to the ground plane and rotates the character toward the hit point.
        /// Exits early if the ray misses the plane or the resulting direction is below <c>MinimumRotationMagnitude</c>.
        /// </summary>
        /// <param name="parameters">Not used; reserved for base-class contract compatibility.</param>
        public override void Execute(params object[] parameters)
        {
            TopDownCharacterSettings settings = Model.Settings;
            Transform viewTransform = View.transform;
            Vector2 lookInput = Model.LookInput;

            Ray ray = _gameCamera.ScreenPointToRay(new Vector3(lookInput.x, lookInput.y, 0f));
            Plane groundPlane = new Plane(Vector3.up, viewTransform.position);

            if (!groundPlane.Raycast(ray, out float distance))
                return;

            Vector3 dir = ray.GetPoint(distance) - viewTransform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude < settings.MinimumRotationMagnitude)
                return;

            viewTransform.rotation = ApplyRotationAlgorithm(viewTransform.rotation, Quaternion.LookRotation(dir),
                settings.RotationMethod, settings.RotationSpeed, GetTimeFactor());
        }
        #endregion
    }
}