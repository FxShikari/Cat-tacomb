using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interfacemanager : MonoBehaviour
{
    public string[] _listeTexte;
    public TextMeshProUGUI leText;
    public Image _lazoneDeTexte;
    int indexImage = 0;

    public Image _illustrationMamie;

    public void NextImage()
    {
        leText.text = _listeTexte[indexImage];
        indexImage ++;
    }

    public void EnableZoneText()
    {
        leText.gameObject.SetActive(true);
    }

}
