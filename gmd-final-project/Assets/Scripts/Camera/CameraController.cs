
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private Vector3 _offset;

        private void LateUpdate()
        {
            if (_player)
            {
                Vector3 desiredPosition = _player.transform.position + _offset;
                transform.position = desiredPosition;
            }
        }
    }
}
