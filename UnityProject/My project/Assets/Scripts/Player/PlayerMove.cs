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
        [SerializeField] private float _castDistance = 0.6f;
        [SerializeField] private CircleCollider2D _circleCollider; // 변수명과 타입 매칭
        
        [Range(0f, 90f)]
        [SerializeField] private float _maxRampAngle = 45f; // 허용할 최대 경사각 (이보다 높으면 경사로 처리 안 함)
        
        private Vector2 _normalVector;
        private Vector2 _perpendicularVector;

        [Header("CheckList")]
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isJumping;
        [SerializeField] private bool _isRamp;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _circleCollider = GetComponent<CircleCollider2D>();
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
            if (_isJumping)
            {
                _rb.linearVelocity = new Vector2(_currentMovementInput.x * _moveSpeed, _rb.linearVelocity.y);
                return;
            }

            if (Mathf.Abs(_currentMovementInput.x) > 0.01f)
            {
                if (_isGrounded && _isRamp)
                {
                    // 경사로 이동
                    Vector2 rampVelocity = _currentMovementInput.x * _moveSpeed * _perpendicularVector;
                    _rb.linearVelocity = rampVelocity;
                }
                else
                {
                    // 평지 이동
                    _rb.linearVelocity = new Vector2(_currentMovementInput.x * _moveSpeed, _rb.linearVelocity.y);
                }
            }
            else
            {
                if (_isGrounded && _isRamp)
                {
                    // 경사로에서 미끄러짐 방지 (중력 상쇄)
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
            if (ctx.performed && _isGrounded && !_isJumping)
            {
                _isJumping = true;
                _isGrounded = false;
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }

        private void CheckRamp()
        {
            if (_circleCollider == null) return;

            Vector2 startPosition = _circleCollider.bounds.center;
            // CircleCollider2D 이므로 CapsuleCast 대신 CircleCast를 쓰는 것이 더 정확하고 가볍습니다.
            RaycastHit2D hit = Physics2D.CircleCast(startPosition, _circleCollider.radius * transform.lossyScale.x, Vector2.down, _castDistance, _groundLayer);

            if (hit)
            {
                // 법선과 수직 윗방향 벡터(Vector2.up) 사이의 각도를 구합니다.
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // 점프 상승 중일 때는 바닥 체크 패스
                if (_rb.linearVelocity.y > 0.1f && _isJumping)
                {
                    SetAirborne();
                    return;
                }

                // 경사각이 우리가 지정한 최대 경사각보다 크다면 바닥으로 인정하지 않거나 경사로 처리를 안 합니다.
                // 90도 낭떠러지 모서리에 걸치면 순간적으로 각도가 매우 높게 나오므로 여기서 걸러집니다.
                if (slopeAngle > _maxRampAngle)
                {
                    // 만약 완전 벽(90도에 가까운)이라면 땅이 아니라고 판단
                    SetAirborne();
                    return;
                }

                _isGrounded = true;
                _isJumping = false;
                
                _normalVector = hit.normal;
                // 평지가 아닐 때(예: 각도가 2도 이상일 때)만 경사로로 판단
                _isRamp = slopeAngle > 2f; 
                _perpendicularVector = -Vector2.Perpendicular(_normalVector).normalized;
            }
            else
            {
                SetAirborne();
            }
        }

        private void SetAirborne()
        {
            _isGrounded = false;
            _isRamp = false;
            _normalVector = Vector2.up;
            _perpendicularVector = Vector2.right;
            if (_rb.linearVelocity.y < -0.1f)
            {
                _isJumping = false;
            }
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (_circleCollider == null) return;

            Vector2 startPosition = _circleCollider.bounds.center;
            float radius = _circleCollider.radius * transform.lossyScale.x;

            RaycastHit2D hit = Physics2D.CircleCast(startPosition, radius, Vector2.down, _castDistance, _groundLayer);

            Gizmos.color = _isGrounded ? (_isRamp ? Color.blue : Color.green) : Color.red;
            
            // 캐스트 시작과 끝 원 그리기
            Gizmos.DrawWireSphere(startPosition, radius);
            Vector2 endPosition = startPosition + (Vector2.down * _castDistance);
            Gizmos.DrawWireSphere(endPosition, radius);
            
            Gizmos.DrawLine(startPosition + Vector2.left * radius, endPosition + Vector2.left * radius);
            Gizmos.DrawLine(startPosition + Vector2.right * radius, endPosition + Vector2.right * radius);

            if (hit)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(hit.point, 0.05f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(hit.point, _normalVector * 0.5f);
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(hit.point, _perpendicularVector * 0.5f);
            }
        }
#endif
    }
}