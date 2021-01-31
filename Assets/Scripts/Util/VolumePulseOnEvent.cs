using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class VolumePulseOnEvent : MonoBehaviour 
{
    [SerializeField] SimpleEventChannel _channel = default;

    [SerializeField] AnimationCurve _curve = default;
    [SerializeField] float _pulseDuration = 0.5f;
    float _initialWeight = 0f;

    Tween _tween = default;
    Volume _volume = default;

    void Awake() 
    {
        _volume = GetComponent<Volume>();
        _initialWeight = _volume.weight;
        _volume.weight = 0f;
        _volume.enabled = true;
    }
    
    void OnEnable() => _channel?.AddListener(Pulse);
    void OnDisable() => _channel?.RemoveListener(Pulse);

    public void Pulse()
    {
        // Then pulse
        _tween?.Kill();
        _tween = DOTween.To(() => _volume.weight, val => _volume.weight = val, _initialWeight, _pulseDuration)
            .From(0f)
            .SetEase(_curve);
    }
}