using UnityEditor;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void ChargeScene(SceneAsset scene)
    {
        print("ok");
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.name.ToString());
    }
}
