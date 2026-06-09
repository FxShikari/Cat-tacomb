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
    [SerializeField] private Transform _characterBottom;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Animator _animator;

    [SerializeField] private int _jumpCount;
    private bool _canDash = true;
    private bool _canMove = true;
    [SerializeField] private bool _isWalled = false;
    [SerializeField] private bool _canWallJump = true;
    [SerializeField] float _characterDirection = 1f; // 1 = regarde à droite
    [SerializeField] bool _haveMovement = true;

    [SerializeField] private bool _facingRight = true;
    [SerializeField] private float _facingDirection = 1f;
    Vector3 _movementInput;
    public float _nonPlayerMovementInput;
    [SerializeField] Vector3 _direction;

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask _enemyMask;
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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
        {
            print("zebi");
            ReturnToCheckpoint();
        }

#endif
        if (_canMove)
        {
            if (_movementInput.x > 0 && !_facingRight)
            {
                Flip();
            }
            else if (_movementInput.x < 0 && _facingRight)
            {
                Flip();
            }
            _direction = transform.right * _movementInput.x + Vector3.zero + transform.up * velocity;
        }
        else
        {
            //_direction = transform.right * _nonPlayerMovementInput + Vector3.zero + transform.up * velocity;
            _direction = transform.right * _facingDirection + Vector3.zero + transform.up * velocity;
        }


        // animation
        _animator.SetFloat("VelocityJump", velocity);
        if (_direction.x != 0)
        {
            _animator.SetBool("Running", true);
        }
        else
        {
            _animator.SetBool("Running", false);
        }

        _animator.SetBool("Grounded", _characterController.isGrounded);


        // Character Control for movement
        if (_characterController.isGrounded)
        {
            _jumpCount = 0;
        }
        else
        {
            BottomCheck();
        }

        if (_haveMovement)
        {
            Movement();
        }

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
        _facingDirection *= -1;
        transform.localScale = theScale;

        _animator.SetBool("Running", false);
        _animator.SetBool("Running", true);
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

                _animator.SetBool("Walled", true);
            }
            else
            {
                _isWalled = false;

                _animator.SetBool("Walled", false);
            }
        }
    }

    private void BottomCheck()
    {
        Collider[] ennemyAtBottom = new Collider[16];
        ennemyAtBottom = Physics.OverlapBox(_characterBottom.position, new Vector3(0.75f, 0.5f), Quaternion.identity, _enemyMask);
        //print(ennemyAtBottom.Length);

        //Debug.DrawRay(transform.position - new Vector3(0.05f, 0.05f), transform.right.normalized * 0.05f, Color.red);
        //Debug.DrawRay(transform.position - new Vector3(0.05f, 0.05f), -transform.up * 0.05f, Color.red);

        if (ennemyAtBottom != null)
        {
            if (ennemyAtBottom.Length != 0 && ennemyAtBottom[0] != null)
            {
                //print(ennemyAtBottom[0].name);
                Debug.DrawLine(_characterBottom.position, ennemyAtBottom[0].transform.position);
                velocity = Mathf.Sqrt((_jumpPower / 2) * -2f * _gravity);
                ennemyAtBottom[0].GetComponent<IAttackable>().GetAttacked();
            }
        }


        ennemyAtBottom = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_characterBottom.position, new Vector3(0.75f, 0.25f));
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
        if (ctx.performed)
        {
            //_cameraTarget.transform.position = _cameraTarget.transform.position + (_movementInput * 2);
            //_animator.SetBool("Running", true);
        }
        else if (ctx.canceled)
        {
            //_cameraTarget.transform.localPosition = Vector3.zero;
            //_animator.SetBool("Running", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DoDamage"))
        {

        }
    }

    //Juuuuuump
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_isWalled)
            {
                _isWalled = false;
                Flip();
                DisablePlayerInput(0.1f);
                DisableWallJump();
                velocity = Mathf.Sqrt(_jumpPower * -2f * _gravity);

                _animator.SetTrigger("Jump");
                _animator.SetBool("Walled", false);
            }
            else if (_characterController.isGrounded || _jumpCount < 1)
            {
                _jumpCount++;
                velocity = Mathf.Sqrt(_jumpPower * -2f * _gravity);

                _animator.SetTrigger("Jump");
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

        //_canMove = false;

        while (Time.time < dashTime + _dashTime)
        {
            _characterController.Move(Vector3.right * _facingDirection * _dashSpeed * Time.deltaTime);
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

    void StopAllMovement(float duration)
    {
        print("on va y aller peu par peu");
        StartCoroutine(StopAllMovementRoutine(duration));
    }

    IEnumerator StopAllMovementRoutine(float duration)
    {
        _haveMovement = false;
        print("fakse");
        yield return new WaitForSeconds(duration);
        print("true");
        _haveMovement = true;
    }

    void ReturnToCheckpoint()
    {
        StopAllMovement(0.5f);
        CheckpointManager.Instance.ReturnToLastCheckpoint();
    }




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

    // TODO
    // Rajouter de la gélatine
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if (hit.gameObject.layer == 69)
        //{
        //    Debug.Log(gameObject.name);
        //    hit.gameObject.GetComponent<Interactable>().Interation();
        //}
    }
}
