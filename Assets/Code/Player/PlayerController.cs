using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private List<PlayerHandler> _handlers;

    void Start()
    {
        _handlers.Add(new MovementHandler(this));
        _handlers.Add(new JumpHandler(this));
    }

    void Update()
    {
        foreach (PlayerHandler handler in _handlers)
        {
            handler.Update();
        }
    }
}
