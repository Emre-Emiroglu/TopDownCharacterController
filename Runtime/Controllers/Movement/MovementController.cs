using ModelViewMediatorController.Runtime;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Movement
{
    /// <summary>
    /// Abstract base for all movement implementations. Provides shared time-factor helpers and a smoothing algorithm resolver
    /// so subclasses avoid duplicating interpolation logic.
    /// </summary>
    public abstract class
        MovementController : Controller<TopDownCharacterModel, TopDownCharacterSettings, TopDownCharacterView>
    {
        #region Fields
        private Vector3 _smoothDampRef;
        #endregion

        #region Constructor
        protected MovementController(TopDownCharacterModel model, TopDownCharacterView view) : base(model, view) { }
        #endregion

        #region Executes
        protected float GetTimeFactor() =>
            Model.Settings.UseUnscaledDeltaTimeOnMovement ? Time.unscaledDeltaTime : Time.deltaTime;
        protected float GetFixedTimeFactor() => Model.Settings.UseUnscaledDeltaTimeOnMovement
            ? Time.fixedUnscaledDeltaTime
            : Time.fixedDeltaTime;
        protected Vector3 ResolveVector3(Vector3 current, Vector3 target, float timeFactor)
        {
            TopDownCharacterSettings settings = Model.Settings;

            return settings.TransformMovementMethod switch
            {
                TransformMovementMethod.Direct => target,
                TransformMovementMethod.Lerp => Vector3.Lerp(current, target, settings.MovementSmoothTime * timeFactor),
                TransformMovementMethod.SmoothDamp => Vector3.SmoothDamp(current, target, ref _smoothDampRef,
                    settings.MovementSmoothTime, Mathf.Infinity, timeFactor),
                TransformMovementMethod.MoveTowards => Vector3.MoveTowards(current, target,
                    settings.MovementSmoothTime * timeFactor),
                _ => target
            };
        }
        #endregion
    }
}