using System;
using ModelViewMediatorController.Runtime;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDownCharacterController.Runtime.Controllers.Input
{
    public abstract class
        InputController : Controller<TopDownCharacterModel, TopDownCharacterSettings, TopDownCharacterView>
    {
        #region Fields
        protected InputAction MoveAction;
        protected InputAction LookAction;
        #endregion

        #region Properties
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        #endregion

        #region Constructor
        protected InputController(TopDownCharacterModel model, TopDownCharacterView view, InputAction moveAction,
            InputAction lookAction) : base(model, view)
        {
            MoveAction = moveAction;
            LookAction = lookAction;
        }
        #endregion

        #region Executes
        public override void Execute(params object[] parameters)
        {
            if (parameters[0] is not InputStateType inputState)
                throw new ArgumentException(
                    $"Expected {nameof(InputStateType)} but received {parameters[0]?.GetType().Name ?? "null"}.",
                    nameof(parameters));

            switch (inputState)
            {
                case InputStateType.OnEnable:
                    Enable();
                    break;
                case InputStateType.OnDisable:
                    Disable();
                    break;
                case InputStateType.OnUpdate:
                    Update();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputState), inputState,
                        "Unhandled InputStateType value. Expected OnEnable, OnDisable, or OnUpdate.");
            }
        }
        private void Enable()
        {
            MoveAction.Enable();
            LookAction.Enable();
        }
        private void Disable()
        {
            MoveAction.Disable();
            LookAction.Disable();
        }
        private void Update()
        {
            MoveInput = MoveAction.ReadValue<Vector2>();
            LookInput = LookAction.ReadValue<Vector2>();
        }
        #endregion
    }
}