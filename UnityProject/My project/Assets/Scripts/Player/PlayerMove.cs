using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Karon.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class PlayerMove: MonoBehaviour
    {
        [Header("Movement Related Element")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private Vector2 currentMovementInput;
        [SerializeField] private Vector2 currentJumpInput;
        [SerializeField] private double holdingTime;
        private Rigidbody2D _rb;

        [Header("CheckList")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isJumping;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _rb.linearVelocity = new Vector2(currentMovementInput.x * moveSpeed, _rb.linearVelocity.y);
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            currentMovementInput = ctx.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && isGrounded)
            {
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
        
        public void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
}