using UnityEngine;
using ActiveRagdoll;

/// <summary> Default behaviour of an Active Ragdoll </summary>
public class BoyfriendBehaviour : MonoBehaviour {
    // Author: Sergio Abreu García | https://sergioabreu.me

    [Header("Modules")]
    [SerializeField] private ActiveRagdoll.ActiveRagdoll _activeRagdoll = default;
    [SerializeField] private PhysicsModule _physicsModule = default;
    [SerializeField] private AnimationModule _animationModule = default;
    [SerializeField] private Transform _follow = default;

    [Header("Movement")]
    [SerializeField] private bool _enableMovement = true;
    [SerializeField] private float _playerRadiusSense = 10f;
    [SerializeField] private float _movementScalar = 10f;
    
    private Vector3 _movement = default;
    private Vector3 _aimPoint = default;
    bool _canMove = true;

    Vector3 SelfMovement =>  -Auxiliary.GetFloorProjection(_activeRagdoll.PhysicalTorso.position).normalized;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _playerRadiusSense);
    }

    public void SetCanMove(bool canMove) => _canMove = canMove;

    void Start()
    {
        _physicsModule.TargetDirection = SelfMovement;
    }

    private void Update() 
    {
        var playerInRadius = Vector3.Distance(_follow.position, _activeRagdoll.PhysicalTorso.position) < _playerRadiusSense;
        var playerDirection = (_follow.position - _activeRagdoll.PhysicalTorso.position).normalized;
        Vector3 floorDirection = Auxiliary.GetFloorProjection(playerDirection);
        _movement = playerInRadius && _canMove ? floorDirection.normalized : Vector3.zero;
        _animationModule.AimDirection = _movement;

        Debug.DrawRay(_activeRagdoll.PhysicalTorso.position, floorDirection * 3, Color.red);

        // MOVEMENT
        if (_movement != Vector3.zero && _enableMovement) {
            _animationModule.Animator.SetBool("moving", true);
            _animationModule.Animator.SetFloat("speed", _movement.magnitude);        

            // TODO(make the grab point towards mouse position!!)
            _physicsModule.TargetDirection = _movement;
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
