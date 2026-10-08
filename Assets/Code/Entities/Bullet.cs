using UnityEngine;

public class Bullet : MonoBehaviour, IResettable
{
    private Vector3 _direction;

    public void Assign(params object[] parameters)
    {
        _direction = (Vector3)parameters[0];
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Translate(_direction * Time.deltaTime);
    }
}