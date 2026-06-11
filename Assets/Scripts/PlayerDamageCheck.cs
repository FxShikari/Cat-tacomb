using UnityEngine;

public class PlayerDamageCheck : MonoBehaviour
{
    [SerializeField] PlayerCharacter character;

    private void OnTriggerEnter(Collider other)
    {
        print("eee trigger");
        if (other.gameObject.CompareTag("DoDamage"))
        {
            character.ExplodeCat();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        print("ekke trigger");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("eee collide");
        if (collision.gameObject.CompareTag("DoDamage"))
        {
            print("zobi");
            character.ExplodeCat();
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //}
}
