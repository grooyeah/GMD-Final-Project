using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _initialPoolSize = 10;
    [SerializeField] private int _maxPoolSize = 20; // Maximum number of bullets in the pool
    private Queue<GameObject> _bulletPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            var bullet = Instantiate(_bulletPrefab, transform);
            bullet.SetActive(false);
            _bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        // Check if there are any inactive bullets in the pool first
        while (_bulletPool.Count > 0 && !_bulletPool.Peek().activeInHierarchy)
        {
            var bullet = _bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }

        // If all bullets in the pool are active, and the pool has not reached its maximum size,
        // instantiate a new bullet
        if (_bulletPool.Count < _maxPoolSize)
        {
            var bullet = Instantiate(_bulletPrefab, transform);
            return bullet;
        }

        // If the pool has reached its maximum size, return null or reuse the oldest bullet
        // Option 1: Return null
        // return null;

        // Option 2: Reuse the oldest bullet (forcefully recycle the first bullet in the queue)
        var recycledBullet = _bulletPool.Dequeue();
        recycledBullet.SetActive(false); // Optional: reset the bullet's state if necessary
        recycledBullet.SetActive(true);
        return recycledBullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        if (_bulletPool.Count < _maxPoolSize && !bullet.activeInHierarchy)
        {
            bullet.SetActive(false);
            bullet.transform.SetParent(transform);
            _bulletPool.Enqueue(bullet);
        }
        else
        {
            // The pool is full or the bullet is already active, so destroy it or handle accordingly
            Destroy(bullet);
        }
    }
}
