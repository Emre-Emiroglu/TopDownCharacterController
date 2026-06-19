namespace TopDownCharacterController.Runtime.Enums
{
    /// <summary>
    /// Lifecycle command passed to <c>InputController.Execute</c> to drive input action state.
    /// </summary>
    public enum InputStateType
    {
        /// <summary>Enables all input actions so they start receiving device events.</summary>
        OnEnable,
        /// <summary>Disables all input actions to stop receiving device events.</summary>
        OnDisable,
        /// <summary>Polls and caches the current move and look values from the active input actions.</summary>
        OnUpdate
    }
}