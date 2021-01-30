﻿using UnityEngine;
using ActiveRagdoll;

/// <summary> Default behaviour of an Active Ragdoll </summary>
public class DefaultBehaviour : MonoBehaviour {
    // Author: Sergio Abreu García | https://sergioabreu.me

    [Header("Modules")]
    [SerializeField] private ActiveRagdoll.ActiveRagdoll _activeRagdoll = default;
    [SerializeField] private InputModule _input = default;
    [SerializeField] private PhysicsModule _physicsModule = default;
    [SerializeField] private AnimationModule _animationModule = default;
    [SerializeField] private GripModule _gripModule = default;
    [SerializeField] private Camera _followCam = default;

    [Header("Movement")]
    [SerializeField] private bool _enableMovement = true;
    
    private Vector2 _movement = default;
    private Vector3 _aimDirection = default;

    private void Start() {
        // Link all the functions to its input to define how the ActiveRagdoll will behave.
        // This is a default implementation, where the input player is binded directly to
        // the ActiveRagdoll actions in a very simple way. But any implementation is
        // possible, such as assigning those same actions to the output of an AI system.
        _input.onMove += MovementInput;
        _input.onMove += _physicsModule.ManualTorqueInput;
        _input.onFloorChanged += ProcessFloorChanged;

        _input.onLeftArm += _animationModule.UseLeftArm;
        _input.onLeftArm += _gripModule.UseLeftGrip;
        _input.onRightArm += _animationModule.UseRightArm;
        _input.onRightArm += _gripModule.UseRightGrip;
    }    

    private void Update() {
        if (_movement == Vector2.zero || !_enableMovement) {
            _animationModule.Animator.SetBool("moving", false);
            return;
        }

        _animationModule.Animator.SetBool("moving", true);
        _animationModule.Animator.SetFloat("speed", _movement.magnitude);        

        // Project camera vectors onto the ground plane, and then normalize them for our move direction!
        var worldForward = Vector3.ProjectOnPlane(_followCam.transform.forward, Vector3.up).normalized;
        var worldRight = Vector3.ProjectOnPlane(_followCam.transform.right, Vector3.up).normalized;
        var movementForward = _movement.y * worldForward;
        var movementRight = _movement.x * worldRight;
        var targetForward = (movementForward + movementRight).normalized;


        // TODO(make the grab point towards mouse position!!)
        _aimDirection = targetForward;
        _animationModule.AimDirection = targetForward;
        _physicsModule.TargetDirection = targetForward;
    }

    private void ProcessFloorChanged(bool onFloor) {
        if (onFloor) {
            _physicsModule.SetBalanceMode(PhysicsModule.BalanceMode.StabalizerJoint);
            _enableMovement = true;
            _activeRagdoll.HeadNeck.SetStrengthScale(1);
            _activeRagdoll.RightLeg.SetStrengthScale(1);
            _activeRagdoll.LeftLeg.SetStrengthScale(1);
            _animationModule.PlayAnimation("Idle");
        }
        else {
            _physicsModule.SetBalanceMode(PhysicsModule.BalanceMode.ManualTorque);
            _enableMovement = false;
            _activeRagdoll.HeadNeck.SetStrengthScale(0.1f);
            _activeRagdoll.RightLeg.SetStrengthScale(0.05f);
            _activeRagdoll.LeftLeg.SetStrengthScale(0.05f);
            _animationModule.PlayAnimation("InTheAir");
        }
    }

    /// <summary> Make the player move and rotate </summary>
    private void MovementInput(Vector2 movement) => _movement = movement; 
}