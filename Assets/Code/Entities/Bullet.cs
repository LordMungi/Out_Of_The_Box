using UnityEngine;

public class Bullet : MonoBehaviour, IResettable
{
    [SerializeField] private Rigidbody body;

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

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

    private void FixedUpdate()
    {
        body.MovePosition(body.position + _direction * Time.fixedDeltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
            EventBus.Raise<OnBulletCollision>(this);
    }
}