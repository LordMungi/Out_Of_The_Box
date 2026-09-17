using UnityEngine;

public struct OnNearbyItemEntered : IEvent
{
    public GameObject item;
    public void Assign(params object[] parameters)
    {
        item = (GameObject)parameters[0];
    }

    public void Reset()
    {
        item = null;
    }
}