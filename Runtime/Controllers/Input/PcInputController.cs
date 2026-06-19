using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine.InputSystem;

namespace TopDownCharacterController.Runtime.Controllers.Input
{
    public sealed class PcInputController : InputController
    {
        #region Constructor
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