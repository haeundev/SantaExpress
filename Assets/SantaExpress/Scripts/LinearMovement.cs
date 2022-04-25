using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public Vector3 direction = Vector3.left;
    public Space movementSpace = Space.World;
    private float _initialSpeed;
    private Vector3 _movement;
    
    private void Awake()
    {
        _initialSpeed = speed;
    }

    private void Update()
    {
        Move();
    }
    
    private void OnEnable()
    {
        speed = _initialSpeed;
    }

    private void Move()
    {
        _movement = direction * (speed / 10) * Time.deltaTime;
        transform.Translate(_movement, movementSpace);
        speed += acceleration * Time.deltaTime;
    }
}