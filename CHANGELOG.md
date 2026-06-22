## [1.0.0] - 2026-06-22

### Added
- Core MVMC stack: TopDownCharacterModel, TopDownCharacterView, TopDownCharacterMediator, TopDownCharacterManager.
- TopDownCharacterSettings ScriptableObject holding all runtime configuration in a single asset.
- InputController base class managing InputAction lifecycle (enable, disable, per-frame poll).
- PcInputController with WASD, arrow-key composite bindings for movement and mouse position for look.
- ConsoleInputController with left-stick movement and right-stick look bindings for gamepad.
- MobileInputController inheriting console gamepad bindings, targeted by OnScreenStick components at runtime.
- MovementController base class with time-factor helpers and a shared ResolveVector3 smoothing dispatcher.
- TransformMovementController supporting Direct, Lerp, SmoothDamp, and MoveTowards smoothing algorithms.
- PhysicMovementController supporting SetVelocity, AddForce, AddRelativeForce, and MovePosition methods.
- CharacterControllerMovementController supporting Move (manual gravity) and SimpleMove (auto gravity) methods.
- RotationController base class with time-factor helper and a shared ApplyRotationAlgorithm dispatcher.
- DirectionBasedRotationController rotating the character toward the current movement direction.
- MouseBasedRotationController rotating the character toward the mouse cursor via ground-plane raycasting.
- JoystickBasedRotationController rotating the character toward the right-stick look input direction.
- Lerp, Slerp, and RotateTowards quaternion interpolation algorithms across all rotation backends.
- MovementType enum: Transform, Physic, CharacterController.
- RotationType enum: DirectionBased, MouseBased, JoystickDelta.
- InputType enum: PC, Console, Mobile.
- InputStateType enum: OnEnable, OnDisable, OnUpdate.
- TransformMovementMethod enum: Direct, Lerp, SmoothDamp, MoveTowards.
- PhysicMovementMethod enum: SetVelocity, AddForce, AddRelativeForce, MovePosition.
- CharacterControllerMovementMethod enum: Move, SimpleMove.
- RotationMethod enum: Lerp, Slerp, RotateTowards.
- Play Mode test suite covering Model, InputController, MovementController, RotationController, Mediator, and Manager systems.
- Unity package configuration and UPM setup.
- Full XML documentation on all public classes, properties, and methods.
- MIT License.
- README with setup guide, settings reference, and usage examples.

### Changed
- N/A

### Fixed
- N/A

### Removed
- N/A