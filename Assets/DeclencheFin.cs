using UnityEngine;

public class DeclencheFin : MonoBehaviour
{
    public Interfacemanager manag;
    private void OnTriggerEnter(Collider other)
    {
        print("dd");
        manag.gameObject.SetActive(true);
        GameManager.Instance.GetPlayer().OutOfGameplay();
    }
}
