# TopDownCharacterController
TopDownCharacterController is a modular, cross-platform top-down character controller package built on the MVMC architecture. It decouples input handling, movement, and rotation into independent controller layers so each can be swapped or extended without touching the others. The package supports PC, console, and mobile input out of the box and offers multiple locomotion and rotation backends selectable entirely from the Inspector.

## Features
TopDownCharacterController offers the following capabilities:

* Input System Integration: Platform-specific input controllers built on Unity's Input System. PC uses WASD and arrow keys for movement and the mouse position for look. Console uses the left stick for movement and the right stick for look. Mobile reuses the console bindings, targeting gamepad paths that OnScreenStick components map to at runtime.
* Transform Movement: Moves the character by directly offsetting the Transform position each frame. Supports four smoothing algorithms: Direct (no smoothing), Lerp, SmoothDamp, and MoveTowards, each configurable via the settings asset.
* Physics Movement: Applies locomotion to a Rigidbody each physics step. Supports four methods: SetVelocity (sets linearVelocity directly, preserving the Y component), AddForce (world-space force), AddRelativeForce (local-space force), and MovePosition (suitable for kinematic rigidbodies).
* CharacterController Movement: Drives Unity's CharacterController component. Move mode accumulates vertical velocity manually for gravity simulation. SimpleMove mode applies gravity automatically and ignores the Y axis of the provided velocity.
* Direction-Based Rotation: Rotates the character Transform toward the current movement direction. The mediator routes move input into the look input so no separate look axis is required.
* Mouse-Based Rotation: Raycasts from the mouse screen position onto the character's ground plane and rotates the Transform toward the resulting world point. Requires a Camera reference assigned in the Inspector.
* Joystick-Based Rotation: Rotates the character toward the right-stick look input direction, sharing execution logic with direction-based rotation but reading a dedicated look axis.
* Rotation Methods: All rotation backends support three quaternion interpolation algorithms selectable from settings: Lerp, Slerp, and RotateTowards.
* MVMC Architecture: Clean separation between Model (runtime state), View (MonoBehaviour entry point and component references), Mediator (per-frame pipeline orchestrator), and Controllers (platform-specific input, movement, and rotation implementations).

## Getting Started
Install via UPM with git URL

`https://github.com/Emre-Emiroglu/TopDownCharacterController.git`

Clone the repository
```bash
git clone https://github.com/Emre-Emiroglu/TopDownCharacterController.git
```

### Dependencies
* com.unity.inputsystem: 1.19.0
* com.emreemiroglu.modelviewmediatorcontroller: 1.0.0

This project is developed using Unity version 6000.3.18f1.

## Usage

### Scene Setup
* Add a `TopDownCharacterManager` component to your character GameObject.
* Create a `TopDownCharacterSettings` asset via the Asset menu: `TopDownCharacterController / TopDownCharacterSettings`.
* Assign the settings asset and the `TopDownCharacterView` component in the Manager's Inspector fields.
* Assign the game camera if you intend to use Mouse-Based rotation.

### Settings Configuration
Open the `TopDownCharacterSettings` asset and configure the fields for your use case.

Input Settings:
* `Input Type`: Select `PC`, `Console`, or `Mobile`. This determines which input controller is constructed at runtime.

Movement Settings:
* `Movement Type`: Select `Transform`, `Physic`, or `CharacterController` to choose the locomotion backend.
* `Movement Speed`: Units per second applied to the character.
* `Minimum Movement Magnitude`: Input sqrMagnitude threshold below which movement is skipped.
* `Movement Smooth Time`: Blend factor or step size used by Lerp, SmoothDamp, and MoveTowards algorithms.
* `Transform Movement Method`: Smoothing algorithm used by the Transform backend (Direct, Lerp, SmoothDamp, MoveTowards).
* `Physic Movement Method`: API used by the Physics backend (SetVelocity, AddForce, AddRelativeForce, MovePosition).
* `Physic Movement Force Mode`: ForceMode passed to AddForce and AddRelativeForce calls.
* `Character Controller Movement Method`: API used by the CharacterController backend (Move, SimpleMove).

Rotation Settings:
* `Rotation Type`: Select `DirectionBased`, `MouseBased`, or `JoystickDelta`.
* `Rotation Method`: Quaternion interpolation algorithm (Lerp, Slerp, RotateTowards).
* `Rotation Speed`: Degrees per second for RotateTowards; blend factor for Lerp and Slerp.
* `Minimum Rotation Magnitude`: Look-input sqrMagnitude threshold below which rotation is skipped.

### Physics-Based Movement
Attach a `Rigidbody` to the character GameObject and assign it to the `TopDownCharacterView` Rb field in the Inspector. Select `Physic` as the Movement Type in settings.

```csharp
// TopDownCharacterManager.Awake() builds the full MVMC stack automatically.
// No additional code is required beyond Inspector configuration.
```

### CharacterController Movement
Attach a `CharacterController` to the character GameObject and assign it to the `TopDownCharacterView` Character Controller field in the Inspector. Select `CharacterController` as the Movement Type in settings.

### Mobile Input
Add OnScreenStick components to your canvas for the movement and look sticks. Set their control paths to the following:

* Movement stick: `<Gamepad>/leftStick`
* Look stick: `<Gamepad>/rightStick`

Set the Input Type in settings to `Mobile`. The MobileInputController reuses the console gamepad bindings, so OnScreenStick components that map to those paths are recognized automatically at runtime.

```csharp
// No code required. The binding between OnScreenStick and MobileInputController
// is handled by the Input System's virtual gamepad device at runtime.
```

### Custom Input Actions

If you need to supply your own InputAction configuration instead of using the built-in platform bindings, assign the Move Action and Look Action fields directly on the `TopDownCharacterSettings` asset. Note that the built-in platform controllers (PC, Console, Mobile) replace these with their own bindings at construction time; the fields serve as a starting point for custom controller subclasses.

```csharp
// Extend InputController and override the constructor to bind any custom device or action.
public sealed class CustomInputController : InputController
{
    public CustomInputController(TopDownCharacterModel model, TopDownCharacterView view,
        InputAction moveAction, InputAction lookAction) : base(model, view, moveAction, lookAction)
    {
        MoveAction = new InputAction("Move");
        LookAction = new InputAction("Look");
        // Add your custom bindings here.
    }
}
```

## Acknowledgments
Special thanks to the Unity community for their invaluable resources and tools.

For more information, visit the GitHub repository.