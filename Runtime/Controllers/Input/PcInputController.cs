using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine.InputSystem;

namespace TopDownCharacterController.Runtime.Controllers.Input
{
    /// <summary>
    /// Input handler for PC: WASD and arrow keys for movement, mouse screen position for look.
    /// </summary>
    public sealed class PcInputController : InputController
    {
        #region Constructor
        /// <summary>
        /// Creates and binds WASD, arrow-key composite actions and a mouse-position look action.
        /// </summary>
        /// <param name="model">Shared runtime state of the character.</param>
        /// <param name="view">MonoBehaviour providing component references.</param>
        /// <param name="moveAction">Unused; overridden with keyboard composite bindings.</param>
        /// <param name="lookAction">Unused; overridden with mouse position binding.</param>
        public PcInputController(TopDownCharacterModel model, TopDownCharacterView view, InputAction moveAction,
            InputAction lookAction) : base(model, view, moveAction, lookAction)
        {
            MoveAction = new InputAction("Move");
            LookAction = new InputAction("Look");

            MoveAction.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
            MoveAction.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow").With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");

            LookAction.AddBinding("<Mouse>/position");
        }
        #endregion
    }
}