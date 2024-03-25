
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        public Transform circleTransform; // Reference to the circle's Transform
        public float smoothSpeed = 0.125f; // How smoothly the camera follows the circle
        public Vector3 offset; // Offset from the circle's position

        void LateUpdate()
        {
            Vector3 desiredPosition = circleTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z); // Keep the camera's original Z position
        }
    }
}
