using ModelViewMediatorController.Runtime;
using TopDownCharacterController.Runtime.Data;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Models
{
    /// <summary>
    /// Runtime state model for the top-down character. Stores processed input values consumed by movement and rotation controllers.
    /// </summary>
    public sealed class TopDownCharacterModel : Model<TopDownCharacterSettings>
    {
        #region Properties
        /// <summary>The current movement input vector, updated every frame by the active <c>InputController</c>.</summary>
        public Vector2 MoveInput { get; private set; }
        /// <summary>The current look/aim input vector, updated every frame by the mediator based on the active <c>RotationType</c>.</summary>
        public Vector2 LookInput { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the model with the given settings asset.
        /// </summary>
        /// <param name="settings">The ScriptableObject containing all controller configuration.</param>
        public TopDownCharacterModel(TopDownCharacterSettings settings) : base(settings) { }
        #endregion

        #region Core
        /// <inheritdoc/>
        public override void LoadData() { }
        /// <inheritdoc/>
        public override void SaveData() { }
        #endregion

        #region Executes
        /// <summary>
        /// Overwrites the stored movement input with the given value.
        /// </summary>
        /// <param name="input">Raw movement vector from the active input handler.</param>
        public void SetMoveInput(Vector2 input) => MoveInput = input;
        /// <summary>
        /// Overwrites the stored look input with the given value.
        /// </summary>
        /// <param name="input">Look vector resolved by the mediator (move direction or raw look axis depending on <c>RotationType</c>).</param>
        public void SetLookInput(Vector2 input) => LookInput = input;
        #endregion
    }
}