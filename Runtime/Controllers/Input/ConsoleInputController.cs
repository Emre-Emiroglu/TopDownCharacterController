using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine.InputSystem;

namespace TopDownCharacterController.Runtime.Controllers.Input
{
    public class ConsoleInputController : InputController
    {
        #region Constructor
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