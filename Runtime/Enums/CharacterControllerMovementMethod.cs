namespace TopDownCharacterController.Runtime.Enums
{
    /// <summary>
    /// Selects which <c>CharacterController</c> API <c>CharacterControllerMovementController</c> uses for locomotion.
    /// </summary>
    public enum CharacterControllerMovementMethod
    {
        /// <summary>Uses <c>CharacterController.Move</c>; requires manual gravity simulation via vertical velocity accumulation.</summary>
        Move,
        /// <summary>Uses <c>CharacterController.SimpleMove</c>; gravity is applied automatically and the Y axis is ignored.</summary>
        SimpleMove
    }
}