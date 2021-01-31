using UnityEngine;
using ActiveRagdoll;

/// <summary> Default behaviour of an Active Ragdoll </summary>
public class DancerBehaviour : MonoBehaviour, IGrippable {
    public enum Dances
    {
        ChickenDance,
        YMCA,
        Twist,
    }

    [Header("Config")]
    [SerializeField] Dances _dance = default;

    [Header("Modules")]
    [SerializeField] private ActiveRagdoll.ActiveRagdoll _activeRagdoll = default;
    [SerializeField] private PhysicsModule _physicsModule = default;
    [SerializeField] private AnimationModule _animationModule = default;
    [SerializeField] private GripModule _gripModule = default;    

    private Vector2 _movement = default;
    private Vector2 _aimPoint = default;

    void Start()
    {
        Ungrip();
        _physicsModule.TargetDirection = Auxiliary.GetFloorProjection(_activeRagdoll.PhysicalTorso.position).normalized;
    }

    public void Grip()
    {
        _physicsModule.SetBalanceMode(PhysicsModule.BalanceMode.ManualTorque);
        _activeRagdoll.HeadNeck.SetStrengthScale(0.1f);
        _activeRagdoll.RightLeg.SetStrengthScale(0.05f);
        _activeRagdoll.LeftLeg.SetStrengthScale(0.05f);
        _animationModule.PlayAnimation("InTheAir");
    }

    string GetDanceString()
    {
        switch(_dance)
        {
            case Dances.ChickenDance: return "ChickenDance";
            case Dances.YMCA: return "YMCA";
            case Dances.Twist: return "Twist";
        }
        throw new System.InvalidOperationException();
    }

    public void Ungrip()
    {
        _physicsModule.SetBalanceMode(PhysicsModule.BalanceMode.StabalizerJoint);
        _activeRagdoll.HeadNeck.SetStrengthScale(1);
        _activeRagdoll.RightLeg.SetStrengthScale(1);
        _activeRagdoll.LeftLeg.SetStrengthScale(1);
        _animationModule.PlayAnimation(GetDanceString(), time: Random.Range(0f,1f));
    }
}
