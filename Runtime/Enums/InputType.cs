namespace TopDownCharacterController.Runtime.Enums
{
    /// <summary>
    /// Specifies the target input device scheme used to construct the appropriate <c>InputController</c>.
    /// </summary>
    public enum InputType
    {
        /// <summary>Keyboard (WASD / arrow keys) for movement and mouse position for look.</summary>
        PC,
        /// <summary>Gamepad left stick for movement and right stick for look.</summary>
        Console,
        /// <summary>On-screen virtual sticks mapped to Gamepad paths; shares bindings with <see cref="Console"/>.</summary>
        Mobile
    }
}