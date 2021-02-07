using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Rigidbody _rb = null;
    [Header("Physic")]
    [SerializeField] private LayerMask _groundLayer = 0;
    [SerializeField] private float _groundThreshold = 1.1f;
    [SerializeField] private float _castRadius = 0.45f;
    [Header("Speeds")]
    [SerializeField] private float _walkSpeed = 250f;
    [SerializeField] private float _airControlSpeed = 250f;
    [SerializeField] private float rotateSpeed = 5;
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 40f;
    [SerializeField] private int _jumpLimit = 0;

    private Vector3 _direction = Vector3.zero;
    private int _jumpCount = 0;
    private bool _isGrounded = false;
    private RaycastHit _hit;
    private ECharacterState _currentStateType = ECharacterState.NONE;
    private Dictionary<ECharacterState, ACharacterState> _states = null;
    #endregion Fields

    #region Properties
    public int JumpCount
    {
        get => _jumpCount;
        set => _jumpCount = value;
    }
    public int JumpLimit => _jumpLimit;
    public bool IsGrounded => _isGrounded;
    public Rigidbody Rb => _rb;
    public ACharacterState CurrentState => _states[_currentStateType];
    #endregion Properties

    #region Methods
    #region MonoBehaviour
    private void Start()
    {
        _states = new Dictionary<ECharacterState, ACharacterState>();

        IdleState idleState = new IdleState();
        idleState.Initialize(this, ECharacterState.IDLE);
        _states.Add(ECharacterState.IDLE, idleState);

        WalkState walkState = new WalkState();
        walkState.Initialize(this, ECharacterState.WALK);
        _states.Add(ECharacterState.WALK, walkState);

        JumpState jumpState = new JumpState();
        jumpState.Initialize(this, ECharacterState.JUMP);
        _states.Add(ECharacterState.JUMP, jumpState);

        FallState fallState = new FallState();
        fallState.Initialize(this, ECharacterState.FALL);
        _states.Add(ECharacterState.FALL, fallState);

        _currentStateType = ECharacterState.FALL;
        CurrentState.EnterState();
    }

    private void Update()
    {
       
    }

    public void FixedUpdate()
    {
        CurrentState.UpdateState();

        _isGrounded = Physics.SphereCast(transform.position, _castRadius, Vector3.down, 
            out _hit, _groundThreshold, _groundLayer);
    }
    #endregion MonoBehaviour

    #region State Machine
    public void ChangeState(ECharacterState newState)
    {
        Debug.Log("Transition from " + _currentStateType + " to " + newState);
        CurrentState.ExitState();
        _currentStateType = newState;
        CurrentState.EnterState();
    }
    #endregion State Machine

    #region Physic
    public void Walk()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        _direction = new Vector3(x:horizontalMove, y:0.0f, z:verticalMove);

        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), rotateSpeed * Time.deltaTime);
        }

        _rb.MovePosition(transform.position + _walkSpeed * Time.deltaTime * _direction); 

        //_rb.velocity = InputManager.Instance.MoveDir * _walkSpeed;
    }

    public void Jump()
    {
        Vector3 tmpVel = _rb.velocity;
        tmpVel.y = 0;
        _rb.velocity = tmpVel;
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    public void AirControl()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        _direction = new Vector3(x: horizontalMove, y: 0.0f, z: verticalMove);

        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), rotateSpeed * Time.deltaTime);
        }

        _rb.MovePosition(transform.position + _airControlSpeed * Time.deltaTime * _direction);
    }
    #endregion Physic
    #endregion Methods
}
