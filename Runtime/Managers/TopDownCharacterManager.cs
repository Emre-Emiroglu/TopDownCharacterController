using System;
using TopDownCharacterController.Runtime.Controllers.Input;
using TopDownCharacterController.Runtime.Controllers.Movement;
using TopDownCharacterController.Runtime.Controllers.Rotation;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDownCharacterController.Runtime.Managers
{
    public sealed class TopDownCharacterManager : MonoBehaviour
    {
        #region Fields
        [Header("Top Down Character Manager Fields")]
        [SerializeField] private Camera gameCamera;
        [SerializeField] private TopDownCharacterSettings topDownCharacterSettings;
        [SerializeField] private TopDownCharacterView topDownCharacterView;
        private TopDownCharacterModel _topDownCharacterModel;
        private TopDownCharacterMediator _topDownCharacterMediator;
        private InputController _inputController;
        private MovementController _movementController;
        private RotationController _rotationController;
        private InputAction _moveAction;
        private InputAction _lookAction;
        #endregion

        #region Core
        private void Awake() => Bindings();
        private void Start() => _topDownCharacterMediator?.Initialize();
        private void OnDestroy() => _topDownCharacterMediator?.Dispose();
        private void Update() => _topDownCharacterMediator?.Tick();
        #endregion

        #region Bindings
        private void Bindings()
        {
            ModelBindings();
            ControllerBindings();
            MediationBindings();
        }
        private void ModelBindings()
        {
            _topDownCharacterModel = new TopDownCharacterModel(topDownCharacterSettings);
            
            _moveAction = topDownCharacterSettings.MoveAction;
            _lookAction = topDownCharacterSettings.LookAction;
        }
        private void ControllerBindings()
        {
            _inputController = topDownCharacterSettings.InputType switch
            {
                InputType.PC => new PcInputController(_topDownCharacterModel, topDownCharacterView, _moveAction,
                    _lookAction),
                InputType.Console => new ConsoleInputController(_topDownCharacterModel, topDownCharacterView,
                    _moveAction, _lookAction),
                InputType.Mobile => new MobileInputController(_topDownCharacterModel, topDownCharacterView, _moveAction,
                    _lookAction),
                _ => throw new ArgumentOutOfRangeException(nameof(topDownCharacterSettings.InputType),
                    topDownCharacterSettings.InputType,
                    "Unsupported InputType. Expected PC, Console, or Mobile.")
            };

            _movementController = topDownCharacterSettings.MovementType switch
            {
                MovementType.Transform => new TransformMovementController(_topDownCharacterModel, topDownCharacterView),
                MovementType.Physic => new PhysicMovementController(_topDownCharacterModel, topDownCharacterView),
                MovementType.CharacterController => new CharacterControllerMovementController(_topDownCharacterModel,
                    topDownCharacterView),
                _ => throw new ArgumentOutOfRangeException(nameof(topDownCharacterSettings.MovementType),
                    topDownCharacterSettings.MovementType,
                    "Unsupported MovementType. Expected Transform, Physic, or CharacterController.")
            };

            _rotationController = topDownCharacterSettings.RotationType switch
            {
                RotationType.DirectionBased => new DirectionBasedRotationController(_topDownCharacterModel,
                    topDownCharacterView),
                RotationType.MouseBased => new MouseBasedRotationController(_topDownCharacterModel,
                    topDownCharacterView, gameCamera),
                RotationType.JoystickDelta => new JoystickBasedRotationController(_topDownCharacterModel,
                    topDownCharacterView),
                _ => throw new ArgumentOutOfRangeException(nameof(topDownCharacterSettings.RotationType),
                    topDownCharacterSettings.RotationType,
                    "Unsupported RotationType. Expected DirectionBased, MouseBased, or JoystickDelta.")
            };
        }
        private void MediationBindings() => _topDownCharacterMediator =
            new TopDownCharacterMediator(_topDownCharacterModel, topDownCharacterView, _inputController,
                _movementController, _rotationController);
        #endregion
    }
}