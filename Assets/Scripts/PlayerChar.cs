using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
class PlayerChar : MonoBehaviour
{
    [Header("Player")]
    //[SerializeField] private PlayerAnimation _playerAnim;
    private int _jumpCount;
    private CharacterController _characterController;
    private bool _canDash = true;
    private float _dashCd = 1f;
    Vector3 _movementInput;
    Vector3 _direction;

    [SerializeField] private float _jumpPower;
    [SerializeField] private float _gravityMultiplier = 3.0f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _dashSpeed = 20;
    [SerializeField] private float _dashTime = 0.25f;
    [SerializeField] private float _gravity = -9.81f;
    private float velocity;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    protected void FixedUpdate()
    {
        if (_characterController.isGrounded) _jumpCount = 0;
        _direction = transform.right * _movementInput.x + Vector3.zero + transform.up * velocity;
        ApplyGravity();
        print (velocity);
        Movement();
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
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_canDash == true)
            {
            StartCoroutine(Dash());
            }
        }
    }

    IEnumerator Dash()
    {
        float dashTime = Time.time;

        while (Time.time < dashTime + _dashTime)
        {
            _characterController.Move(_direction * _dashSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(DashCooldown(_dashCd));
    }

    IEnumerator DashCooldown(float delay)
    {
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
        else
        {
            velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }

        _direction.y = velocity;
    }
}
