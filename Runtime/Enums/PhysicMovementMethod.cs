namespace TopDownCharacterController.Runtime.Enums
{
    /// <summary>
    /// Defines how <c>PhysicMovementController</c> applies locomotion to the Rigidbody each physics step.
    /// </summary>
    public enum PhysicMovementMethod
    {
        /// <summary>Sets <c>Rigidbody.linearVelocity</c> directly; preserves the Y component for gravity.</summary>
        SetVelocity,
        /// <summary>Calls <c>Rigidbody.AddForce</c> in world space using the configured <c>ForceMode</c>.</summary>
        AddForce,
        /// <summary>Calls <c>Rigidbody.AddRelativeForce</c> in local space using the configured <c>ForceMode</c>.</summary>
        AddRelativeForce,
        /// <summary>Calls <c>Rigidbody.MovePosition</c>; suitable for kinematic rigidbodies.</summary>
        MovePosition
    }
}