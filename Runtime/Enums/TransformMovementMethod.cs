namespace TopDownCharacterController.Runtime.Enums
{
    /// <summary>
    /// Smoothing algorithm applied when interpolating toward the target velocity in <c>TransformMovementController</c>.
    /// </summary>
    public enum TransformMovementMethod
    {
        /// <summary>No smoothing; target velocity is applied immediately each frame.</summary>
        Direct,
        /// <summary>Linear interpolation between current and target velocity using <c>MovementSmoothTime</c> as the blend factor.</summary>
        Lerp,
        /// <summary>Critically-damped spring (Vector3.SmoothDamp) for organic, framerate-independent smoothing.</summary>
        SmoothDamp,
        /// <summary>Constant-speed step toward target velocity; reaches target in a predictable time regardless of distance.</summary>
        MoveTowards
    }
}