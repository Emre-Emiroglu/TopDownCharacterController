using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TopDownCharacterController.Runtime.Controllers.Input;
using TopDownCharacterController.Runtime.Controllers.Movement;
using TopDownCharacterController.Runtime.Controllers.Rotation;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Managers;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace TopDownCharacterController.Tests.PlayMode
{
    [TestFixture]
    public sealed class PlayModeTests : InputTestFixture
    {
        #region Fields
        private readonly List<GameObject> _createdObjects = new();
        private readonly List<ScriptableObject> _createdAssets = new();
        #endregion

        #region Core
        [SetUp]
        public override void Setup() => base.Setup();
        [TearDown]
        public override void TearDown()
        {
            foreach (GameObject go in _createdObjects.Where(go => go))
                Object.Destroy(go);
            
            _createdObjects.Clear();

            foreach (ScriptableObject so in _createdAssets.Where(so => so))
                Object.Destroy(so);
            
            _createdAssets.Clear();

            base.TearDown();
        }
        #endregion

        #region Mocaps
        private TopDownCharacterSettings MakeSettings(InputType inputType = InputType.PC,
            MovementType movementType = MovementType.Transform, RotationType rotationType = RotationType.DirectionBased,
            float movementSpeed = 10f, float minimumMovementMagnitude = .01f, float minimumRotationMagnitude = .01f,
            float rotationSpeed = 10f, float movementSmoothTime = .1f,
            TransformMovementMethod transformMovementMethod = TransformMovementMethod.Direct,
            PhysicMovementMethod physicMovementMethod = PhysicMovementMethod.SetVelocity,
            CharacterControllerMovementMethod characterControllerMovementMethod =
                CharacterControllerMovementMethod.SimpleMove, RotationMethod rotationMethod = RotationMethod.Lerp,
            ForceMode forceMode = ForceMode.Force, bool useUnscaledDeltaTimeOnMovement = false,
            bool useUnscaledDeltaTimeOnRotation = false)
        {
            TopDownCharacterSettings settings = ScriptableObject.CreateInstance<TopDownCharacterSettings>();
            
            _createdAssets.Add(settings);

            SetPrivateField(settings, "inputType", inputType);
            SetPrivateField(settings, "movementType", movementType);
            SetPrivateField(settings, "rotationType", rotationType);
            SetPrivateField(settings, "movementSpeed", movementSpeed);
            SetPrivateField(settings, "minimumMovementMagnitude", minimumMovementMagnitude);
            SetPrivateField(settings, "minimumRotationMagnitude", minimumRotationMagnitude);
            SetPrivateField(settings, "rotationSpeed", rotationSpeed);
            SetPrivateField(settings, "movementSmoothTime", movementSmoothTime);
            SetPrivateField(settings, "transformMovementMethod", transformMovementMethod);
            SetPrivateField(settings, "physicMovementMethod", physicMovementMethod);
            SetPrivateField(settings, "characterControllerMovementMethod", characterControllerMovementMethod);
            SetPrivateField(settings, "rotationMethod", rotationMethod);
            SetPrivateField(settings, "physicMovementForceMode", forceMode);
            SetPrivateField(settings, "useUnscaledDeltaTimeOnMovement", useUnscaledDeltaTimeOnMovement);
            SetPrivateField(settings, "useUnscaledDeltaTimeOnRotation", useUnscaledDeltaTimeOnRotation);
            SetPrivateField(settings, "moveAction", new InputAction("Move"));
            SetPrivateField(settings, "lookAction", new InputAction("Look"));

            return settings;
        }
        private TopDownCharacterView MakeView(bool withRigidbody = false, bool withCharacterController = false)
        {
            GameObject go = new GameObject("TestCharacter");
            
            _createdObjects.Add(go);
            
            go.SetActive(false);

            TopDownCharacterView view = go.AddComponent<TopDownCharacterView>();

            if (withRigidbody)
            {
                Rigidbody rb = go.AddComponent<Rigidbody>();
                
                SetPrivateField(view, "rb", rb);
            }

            if (withCharacterController)
            {
                CharacterController cc = go.AddComponent<CharacterController>();
                
                SetPrivateField(view, "characterController", cc);
            }

            go.SetActive(true);
            
            return view;
        }
        private static TopDownCharacterModel MakeModel(TopDownCharacterSettings settings) => new(settings);
        private Camera MakeTopDownCamera()
        {
            GameObject camGo = new GameObject("Camera");
            
            _createdObjects.Add(camGo);
            
            Camera cam = camGo.AddComponent<Camera>();
            
            camGo.transform.SetPositionAndRotation(new Vector3(0f, 10f, 0f), Quaternion.Euler(90f, 0f, 0f));
            
            cam.orthographic = true;
            
            return cam;
        }
        #endregion

        #region Utilities
        private static void SetPrivateField(object target, string fieldName, object value)
        {
            Type type = target.GetType();
            
            while (type != null)
            {
                FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (field != null)
                {
                    field.SetValue(target, value);
                    
                    return;
                }
                
                type = type.BaseType;
            }
        }
        private static T GetPrivateField<T>(object target, string fieldName)
        {
            Type type = target.GetType();
            
            while (type != null)
            {
                FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (field != null)
                    return (T)field.GetValue(target);
                
                type = type.BaseType;
            }
            return default;
        }
        #endregion
        
        #region Tests
        [Test]
        public void TopDownCharacterModel_Constructor_StoresSettings()
        {
            TopDownCharacterSettings settings = MakeSettings();

            TopDownCharacterModel model = MakeModel(settings);

            Assert.That(model.Settings, Is.SameAs(settings));
        }
        [Test]
        public void TopDownCharacterModel_SetMoveInput_UpdatesMoveInput()
        {
            TopDownCharacterModel model = MakeModel(MakeSettings());
            
            Vector2 input = new(.5f, .25f);

            model.SetMoveInput(input);

            Assert.That(model.MoveInput, Is.EqualTo(input));
        }
        [Test]
        public void TopDownCharacterModel_SetLookInput_UpdatesLookInput()
        {
            TopDownCharacterModel model = MakeModel(MakeSettings());
            
            Vector2 input = new(-1f, .75f);

            model.SetLookInput(input);

            Assert.That(model.LookInput, Is.EqualTo(input));
        }
        [Test]
        public void InputController_Execute_OnEnable_EnablesActions()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC);
            
            TopDownCharacterView view = MakeView();
            
            PcInputController controller = new(MakeModel(settings), view, settings.MoveAction, settings.LookAction);

            controller.Execute(InputStateType.OnEnable);

            InputAction moveAction = GetPrivateField<InputAction>(controller, "MoveAction");
            InputAction lookAction = GetPrivateField<InputAction>(controller, "LookAction");
            
            Assert.That(moveAction.enabled, Is.True);
            Assert.That(lookAction.enabled, Is.True);
        }
        [Test]
        public void InputController_Execute_OnDisable_DisablesActions()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC);
            
            TopDownCharacterView view = MakeView();
            
            PcInputController controller = new(MakeModel(settings), view, settings.MoveAction, settings.LookAction);

            controller.Execute(InputStateType.OnEnable);
            controller.Execute(InputStateType.OnDisable);

            InputAction moveAction = GetPrivateField<InputAction>(controller, "MoveAction");
            InputAction lookAction = GetPrivateField<InputAction>(controller, "LookAction");
            
            Assert.That(moveAction.enabled, Is.False);
            Assert.That(lookAction.enabled, Is.False);
        }
        [Test]
        public void InputController_Execute_OnUpdate_WithNoDevice_ReturnsZeroInputs()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC);
            
            TopDownCharacterView view = MakeView();
            
            PcInputController controller = new(MakeModel(settings), view, settings.MoveAction, settings.LookAction);

            controller.Execute(InputStateType.OnEnable);
            controller.Execute(InputStateType.OnUpdate);

            Assert.That(controller.MoveInput, Is.EqualTo(Vector2.zero));
            Assert.That(controller.LookInput, Is.EqualTo(Vector2.zero));
        }
        [Test]
        public void InputController_Execute_InvalidParameter_ThrowsArgumentException()
        {
            TopDownCharacterSettings settings = MakeSettings();
            
            TopDownCharacterView view = MakeView();
            
            PcInputController controller = new(MakeModel(settings), view, settings.MoveAction, settings.LookAction);

            Assert.Throws<ArgumentException>(() => controller.Execute("invalid_param"));
        }
        [Test]
        public void InputController_Execute_UnhandledInputStateType_ThrowsArgumentOutOfRangeException()
        {
            TopDownCharacterSettings settings = MakeSettings();
            
            TopDownCharacterView view = MakeView();
            
            PcInputController controller = new(MakeModel(settings), view, settings.MoveAction, settings.LookAction);

            Assert.Throws<ArgumentOutOfRangeException>(() => controller.Execute((InputStateType)99));
        }
        [Test]
        public void PcInputController_Constructor_BindsKeyboardAndMouse()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC);
            
            TopDownCharacterView view = MakeView();
            
            PcInputController controller = new(MakeModel(settings), view, settings.MoveAction, settings.LookAction);

            InputAction moveAction = GetPrivateField<InputAction>(controller, "MoveAction");
            InputAction lookAction = GetPrivateField<InputAction>(controller, "LookAction");
            
            Assert.That(moveAction.bindings, Has.Count.GreaterThan(0));
            Assert.That(lookAction.bindings, Has.Count.GreaterThan(0));
        }
        [Test]
        public void ConsoleInputController_Constructor_BindsGamepadLeftAndRightStick()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.Console);
            
            TopDownCharacterView view = MakeView();

            ConsoleInputController controller = new(MakeModel(settings), view, settings.MoveAction,
                settings.LookAction);

            InputAction moveAction = GetPrivateField<InputAction>(controller, "MoveAction");
            InputAction lookAction = GetPrivateField<InputAction>(controller, "LookAction");
            
            Assert.That(moveAction.bindings[0].path, Is.EqualTo("<Gamepad>/leftStick"));
            Assert.That(lookAction.bindings[0].path, Is.EqualTo("<Gamepad>/rightStick"));
        }
        [Test]
        public void MobileInputController_IsSubclassOfConsoleInputController()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.Mobile);
            
            TopDownCharacterView view = MakeView();

            MobileInputController controller = new(MakeModel(settings), view, settings.MoveAction, settings.LookAction);

            Assert.That(controller, Is.InstanceOf<ConsoleInputController>());
        }
        [UnityTest]
        public IEnumerator InputController_Execute_OnUpdate_WithKeyboard_ReadsMoveInput()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC);
            
            TopDownCharacterView view = MakeView();
            
            PcInputController controller = new(MakeModel(settings), view, settings.MoveAction, settings.LookAction);
            
            controller.Execute(InputStateType.OnEnable);

            yield return null;

            Press(keyboard.wKey);

            yield return null;

            controller.Execute(InputStateType.OnUpdate);

            Assert.That(controller.MoveInput.y, Is.GreaterThan(0f));
        }
        [UnityTest]
        public IEnumerator InputController_Execute_OnUpdate_WithGamepad_ReadsMoveAndLookInput()
        {
            Gamepad gamepad = InputSystem.AddDevice<Gamepad>();

            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.Console);
            
            TopDownCharacterView view = MakeView();

            ConsoleInputController controller = new(MakeModel(settings), view, settings.MoveAction,
                settings.LookAction);
            
            controller.Execute(InputStateType.OnEnable);

            yield return null;

            Set(gamepad.leftStick, new Vector2(1f, 0f));
            Set(gamepad.rightStick, new Vector2(0f, -1f));

            yield return null;

            controller.Execute(InputStateType.OnUpdate);

            Assert.That(controller.MoveInput.x, Is.GreaterThan(0f));
            Assert.That(controller.LookInput.y, Is.LessThan(0f));
        }
        [Test]
        public void TransformMovementController_Execute_ZeroInput_DoesNotMoveTransform()
        {
            TopDownCharacterSettings settings = MakeSettings(minimumMovementMagnitude: .01f);
            
            TopDownCharacterView view = MakeView();
            
            TransformMovementController controller = new(MakeModel(settings), view);
            
            Vector3 initial = view.transform.position;

            controller.Execute();

            Assert.That(view.transform.position, Is.EqualTo(initial));
        }
        [Test]
        public void TransformMovementController_Execute_BelowMinimumMagnitude_DoesNotMove()
        {
            TopDownCharacterSettings settings = MakeSettings(minimumMovementMagnitude: .5f);
            
            TopDownCharacterModel model = MakeModel(settings);
            
            TopDownCharacterView view = MakeView();
            
            TransformMovementController controller = new(model, view);
            
            Vector3 initial = view.transform.position;

            model.SetMoveInput(new Vector2(.1f, .1f)); // sqrMagnitude ≈ 0.02 < 0.5
            
            controller.Execute();

            Assert.That(view.transform.position, Is.EqualTo(initial));
        }
        [UnityTest]
        public IEnumerator TransformMovementController_Execute_DirectMethod_MovesTransformAlongInput()
        {
            TopDownCharacterSettings settings = MakeSettings(movementSpeed: 10f, minimumMovementMagnitude: .01f,
                transformMovementMethod: TransformMovementMethod.Direct);
            
            TopDownCharacterModel model = MakeModel(settings);
            
            TopDownCharacterView view = MakeView();
            
            TransformMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);
            
            yield return null;

            controller.Execute();

            Assert.That(view.transform.position.x, Is.GreaterThan(0f));
        }
        [UnityTest]
        public IEnumerator TransformMovementController_Execute_LerpMethod_ConvergesOnTarget()
        {
            TopDownCharacterSettings settings = MakeSettings(movementSpeed: 10f, minimumMovementMagnitude: .01f,
                movementSmoothTime: 5f, transformMovementMethod: TransformMovementMethod.Lerp);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            TransformMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);
            
            yield return null;

            for (int i = 0; i < 8; i++)
            {
                controller.Execute();
                
                yield return null;
            }

            Assert.That(view.transform.position.x, Is.GreaterThan(0f));
        }
        [UnityTest]
        public IEnumerator TransformMovementController_Execute_SmoothDampMethod_ConvergesOnTarget()
        {
            TopDownCharacterSettings settings = MakeSettings(movementSpeed: 10f, minimumMovementMagnitude: .01f,
                movementSmoothTime: .1f, transformMovementMethod: TransformMovementMethod.SmoothDamp);
           
            TopDownCharacterModel model = MakeModel(settings);
          
            TopDownCharacterView view = MakeView();
            TransformMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);
           
            yield return null;

            for (int i = 0; i < 8; i++)
            {
                controller.Execute();
                
                yield return null;
            }

            Assert.That(view.transform.position.x, Is.GreaterThan(0f));
        }
        [UnityTest]
        public IEnumerator TransformMovementController_Execute_MoveTowardsMethod_ConvergesOnTarget()
        {
            TopDownCharacterSettings settings = MakeSettings(movementSpeed: 10f, minimumMovementMagnitude: .01f,
                movementSmoothTime: 5f, transformMovementMethod: TransformMovementMethod.MoveTowards);
           
            TopDownCharacterModel model = MakeModel(settings);
           
            TopDownCharacterView view = MakeView();
            
            TransformMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);
           
            yield return null;

            for (int i = 0; i < 8; i++)
            {
                controller.Execute();
               
                yield return null;
            }

            Assert.That(view.transform.position.x, Is.GreaterThan(0f));
        }
        [Test]
        public void PhysicMovementController_Execute_WithNullRigidbody_DoesNotThrow()
        {
            TopDownCharacterSettings settings = MakeSettings(movementType: MovementType.Physic);
            
            TopDownCharacterModel model = MakeModel(settings);
            
            TopDownCharacterView view = MakeView();
            
            PhysicMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);

            Assert.DoesNotThrow(() => controller.Execute());
        }
        [UnityTest]
        public IEnumerator PhysicMovementController_Execute_SetVelocity_ModifiesRigidbodyVelocity()
        {
            TopDownCharacterSettings settings = MakeSettings(movementType: MovementType.Physic, movementSpeed: 5f,
                transformMovementMethod: TransformMovementMethod.Direct,
                physicMovementMethod: PhysicMovementMethod.SetVelocity);
           
            TopDownCharacterModel model = MakeModel(settings);
           
            TopDownCharacterView view = MakeView(withRigidbody: true);
           
            PhysicMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);
            
            yield return new WaitForFixedUpdate();

            controller.Execute();

            if (view.Rb)
                Assert.That(view.Rb.linearVelocity.x, Is.Not.EqualTo(0f));
        }
        [UnityTest]
        public IEnumerator PhysicMovementController_Execute_AddForce_AcceleratesRigidbody()
        {
            TopDownCharacterSettings settings = MakeSettings(movementType: MovementType.Physic, movementSpeed: 50f,
                transformMovementMethod: TransformMovementMethod.Direct,
                physicMovementMethod: PhysicMovementMethod.AddForce, forceMode: ForceMode.Force);
           
            TopDownCharacterModel model = MakeModel(settings);
          
            TopDownCharacterView view = MakeView(withRigidbody: true);
            PhysicMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);

            for (int i = 0; i < 8; i++)
            {
                controller.Execute();
                
                yield return new WaitForFixedUpdate();
            }

            if (view.Rb)
                Assert.That(view.Rb.linearVelocity.x, Is.GreaterThan(0f));
        }
        [UnityTest]
        public IEnumerator PhysicMovementController_Execute_AddRelativeForce_AcceleratesRigidbody()
        {
            TopDownCharacterSettings settings = MakeSettings(movementType: MovementType.Physic, movementSpeed: 50f,
                transformMovementMethod: TransformMovementMethod.Direct,
                physicMovementMethod: PhysicMovementMethod.AddRelativeForce, forceMode: ForceMode.Force);
         
            TopDownCharacterModel model = MakeModel(settings);
        
            TopDownCharacterView view = MakeView(withRigidbody: true);
         
            PhysicMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);

            for (int i = 0; i < 8; i++)
            {
                controller.Execute();
              
                yield return new WaitForFixedUpdate();
            }

            if (view.Rb)
                Assert.That(view.Rb.linearVelocity.sqrMagnitude, Is.GreaterThan(0f));
        }
        [UnityTest]
        public IEnumerator PhysicMovementController_Execute_MovePosition_UpdatesKinematicRigidbodyPosition()
        {
            TopDownCharacterSettings settings = MakeSettings(movementType: MovementType.Physic, movementSpeed: 5f,
                transformMovementMethod: TransformMovementMethod.Direct,
                physicMovementMethod: PhysicMovementMethod.MovePosition);
            
            TopDownCharacterModel model = MakeModel(settings);
            
            TopDownCharacterView view = MakeView(withRigidbody: true);

            if (!view.Rb)
                yield break;
            
            view.Rb.isKinematic = true;
            
            PhysicMovementController controller = new(model, view);
            
            Vector3 initial = view.Rb.position;

            model.SetMoveInput(Vector2.right);
            
            for (int i = 0; i < 8; i++)
            {
                controller.Execute();
              
                yield return new WaitForFixedUpdate();
            }

            Assert.That(view.Rb.position.x, Is.GreaterThan(initial.x));
        }
        [Test]
        public void CharacterControllerMovementController_Execute_WithNullCharacterController_DoesNotThrow()
        {
            TopDownCharacterSettings settings = MakeSettings(movementType: MovementType.CharacterController);
            
            TopDownCharacterModel model = MakeModel(settings);
            
            TopDownCharacterView view = MakeView();
            
            CharacterControllerMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);

            Assert.DoesNotThrow(() => controller.Execute());
        }
        [UnityTest]
        public IEnumerator CharacterControllerMovementController_Execute_SimpleMove_MovesCharacter()
        {
            TopDownCharacterSettings settings = MakeSettings(movementType: MovementType.CharacterController,
                movementSpeed: 10f, minimumMovementMagnitude: .01f,
                transformMovementMethod: TransformMovementMethod.Direct,
                characterControllerMovementMethod: CharacterControllerMovementMethod.SimpleMove);
            
            TopDownCharacterModel model = MakeModel(settings);
            
            TopDownCharacterView view = MakeView(withCharacterController: true);

            CharacterControllerMovementController controller = new(model, view);

            view.transform.position = new Vector3(0f, 2f, 0f);

            model.SetMoveInput(Vector2.right);

            for (int i = 0; i < 8; i++)
            {
                controller.Execute();
              
                yield return new WaitForFixedUpdate();
            }

            if (view.CharacterController)
                Assert.That(view.CharacterController.velocity.sqrMagnitude, Is.GreaterThan(0f));
        }
        [UnityTest]
        public IEnumerator CharacterControllerMovementController_Execute_Move_DoesNotThrowAndChangesPosition()
        {
            TopDownCharacterSettings settings = MakeSettings(movementType: MovementType.CharacterController,
                movementSpeed: 10f, minimumMovementMagnitude: .01f,
                transformMovementMethod: TransformMovementMethod.Direct,
                characterControllerMovementMethod: CharacterControllerMovementMethod.Move);
            
            TopDownCharacterModel model = MakeModel(settings);
            
            TopDownCharacterView view = MakeView(withCharacterController: true);
            
            CharacterControllerMovementController controller = new(model, view);

            model.SetMoveInput(Vector2.right);
            
            yield return null;

            Assert.DoesNotThrow(() => controller.Execute());
        }
        [Test]
        public void DirectionBasedRotationController_Execute_BelowMinimumMagnitude_DoesNotRotate()
        {
            TopDownCharacterSettings settings = MakeSettings(minimumRotationMagnitude: .5f);
            
            TopDownCharacterModel model = MakeModel(settings);
            
            TopDownCharacterView view = MakeView();
            
            DirectionBasedRotationController controller = new(model, view);
            
            Quaternion initial = view.transform.rotation;

            model.SetLookInput(new Vector2(.1f, 0f)); // sqrMagnitude 0.01 < 0.5
            
            controller.Execute();

            Assert.That(view.transform.rotation, Is.EqualTo(initial));
        }
        [UnityTest]
        public IEnumerator DirectionBasedRotationController_Execute_LerpMethod_RotatesTransformTowardInput()
        {
            TopDownCharacterSettings settings = MakeSettings(rotationSpeed: 10f, minimumRotationMagnitude: .01f,
                rotationMethod: RotationMethod.Lerp);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            DirectionBasedRotationController controller = new(model, view);

            model.SetLookInput(Vector2.right);
            
            yield return null;

            controller.Execute();

            Quaternion expected = Quaternion.LookRotation(Vector3.right);
            
            Assert.That(Quaternion.Angle(view.transform.rotation, expected), Is.LessThan(90f));
        }
        [UnityTest]
        public IEnumerator DirectionBasedRotationController_Execute_SlerpMethod_RotatesTransformTowardInput()
        {
            TopDownCharacterSettings settings = MakeSettings(rotationSpeed: 10f, minimumRotationMagnitude: .01f,
                rotationMethod: RotationMethod.Slerp);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            DirectionBasedRotationController controller = new(model, view);

            model.SetLookInput(Vector2.right);
            
            yield return null;

            controller.Execute();

            Quaternion expected = Quaternion.LookRotation(Vector3.right);
            
            Assert.That(Quaternion.Angle(view.transform.rotation, expected), Is.LessThan(90f));
        }
        [UnityTest]
        public IEnumerator DirectionBasedRotationController_Execute_RotateTowardsMethod_RotatesTransformTowardInput()
        {
            TopDownCharacterSettings settings = MakeSettings(rotationSpeed: 720f, minimumRotationMagnitude: .01f,
                rotationMethod: RotationMethod.RotateTowards);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            DirectionBasedRotationController controller = new(model, view);

            model.SetLookInput(Vector2.right);
            
            yield return null;

            controller.Execute();

            Quaternion expected = Quaternion.LookRotation(Vector3.right);
            
            Assert.That(Quaternion.Angle(view.transform.rotation, expected), Is.LessThan(90f));
        }
        [UnityTest]
        public IEnumerator JoystickBasedRotationController_Execute_RotatesTransformTowardLookInput()
        {
            TopDownCharacterSettings settings = MakeSettings(rotationType: RotationType.JoystickDelta,
                rotationSpeed: 10f, minimumRotationMagnitude: .01f, rotationMethod: RotationMethod.Slerp);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            JoystickBasedRotationController controller = new(model, view);

            model.SetLookInput(new Vector2(0f, 1f));
            
            yield return null;

            controller.Execute();

            Quaternion expected = Quaternion.LookRotation(Vector3.forward);

            Assert.That(Quaternion.Angle(view.transform.rotation, expected), Is.LessThan(90f));
        }
        [Test]
        public void JoystickBasedRotationController_IsSubclassOfDirectionBasedRotationController()
        {
            TopDownCharacterSettings settings = MakeSettings(rotationType: RotationType.JoystickDelta);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();

            JoystickBasedRotationController controller = new(model, view);

            Assert.That(controller, Is.InstanceOf<DirectionBasedRotationController>());
        }
        [UnityTest]
        public IEnumerator MouseBasedRotationController_Execute_RotatesTransformTowardWorldPoint()
        {
            Camera cam = MakeTopDownCamera();

            TopDownCharacterSettings settings = MakeSettings(rotationType: RotationType.MouseBased, rotationSpeed: 100f,
                minimumRotationMagnitude: .01f, rotationMethod: RotationMethod.Slerp);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            view.transform.position = Vector3.zero;

            MouseBasedRotationController controller = new(model, view, cam);
            
            model.SetLookInput(new Vector2(Screen.width * .75f, Screen.height * .5f));
            
            yield return null;

            controller.Execute();

            Assert.That(view.transform.rotation, Is.Not.EqualTo(Quaternion.identity));
        }
        [Test]
        public void MouseBasedRotationController_Execute_CameraParallelToGroundPlane_DoesNotThrow()
        {
            GameObject camGo = new GameObject("SideCamera");
            
            _createdObjects.Add(camGo);
            
            Camera cam = camGo.AddComponent<Camera>();
            
            camGo.transform.rotation = Quaternion.Euler(0f, 90f, 0f); // looks along X — ray is parallel to Y=0 plane

            TopDownCharacterSettings settings = MakeSettings(rotationType: RotationType.MouseBased);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            MouseBasedRotationController controller = new(model, view, cam);
            
            model.SetLookInput(new Vector2(Screen.width * .5f, Screen.height * .5f));

            Assert.DoesNotThrow(() => controller.Execute());
        }
        [Test]
        public void TopDownCharacterMediator_SetSubscriptions_True_EnablesInputActions()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            PcInputController inputCtrl = new(model, view, settings.MoveAction, settings.LookAction);
            TransformMovementController movementCtrl = new(model, view);
            DirectionBasedRotationController rotationCtrl = new(model, view);
            TopDownCharacterMediator mediator = new(model, view, inputCtrl, movementCtrl, rotationCtrl);

            mediator.SetSubscriptions(true);

            InputAction moveAction = GetPrivateField<InputAction>(inputCtrl, "MoveAction");
            
            Assert.That(moveAction.enabled, Is.True);
        }
        [Test]
        public void TopDownCharacterMediator_SetSubscriptions_False_DisablesInputActions()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            PcInputController inputCtrl = new(model, view, settings.MoveAction, settings.LookAction);
            TransformMovementController movementCtrl = new(model, view);
            DirectionBasedRotationController rotationCtrl = new(model, view);
            TopDownCharacterMediator mediator = new(model, view, inputCtrl, movementCtrl, rotationCtrl);

            mediator.SetSubscriptions(true);
            mediator.SetSubscriptions(false);

            InputAction moveAction = GetPrivateField<InputAction>(inputCtrl, "MoveAction");
            
            Assert.That(moveAction.enabled, Is.False);
        }
        [UnityTest]
        public IEnumerator TopDownCharacterMediator_Tick_DirectionBased_SetsLookInputEqualToMoveInput()
        {
            Gamepad gamepad = InputSystem.AddDevice<Gamepad>();

            TopDownCharacterSettings settings =
                MakeSettings(inputType: InputType.Console, rotationType: RotationType.DirectionBased);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            ConsoleInputController inputCtrl = new(model, view, settings.MoveAction, settings.LookAction);
            TransformMovementController movementCtrl = new(model, view);
            DirectionBasedRotationController rotationCtrl = new(model, view);
            TopDownCharacterMediator mediator = new(model, view, inputCtrl, movementCtrl, rotationCtrl);

            mediator.SetSubscriptions(true);
            
            Set(gamepad.leftStick, new Vector2(1f, 0f));
            Set(gamepad.rightStick, new Vector2(0f, 1f));
            
            yield return null;

            mediator.Tick();

            Assert.That(model.LookInput, Is.EqualTo(model.MoveInput));
        }
        [UnityTest]
        public IEnumerator TopDownCharacterMediator_Tick_JoystickDelta_SetsLookInputFromRightStick()
        {
            Gamepad gamepad = InputSystem.AddDevice<Gamepad>();

            TopDownCharacterSettings settings =
                MakeSettings(inputType: InputType.Console, rotationType: RotationType.JoystickDelta);
            
            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();
            
            ConsoleInputController inputCtrl = new(model, view, settings.MoveAction, settings.LookAction);
            TransformMovementController movementCtrl = new(model, view);
            JoystickBasedRotationController rotationCtrl = new(model, view);
            TopDownCharacterMediator mediator = new(model, view, inputCtrl, movementCtrl, rotationCtrl);

            mediator.SetSubscriptions(true);
            
            Set(gamepad.leftStick, new Vector2(1f, 0f));
            Set(gamepad.rightStick, new Vector2(0f, 1f));
            
            yield return null;

            mediator.Tick();

            Assert.That(model.LookInput.y, Is.GreaterThan(0f));
            Assert.That(model.LookInput, Is.Not.EqualTo(model.MoveInput));
        }
        [UnityTest]
        public IEnumerator TopDownCharacterMediator_Tick_ExecutesMovementAndRotationPipeline()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC, movementSpeed: 10f,
                minimumMovementMagnitude: .01f, minimumRotationMagnitude: .01f,
                transformMovementMethod: TransformMovementMethod.Direct, rotationMethod: RotationMethod.Lerp,
                rotationSpeed: 10f);

            TopDownCharacterModel model = MakeModel(settings);
            TopDownCharacterView view = MakeView();

            PcInputController inputCtrl = new(model, view, settings.MoveAction, settings.LookAction);
            TransformMovementController movementCtrl = new(model, view);
            DirectionBasedRotationController rotationCtrl = new(model, view);
            TopDownCharacterMediator mediator = new(model, view, inputCtrl, movementCtrl, rotationCtrl);

            mediator.SetSubscriptions(true);
            
            Press(keyboard.dKey);

            yield return null;

            Vector3 positionBefore = view.transform.position;

            mediator.Tick();

            Assert.That(view.transform.position.x, Is.GreaterThan(positionBefore.x));
        }
        [UnityTest]
        public IEnumerator TopDownCharacterManager_Awake_WithTransformMovement_DirectionBased_BuildsMediator()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC,
                movementType: MovementType.Transform, rotationType: RotationType.DirectionBased);

            GameObject go = new GameObject("Manager");
            
            _createdObjects.Add(go);
            
            go.SetActive(false);

            TopDownCharacterView view = go.AddComponent<TopDownCharacterView>();
            TopDownCharacterManager manager = go.AddComponent<TopDownCharacterManager>();
            
            SetPrivateField(manager, "topDownCharacterSettings", settings);
            SetPrivateField(manager, "topDownCharacterView", view);
            SetPrivateField(manager, "gameCamera", go.AddComponent<Camera>());

            go.SetActive(true);
            
            yield return null;

            TopDownCharacterMediator mediator =
                GetPrivateField<TopDownCharacterMediator>(manager, "_topDownCharacterMediator");
            
            Assert.That(mediator, Is.Not.Null);
        }
        [UnityTest]
        public IEnumerator TopDownCharacterManager_Awake_WithPhysicMovement_JoystickDelta_BuildsMediator()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.Console,
                movementType: MovementType.Physic, rotationType: RotationType.JoystickDelta);

            GameObject go = new GameObject("Manager");
            
            _createdObjects.Add(go);
            
            go.SetActive(false);

            TopDownCharacterView view = go.AddComponent<TopDownCharacterView>();
            
            Rigidbody rb = go.AddComponent<Rigidbody>();
            
            SetPrivateField(view, "rb", rb);
            
            TopDownCharacterManager manager = go.AddComponent<TopDownCharacterManager>();
            
            SetPrivateField(manager, "topDownCharacterSettings", settings);
            SetPrivateField(manager, "topDownCharacterView", view);
            SetPrivateField(manager, "gameCamera", go.AddComponent<Camera>());

            go.SetActive(true);
            
            yield return null;

            TopDownCharacterMediator mediator =
                GetPrivateField<TopDownCharacterMediator>(manager, "_topDownCharacterMediator");
            
            Assert.That(mediator, Is.Not.Null);
        }
        [UnityTest]
        public IEnumerator TopDownCharacterManager_Awake_WithCharacterControllerMovement_MouseBased_BuildsMediator()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.Mobile,
                movementType: MovementType.CharacterController, rotationType: RotationType.MouseBased);

            GameObject go = new GameObject("Manager");
            
            _createdObjects.Add(go);
            
            go.SetActive(false);

            TopDownCharacterView view = go.AddComponent<TopDownCharacterView>();
            CharacterController cc = go.AddComponent<CharacterController>();
            
            SetPrivateField(view, "characterController", cc);
            
            Camera cam = go.AddComponent<Camera>();
            TopDownCharacterManager manager = go.AddComponent<TopDownCharacterManager>();
            
            SetPrivateField(manager, "topDownCharacterSettings", settings);
            SetPrivateField(manager, "topDownCharacterView", view);
            SetPrivateField(manager, "gameCamera", cam);

            go.SetActive(true);
            
            yield return null;

            TopDownCharacterMediator mediator =
                GetPrivateField<TopDownCharacterMediator>(manager, "_topDownCharacterMediator");
            
            Assert.That(mediator, Is.Not.Null);
        }
        [UnityTest]
        public IEnumerator TopDownCharacterManager_Update_DrivesMediatorTickEachFrame()
        {
            TopDownCharacterSettings settings = MakeSettings(inputType: InputType.PC,
                movementType: MovementType.Transform, rotationType: RotationType.DirectionBased, movementSpeed: 10f,
                minimumMovementMagnitude: .01f, transformMovementMethod: TransformMovementMethod.Direct);

            GameObject go = new GameObject("Manager");
            
            _createdObjects.Add(go);
            
            go.SetActive(false);

            TopDownCharacterView view = go.AddComponent<TopDownCharacterView>();
            TopDownCharacterManager manager = go.AddComponent<TopDownCharacterManager>();
            
            SetPrivateField(manager, "topDownCharacterSettings", settings);
            SetPrivateField(manager, "topDownCharacterView", view);
            SetPrivateField(manager, "gameCamera", go.AddComponent<Camera>());

            go.SetActive(true);
            
            yield return null; // Awake + Start

            TopDownCharacterModel model = GetPrivateField<TopDownCharacterModel>(manager, "_topDownCharacterModel");
            
            model.SetMoveInput(Vector2.right);
            
            Vector3 positionBefore = view.transform.position;

            yield return null; // Update fires Tick

            Assert.That(view.transform.position.x, Is.GreaterThanOrEqualTo(positionBefore.x));
        }
        [UnityTest]
        public IEnumerator TopDownCharacterManager_OnDestroy_DisposesMediator()
        {
            TopDownCharacterSettings settings = MakeSettings();

            GameObject go = new GameObject("Manager");
            
            _createdObjects.Add(go);
            
            go.SetActive(false);

            TopDownCharacterView view = go.AddComponent<TopDownCharacterView>();
            TopDownCharacterManager manager = go.AddComponent<TopDownCharacterManager>();
            
            SetPrivateField(manager, "topDownCharacterSettings", settings);
            SetPrivateField(manager, "topDownCharacterView", view);
            SetPrivateField(manager, "gameCamera", go.AddComponent<Camera>());

            go.SetActive(true);
            
            yield return null;

            Assert.DoesNotThrow(() => Object.Destroy(go));

            yield return null; // let Destroy propagate
        }
        #endregion
    }
}