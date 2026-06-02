using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("Player")]
    //[SerializeField] private PlayerAnimation _playerAnim;
    [SerializeField] private int _jumpCount;
    private CharacterController _characterController;
    private bool _canDash = true;
    private bool _canMove = true;
    [SerializeField] private bool _isWalled = false;
    private float _dashCd = 1f;
    Vector3 _movementInput;
    [SerializeField] Vector3 _direction;

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _speed;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _gravity;
    [SerializeField] private float _wallGravity;
    private float velocity;

    public bool canjump = true;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    protected void FixedUpdate()
    {
        // Character Control for movement
        if (_characterController.isGrounded)
        {
            _jumpCount = 0;
            canjump = true;
        }

        //if(!_isWalled)
        //{
            if(_canMove)
            {
                _direction = transform.right * _movementInput.x + Vector3.zero + transform.up * velocity;
            }
            else
            {
                _direction = Vector3.zero + transform.up * velocity;
            }
        //}
        //else
        //{
        //    _direction = transform.right * _movementInput.x + Vector3.zero  ;
        //}

        Movement();
        
        // Checking for walls
        WallCheck();

        ApplyGravity();
        
    }

    private void DisablePlayerInput(float time)
    {
        StartCoroutine(DisablePlayerInputRoutine(time));
    }

    IEnumerator DisablePlayerInputRoutine(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
    }

    private void WallCheck()
    {
        if (!_characterController.isGrounded)
        {
            Debug.DrawRay(transform.position, -transform.right.normalized * 1f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.right, out hit, 1f, wallLayer))
            {
                _jumpCount--;
                _isWalled = true;
            }
            else
            {
                _isWalled = false;
            }
        }
        else
        {
            
        }
    }

    private void Movement()
    {
        if (_direction.sqrMagnitude == 0) return;
        _characterController.Move(_direction * _speed * Time.deltaTime);
    }

    //get the input value
    public void OnMove(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
    }

    //Juuuuuump
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_characterController.isGrounded || _jumpCount < 1)
            {
                _jumpCount++;
                velocity = Mathf.Sqrt(_jumpPower * -2f * _gravity);
                if (_jumpCount >= 1)
                {
                    canjump = false;
                }
            }
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_canDash == true)
            {
                StartCoroutine(Dash());
                DisablePlayerInput(_dashTime);
            }
        }
    }

    IEnumerator Dash()
    {
        float dashTime = Time.time;
        Vector3 newDirection = _direction;

        //_canMove = false;

        while (Time.time < dashTime + _dashTime)
        {
            _characterController.Move(newDirection * _dashSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(DashCooldown(_dashCd));
    }

    IEnumerator DashCooldown(float delay)
    {
        //_canMove = true;
        _canDash = false;
        yield return new WaitForSeconds(delay);
        _canDash = true;
    }
   
    //it's just the gravity 
    private void ApplyGravity()
    {
            if (_characterController.isGrounded)
            {
    
                velocity += -1;
                velocity = Mathf.Clamp(velocity, -0.1f , 100 );
            }
            else if (_isWalled)
            {
                velocity = _wallGravity * Time.deltaTime;
            }
            else
            {
                velocity += _gravity * _gravityMultiplier * Time.deltaTime;
            }

        _direction.y = velocity;
    }
}
