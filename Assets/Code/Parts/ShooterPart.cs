using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterPart : RobotPart
{
    [SerializeField] private Bullet bulletPrefab;

    private ConcurrentPool _bulletPool;

    protected override void SetActionPair()
    {
        type = Type.Shooter;

        actions.Add(new ActionPair(_inputActions.Player.Attack, ActionPair.Phase.Performed, Shoot));

        _bulletPool = new ConcurrentPool();
    }

    private void Shoot(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Pow!");
        Bullet b = _bulletPool.GetMono<Bullet>(bulletPrefab, new Vector3(1, 0, 0));
    }

    private void Aim(InputAction.CallbackContext callbackContext)
    {

    }
}
