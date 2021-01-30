using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightPulseOnEvent : MonoBehaviour 
{
    [SerializeField] SimpleEventChannel _channel = default;

    [SerializeField] AnimationCurve _curve = default;
    [SerializeField] float _pulseDuration = 0.5f;
    float _initialIntensity = 0f;

    Tween _tween = default;
    Light _light = default;

    void Awake() 
    {
        _light = GetComponent<Light>();
        _initialIntensity = _light.intensity;
    }
    
    void OnEnable() => _channel.AddListener(Pulse);
    void OnDisable() => _channel.RemoveListener(Pulse);

    void Pulse()
    {
        // Then pulse
        _tween?.Kill();
        _tween = GetComponent<Light>().DOIntensity(_initialIntensity, _pulseDuration)!
            .From(0f)
            .SetEase(_curve);
    }
}