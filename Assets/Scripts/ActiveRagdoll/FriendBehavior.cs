using UnityEngine;
using ActiveRagdoll;

/// <summary> Default behaviour of an Active Ragdoll </summary>
public class FriendBehavior : MonoBehaviour, IGrippable
{
  // Author: Sergio Abreu García | https://sergioabreu.me

  [Header("Modules")]
  [SerializeField] private ActiveRagdoll.ActiveRagdoll _activeRagdoll = default;
  [SerializeField] private PhysicsModule _physicsModule = default;
  [SerializeField] private AnimationModule _animationModule = default;

  private Vector2 _movement = default;
  private Vector2 _aimPoint = default;

  void Start()
  {
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

  public void Ungrip()
  {
    _physicsModule.SetBalanceMode(PhysicsModule.BalanceMode.StabalizerJoint);
    _activeRagdoll.HeadNeck.SetStrengthScale(1);
    _activeRagdoll.RightLeg.SetStrengthScale(1);
    _activeRagdoll.LeftLeg.SetStrengthScale(1);
    _animationModule.PlayAnimation("Default");
  }

  public void PutInUber()
  {
    GameStateManager.Instance.FriendFound();
    Debug.Log("Friend put in uber!");
    // This fails
    // Destroy(this.gameObject);
  }
}
