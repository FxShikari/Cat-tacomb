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
    private CharacterController _characterController;

    [SerializeField] private int _jumpCount;
    private bool _canDash = true;
    private bool _canMove = true;
    [SerializeField] private bool _isWalled = false;
    [SerializeField] private bool _canWallJump = true;
    [SerializeField] float _characterDirection = 1f; // 1 = regarde à droite

    private bool _facingRight = true;
    Vector3 _movementInput;
    public float _nonPlayerMovementInput;
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
        }


        if (_canMove)
        {
            _direction = transform.right * _movementInput.x + Vector3.zero + transform.up * velocity;
            if (_movementInput.x > 0 && !_facingRight)
            {
                Flip();
            }
            else if (_movementInput.x < 0 && _facingRight)
            {
                Flip();
            }
        }
        else
        {
            _direction = transform.right * _nonPlayerMovementInput + Vector3.zero + transform.up * velocity;
        }

        Movement();

        // Checking for walls
        if (_canWallJump)
        {
            WallCheck();
        }

        ApplyGravity();

    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
            Debug.DrawRay(transform.position, _movementInput.x * transform.right.normalized * 1f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, _movementInput.x * transform.right, out hit, 1f, wallLayer))
            {
                _jumpCount = 0;
                _isWalled = true;
            }
            else
            {
                _isWalled = false;
            }
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
            }

            if (_isWalled)
            {
                _isWalled = false;
                Flip();
                DisablePlayerInput(0.2f);
                DisableWallJump();
                velocity = Mathf.Sqrt(_jumpPower * -2f * _gravity);
                if (_facingRight)
                {
                    _nonPlayerMovementInput = 1f;
                }
                else
                {
                    _nonPlayerMovementInput = -1f;
                }
            }
        }
    }

    private void DisableWallJump()
    {
        StartCoroutine(DisableWallJumpRoutine());
    }

    IEnumerator DisableWallJumpRoutine()
    {
        _canWallJump = false;
        yield return new WaitForSeconds(0.2f);
        _canWallJump = true;
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

        StartCoroutine(DashCooldown(_dashTime));
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
            velocity = Mathf.Clamp(velocity, -0.1f, 100);
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
