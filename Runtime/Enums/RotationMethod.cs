namespace TopDownCharacterController.Runtime.Enums
{
    /// <summary>
    /// Interpolation algorithm used by rotation controllers when blending from the current to the target quaternion.
    /// </summary>
    public enum RotationMethod
    {
        /// <summary>Linear quaternion interpolation; fast but may shorten arcs at large angles.</summary>
        Lerp,
        /// <summary>Spherical linear interpolation; maintains constant angular velocity and handles large angles correctly.</summary>
        Slerp,
        /// <summary>Constant angular-speed step; reaches the target in a predictable time regardless of the angular distance.</summary>
        RotateTowards
    }
}