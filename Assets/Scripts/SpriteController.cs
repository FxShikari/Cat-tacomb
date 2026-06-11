using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;



    public void DisableRenderer()
    {
        _spriteRenderer.enabled = false;
    }
    public void EnableRenderer()
    {
        _spriteRenderer.enabled = true;
    }
}
