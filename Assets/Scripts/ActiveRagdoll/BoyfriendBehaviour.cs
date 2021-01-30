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
    
    private Vector2 _movement = default;
    private Vector2 _aimPoint = default;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _playerRadiusSense);
    }

    private void Update() 
    {
        var playerInRadius = Vector3.Distance(_follow.position, transform.position) < _playerRadiusSense;
        var playerDirection = (_follow.position - transform.position).normalized;
        _movement = playerInRadius ? new Vector2(playerDirection.x, playerDirection.z) : Vector2.zero;
        _movement.Normalize();
        _aimPoint = _movement;

        _animationModule.AimDirection = _movement;

        // MOVEMENT
        if (_movement != Vector2.zero & _enableMovement) {
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
