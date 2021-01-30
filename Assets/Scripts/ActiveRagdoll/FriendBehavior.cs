using UnityEngine;
using ActiveRagdoll;

/// <summary> Default behaviour of an Active Ragdoll </summary>
public class FriendBehavior : MonoBehaviour, IGrippable {
    // Author: Sergio Abreu García | https://sergioabreu.me

    [Header("Modules")]
    [SerializeField] private ActiveRagdoll.ActiveRagdoll _activeRagdoll = default;
    [SerializeField] private PhysicsModule _physicsModule = default;
    [SerializeField] private AnimationModule _animationModule = default;
    [SerializeField] private GripModule _gripModule = default;

    [Header("Movement")]
    [SerializeField] private bool _enableMovement = true;
    
    private Vector2 _movement = default;
    private Vector2 _aimPoint = default;

    public void Grip()
    {
        Debug.Log("FriendGrip");
        _physicsModule.SetBalanceMode(PhysicsModule.BalanceMode.ManualTorque);
        _enableMovement = false;
        _activeRagdoll.HeadNeck.SetStrengthScale(0.1f);
        _activeRagdoll.RightLeg.SetStrengthScale(0.05f);
        _activeRagdoll.LeftLeg.SetStrengthScale(0.05f);
        _animationModule.PlayAnimation("InTheAir");
        
    }

    public void Ungrip()
    {
        Debug.Log("FriendUngrip");
        _physicsModule.SetBalanceMode(PhysicsModule.BalanceMode.StabalizerJoint);
        _enableMovement = true;
        _activeRagdoll.HeadNeck.SetStrengthScale(1);
        _activeRagdoll.RightLeg.SetStrengthScale(1);
        _activeRagdoll.LeftLeg.SetStrengthScale(1);
        _animationModule.PlayAnimation("Idle");

    }
}
