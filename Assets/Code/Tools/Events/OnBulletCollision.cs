public struct OnBulletCollision : IEvent
{
    public Bullet bullet;

    public void Assign(params object[] parameters)
    {
        bullet = (Bullet)parameters[0];
    }

    public void Reset()
    {
        bullet = null;
    }
}