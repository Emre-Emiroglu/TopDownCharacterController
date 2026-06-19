using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;

namespace TopDownCharacterController.Runtime.Controllers.Rotation
{
    public sealed class JoystickBasedRotationController : DirectionBasedRotationController
    {
        #region Constructor
        public JoystickBasedRotationController(TopDownCharacterModel model, TopDownCharacterView view) : base(model,
            view) { }
        #endregion
    }
}
