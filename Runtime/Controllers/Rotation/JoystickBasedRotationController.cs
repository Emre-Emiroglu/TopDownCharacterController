using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;

namespace TopDownCharacterController.Runtime.Controllers.Rotation
{
    /// <summary>
    /// Rotates the character toward the right-stick (look joystick) input direction.
    /// Semantically distinct from <see cref="DirectionBasedRotationController"/> for clarity in settings,
    /// but shares the same execution logic.
    /// </summary>
    public sealed class JoystickBasedRotationController : DirectionBasedRotationController
    {
        #region Constructor
        /// <summary>
        /// Initializes the controller with the shared model and view.
        /// </summary>
        /// <param name="model">Shared runtime state providing look input and settings.</param>
        /// <param name="view">MonoBehaviour whose Transform rotation is updated each frame.</param>
        public JoystickBasedRotationController(TopDownCharacterModel model, TopDownCharacterView view) : base(model,
            view) { }
        #endregion
    }
}
