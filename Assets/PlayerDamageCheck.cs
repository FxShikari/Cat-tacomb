using UnityEngine;

public class PlayerDamageCheck : MonoBehaviour
{
    [SerializeField] PlayerCharacter character;

    private void OnTriggerEnter(Collider other)
    {
        print("eee");
        if (other.gameObject.CompareTag("DoDamage"))
        {
            character.ExplodeCat();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        print("ekke");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("eee");
        if (collision.gameObject.CompareTag("DoDamage"))
        {
            character.ExplodeCat();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        print("ekke");
    }
}
