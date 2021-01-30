using UnityEngine;
using ActiveRagdoll;
using UnityEngine.Playables;
using System.Collections;

public class InputWaiter : MonoBehaviour 
{
    [SerializeField] PlayableDirector _director = default;

    ActiveRagdollActions _input = default;

    bool _moved = false;
    bool _clickedLeft = false;
    bool _clickedRight = false;

    void Awake() 
    {
        _input = new ActiveRagdollActions();
        _input.Enable();
        _input.Player.Move.performed += ctx => OnMove(ctx.ReadValue<Vector2>());
        _input.Player.LeftArm.performed += ctx => OnLeft(ctx.ReadValue<float>());
        _input.Player.RightArm.performed += ctx => OnRight(ctx.ReadValue<float>());
    }

    public void OnMove(Vector2 value) => _moved = _moved || value.magnitude > 0f;
    public void OnLeft(float value) => _clickedLeft = _clickedLeft || value > 0f;
    public void OnRight(float value) => _clickedRight = _clickedRight || value > 0f;

    IEnumerator WasdWaitSequence() 
    {
        _director.Pause();
        yield return new WaitUntil(() => _moved);
        _director.Play();
    }

    IEnumerator WaitForGrabsSequence()
    {
        _director.Pause();
        yield return new WaitUntil(() => _clickedLeft);
        yield return new WaitUntil(() => _clickedRight);
        _director.Play();
    }

    public void WaitForWasd() => StartCoroutine(WasdWaitSequence());
    public void WaitForGrabs() => StartCoroutine(WaitForGrabsSequence());
}
