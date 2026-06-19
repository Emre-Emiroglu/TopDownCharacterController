using ModelViewMediatorController.Runtime;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Rotation
{
    /// <summary>
    /// Abstract base for all rotation implementations. Provides a time-factor helper and a shared quaternion
    /// interpolation method so subclasses avoid duplicating per-algorithm switch logic.
    /// </summary>
    public abstract class
        RotationController : Controller<TopDownCharacterModel, TopDownCharacterSettings, TopDownCharacterView>
    {
        #region Constructor
        protected RotationController(TopDownCharacterModel model, TopDownCharacterView view) : base(model, view) { }
        #endregion

        #region Executes
        protected float GetTimeFactor() =>
            Model.Settings.UseUnscaledDeltaTimeOnRotation ? Time.unscaledDeltaTime : Time.deltaTime;
        protected static Quaternion ApplyRotationAlgorithm(Quaternion from, Quaternion to,
            RotationMethod algorithm, float speed, float timeFactor) => algorithm switch
        {
            RotationMethod.Lerp => Quaternion.Lerp(from, to, speed * timeFactor),
            RotationMethod.Slerp => Quaternion.Slerp(from, to, speed * timeFactor),
            RotationMethod.RotateTowards => Quaternion.RotateTowards(from, to, speed * timeFactor),
            _ => to
        };
        #endregion
    }
}