using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
class PlayerChar : MonoBehaviour
{
    [Header("Player")]
    //[SerializeField] private PlayerAnimation _playerAnim;
    private CharacterController _characterController;
    private bool _isJumping;
    Vector3 _movementInput;
    Vector3 _direction;

    [SerializeField] private float _jumpPower;
    [SerializeField] private float _gravityMultiplier = 3.0f;
    [SerializeField] private float _speed = 5f;
    private float _gravity = -9.81f;
    private float velocity;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    protected void FixedUpdate()
    {
        _direction = transform.right * _movementInput.x + Vector3.zero + transform.up * velocity;
        ApplyGravity();
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
            if (_characterController.isGrounded)
            {
                velocity = Mathf.Sqrt(_jumpPower * -2f * _gravity);
                _isJumping = true;
            } 
        }
    }
   
    //it's just the gravity 
    private void ApplyGravity()
    {
        if (_characterController.isGrounded)
        {

            velocity += -1;
        }
        else
        {
            velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }

        _direction.y = velocity;
    }
}
