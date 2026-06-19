using ModelViewMediatorController.Runtime;
using TopDownCharacterController.Runtime.Controllers.Input;
using TopDownCharacterController.Runtime.Controllers.Movement;
using TopDownCharacterController.Runtime.Controllers.Rotation;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;

namespace TopDownCharacterController.Runtime.Views
{
    /// <summary>
    /// Orchestrates the per-frame input → model → movement → rotation pipeline for the top-down character.
    /// </summary>
    public sealed class
        TopDownCharacterMediator : Mediator<TopDownCharacterModel, TopDownCharacterSettings, TopDownCharacterView>
    {
        #region ReadonlyFields
        private readonly InputController _inputController;
        private readonly MovementController _movementController;
        private readonly RotationController _rotationController;
        #endregion

        #region Constructor
        /// <summary>
        /// Wires the MVMC stack with the three pre-built sub-controllers.
        /// </summary>
        /// <param name="model">Runtime state shared across the controller layer.</param>
        /// <param name="view">MonoBehaviour entry point providing component references.</param>
        /// <param name="inputController">Platform-specific handler that reads move and look input.</param>
        /// <param name="movementController">Backend that applies position changes each frame.</param>
        /// <param name="rotationController">Backend that applies rotation changes each frame.</param>
        public TopDownCharacterMediator(TopDownCharacterModel model, TopDownCharacterView view,
            InputController inputController, MovementController movementController,
            RotationController rotationController) : base(model, view)
        {
            _inputController = inputController;
            _movementController = movementController;
            _rotationController = rotationController;
        }
        #endregion

        #region Core
        /// <summary>
        /// Enables or disables input action polling based on the subscription state.
        /// </summary>
        /// <param name="isSubscribed">Pass <c>true</c> to enable input actions; <c>false</c> to disable them.</param>
        public override void SetSubscriptions(bool isSubscribed) =>
            _inputController.Execute(isSubscribed ? InputStateType.OnEnable : InputStateType.OnDisable);
        #endregion

        #region Executes
        /// <summary>
        /// Per-frame update: polls input, feeds the model, then executes movement and rotation.
        /// Call this from <c>MonoBehaviour.Update</c>.
        /// </summary>
        public void Tick()
        {
            _inputController.Execute(InputStateType.OnUpdate);

            Model.SetMoveInput(_inputController.MoveInput);

            Model.SetLookInput(Model.Settings.RotationType == RotationType.DirectionBased
                ? _inputController.MoveInput
                : _inputController.LookInput);

            _movementController.Execute();

            _rotationController.Execute();
        }
        #endregion
    }
}