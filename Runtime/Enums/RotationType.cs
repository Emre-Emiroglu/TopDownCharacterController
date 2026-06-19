namespace TopDownCharacterController.Runtime.Enums
{
    /// <summary>
    /// Defines which input source drives the character's look direction each frame.
    /// </summary>
    public enum RotationType
    {
        /// <summary>Rotates toward the current movement direction. Suitable for twin-stick and isometric games.</summary>
        DirectionBased,
        /// <summary>Rotates to face the mouse cursor projected onto the ground plane via screen-to-world raycasting.</summary>
        MouseBased,
        /// <summary>Rotates toward the right-stick (look) input direction. Suitable for console twin-stick games.</summary>
        JoystickDelta
    }
}