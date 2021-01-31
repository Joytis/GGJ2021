using UnityEngine;

/// <summary> Default behaviour of an Active Ragdoll </summary>
public class CollisionForwarder : MonoBehaviour {

    ActiveRagdoll.ActiveRagdoll _daddyRagdoll = default;

    public void Initialize(ActiveRagdoll.ActiveRagdoll daddy) => _daddyRagdoll = daddy;

    void OnCollisionEnter(Collision collision) => _daddyRagdoll.ChildCollisionEnter(collision);
    void OnCollisionExit(Collision collision) => _daddyRagdoll.ChildCollisionExit(collision);
    void OnCollisionStay(Collision collision) => _daddyRagdoll.ChildCollisionStay(collision);
}
