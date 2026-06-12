using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interfacemanager : MonoBehaviour
{
    public static Interfacemanager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Image _lazoneDeTexte;
    int indexImage = -1;

    public Image[] _illustrations;

    public void AfficherMamie()
    {
        if (indexImage > 0)
        {
            _illustrations[indexImage - 1].gameObject.SetActive(false);
        }
        indexImage++;
        _illustrations[indexImage].gameObject.SetActive(true);
    }

}
