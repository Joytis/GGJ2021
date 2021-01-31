using UnityEngine;
using UnityEngine.Events;

public class CollisionEvents : MonoBehaviour 
{
    [SerializeField] bool _triggerOnce = default;
    [SerializeField] LayerMask _mask = default;
    [SerializeField] UnityEvent _event = default;
    bool _happened = false;

    void OnCollisionEnter(Collision other)
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