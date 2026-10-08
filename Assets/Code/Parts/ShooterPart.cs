using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterPart : RobotPart
{
    [SerializeField] private Bullet bulletPrefab;

    private BulletManager _bulletManager;

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

        _bulletManager = new BulletManager();
    }

    private void Shoot(InputAction.CallbackContext callbackContext)
    {
        _bulletManager.Shoot(bulletPrefab, _parents.handParent.transform, _config.bulletSpeed);
    }

    private void Aim(InputAction.CallbackContext callbackContext)
    {

    }
}
