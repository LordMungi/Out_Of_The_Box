using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterPart : RobotPart
{
    [SerializeField] private Bullet bulletPrefab;

    private ConcurrentPool _bulletPool;

    private PlayerConfig _config;
    private PlayerParents _parents;

    public override void GetPlayerData(in OnPlayerInstantiated onPlayerInstantiated)
    {
        base.GetPlayerData(onPlayerInstantiated);
        _config = onPlayerInstantiated.playerData.config;
        _parents = onPlayerInstantiated.playerData.parents;
    }

    protected override void SetActionPair()
    {
        type = Type.Shooter;

        actions.Add(new ActionPair(_inputActions.Player.Attack, ActionPair.Phase.Performed, Shoot));

        _bulletPool = new ConcurrentPool();
    }

    private void Shoot(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Pow!");
        Bullet b = _bulletPool.GetMono(bulletPrefab, _parents.handParent.position, _parents.handParent.forward, _config.bulletSpeed);
    }

    private void Aim(InputAction.CallbackContext callbackContext)
    {

    }
}
