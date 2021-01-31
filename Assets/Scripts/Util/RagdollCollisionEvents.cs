using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ActiveRagdoll.ActiveRagdoll))]
public class RagdollCollisionEvents : MonoBehaviour 
{
    [SerializeField] bool _triggerOnce = default;
    [SerializeField] LayerMask _mask = default;
    [SerializeField] UnityEvent _event = default;
    bool _happened = false;
    ActiveRagdoll.ActiveRagdoll _ragdoll = default;

    void Awake() => _ragdoll = GetComponent<ActiveRagdoll.ActiveRagdoll>();
    void OnEnable() => _ragdoll.onCollisionEnter += RagdollCollisionEnter;
    void OnDisable() => _ragdoll.onCollisionEnter -= RagdollCollisionEnter;

    void RagdollCollisionEnter(Collision other)
    {
        Debug.Log("Collided!");
        // Check if this has already happened. 
        if(_triggerOnce && _happened) return;
        
        // Check if we're colliding with the thing we care about. 
        if((_mask.value & (1 << other.gameObject.layer)) == 0) return;

        _event.Invoke();
        _happened = true;
    }

}