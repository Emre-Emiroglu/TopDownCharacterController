using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Rotation
{
    public class DirectionBasedRotationController : RotationController
    {
        #region Constructor
        public DirectionBasedRotationController(TopDownCharacterModel model, TopDownCharacterView view) : base(model,
            view) { }
        #endregion

        #region Executes
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