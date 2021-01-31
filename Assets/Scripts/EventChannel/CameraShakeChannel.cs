using UnityEngine;

public struct CameraShakeArgs 
{
    public float Intensity;
    public Vector3 Location;
}

[CreateAssetMenu(menuName = "GGJ/Camera Shake Channel")]
public class CameraShakeChannel : GenericEventChannel<CameraShakeArgs> {}

