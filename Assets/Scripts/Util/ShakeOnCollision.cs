using UnityEngine;

public class ShakeOnCollision : MonoBehaviour
{
    [SerializeField] CameraShakeChannel _channel = default;
    [SerializeField] float _shakeMultiplier = default;

    void OnCollisionEnter(Collision collision)
    {
        _channel.Raise(new CameraShakeArgs
        {
            Intensity = collision.relativeVelocity.magnitude * _shakeMultiplier,
            Location = collision.GetContact(0).point,
        });
    }
}

