using UnityEngine;

[ExecuteAlways]
public class XelOrientation : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(_camera.transform.position);
    }
}
