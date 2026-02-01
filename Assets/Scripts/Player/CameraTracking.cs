using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Transform target; // Kéo Player vào đây
    [SerializeField] private float smoothTime = 0.2f; 
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        
        // SmoothDamp giúp camera chạy theo mượt mà, có gia tốc
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPos, 
            ref currentVelocity, 
            smoothTime
        );
    }
}