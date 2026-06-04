using UnityEngine;

public class Enemy : MonoBehaviour, IAttackable
{
    [SerializeField] Transform _pointA;
    [SerializeField] Transform _pointB;
    Transform _currentTarget;
    public void GetAttacked(int damage)
    {
        // TODO Play Animation
        enabled = false;

    }

    public void PlayhitFx(FX hitFx, Vector3 positionFx)
    {

    }

    void IAttackable.GetAttacked()
    {
        // TODO Play Animation
        //Destroy(gameObject);
        gameObject.SetActive(false);
        print("skibidi");
    }

    private void FixedUpdate()
    {
        
    }
}


