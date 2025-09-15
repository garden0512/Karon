using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Karon
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float PlayerMoveSpeed;
        [SerializeField] private float PlayerJumpForce;
        [SerializeField] private float PlayerInputValue;
        [SerializeField] private bool isGround;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.linearVelocityX = PlayerInputValue * PlayerMoveSpeed;
        }

        private void LateUpdate()
        {
            if (PlayerInputValue != 0)
            {
                _spriteRenderer.flipX = PlayerInputValue > 0;
            }
        }

        private void OnMove(InputValue value)
        {
            PlayerInputValue = value.Get<Vector2>().x;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Ground"))
            {
                isGround = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.CompareTag("Ground"))
            {
                isGround = false;
            }
        }

        private void OnJump(InputValue value)
        {
            if (isGround)
            {
                _rigidbody2D.AddForceY(PlayerJumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
