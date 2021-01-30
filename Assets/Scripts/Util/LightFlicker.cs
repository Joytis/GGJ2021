using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour 
{
    [SerializeField] AnimationCurve _flickerCurve = default;
    [SerializeField] Vector2 _flickerTimeRange = new Vector2(4.0f, 7.0f);
    [SerializeField] float _flickerDuration = 1.5f;
    float _initialIntensity = 0f;

    Tween _tween = default;

    IEnumerator Start()
    {
        var light = GetComponent<Light>();
        _initialIntensity = light.intensity;

        while(true)
        {
            // Wait for a time, flicker, then do it again. 
            var randomTime = Random.Range(_flickerTimeRange.x, _flickerTimeRange.y);
            yield return new WaitForSeconds(randomTime);

            // Then flicker
            _tween?.Kill();
            _tween = light.DOIntensity(_initialIntensity, _flickerDuration)
                .From(0f)
                .SetEase(_flickerCurve);
        }
    }
}