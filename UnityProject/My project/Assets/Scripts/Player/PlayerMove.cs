using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Karon.Player
{
    public class PlayerMove: MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private Vector2 currentMovementInput;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (Mathf.Approximately(currentMovementInput.x, 0f))
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            float direction = currentMovementInput.x * moveSpeed;
            rb.linearVelocity = new Vector2(direction, rb.linearVelocity.y);
        }

        public void OnMove(InputValue value)
        {
            currentMovementInput = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }
}