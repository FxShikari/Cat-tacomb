using UnityEngine;

public class PlayerDamageCheck : MonoBehaviour
{
    [SerializeField] PlayerCharacter character;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoDamage"))
        {
            print("eee");
            character.ExplodeCat();
        }
    }
}
