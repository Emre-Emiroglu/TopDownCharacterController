using ModelViewMediatorController.Runtime;
using TopDownCharacterController.Runtime.Controllers.Input;
using TopDownCharacterController.Runtime.Controllers.Movement;
using TopDownCharacterController.Runtime.Controllers.Rotation;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;

namespace TopDownCharacterController.Runtime.Views
{
    public sealed class
        TopDownCharacterMediator : Mediator<TopDownCharacterModel, TopDownCharacterSettings, TopDownCharacterView>
    {
        #region ReadonlyFields
        private readonly InputController _inputController;
        private readonly MovementController _movementController;
        private readonly RotationController _rotationController;
        #endregion

        #region Constructor
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
        public override void SetSubscriptions(bool isSubscribed) =>
            _inputController.Execute(isSubscribed ? InputStateType.OnEnable : InputStateType.OnDisable);
        #endregion

        #region Executes
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