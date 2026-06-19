using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Rotation
{
    public sealed class MouseBasedRotationController : RotationController
    {
        #region ReadonlyFields
        private readonly Camera _gameCamera;
        #endregion

        #region Constructor
        public MouseBasedRotationController(TopDownCharacterModel model, TopDownCharacterView view, Camera gameCamera) :
            base(model, view) => _gameCamera = gameCamera;
        #endregion

        #region Executes
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