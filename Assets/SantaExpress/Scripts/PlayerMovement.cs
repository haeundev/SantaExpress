using UnityEngine;

namespace SantaExpress.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private VariableJoystick joystick;

        public void FixedUpdate()
        {
            var direction = Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical;
            transform.position += direction * speed * 0.01f;
        }
    }
}