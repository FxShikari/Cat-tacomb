using System.Collections;
using UnityEngine;

public class FallingPlatformes : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _col;
    [SerializeField] private Vector3 _pos;

    [SerializeField] private float _fallingTilme;
    [SerializeField] private float _returnSpeed;

    private void Awake()
    {
        _pos = transform.position;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    public void fall()
    {
            _rb.useGravity = true;
            //_col.enabled = false;
            StartCoroutine(Cooldown(_fallingTilme));
    }

    IEnumerator Reset()
    {
        _rb.useGravity = false;
        _rb.linearVelocity = Vector3.zero;
        transform.position = _pos;
        //_col.enabled = true;
        yield return null;
    }

    IEnumerator Cooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Reset());
    }

    
}
