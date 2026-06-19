using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine.InputSystem;

namespace TopDownCharacterController.Runtime.Controllers.Input
{
    /// <summary>
    /// Input handler for mobile: reuses <see cref="ConsoleInputController"/> gamepad bindings targeted by
    /// <c>OnScreenStick</c> components that map to Gamepad paths at runtime.
    /// </summary>
    public sealed class MobileInputController : ConsoleInputController
    {
        #region Constructor
        /// <summary>
        /// Delegates entirely to <see cref="ConsoleInputController"/> bindings; no additional setup required.
        /// </summary>
        /// <param name="model">Shared runtime state of the character.</param>
        /// <param name="view">MonoBehaviour providing component references.</param>
        /// <param name="moveAction">Unused; inherited left-stick binding is used.</param>
        /// <param name="lookAction">Unused; inherited right-stick binding is used.</param>
        public MobileInputController(TopDownCharacterModel model, TopDownCharacterView view, InputAction moveAction,
            InputAction lookAction) : base(model, view, moveAction, lookAction) { }
        #endregion
    }
}