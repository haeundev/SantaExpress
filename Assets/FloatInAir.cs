using UnityEngine;

public class FloatInAir : MonoBehaviour
{
    [SerializeField] private float amplitude;
    [SerializeField] private float speed;
    private Vector3 _tempPos;
    private float _tempValX;
    private float _tempValY;

    private void Start()
    {
        _tempValY = transform.localPosition.y;
        _tempValX = transform.localPosition.x;
    }

    private void Update()
    {
        _tempPos.y = _tempValY + amplitude * Mathf.Sin(speed * Time.time);
        _tempPos.x = _tempValX + amplitude * Mathf.Sin(speed * Time.time);
        transform.localPosition = new Vector3(_tempPos.x, _tempPos.y, transform.localPosition.z);
    }
}