using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IAttackable
{
    [SerializeField] CharacterController _characterController;

    [SerializeField] Animator _animator;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask _decorLayer;

    [SerializeField] private bool _stunLocked = false;
    IEnumerator stunlockRoutine;


    private void FixedUpdate()
    {
        WallCheck();
        GroundCheck();
        if (!_stunLocked)
        {
            _characterController.Move(transform.right * transform.localScale.x * _speed * Time.deltaTime);
        }
    }

    public void PlayhitFx(FX hitFx, Vector3 positionFx)
    {

    }

    void IAttackable.GetAttacked()
    {
        // TODO Play Animation
        stunlockRoutine = StunLockRoutine();
        if (!_stunLocked) StunLock(stunlockRoutine);
    }

    void StunLock(IEnumerator routine)
    {
        StopCoroutine(routine);
        StartCoroutine(routine);
    }

    IEnumerator StunLockRoutine()
    {
        _stunLocked = true;
        _animator.SetBool("Stun", true);
        yield return new WaitForSeconds(1.5f);
        _stunLocked = false;
        _animator.SetBool("Stun", false);
    }

    void GroundCheck()
    {
        Collider[] groundAtBottom = new Collider[1];
        groundAtBottom = Physics.OverlapBox(_groundCheck.position, new Vector3(0.75f, 0.25f), Quaternion.identity, _decorLayer);

        if (groundAtBottom != null)
        {
            if (groundAtBottom.Length <= 0)
            {
                Flip();
            }
        }
    }

    void WallCheck()
    {
        Collider[] wallAtSide = new Collider[1];
        wallAtSide = Physics.OverlapBox(_wallCheck.position, new Vector3(0.5f, 0.75f), Quaternion.identity, _decorLayer);

        if (wallAtSide != null)
        {
            if (wallAtSide.Length > 0)
            {
                Flip();
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_groundCheck.position, new Vector3(0.75f, 0.25f));
        Gizmos.DrawWireCube(_wallCheck.position, new Vector3(0.5f, 0.75f));
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}


