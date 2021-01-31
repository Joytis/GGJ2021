using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ShakeOnBeat : MonoBehaviour
{
    [SerializeField] SimpleEventChannel _channel = default;
    [SerializeField] CinemachineImpulseSource _source = default;

    void OnEnable() => _channel.AddListener(Shake);
    void OnDisable() => _channel.RemoveListener(Shake);

    void Shake() => _source.GenerateImpulse();
}

