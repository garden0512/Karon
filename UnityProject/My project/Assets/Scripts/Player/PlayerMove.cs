using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Karon.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class PlayerMove : MonoBehaviour, PlayerInputActions.IPlayerActions
    {
        [Header("Movement Related Element")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private Vector2 _currentMovementInput;
        private Rigidbody2D _rb;
        private PlayerInputActions _playerInputActions;

        [Header("Raycast Related")] 
        [SerializeField] private LayerMask _groundLayer;
        private Vector2 _raycastDirection = Vector2.down;
        private Vector2 _normalVector;
        [SerializeField] private float _raycastDistance = 1f;

        [Header("CheckList")] 
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isJumping;
        [SerializeField] private bool _isRamp;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.SetCallbacks(this);
        }

        private void OnEnable()
        {
            _playerInputActions.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
        }

        private void Update()
        {
            CheckRamp();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _rb.linearVelocity = new Vector2(_currentMovementInput.x * _moveSpeed, _rb.linearVelocity.y);
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            _currentMovementInput = ctx.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && _isGrounded)
            {
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true;
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = false;
            }
        }

        private void CheckRamp()
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, _raycastDirection, _raycastDistance, _groundLayer);
            if (hit)
            {
                Transform objectHit = hit.transform;
                _normalVector = hit.normal;
                _isRamp = Mathf.Abs(_normalVector.x) > 0.05f;
                if (objectHit.parent != null)
                {
                    Debug.Log(objectHit.parent.name);
                }
                else
                {
                    Debug.Log(objectHit.name);
                }
            }
            else
            {
                _isRamp = false;
                _normalVector = Vector2.up;
            }
        }
        
        #if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Gizmos.color = _isGrounded ? (_isRamp ? Color.blue : Color.green) : Color.red;
            Gizmos.DrawRay(this.transform.position, _raycastDirection * _raycastDistance);
        }
        #endif
    }
}