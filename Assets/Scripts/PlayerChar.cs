using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
class PlayerChar : LivingObject
{
    [Header("Player")]
    [SerializeField] private PlayerAnimation _playerAnim;
    [SerializeField] private Weapon _weapon;
    private CharacterController _characterController;
    private bool _isJumping;
    Vector3 _movementInput;
    Vector3 _direction;

    [SerializeField] private float _jumpPower;
    [SerializeField] private float _gravityMultiplier = 3.0f;
    private float _gravity = -9.81f;
    private float velocity;

    private bool _isShooting;
    private bool _isImmune;


    [Header("Camera Parameters")]

    [SerializeField] private float TopClamp = 90.0f;
    [SerializeField] private float BottomClamp = -90.0f;
    [SerializeField] private GameObject CinemachineCameraTarget;
    [SerializeField] float _rotSpeed = 1.0f;

    Vector2 look;
    private float _cinemachineTargetPitch;
    private float _rotationVelocity;
    private const float _threshold = 0.01f;

    protected override void Start()
    {
        base.Start();
        _characterController = GetComponent<CharacterController>();
    }

    protected void FixedUpdate()
    {
        _direction = transform.right * _movementInput.x + transform.forward * _movementInput.y + transform.up * velocity;
        ApplyGravity();
        Movement();
        CameraRotation();
        if (_isShooting) _weapon.Attack();
    }

    protected override void Movement()
    {
        if (CanMove() == false) return;
        if (_direction.sqrMagnitude == 0) return;
        _characterController.Move(_direction * GetSpeed() * Time.deltaTime);
        _playerAnim.Moving(new(_movementInput.x, 0 , _movementInput.y));
    }

    //get the input value
    public void OnMove(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
    }

    //look if the right clic has been pressed or released
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _playerAnim.isShooting(true);
            Move(false);
            _isShooting = true;
        }
        else if (ctx.canceled)
        {
            _playerAnim.isShooting(false);
            Move(true);
            _isShooting = false;
        }
    }

    public void OnUlti(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Move(false);
            _weapon.Ultime();
        }
        else if (ctx.canceled)
        {
            Move(true);
        }
    }

    public override void LoseHp(int _amount)
    {
        if (!_isImmune)
        {
            _isImmune = true;
            base.LoseHp(_amount);
            StartCoroutine(Safe(1f));
        }
    }

    protected override void dead()
    {
        SceneMaster.SetScene("MainMenu");
    }

    IEnumerator Safe(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isImmune = false;
        print("false");
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

    //look if the shift key has been pressed or released
    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SetSpeed(GetSpeed() + 10);
            _playerAnim.IsRunning(true);
        }
        else if (ctx.canceled)
        {
            SetSpeed(GetSpeed() - 10);
            _playerAnim.IsRunning(false);
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

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }
    //camera rotation

    private void CameraRotation()
    {

        // if there is an input
        if (look.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier =  1.0f;

            _cinemachineTargetPitch += look.y * _rotSpeed * deltaTimeMultiplier;
            _rotationVelocity = look.x * _rotSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }
}
