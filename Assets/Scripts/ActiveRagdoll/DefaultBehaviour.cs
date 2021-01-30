using UnityEngine;
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
    private Vector2 _aimPoint = default;

    private void MovementInput(Vector2 movement) => _movement = movement; 
    private void AimInput(Vector2 movement) => _aimPoint = movement; 

    private void Start() {
        _input.onMove += MovementInput;
        _input.onMove += _physicsModule.ManualTorqueInput;
        _input.onAim += AimInput;
        _input.onFloorChanged += ProcessFloorChanged;

        _input.onLeftArm += _animationModule.UseLeftArm;
        _input.onLeftArm += _gripModule.UseLeftGrip;
        _input.onRightArm += _animationModule.UseRightArm;
        _input.onRightArm += _gripModule.UseRightGrip;
    }    

    private void Update() {
        // Project camera vectors onto the ground plane, and then normalize them for our move direction!
        var worldForward = Vector3.ProjectOnPlane(_followCam.transform.forward, Vector3.up).normalized;
        var worldRight = Vector3.ProjectOnPlane(_followCam.transform.right, Vector3.up).normalized;
        var movementForward = _movement.y * worldForward;
        var movementRight = _movement.x * worldRight;
        var targetForward = (movementForward + movementRight).normalized;

        // TARGET AIMING
        var screenRay = _followCam.ScreenPointToRay(_aimPoint);
        var wasHit = Physics.Raycast(screenRay, out var hit);
        _animationModule.AimDirection = wasHit ? hit.point : targetForward;

        // MOVEMENT
        if (_movement != Vector2.zero & _enableMovement) {
            _animationModule.Animator.SetBool("moving", true);
            _animationModule.Animator.SetFloat("speed", _movement.magnitude);        

            // TODO(make the grab point towards mouse position!!)
            _physicsModule.TargetDirection = targetForward;
        }
        else
        {
            _animationModule.Animator.SetBool("moving", false);
        }
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

}
