using ModelViewMediatorController.Runtime;
using TopDownCharacterController.Runtime.Data;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Models
{
    public sealed class TopDownCharacterModel : Model<TopDownCharacterSettings>
    {
        #region Properties
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        #endregion

        #region Constructor
        public TopDownCharacterModel(TopDownCharacterSettings settings) : base(settings) { }
        #endregion

        #region Core
        public override void LoadData() { }
        public override void SaveData() { }
        #endregion

        #region Executes
        public void SetMoveInput(Vector2 input) => MoveInput = input;
        public void SetLookInput(Vector2 input) => LookInput = input;
        #endregion
    }
}