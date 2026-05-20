using UnityEngine;
using UnityEngine.InputSystem;

namespace Karon.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
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
        [SerializeField] private float _raycastDistance = 1f;
        [SerializeField] private float _raycastPadding = 0.5f;
        [SerializeField] private float _castDistance = 0.6f;
        [SerializeField] private CircleCollider2D _capsuleCollider;
        private Vector2 _raycastDirection = Vector2.down;
        private Vector2 _normalVector;
        private Vector2 _perpendicularVector;

        [Header("CheckList")]
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isJumping;
        [SerializeField] private bool _isRamp;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _capsuleCollider = GetComponent<CircleCollider2D>();
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
            if (Mathf.Abs(_currentMovementInput.x) > 0.01f)
            {
                if (_isGrounded && _isRamp)
                {
                    Vector2 rampVelocity = _currentMovementInput.x * _moveSpeed * _perpendicularVector;
                    _rb.linearVelocity = rampVelocity;
                }
                else
                {
                    _rb.linearVelocity = new Vector2(_currentMovementInput.x * _moveSpeed, _rb.linearVelocity.y);
                }
            }
            else
            {
                if (_isGrounded && _isRamp)
                {
                    Vector2 gravityForce = _rb.mass * _rb.gravityScale * Physics2D.gravity;
                    _rb.AddForce(-gravityForce);
                    _rb.linearVelocity = Vector2.zero;
                }
                else if (_isGrounded && !_isRamp)
                {
                    _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
                }
            }
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            _currentMovementInput = ctx.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && _isGrounded)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }

        private void CheckRamp()
        {
            Vector2 startPosition = _capsuleCollider.bounds.center;
            RaycastHit2D hit = Physics2D.CapsuleCast(startPosition, _capsuleCollider.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, _castDistance, _groundLayer);

            if (hit)
            {
                _isGrounded = true;
                Transform objectHit = hit.transform;
                _normalVector = hit.normal;
                _isRamp = Mathf.Abs(_normalVector.x) > 0.05f;
                _perpendicularVector = -Vector2.Perpendicular(_normalVector).normalized;

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
                _isGrounded = false;
                _isRamp = false;
                _normalVector = Vector2.up;
                _perpendicularVector = Vector2.right;
            }
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (_capsuleCollider == null) return;

            Vector2 startPosition = _capsuleCollider.bounds.center;
            Vector2 size = _capsuleCollider.bounds.size;
            float radius = size.x / 2f;

            // 1. 실제 작동하는 것과 동일하게 CapsuleCast를 한 번 더 체크해서 정보를 가져옵니다.
            RaycastHit2D hit = Physics2D.CapsuleCast(startPosition, size, CapsuleDirection2D.Vertical, 0f, Vector2.down, _castDistance, _groundLayer);

            // 2. 바닥 체크 범위 시각화 (캡슐이 시작점에서 끝점까지 내려가는 경로)
            Gizmos.color = _isGrounded ? (_isRamp ? Color.blue : Color.green) : Color.red;
        
            // 캐스트 시작 위치의 원
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(startPosition, radius);
        
            // 캐스트 최대 도달 위치의 원
            Vector2 endPosition = startPosition + (Vector2.down * _castDistance);
            Gizmos.color = _isGrounded ? (_isRamp ? Color.blue : Color.green) : Color.red;
            Gizmos.DrawWireSphere(endPosition, radius);
        
            // 시작과 끝을 잇는 옆선
            Gizmos.DrawLine(startPosition + Vector2.left * radius, endPosition + Vector2.left * radius);
            Gizmos.DrawLine(startPosition + Vector2.right * radius, endPosition + Vector2.right * radius);

            // 3. 충돌했을 때 법선(Normal)과 접선(Perpendicular)을 충돌 지점에 정확히 그리기
            if (hit)
            {
                Vector2 hitPoint = hit.point;

                // 실제 충돌한 지점에 작은 점 표시
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(hitPoint, 0.05f);

                // 법선 (경사면에서 수직으로 뻗어나오는 노란 선)
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(hitPoint, _normalVector * 0.5f);

                // 접선 (플레이어가 움직일 자줏빛 진행 방향 선)
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(hitPoint, _perpendicularVector * 0.5f);
            }
        }
#endif
    }
}