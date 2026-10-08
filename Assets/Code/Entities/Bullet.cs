using UnityEngine;

public class Bullet : MonoBehaviour, IResettable
{
    private Vector3 _direction;
    private float _speed;

    public void Assign(params object[] parameters)
    {
        transform.position = (Vector3)parameters[0];
        _direction = (Vector3)parameters[1];
        _speed = (float)parameters[2];
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        _direction = Vector3.zero;
        _speed = 0f;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Translate(_direction * Time.deltaTime * _speed);
    }
}