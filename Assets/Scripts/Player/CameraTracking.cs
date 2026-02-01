using System;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
