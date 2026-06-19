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
    /// <summary>
    /// Abstract base for all platform-specific input handlers. Manages <c>InputAction</c> lifecycle and caches
    /// the current move and look vectors each frame.
    /// </summary>
    public abstract class
        InputController : Controller<TopDownCharacterModel, TopDownCharacterSettings, TopDownCharacterView>
    {
        #region Fields
        protected InputAction MoveAction;
        protected InputAction LookAction;
        #endregion

        #region Properties
        /// <summary>The movement vector polled from <c>MoveAction</c> on the last <c>OnUpdate</c> tick.</summary>
        public Vector2 MoveInput { get; private set; }
        /// <summary>The look/aim vector polled from <c>LookAction</c> on the last <c>OnUpdate</c> tick.</summary>
        public Vector2 LookInput { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the controller with pre-built input actions that subclasses may override with platform-specific bindings.
        /// </summary>
        /// <param name="model">Shared runtime state of the character.</param>
        /// <param name="view">MonoBehaviour providing component references.</param>
        /// <param name="moveAction">Fallback move <c>InputAction</c> supplied by the settings asset.</param>
        /// <param name="lookAction">Fallback look <c>InputAction</c> supplied by the settings asset.</param>
        protected InputController(TopDownCharacterModel model, TopDownCharacterView view, InputAction moveAction,
            InputAction lookAction) : base(model, view)
        {
            MoveAction = moveAction;
            LookAction = lookAction;
        }
        #endregion

        #region Executes
        /// <summary>
        /// Dispatches the requested lifecycle operation on the managed input actions.
        /// </summary>
        /// <param name="parameters">Single-element array containing an <see cref="InputStateType"/> value.</param>
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