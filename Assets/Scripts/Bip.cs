using UnityEngine;

class Bip : Interactable
{
    [SerializeField] private GameObject _door;
    public override void Interation()
    {
        Destroy(_door);
    }
}
