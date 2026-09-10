using UnityEngine;

public class JumpHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private float _jumpForce;

    public JumpHandler(ref PlayerData p, float jumpForce)
    {
        _jumpForce = jumpForce;    
    }
    public void Update()
    {

        //Debug.DrawRay(_player.config.feetOrigin.position, Vector3.down, Color.red);
        //if (Physics.Raycast(new Ray(_player.config.feetOrigin.position, Vector3.down), 0.1f))
        //{

        //    EventBus.Raise<OnPlayerLanded>();
        //    Debug.Log("S");
        //}
    }

    public void Jump()
    {
        //_player.body.AddForce(Vector3.up * _jumpForce);
    }
}
