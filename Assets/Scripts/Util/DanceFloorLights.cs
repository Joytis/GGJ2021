using UnityEngine;

public class DanceFloorLights : MonoBehaviour 
{
    [SerializeField] Color[] _colors = default;
    [SerializeField] float _secondsPerSwap = default;
    Light[] _lights = default;
    int _currentTime = default;

    void Awake() => _lights = GetComponentsInChildren<Light>(true);


    void Update() 
    {
        var lastTime = _currentTime;
        _currentTime = Mathf.FloorToInt(Time.time / _secondsPerSwap);
        if(_currentTime != lastTime)
        {
            foreach(var light in _lights)
            {
                light.color = _colors[Random.Range(0, _colors.Length)];
            }
        }
    }
}