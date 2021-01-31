using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderTriggerEvents : MonoBehaviour 
{
    [SerializeField] bool _triggerOnce = default;
    [SerializeField] LayerMask _mask = default;
    [SerializeField] UnityEvent _event = default;
    [SerializeField] UnityEvent<Collider> _eventWithInfo = default;
    bool _happened = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if this has already happened. 
        if(_triggerOnce && _happened) return;
        
        // Check if we're colliding with the thing we care about. 
        if((_mask.value & (1 << other.gameObject.layer)) == 0) return;

        _event.Invoke();
        _eventWithInfo.Invoke(other);
        _happened = true;
    }

}