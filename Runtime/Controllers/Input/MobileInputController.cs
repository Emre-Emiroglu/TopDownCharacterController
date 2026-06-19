using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine.InputSystem;

namespace TopDownCharacterController.Runtime.Controllers.Input
{
    public sealed class MobileInputController : ConsoleInputController
    {
        #region Constructor
        public MobileInputController(TopDownCharacterModel model, TopDownCharacterView view, InputAction moveAction,
            InputAction lookAction) : base(model, view, moveAction, lookAction) { }
        #endregion
    }
}