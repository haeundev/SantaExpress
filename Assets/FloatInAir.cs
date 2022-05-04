using UnityEngine;

public class FloatInAir : MonoBehaviour
{
    [SerializeField] private float amplitude;
    [SerializeField] private float speed;
    private Vector3 _tempPos;
    private float _tempValY;

    private void Update()
    {
        //if (Input.anyKey)
        //    return;
        _tempPos.y = transform.localPosition.y + amplitude * 0.01f * Mathf.Sin(speed * Time.time);
        transform.localPosition = new Vector3(transform.localPosition.x, _tempPos.y, transform.localPosition.z);
    }
}