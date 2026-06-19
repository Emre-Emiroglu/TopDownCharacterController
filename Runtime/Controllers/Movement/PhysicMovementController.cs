using System;
using TopDownCharacterController.Runtime.Data;
using TopDownCharacterController.Runtime.Enums;
using TopDownCharacterController.Runtime.Models;
using TopDownCharacterController.Runtime.Views;
using UnityEngine;

namespace TopDownCharacterController.Runtime.Controllers.Movement
{
    /// <summary>
    /// Moves the character by applying force or velocity to a <see cref="Rigidbody"/> component each physics step.
    /// Requires a Rigidbody assigned on <see cref="TopDownCharacterView"/>; exits silently if it is null.
    /// </summary>
    public sealed class PhysicMovementController : MovementController
    {
        #region Fields
        private Vector3 _currentApplied;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the controller with the shared model and view.
        /// </summary>
        /// <param name="model">Shared runtime state providing move input and settings.</param>
        /// <param name="view">MonoBehaviour whose Rigidbody receives force or velocity each step.</param>
        public PhysicMovementController(TopDownCharacterModel model, TopDownCharacterView view) : base(model, view) { }
        #endregion

        #region Executes
        /// <summary>
        /// Applies smoothed force or velocity to the Rigidbody according to the configured <see cref="PhysicMovementMethod"/>.
        /// No-ops silently if <see cref="TopDownCharacterView.Rb"/> is null.
        /// </summary>
        /// <param name="parameters">Not used; reserved for base-class contract compatibility.</param>
        public override void Execute(params object[] parameters)
        {
            Rigidbody rb = View.Rb;

            if (!rb)
                return;

            TopDownCharacterSettings settings = Model.Settings;
            
            float timeFactor = GetFixedTimeFactor();
            
            Vector2 moveInput = Model.MoveInput;
            
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);

            switch (settings.PhysicMovementMethod)
            {
                case PhysicMovementMethod.SetVelocity:
                {
                    Vector3 target = direction * (settings.MovementSpeed * timeFactor);
                    Vector3 current = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                    Vector3 resolved = ResolveVector3(current, target, timeFactor);
                    
                    rb.linearVelocity = new Vector3(resolved.x, rb.linearVelocity.y, resolved.z);
                    break;
                }
                case PhysicMovementMethod.AddForce:
                {
                    Vector3 target = direction * settings.MovementSpeed;
                    
                    _currentApplied = ResolveVector3(_currentApplied, target, timeFactor);
                    
                    rb.AddForce(_currentApplied, settings.PhysicMovementForceMode);
                    break;
                }
                case PhysicMovementMethod.AddRelativeForce:
                {
                    Vector3 target = direction * settings.MovementSpeed;
                    
                    _currentApplied = ResolveVector3(_currentApplied, target, timeFactor);
                    
                    rb.AddRelativeForce(_currentApplied, settings.PhysicMovementForceMode);
                    break;
                }
                case PhysicMovementMethod.MovePosition:
                {
                    Vector3 target = direction * (settings.MovementSpeed * timeFactor);
                    
                    _currentApplied = ResolveVector3(_currentApplied, target, timeFactor);
                    
                    rb.MovePosition(rb.position + _currentApplied);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(settings.PhysicMovementMethod),
                        settings.PhysicMovementMethod,
                        "Unsupported PhysicMovementMethod. Expected SetVelocity, AddForce, AddRelativeForce, or MovePosition.");
            }
        }
        #endregion
    }
}