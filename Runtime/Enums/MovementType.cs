namespace TopDownCharacterController.Runtime.Enums
{
    /// <summary>
    /// Defines which locomotion backend drives the character's position each frame.
    /// </summary>
    public enum MovementType
    {
        /// <summary>Moves by directly offsetting the Transform position. No physics involved.</summary>
        Transform,
        /// <summary>Moves by applying force or velocity to a Rigidbody component.</summary>
        Physic,
        /// <summary>Moves using Unity's CharacterController component with built-in collision detection.</summary>
        CharacterController
    }
}