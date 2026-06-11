using UnityEngine;

public class PlayerDamageCheck : MonoBehaviour
{
    [SerializeField] PlayerCharacter character;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoDamage"))
        {
            character.ExplodeCat();
        }
    }


    private void OnTriggerExit(Collider other)
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
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
