using UnityEngine;
using UnityEngine.Audio;

public class InitialSnapshot : MonoBehaviour 
{
    [SerializeField] AudioMixerSnapshot _snapshot = default;
    void Awake() => _snapshot.TransitionTo(0f);
}
