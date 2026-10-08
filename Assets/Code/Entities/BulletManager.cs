using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private List<Bullet> _bullets = new List<Bullet>();
    private ConcurrentPool _bulletPool;

    public BulletManager()
    {
        _bulletPool = new ConcurrentPool();
        EventBus.Subscribe<OnBulletCollision>(Destroy);
    }

    ~BulletManager()
    {
        EventBus.Unsubscribe<OnBulletCollision>(Destroy);
    }

    public void Shoot(Bullet bulletPrefab, Transform parent, float speed)
    {
        _bullets.Add(_bulletPool.GetMono(bulletPrefab, parent.position, parent.forward, speed));
    }

    private void Destroy(Bullet bullet)
    {
        if (_bullets.Contains(bullet))
        {
            _bulletPool.Release(bullet);
            _bullets.Remove(bullet);
        }
    }
    
    private void Destroy(in OnBulletCollision context)
    {
        Destroy(context.bullet);
    }
}