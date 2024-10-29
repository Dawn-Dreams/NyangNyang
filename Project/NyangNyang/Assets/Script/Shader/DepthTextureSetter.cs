using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DepthTextureSetter : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.DepthNormals;
    }
}
