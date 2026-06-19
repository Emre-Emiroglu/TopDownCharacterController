using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine.InputSystem;

namespace TopDownCharacterController.Runtime.Controllers.Input
{
    /// <summary>
    /// Input handler for console gamepad: left stick for movement, right stick for look.
    /// Also serves as the binding base for <see cref="MobileInputController"/>.
    /// </summary>
    public class ConsoleInputController : InputController
    {
        #region Constructor
        /// <summary>
        /// Creates and binds left-stick and right-stick gamepad actions.
        /// </summary>
        /// <param name="model">Shared runtime state of the character.</param>
        /// <param name="view">MonoBehaviour providing component references.</param>
        /// <param name="moveAction">Unused; overridden with left-stick gamepad binding.</param>
        /// <param name="lookAction">Unused; overridden with right-stick gamepad binding.</param>
        public ConsoleInputController(TopDownCharacterModel model, TopDownCharacterView view, InputAction moveAction,
            InputAction lookAction) : base(model, view, moveAction, lookAction)
        {
            MoveAction = new InputAction("Move");
            LookAction = new InputAction("Look");
            
            MoveAction.AddBinding("<Gamepad>/leftStick");
            LookAction.AddBinding("<Gamepad>/rightStick");
        }
        #endregion
    }
}