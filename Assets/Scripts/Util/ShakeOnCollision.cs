using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ShakeOnCollision : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource _source = default;

    void Awake() => _source = GetComponent<CinemachineImpulseSource>();

    void OnCollisionEnter(Collision collision) => _source.GenerateImpulse(collision.relativeVelocity);
}

