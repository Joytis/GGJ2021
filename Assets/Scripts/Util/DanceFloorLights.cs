using UnityEngine;

public class DanceFloorLights : MonoBehaviour 
{
    // Lol APIs.
    [SerializeField] bool _eventOrTime = default;
    [SerializeField] SimpleEventChannel _optionalEvent = default;
    [SerializeField] Color[] _colors = default;
    [SerializeField] float _secondsPerSwap = default;
    Light[] _lights = default;
    int _currentTime = default;

    void Awake() => _lights = GetComponentsInChildren<Light>(true);
    void OnEnable() => _optionalEvent?.AddListener(ChangeColor);
    void OnDisable() => _optionalEvent?.RemoveListener(ChangeColor);

    void ChangeColor() 
    {
        if(!_eventOrTime) return;
        foreach(var light in _lights)
        {
            light.color = _colors[Random.Range(0, _colors.Length)];
        }
    } 

    void Update() 
    {
        var lastTime = _currentTime;
        _currentTime = Mathf.FloorToInt(Time.time / _secondsPerSwap);
        if(_currentTime != lastTime && !_eventOrTime)
        {
            ChangeColor();
        }
    }
}