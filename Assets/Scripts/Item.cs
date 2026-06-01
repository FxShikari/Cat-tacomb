using System;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    protected Collider itemCollider;

    // Getters
    public string getName()
    {
        return itemName;
    }

    // M�thode sp�cifique � r��crire
    public virtual void UseItem(PlayerCharacter player)
    {
    }

    public virtual void GetPickedUp(PlayerCharacter player)
    {
        Destroy(gameObject);
    }

}