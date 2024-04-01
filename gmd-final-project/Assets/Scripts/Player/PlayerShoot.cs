using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shootOffset;
    [SerializeField] private float _timeBetweenBullets;
    private bool _fireContinuously;
    private float _lastFireTime;

    private void Update()
    {
        if (_fireContinuously)
        {
            if (Time.time - _lastFireTime >= _timeBetweenBullets)
            {
                FireBullet();
                _lastFireTime = Time.time;
            }
        }
    }

    private void FireBullet()
    {
        var bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = _shootOffset.position;
        bullet.transform.rotation = transform.rotation;
        var bulletObject = bullet.GetComponent<Bullet>();
        bulletObject.ActivateBullet();
    }

    private void OnFire(InputValue inputValue)
    {
        _fireContinuously = inputValue.isPressed;
        if (inputValue.isPressed && Time.time - _lastFireTime >= _timeBetweenBullets)
        {
            FireBullet();
            _lastFireTime = Time.time;
        }
    }
}
